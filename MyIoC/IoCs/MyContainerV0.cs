using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MyIoC.IoCs
{
    public class MyContainerV0 : IMyContainerV0
    {
        // key: the full name of the interface; value: type of of implementation
        private readonly Dictionary<string, Type> map = new Dictionary<string, Type>();

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            string key = typeof(TInterface).FullName;
            map.Add(key, typeof(TImplementation));
        }

        public TInterface Resolve<TInterface>()
        {
            string key = typeof(TInterface).FullName;
            if (!map.ContainsKey(key)) throw new Exception();
            Type type = map[key];

            #region prepare constructor parameter
            List<object> parameters = new List<object>();

            ConstructorInfo ctor = type.GetConstructors()[0]; // get the 1st one for now

            foreach (ParameterInfo parameter in ctor.GetParameters())
            {
                Type paraType = parameter.ParameterType; // don't use typeof() here, it will return ParameterInfo
                string paraKey = paraType.FullName;
                if (!map.ContainsKey(paraKey)) throw new Exception();
                Type paraTargetType = map[paraKey];

                object pObject = Activator.CreateInstance(paraTargetType);
                parameters.Add(pObject);
            }
            #endregion

            object o = Activator.CreateInstance(type, parameters.ToArray());
            TInterface t = (TInterface)o;
            return t;
        }

    }
}
