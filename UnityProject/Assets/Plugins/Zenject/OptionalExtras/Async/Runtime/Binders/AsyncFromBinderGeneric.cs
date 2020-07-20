using System;
using System.Threading;
using System.Threading.Tasks;

namespace Zenject
{
    [NoReflectionBaking]
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
        
        public AsyncFromBinderBase FromMethod(Func<CancellationToken, Task<TConcrete>> method)
        {
            BindInfo.RequireExplicitScope = false;
            // Don't know how it's created so can't assume here that it violates AsSingle
            BindInfo.MarkAsCreationBinding = false;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, originalType) => new AsyncMethodProviderSimple<TContract, TConcrete>(method));

            return this;
        }
        
    }
}