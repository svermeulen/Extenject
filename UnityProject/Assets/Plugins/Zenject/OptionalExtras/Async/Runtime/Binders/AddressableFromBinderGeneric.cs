#if EXTENJECT_INCLUDE_ADDRESSABLE_BINDINGS
using System;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Zenject
{
    [NoReflectionBaking]
    public class AddressableFromBinderGeneric<TContract, TConcrete> : AsyncFromBinderGeneric<TContract, TConcrete>
        where TConcrete : TContract
    {
        public AddressableFromBinderGeneric(
            DiContainer container, BindInfo bindInfo,
            BindStatement bindStatement)
            : base(container, bindInfo, bindStatement)
        {}
        
        public AsyncFromBinderBase FromAssetReferenceT<TConcreteObj>(AssetReferenceT<TConcreteObj> reference) where TConcreteObj:UnityEngine.Object, TConcrete
        {
            Func<Task<TConcrete>> addressableLoadDelegate = async () =>
            {
                AsyncOperationHandle<TConcrete> loadHandle = Addressables.LoadAssetAsync<TConcrete>(reference);
                await loadHandle.Task;
                return loadHandle.Result;
            }; 
        
            BindInfo.RequireExplicitScope = false;
            // Don't know how it's created so can't assume here that it violates AsSingle
            BindInfo.MarkAsCreationBinding = false;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, originalType) => new AsyncMethodProviderSimple<TContract, TConcrete>(addressableLoadDelegate));

            return this;
        }

    }
}
#endif