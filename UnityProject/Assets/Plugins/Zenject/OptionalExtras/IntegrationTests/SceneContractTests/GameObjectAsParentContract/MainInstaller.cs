using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zenject.Tests.GameObjectAsParentContract
{
    public class MainInstaller : MonoInstaller<MainInstaller>
    {
        [SerializeField] private GameObjectContext sessionContext;
        
        public override void InstallBindings()
        {
            sessionContext.Install(Container);

            SceneManager.LoadScene("ChildScene", LoadSceneMode.Additive);
        }
    }
}