using MyIoC.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyIoC.IoCs
{
    public class MyContainerV1 : IMyContainerV1
    {
        // key: the full name of the interface; value: type of of implementation
        private readonly Dictionary<string, Type> map = new Dictionary<string, Type>();

        // key: the full name of the interface; value: default value for constructor
        private readonly Dictionary<string, object[]> paramMap = new Dictionary<string, object[]>();

        public void Register<TInterface, TImplementation>(
            string shortName = null,  
            object[] constParams = null
            ) where TImplementation : TInterface
        {
            string key = GetKey(typeof(TInterface).FullName, shortName);
            map.Add(key, typeof(TImplementation));

            // enable constant constructor
            if (constParams != null) paramMap[key] = constParams;
        }

        public TInterface Resolve<TInterface>(string shortName = null)
        {
            Type interfaceType = typeof(TInterface);
            object o = ResolveObject(interfaceType, shortName);
            TInterface instance = (TInterface)o;
            return instance;
        }
        
        private object ResolveObject(Type interfaceType, string shortName)
        {
            string key = GetKey(interfaceType.FullName, shortName);
            if (!map.ContainsKey(key)) throw new Exception();
            Type implementationType = map[key];

            ConstructorInfo[] constructors = implementationType.GetConstructors();
            ConstructorInfo ctor = GetCtorWithAttribute(constructors);

            #region DI by constructor
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
            for(int i = 0; i < parameterInfos.Length; i++)
            {
                ParameterInfo parameterInfo = parameterInfos[i];
                if(constParams != null && parameterInfo.IsDefined(typeof(MyConstParamAttribute)))
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
            return info.GetCustomAttribute<MyShortnameAttribute>().ShortName ?? null;
        }

        private string GetKey(string fullName, string shortName)
        {
            if (string.IsNullOrEmpty(shortName)) return fullName;
            return $"{fullName}_{shortName}";
        }

        
    }
}
