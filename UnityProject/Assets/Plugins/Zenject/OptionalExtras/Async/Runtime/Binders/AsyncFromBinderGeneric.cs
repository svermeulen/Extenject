using System;
using System.Threading.Tasks;
#if EXTENJECT_INCLUDE_ADDRESSABLE_BINDINGS
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
#endif

namespace Zenject
{
    public class AsyncFromBinderGeneric<TContract, TConcrete> : AsyncFromBinderBase where TConcrete : TContract
    {
        public AsyncFromBinderGeneric(
            DiContainer container, BindInfo bindInfo,
                BindStatement bindStatement)
            : base(container, typeof(TContract), bindInfo)
        {
            BindStatement = bindStatement;
        }

        protected BindStatement BindStatement
        {
            get; private set;
        }
        
        protected IBindingFinalizer SubFinalizer
        {
            set { BindStatement.SetFinalizer(value); }
        }

        public AsyncFromBinderBase FromMethod(Func<Task<TConcrete>> method)
        {
            BindInfo.RequireExplicitScope = false;
            // Don't know how it's created so can't assume here that it violates AsSingle
            BindInfo.MarkAsCreationBinding = false;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, originalType) => new AsyncMethodProviderSimple<TContract, TConcrete>(method));

            return this;
        }
        
#if EXTENJECT_INCLUDE_ADDRESSABLE_BINDINGS
        public AsyncFromBinderBase FromAssetReferenceT<TConcreteO>(AssetReferenceT<TConcreteO> reference) where TConcreteO:UnityEngine.Object, TConcrete
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
#endif
    }
}