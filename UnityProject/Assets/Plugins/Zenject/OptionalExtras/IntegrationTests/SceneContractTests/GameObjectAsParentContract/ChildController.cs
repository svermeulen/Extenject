using ModestTree;

namespace Zenject.Tests.GameObjectAsParentContract
{
    public class ChildController
    {
        [Inject] private SessionController _sessionController;

        [Inject]
        public void Construct()
        {
            Assert.That(_sessionController.IsInitialized);
            
            Log.Trace("Initialized successfully");
        }
    }
}