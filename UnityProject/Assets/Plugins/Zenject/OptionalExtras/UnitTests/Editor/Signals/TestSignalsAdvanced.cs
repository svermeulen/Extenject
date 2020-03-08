using System;
using NUnit.Framework;
using Assert = ModestTree.Assert;

namespace Zenject.Tests.Signals
{
    [TestFixture]
    public class TestSignalsAdvanced : ZenjectUnitTestFixture
    {
        [SetUp]
        public void CommonInstall()
        {
            ZenjectManagersInstaller.Install(Container);
            SignalBusInstaller.Install(Container);
            Container.Inject(this);
        }

        [Test]
        public void TestSubscribeDeterministicOrder()
        {
            Container.DeclareSignal<FooSignal>();

            var signalBus = Container.Resolve<SignalBus>();

            int count = 1;

            int order1 = 0;
            Action handler1 = () => order1 = count++;

            int order2 = 0;
            Action handler2 = () => order2 = count++;

            int order3 = 0;
            Action handler3 = () => order3 = count++;

            signalBus.Subscribe<FooSignal>(handler1);
            signalBus.Subscribe<FooSignal>(handler2);
            signalBus.Subscribe<FooSignal>(handler3);

            signalBus.Fire<FooSignal>();

            Assert.IsEqual(order1, 1);
            Assert.IsEqual(order2, 2);
            Assert.IsEqual(order3, 3);
        }

        [Test]
        public void TestSubscribeUnsubscribeInsideHandler()
        {
            Container.DeclareSignal<FooSignal>();

            var signalBus = Container.Resolve<SignalBus>();

            bool received = false;

            Action handler2 = () => received = true;

            Action handler = () =>
            {
                signalBus.Subscribe<FooSignal>(handler2);
            };

            Action handler3 = () =>
            {
                signalBus.Unsubscribe<FooSignal>(handler2);
            };

            signalBus.Subscribe<FooSignal>(handler);

            Assert.That(!received);
            signalBus.Fire<FooSignal>();
            Assert.That(!received);
            // handler2 is subscribed now

            signalBus.Unsubscribe<FooSignal>(handler);

            Assert.That(!received);
            signalBus.Fire<FooSignal>();
            Assert.That(received);
            received = false;

            signalBus.Subscribe<FooSignal>(handler3);
            Assert.That(!received);
            signalBus.Fire<FooSignal>();
            // Should be called before handler 3 so should receive it
            Assert.That(received);
            received = false;
            signalBus.Unsubscribe<FooSignal>(handler3);

            Assert.That(!received);
            signalBus.Fire<FooSignal>();
            Assert.That(!received);
            received = false;

            Assert.IsEqual(signalBus.NumSubscribers, 0);

            // Now test unsubscribing ourself in our own handler

            Action handler4 = null;
            handler4 = () =>
            {
                received = true;
                signalBus.Unsubscribe<FooSignal>(handler4);
            };
            signalBus.Subscribe<FooSignal>(handler4);

            Assert.That(!received);
            signalBus.Fire<FooSignal>();
            Assert.That(received);
            received = false;

            signalBus.Fire<FooSignal>();
            Assert.That(!received);
        }

        [Test]
        public void TestSubcontainers1()
        {
            Container.DeclareSignal<FooSignal>();

            var signalBus1 = Container.Resolve<SignalBus>();

            var subContainer = Container.CreateSubContainer();

            var signalBus2 = subContainer.Resolve<SignalBus>();

            bool received = false;
            Action callback = () => received = true;

            signalBus2.Subscribe<FooSignal>(callback);

            Assert.That(!received);
            signalBus1.Fire<FooSignal>();
            Assert.That(received);

            subContainer.Resolve<DisposableManager>().LateDispose();

            // Signal should unregister automatically when the subcontainer is disposed
            received = false;
            signalBus1.Fire<FooSignal>();
            Assert.That(!received);
        }

        [Test]
        public void TestSignalDeclarationSettingsRequireHandlerMissing()
        {
            Container.DeclareSignal<FooSignal>();

            var signalBus = Container.Resolve<SignalBus>();

            signalBus.Fire<FooSignal>();
        }

        [Test]
        public void TestSignalDeclarationSettingsRequireHandler()
        {
            Container.DeclareSignal<FooSignal>().RequireSubscriber();

            var signalBus = Container.Resolve<SignalBus>();

            Assert.Throws(() => signalBus.Fire<FooSignal>());
        }

        [Test]
        public void TestSignalDeclarationSettingsRunAsync1()
        {
            Container.DeclareSignal<FooSignal>().RunAsync();
            Container.ResolveRoots();
            Container.Resolve<InitializableManager>().Initialize();

            var signalBus = Container.Resolve<SignalBus>();

            bool received = false;
            signalBus.Subscribe<FooSignal>(() => received = true);

            Assert.That(!received);
            signalBus.Fire<FooSignal>();
            Assert.That(!received);

            Container.Resolve<TickableManager>().Update();
            Assert.That(received);
        }

        [Test]
        public void TestIsDeclared1()
        {
            Container.DeclareSignal<FooSignal>();
            Container.ResolveRoots();

            var signalBus = Container.Resolve<SignalBus>();
            Assert.That(signalBus.IsSignalDeclared<FooSignal>());
        }

        [Test]
        public void TestIsDeclared2()
        {
            Container.ResolveRoots();

            var signalBus = Container.Resolve<SignalBus>();
            Assert.That(!signalBus.IsSignalDeclared<FooSignal>());
        }

        [Test]
        public void TestSignalDeclarationSettingsRunAsync2()
        {
            Container.DeclareSignal<FooSignal>().RunAsync();
            Container.ResolveRoots();
            Container.Resolve<InitializableManager>().Initialize();

            var signalBus = Container.Resolve<SignalBus>();

            int callCount = 0;

            Action handler = () =>
            {
                callCount++;
                signalBus.Fire<FooSignal>();
            };

            signalBus.Subscribe<FooSignal>(handler);

            Assert.IsEqual(callCount, 0);
            signalBus.Fire<FooSignal>();
            Assert.IsEqual(callCount, 0);

            Container.Resolve<TickableManager>().Update();
            Assert.IsEqual(callCount, 1);
            Container.Resolve<TickableManager>().Update();
            Assert.IsEqual(callCount, 2);
        }

        [Test]
        public void TestAbstractFireReceivedOnSubscribe()
        {
            Container.DeclareSignalWithInterfaces<FizzSignal>();
            var signalBus = Container.Resolve<SignalBus>();

            int callInterfaceCount = 0;
            signalBus.Subscribe<IFizzSignal>((() => callInterfaceCount++));
            int callConcreteCount = 0;
            signalBus.Subscribe<FizzSignal>((() => callConcreteCount++));
            
            signalBus.AbstractFire<FizzSignal>();
            Assert.IsEqual(callInterfaceCount, 1);
            Assert.IsEqual(callConcreteCount, 1);
        }

        public interface IFizzSignal { }
        public class FizzSignal : IFizzSignal
        {            
        }

        public class FooSignal
        {
        }

        public class BarSignal
        {
            public string Value;
        }
    }
}

