#if EXTENJECT_INCLUDE_ADDRESSABLE_BINDINGS
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ModestTree;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Zenject
{
    [NoReflectionBaking]
    public class AddressableProviderSimple<TContract, TConcrete> : IProvider where TConcrete : UnityEngine.Object, TContract
    {
        private AssetReferenceT<TConcrete> assetReference;

        public AddressableProviderSimple(AssetReferenceT<TConcrete> assetReference)
        {
            this.assetReference = assetReference;
        }
        
        public bool TypeVariesBasedOnMemberType => false;
        public bool IsCached => false;
        public Type GetInstanceType(InjectContext context) => typeof(TConcrete);

        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            Assert.IsEmpty(args);
            Assert.IsNotNull(context);
            
            injectAction = null;

            Func<CancellationToken, Task<AsyncOperationHandle<TConcrete>>> addressableLoadDelegate = async (_) =>
            {
                AsyncOperationHandle<TConcrete> loadHandle = Addressables.LoadAssetAsync<TConcrete>(assetReference);
                await loadHandle.Task;
                
                if (loadHandle.Status == AsyncOperationStatus.Failed)
                {
                    throw new Exception("Async operation failed", loadHandle.OperationException);
                }
                
                return  loadHandle;
            }; 
            
            var asyncInject = new AddressableInject<TConcrete>(context, addressableLoadDelegate);

            buffer.Add(asyncInject);
        }
    }
}
#endif