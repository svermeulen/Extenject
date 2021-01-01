using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ModestTree;

namespace Zenject
{
    public interface IAsyncInject
    {
        bool HasResult { get; }
        bool IsCancelled  { get; }
        bool IsFaulted  { get; }
        bool IsCompleted { get; }
        
        TaskAwaiter GetAwaiter();
    }


    [ZenjectAllowDuringValidation]
    [NoReflectionBaking]
    public class AsyncInject<T> : IAsyncInject
    {
        protected readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        protected readonly InjectContext _context;

        public event Action<T> Completed;
        public event Action<AggregateException>  Faulted;
        public event Action Cancelled;

        public bool HasResult { get; protected set; }
        public bool IsSuccessful { get; protected set; }
        public bool IsCancelled  { get; protected set; }
        public bool IsFaulted  { get; protected set; }

        public bool IsCompleted => IsSuccessful || IsCancelled || IsFaulted;
        
        T _result;
        Task<T> task;
        
        protected AsyncInject(InjectContext context)
        {
            _context = context;
        }
        
        public AsyncInject(InjectContext context, Func<CancellationToken, Task<T>> asyncMethod)
        {
            _context = context;

            StartAsync(asyncMethod, cancellationTokenSource.Token);
        }

        public void Cancel()
        {
            cancellationTokenSource.Cancel();
        }
        
        protected async void StartAsync(Func<CancellationToken, Task<T>> asyncMethod, CancellationToken token)
        {
            try
            {
                task = asyncMethod(token);
                await task;
            }
            catch (AggregateException e)
            {
                HandleFaulted(e);
                return;
            }
            catch (Exception e)
            {
                HandleFaulted(new AggregateException(e));
                return;
            }

            if (token.IsCancellationRequested)
            {
                HandleCancelled();
                return;
            }
            
            if (task.IsCompleted)
            {
                HandleCompleted(task.Result);
            }else if (task.IsCanceled)
            {
                HandleCancelled();
            }else if (task.IsFaulted)
            {
                HandleFaulted(task.Exception);
            }
        }

        private void HandleCompleted(T result)
        {
            _result = result;
            HasResult = !result.Equals(default(T));
            IsSuccessful = true;
            Completed?.Invoke(result);
        }

        private void HandleCancelled()
        {
            IsCancelled = true;
            Cancelled?.Invoke();
        }

        private void HandleFaulted(AggregateException exception)
        {
            IsFaulted = true;
            Faulted?.Invoke(exception);
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

        TaskAwaiter IAsyncInject.GetAwaiter() => task.ContinueWith(task => { }).GetAwaiter();
    }
}