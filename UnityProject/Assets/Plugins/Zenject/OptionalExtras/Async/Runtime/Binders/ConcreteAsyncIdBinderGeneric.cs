namespace Zenject
{
    [NoReflectionBaking]
    public class ConcreteAsyncIdBinderGeneric<TContract> : ConcreteAsyncBinderGeneric<TContract>
    {
        public ConcreteAsyncIdBinderGeneric(
            DiContainer bindContainer, BindInfo bindInfo,
            BindStatement bindStatement)
            : base(bindContainer, bindInfo, bindStatement)
        {
        }

        public ConcreteAsyncBinderGeneric<TContract> WithId(object identifier)
        {
            BindInfo.Identifier = identifier;
            return this;
        }
    }
}