using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using ModestTree;

namespace Zenject
{
    [NoReflectionBaking]
    public class AsyncMethodProviderSimple<TContract, TConcrete> : IProvider where TConcrete : TContract
    {
        readonly Func<Task<TConcrete>> _method;
        readonly Func<CancellationToken, Task<TConcrete>> _methodCancellable;

        public AsyncMethodProviderSimple(Func<Task<TConcrete>> method)
        {
            _method = method;
        }
        
        public AsyncMethodProviderSimple(Func<CancellationToken, Task<TConcrete>> method)
        {
            _methodCancellable = method;
        }

        public bool TypeVariesBasedOnMemberType => false;
        public bool IsCached => false;
        public Type GetInstanceType(InjectContext context) => typeof(TConcrete);

        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            Assert.IsEmpty(args);
            Assert.IsNotNull(context);
            
            injectAction = null;

            Func<CancellationToken, Task<TContract>> typeCastAsyncCall = null;
            if (_methodCancellable != null)
            {
                typeCastAsyncCall = async ct =>
                {
                    var task = _methodCancellable(ct);
                    await task;
                    return (TContract) task.Result;
                };
            }else if (_method != null)
            {
                typeCastAsyncCall = async _ =>
                {
                    var task = _method();
                    await task;
                    return (TContract) task.Result;
                };
            }
            Assert.IsNotNull(typeCastAsyncCall);
            
            var asyncInject = new AsyncInject<TContract>(context, typeCastAsyncCall);

            buffer.Add(asyncInject);
        }
    }
}