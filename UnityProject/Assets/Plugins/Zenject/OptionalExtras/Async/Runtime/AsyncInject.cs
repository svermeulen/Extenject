using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ModestTree;

namespace Zenject
{
    public interface AsyncInject
    {
        bool HasResult { get; }
        bool IsCancelled  { get; }
        bool IsFaulted  { get; }
        bool IsCompleted { get; }
        
        TaskAwaiter GetAwaiter();
    }


    [ZenjectAllowDuringValidation]
    [NoReflectionBaking]
    public class AsyncInject<T> : AsyncInject
    {
        readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        readonly InjectContext _context;

        public event Action<T> Completed;
        public event Action<AggregateException>  Faulted;
        public event Action Cancelled;

        public bool HasResult { get; protected set; }
        public bool IsCancelled  { get; protected set; }
        public bool IsFaulted  { get; protected set; }

        public bool IsCompleted => HasResult || IsCancelled || IsFaulted;
        
        T _result;
        Task<T> task;
        
        public AsyncInject(InjectContext context, Func<CancellationToken, Task<T>> asyncMethod)
        {
            _context = context;

            StartAsync(asyncMethod, cancellationTokenSource.Token);
        }

        public void Cancel()
        {
            cancellationTokenSource.Cancel();
        }
        
        private async void StartAsync(Func<CancellationToken, Task<T>> asyncMethod, CancellationToken token)
        {
            task = asyncMethod(token);
            await task;
            
            if (token.IsCancellationRequested)
            {
                return;
            }
            
            if (task.IsCompleted)
            {
                _result = task.Result;
                HasResult = true;
                Completed?.Invoke(task.Result);
            }else if (task.IsCanceled)
            {
                IsCancelled = true;
                Cancelled?.Invoke();
            }else if (task.IsFaulted)
            {
                IsFaulted = true;
                Faulted?.Invoke(task.Exception);
            }
        }

        public bool TryGetResult(out T result)
        {
            if (HasResult)
            {
                result = _result;
                return true;
            }
            result = default;
            return false;
        }

        public T Result
        {
            get
            {
                Assert.That(HasResult, "AsyncInject does not have a result.  ");
                return _result;
            }
        }
        
        public TaskAwaiter<T> GetAwaiter() => task.GetAwaiter();

        TaskAwaiter AsyncInject.GetAwaiter() => task.ContinueWith(task => { }).GetAwaiter();
    }
}