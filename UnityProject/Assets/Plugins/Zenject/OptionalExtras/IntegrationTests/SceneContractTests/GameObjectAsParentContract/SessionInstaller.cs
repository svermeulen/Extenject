namespace Zenject.Tests.GameObjectAsParentContract
{
    public class SessionInstaller : MonoInstaller<MainInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SessionController>().AsSingle();
        }
    }
}