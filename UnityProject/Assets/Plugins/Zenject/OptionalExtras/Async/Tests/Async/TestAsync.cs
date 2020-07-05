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
        
        public interface IFoo
        {
        
        }
    
        public class Foo : IFoo
        {
        
        }
    }
}