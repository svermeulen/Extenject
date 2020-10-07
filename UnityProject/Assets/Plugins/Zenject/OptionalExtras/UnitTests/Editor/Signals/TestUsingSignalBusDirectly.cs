using System;
using NUnit.Framework;
using Assert = ModestTree.Assert;

namespace Zenject.Tests.Signals
{
    [TestFixture]
    public class TestUsingSignalBusDirectly : ZenjectUnitTestFixture
    {
        [SetUp]
        public void CommonInstall()
        {
            ZenjectManagersInstaller.Install(Container);
            SignalBusInstaller.Install(Container);
            Container.Inject(this);
        }

        [Inject]
        SignalBus _signalBus = null;

        [Test]
        public void TestDeclareSignal()
        {
            _signalBus.DeclareSignal<FooSignal>();

            bool receivedResponse = false;

            _signalBus.Subscribe<FooSignal>(() => 
            {
                receivedResponse = true;
            });

            Assert.That(!receivedResponse);

            _signalBus.Fire<FooSignal>();

            Assert.That(receivedResponse);
        }

        [Test]
        public void TestDeclareSignalMissing()
        {
            _signalBus.DeclareSignal<FooSignal>();
            _signalBus.Fire<FooSignal>();
        }

        [Test]
        public void TestDeclareSignalMissingButRequired()
        {
            _signalBus.DeclareSignal<FooSignal>(null, SignalMissingHandlerResponses.Throw);
            Assert.Throws<ZenjectException>(() => _signalBus.Fire<FooSignal>());
        }

        public class FooSignal
        {
        }
    }
}
