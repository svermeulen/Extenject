using System.Collections;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Zenject.Tests.Bindings
{
    public class TestAsync : ZenjectIntegrationTestFixture
    {
        [UnityTest]
        public IEnumerator TestSimpleMethod()
        {
            PreInstall();

            Container.BindAsync<IFoo>().FromMethod(async () =>
            {
                await Task.Delay(100);
                return (IFoo) new Foo();
            }).AsCached();
            PostInstall();

            var asycFoo = Container.Resolve<AsyncInject<IFoo>>();
            
            while (!asycFoo.HasResult)
            {
                yield return null;
            }
            
            if (asycFoo.TryGetResult(out IFoo fooAfterLoad))
            {
                Assert.NotNull(fooAfterLoad);
                yield break;
            }
            Assert.Fail();
        }
        
        [UnityTest]
        public IEnumerator TestUntypedInject()
        {
            PreInstall();

            Container.BindAsync<IFoo>().FromMethod(async () =>
            {
                await Task.Delay(100);
                return (IFoo) new Foo();
            }).AsCached();
            PostInstall();

            var asycFoo = Container.Resolve<AsyncInject>();
            yield return null;
            
            Assert.NotNull(asycFoo);
        }
        

        private IFoo awaitReturn;
        [UnityTest]
        [Timeout(300)]
        public IEnumerator TestSimpleMethodAwaitable()
        {
            PreInstall();

            Container.BindAsync<IFoo>().FromMethod(async () =>
            {
                await Task.Delay(100);
                return (IFoo) new Foo();
            }).AsCached();
            PostInstall();

            var asycFoo = Container.Resolve<AsyncInject<IFoo>>();

            awaitReturn = null;
            TestAwait(asycFoo);

            while (awaitReturn == null)
            {
                yield return null;
            }
            Assert.Pass();
        }
        
        private async void TestAwait(AsyncInject<IFoo> asycFoo)
        {
            awaitReturn = await asycFoo;
        }

        public interface IFoo
        {
        
        }
    
        public class Foo : IFoo
        {
        
        }
    }
}