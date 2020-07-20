#if EXTENJECT_INCLUDE_ADDRESSABLE_BINDINGS
using System;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
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
        
        public AsyncFromBinderBase FromAssetReferenceT<TConcreteObj>(AssetReferenceT<TConcreteObj> reference)
            where TConcreteObj:UnityEngine.Object, TConcrete
        {
            BindInfo.RequireExplicitScope = false;

            var contractType = typeof(TContract);
            if (typeof(UnityEngine.Object).IsAssignableFrom(contractType))
            {
                var addressableInjectType = typeof(AddressableInject<>).MakeGenericType(typeof(TContract));
                BindInfo.ContractTypes.Add(addressableInjectType);
            }
            
            // Don't know how it's created so can't assume here that it violates AsSingle
            BindInfo.MarkAsCreationBinding = false;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, originalType) => new AddressableProviderSimple<TContract, TConcreteObj>(reference));

            return this;
        }

    }
}
#endif