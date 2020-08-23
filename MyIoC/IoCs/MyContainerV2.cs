using MyIoC.AOPs;
using MyIoC.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace MyIoC.IoCs
{
    public class MyContainerV2 : IMyContainerV2
    {
        // key: the full name of the interface; value: RegisteredModel (type of of implementation, lifeTimeEnum)
        private readonly Dictionary<string, RegisteredService> map = new Dictionary<string, RegisteredService>();

        // key: the full name of the interface; value: default value for constructor
        private readonly Dictionary<string, object[]> paramMap = new Dictionary<string, object[]>();

        // key: the full name of the interface; value: initialized scoped instances
        private readonly ConcurrentDictionary<string, object> scopedObjMap = new ConcurrentDictionary<string, object>();

        // key: the full name of the interface; value: initialized per thread instances
        private readonly ConcurrentDictionary<string, object> perThreadObjMap = new ConcurrentDictionary<string, object>();

        public MyContainerV2()
        {
        }

        private MyContainerV2(Dictionary<string, RegisteredService> map, 
            Dictionary<string, object[]> paramMap,
            ConcurrentDictionary<string, object> scopedObjMap)
        {
            this.map = map;
            this.paramMap = paramMap;
            this.scopedObjMap = scopedObjMap;
        }

        public IMyContainerV2 CreateChildContainer()
        {
            return new MyContainerV2(map, paramMap, new ConcurrentDictionary<string, object>());
        }

        public void AddTransient<TInterface, TImplementation>(
            string shortName = null,
            object[] constParams = null
            ) where TImplementation : TInterface
        {
            Register<TInterface, TImplementation>(LifetimeEnum.Transient, shortName, constParams);
        }

        public void AddScoped<TInterface, TImplementation>(
            string shortName = null,
            object[] constParams = null
            ) where TImplementation : TInterface
        {
            Register<TInterface, TImplementation>(LifetimeEnum.Scope, shortName, constParams);
        }

        public void AddSingleton<TInterface, TImplementation>(
            string shortName = null,
            object[] constParams = null
            ) where TImplementation : TInterface
        {
            Register<TInterface, TImplementation>(LifetimeEnum.Singleton, shortName, constParams);
        }

        public void AddPerThread<TInterface, TImplementation>(
            string shortName = null,
            object[] constParams = null
            ) where TImplementation : TInterface
        {
            Register<TInterface, TImplementation>(LifetimeEnum.PerThread, shortName, constParams);
        }

        private void Register<TInterface, TImplementation>(
            LifetimeEnum lifetimeEnum,
            string shortName = null,
            object[] constParams = null
            ) where TImplementation : TInterface
        {
            string key = GetKey(typeof(TInterface).FullName, shortName);

            // define type and life scope
            map[key] = new RegisteredService()
            {
                LifetimeType = lifetimeEnum,
                TargetType = typeof(TImplementation)
            };

            // enable constant constructor
            if (constParams != null) paramMap[key] = constParams;
        }

        public TInterface Resolve<TInterface>(string shortName = null) 
            where TInterface : class
        {
            Type interfaceType = typeof(TInterface);
            object o = ResolveObject(interfaceType, shortName);
            TInterface instance = o as TInterface;
            #region AOP
            instance = instance.AOP();
            #endregion
            return instance;
        }

        private object ResolveObject(Type interfaceType, string shortName)
        {
            string key = GetKey(interfaceType.FullName, shortName);
            if (!map.ContainsKey(key)) throw new Exception($"Service '{key}' is not registered");

            Type implementationType = map[key].TargetType;
            RegisteredService service = map[key];

            switch (service.LifetimeType)
            {
                case LifetimeEnum.Singleton:
                    // double check lock to ensure thread-safe
                    if (service.SingletonInstance == null)
                    {
                        lock (service.SingletonMutex)
                        {
                            if (service.SingletonInstance == null)
                            {
                                object singletonObj = InitObject(key, implementationType);
                                service.SingletonInstance = singletonObj;
                            }
                        }
                    }
                    return service.SingletonInstance;

                case LifetimeEnum.Scope:
                    // need to be thread safe
                    object newScopedObj = InitObject(key, implementationType);
                    object scopedObj = scopedObjMap.GetOrAdd(key, newScopedObj);
                    return scopedObj;

                case LifetimeEnum.Transient:
                    return InitObject(key, implementationType);

                case LifetimeEnum.PerThread:
                    object newPerThreadObj = InitObject(key, implementationType);
                    string modifiedKey = $"{key}-{Thread.CurrentThread.ManagedThreadId}"; // this is not trictly correct. Thread can be reused.
                    //CallContext.GetData(modifiedKey)
                    object perThreadObj = perThreadObjMap.GetOrAdd(modifiedKey, newPerThreadObj);
                    return perThreadObj;

                default:
                    return null;
            }
        }

        private object InitObject(string key, Type implementationType)
        {
            #region DI by constructor
            ConstructorInfo[] constructors = implementationType.GetConstructors();
            ConstructorInfo ctor = GetCtorWithAttribute(constructors);
            var constParams = paramMap.ContainsKey(key) ? paramMap[key] : null;
            object[] parameters = InitParameters(ctor.GetParameters(), constParams); // recursive inside
            #endregion
            object instance = Activator.CreateInstance(implementationType, parameters.ToArray());

            #region DI by property
            IEnumerable<PropertyInfo> properties = implementationType.GetProperties();
            foreach (PropertyInfo info in properties)
            {
                if (!info.IsDefined(typeof(MyPropertyDIAttribute))) continue;
                Type propertyType = info.PropertyType;
                object propertyObject = ResolveObject(propertyType, GetShortName(info)); // recursive
                info.SetValue(instance, propertyObject);
            }
            #endregion

            #region DI by method
            IEnumerable<MethodInfo> methods = implementationType.GetMethods(); // this will only get public methods
            foreach (MethodInfo info in methods)
            {
                if (!info.IsDefined(typeof(MyMethodDIAttribute))) continue;
                object[] methodParas = InitParameters(info.GetParameters());
                info.Invoke(instance, methodParas);
            }
            #endregion

            return instance;
        }

        private ConstructorInfo GetCtorWithMostParameters(ConstructorInfo[] constructorInfos)
        {
            return constructorInfos
                .OrderByDescending(x => x.GetParameters().Length)
                .First(); // choose the constructor with most parameters
        }

        private ConstructorInfo GetCtorWithAttribute(ConstructorInfo[] constructorInfos)
        {
            var c = constructorInfos.FirstOrDefault(x => x.IsDefined(typeof(MyChosenCtorAttribute), true));
            if (c != null) return c;
            return GetCtorWithMostParameters(constructorInfos);
        }

        private object[] InitParameters(ParameterInfo[] parameterInfos, object[] constParams = null)
        {
            object[] objects = new object[parameterInfos.Length];
            int constParamsIndex = 0;
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                ParameterInfo parameterInfo = parameterInfos[i];
                if (constParams != null && parameterInfo.IsDefined(typeof(MyConstParamAttribute)))
                {
                    object o = constParams[constParamsIndex];
                    objects[i] = o;
                    constParamsIndex++;
                }
                else
                {
                    Type paraType = parameterInfo.ParameterType;
                    string shortName = GetShortName(parameterInfo);
                    object paraObject = ResolveObject(paraType, shortName); // recursive
                    objects[i] = paraObject;
                }
            }
            return objects;
        }

        private string GetShortName(ParameterInfo info)
        {
            return info.GetCustomAttribute<MyShortnameAttribute>()?.ShortName ?? null;
        }

        private string GetShortName(PropertyInfo info)
        {
            return info.GetCustomAttribute<MyShortnameAttribute>()?.ShortName ?? null;
        }

        private string GetKey(string fullName, string shortName)
        {
            if (string.IsNullOrEmpty(shortName)) return fullName;
            return $"{fullName}_{shortName}";
        }

    }
}
