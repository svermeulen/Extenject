namespace Zenject.Tests.GameObjectAsParentContract
{
    public class SessionController
    {
        public bool IsInitialized { get; private set; }
        
        [Inject]
        public void Construct()
        {
            IsInitialized = true;
        }
    }
}