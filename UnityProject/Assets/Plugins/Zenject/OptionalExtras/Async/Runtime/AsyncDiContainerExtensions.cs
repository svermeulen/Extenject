using ModestTree;

namespace Zenject
{
    public static class AsyncDiContainerExtensions
    {
        public static ConcreteAsyncIdBinderGeneric<TContract> BindAsync<TContract>(this DiContainer container)
        {
            return BindAsync<TContract>(container, container.StartBinding());
        }

        public static ConcreteAsyncIdBinderGeneric<TContract> BindAsync<TContract>(this DiContainer container, BindStatement bindStatement)
        {
            var bindInfo = bindStatement.SpawnBindInfo();

            Assert.That(!typeof(TContract).DerivesFrom<IPlaceholderFactory>(),
                "You should not use Container.BindAsync for factory classes.  Use Container.BindFactory instead.");

            Assert.That(!bindInfo.ContractTypes.Contains(typeof(AsyncInject<TContract>)));
            bindInfo.ContractTypes.Add(typeof(AsyncInject));
            bindInfo.ContractTypes.Add(typeof(AsyncInject<TContract>));
            
            return new ConcreteAsyncIdBinderGeneric<TContract>(
                container, bindInfo, bindStatement);
        }
    }
}