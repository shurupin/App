namespace App.DataLayer.DbContext
{
    using System;
    using Microsoft.EntityFrameworkCore.Query;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    //https://stackoverflow.com/questions/40476233/how-to-mock-an-async-repository-with-entity-framework-core/40491640#40491640

    internal class AsyncQueryProvider<T> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _queryProvider;

        internal AsyncQueryProvider(IQueryProvider queryProvider)
        {
            this._queryProvider = queryProvider;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new AsyncEnumerable<T>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new AsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return this._queryProvider.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return this._queryProvider.Execute<TResult>(expression);
        }

        TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return this._queryProvider.Execute<TResult>(expression);
        }
    }

    internal class AsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public AsyncEnumerable(IEnumerable<T> enumerable): base(enumerable)
        { }

        public AsyncEnumerable(Expression expression): base(expression)
        { }

        public IAsyncEnumerator<T> GetEnumerator()
        {
            return new AsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new AsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new AsyncQueryProvider<T>(this); }
        }
    }

    internal class AsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public AsyncEnumerator(IEnumerator<T> inner)
        {
            this._inner = inner;
        }

        public T Current
        {
            get
            {
                return this._inner.Current;
            }
        }

        public Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            return Task.FromResult(this._inner.MoveNext());
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(Task.FromResult(this._inner.MoveNext()));
        }

        public ValueTask DisposeAsync()
        {
            try
            {
                this._inner.Dispose();
                return default;
            }
            catch (Exception exception)
            {
                return new ValueTask(Task.FromException(exception));
            }
        }
    }
}
