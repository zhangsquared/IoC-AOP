using System;
using System.Collections.Generic;
using System.Text;

namespace MyIoC.IoCs
{
    /// <summary>
    /// 1. multiple paramters in constructor
    /// recursively init object
    /// 
    /// 2. multiple constructors (IServiceCollection will choose the super constructor, with the most parameters)
    /// use Attribute to mark the desired constructor, e.g. BService
    /// 
    /// 3. DI by property, DI by method (IServiceCollection only support constructor DI)
    /// make property attribute or method attribute
    /// 
    /// 4. how to deal with 1 interface with several implementations?
    /// in Register step, modify key (e.g. MongoDAL)
    /// in Resolve, use extra parameter to get the corresponding instance (shortName)
    /// [MyShortnameAttribute]
    /// 
    /// 5. how to pass a const value to constuctor?
    /// IServiceCollection use options to solve this problem
    /// in Register stop, store the constant parameter in this dictionary (constParams) e.g. IntService
    /// in Resolve, pass const parms into constructor
    /// 
    /// 
    /// IoC is a design pattern
    /// DI is an implementation of IoC
    /// </summary>
    public interface IMyContainerV1
    {
        void Register<TInterface, TImplementation>(string shortName = null, object[] constParams = null) 
            where TImplementation : TInterface;

        TInterface Resolve<TInterface>(string shortName = null);
    }

}
