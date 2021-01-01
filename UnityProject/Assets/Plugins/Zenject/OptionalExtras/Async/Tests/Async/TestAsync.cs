using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
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

            var asycFoo = Container.Resolve<IAsyncInject>();
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

            awaitReturn = null;
            var asycFoo = Container.Resolve<AsyncInject<IFoo>>();

            TestAwait(asycFoo);

            while (awaitReturn == null)
            {
                yield return null;
            }
            Assert.Pass();
        }

        [UnityTest]
        [Timeout(10500)]
        public IEnumerator TestPreloading()
        {
            PreInstall();
            for (int i = 0; i < 4; i++)
            {
                var index = i;
                Container.BindAsync<IFoo>().FromMethod(async () =>
                {
                    var delayAmount = 100 * (4 - index);
                    await Task.Delay(delayAmount);
                    return (IFoo) new Foo();
                }).AsCached();
            }
            
            Container.BindInterfacesTo<DecoratableMonoKernel>().AsCached();
            Container.Decorate<IDecoratableMonoKernel>()
                .With<PreloadAsyncKernel>();
            
            PostInstall();

            var testKernel = Container.Resolve<IDecoratableMonoKernel>() as PreloadAsyncKernel;
            while (!testKernel.IsPreloadCompleted)
            {
                yield return null;
            }

            foreach (var asycFooUntyped in testKernel.asyncInjects)
            {
                var asycFoo = asycFooUntyped as AsyncInject<IFoo>;
                if (asycFoo.TryGetResult(out IFoo fooAfterLoad))
                {
                    Assert.NotNull(fooAfterLoad);
                    yield break;
                }
            }
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
        
        public class PreloadAsyncKernel: BaseMonoKernelDecorator
        {
            [Inject]
            public List<IAsyncInject> asyncInjects;

            public bool IsPreloadCompleted { get; private set; }

            public async override void Initialize()
            {
                foreach (IAsyncInject inject in asyncInjects)
                {
                    if (!inject.IsCompleted)
                    {
                        await Task.Delay(1);
                    }
                }

                IsPreloadCompleted = true;
                DecoratedMonoKernel.Initialize();
            }

            public override void Update()
            {
                if (!IsPreloadCompleted)
                {
                    return;
                }
                DecoratedMonoKernel.Update();
            }

            public override void FixedUpdate()
            {
                if (!IsPreloadCompleted)
                {
                    
                }
                DecoratedMonoKernel.FixedUpdate();
            }
        }
    }

}