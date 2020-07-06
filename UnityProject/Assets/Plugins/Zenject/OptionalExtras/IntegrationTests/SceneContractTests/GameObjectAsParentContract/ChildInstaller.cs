namespace Zenject.Tests.GameObjectAsParentContract
{
    public class ChildInstaller : MonoInstaller<ChildInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ChildController>().AsSingle().NonLazy();
        }
    }
}