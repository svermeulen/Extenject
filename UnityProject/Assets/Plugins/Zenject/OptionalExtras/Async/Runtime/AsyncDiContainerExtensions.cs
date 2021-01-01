using ModestTree;

namespace Zenject
{
    public static class AsyncDiContainerExtensions
    {
        public static
#if EXTENJECT_INCLUDE_ADDRESSABLE_BINDINGS
            ConcreteAddressableIdBinderGeneric<TContract>
#else
            ConcreteAsyncIdBinderGeneric<TContract> 
#endif
            BindAsync<TContract>(this DiContainer container)
        {
            return BindAsync<TContract>(container, container.StartBinding());
        }

        public static
#if EXTENJECT_INCLUDE_ADDRESSABLE_BINDINGS
            ConcreteAddressableIdBinderGeneric<TContract>
#else
            ConcreteAsyncIdBinderGeneric<TContract> 
#endif
            BindAsync<TContract>(this DiContainer container, BindStatement bindStatement)
        {
            var bindInfo = bindStatement.SpawnBindInfo();

            Assert.That(!typeof(TContract).DerivesFrom<IPlaceholderFactory>(),
                "You should not use Container.BindAsync for factory classes.  Use Container.BindFactory instead.");

            Assert.That(!bindInfo.ContractTypes.Contains(typeof(AsyncInject<TContract>)));
            bindInfo.ContractTypes.Add(typeof(IAsyncInject));
            bindInfo.ContractTypes.Add(typeof(AsyncInject<TContract>));

#if EXTENJECT_INCLUDE_ADDRESSABLE_BINDINGS
            return new ConcreteAddressableIdBinderGeneric<TContract>(container, bindInfo, bindStatement);
#else
            return new ConcreteAsyncIdBinderGeneric<TContract>(container, bindInfo, bindStatement);
#endif
        }
    }
}