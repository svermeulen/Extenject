using System.Collections;
using ModestTree;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject.Tests.Factories.BindFactoryOne;

namespace Zenject.Tests.Factories
{
    public class TestBindFactoryOneWithArguments : ZenjectIntegrationTestFixture
    {
        private const string ArgumentValue = "asdf";

        GameObject FooPrefab
        {
            get
            {
                return FixtureUtil.GetPrefab("TestBindFactoryOne/Foo");
            }
        }

        [UnityTest]
        public IEnumerator TestFromNewComponentOnNewGameObjectSelf()
        {
            PreInstall();
            Container.BindIFactory<Foo>()
                .FromNewComponentOnNewGameObject()
                .WithArguments(ArgumentValue);

            AddFactoryUser<Foo>();

            PostInstall();

            FixtureUtil.AssertComponentCount<Foo>(1);
            FixtureUtil.AssertNumGameObjects(1);
            yield break;
        }

        [UnityTest]
        public IEnumerator TestFromNewComponentOnNewGameObjectConcrete()
        {
            PreInstall();
            Container.BindIFactory<IFoo>()
                .To<Foo>()
                .FromNewComponentOnNewGameObject()
                .WithArguments(ArgumentValue);

            AddFactoryUser<IFoo>();

            PostInstall();

            FixtureUtil.AssertComponentCount<Foo>(1);
            FixtureUtil.AssertNumGameObjects(1);
            yield break;
        }

        [UnityTest]
        public IEnumerator TestFromComponentInNewPrefabSelf()
        {
            PreInstall();
            Container.BindIFactory<Foo>()
                .FromComponentInNewPrefab(FooPrefab)
                .WithGameObjectName("asdf")
                .WithArguments(ArgumentValue);

            AddFactoryUser<Foo>();

            PostInstall();

            FixtureUtil.AssertComponentCount<Foo>(1);
            FixtureUtil.AssertNumGameObjects(1);
            FixtureUtil.AssertNumGameObjectsWithName("asdf", 1);
            yield break;
        }

        [UnityTest]
        public IEnumerator TestFromComponentInNewPrefabConcrete()
        {
            PreInstall();
            Container.BindIFactory<IFoo>()
                .To<Foo>()
                .FromComponentInNewPrefab(FooPrefab)
                .WithGameObjectName("asdf")
                .WithArguments(ArgumentValue);

            AddFactoryUser<IFoo>();

            PostInstall();

            FixtureUtil.AssertComponentCount<Foo>(1);
            FixtureUtil.AssertNumGameObjects(1);
            FixtureUtil.AssertNumGameObjectsWithName("asdf", 1);
            yield break;
        }

        [UnityTest]
        public IEnumerator TestFromComponentInNewPrefabResourceSelf()
        {
            PreInstall();
            Container.BindIFactory<Foo>()
                .FromComponentInNewPrefabResource("TestBindFactoryOne/Foo")
                .WithGameObjectName("asdf")
                .WithArguments(ArgumentValue);

            AddFactoryUser<Foo>();

            PostInstall();

            FixtureUtil.AssertComponentCount<Foo>(1);
            FixtureUtil.AssertNumGameObjects(1);
            FixtureUtil.AssertNumGameObjectsWithName("asdf", 1);
            yield break;
        }

        [UnityTest]
        public IEnumerator TestFromComponentInNewPrefabResourceConcrete()
        {
            PreInstall();
            Container.BindIFactory<IFoo>().To<Foo>()
                .FromComponentInNewPrefabResource("TestBindFactoryOne/Foo")
                .WithGameObjectName("asdf")
                .WithArguments(ArgumentValue);

            AddFactoryUser<IFoo>();

            PostInstall();

            FixtureUtil.AssertComponentCount<Foo>(1);
            FixtureUtil.AssertNumGameObjects(1);
            FixtureUtil.AssertNumGameObjectsWithName("asdf", 1);
            yield break;
        }

        // Note that unlike the TestBindFactory tests, WithArguments still doesn't work nicely with subcontainers...

        void AddFactoryUser<TValue>()
            where TValue : IFoo
        {
            Container.Bind<IInitializable>()
                .To<FooFactoryTester<TValue>>().AsSingle();

            Container.BindExecutionOrder<FooFactoryTester<TValue>>(-100);
        }

        public class FooFactoryTester<TValue> : IInitializable
            where TValue : IFoo
        {
            readonly IFactory<TValue> _factory;

            public FooFactoryTester(IFactory<TValue> factory)
            {
                _factory = factory;
            }

            public void Initialize()
            {
                Assert.IsEqual(_factory.Create().Value, ArgumentValue);

                Log.Info("Factory created foo successfully");
            }
        }
    }
}

