#if EXTENJECT_INCLUDE_ADDRESSABLE_BINDINGS

namespace Zenject
{
    [NoReflectionBaking]
    public class ConcreteAddressableIdBinderGeneric<TContract> : ConcreteAddressableBinderGeneric<TContract>
    {
        public ConcreteAddressableIdBinderGeneric(
            DiContainer bindContainer, BindInfo bindInfo, 
            BindStatement bindStatement)
            : base(bindContainer, bindInfo, bindStatement)
        {}
        
        public ConcreteAddressableIdBinderGeneric<TContract> WithId(object identifier)
        {
            BindInfo.Identifier = identifier;
            return this;
        }
        
    }
}
#endif