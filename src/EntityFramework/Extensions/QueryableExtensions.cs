// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Query;
using Microsoft.Data.Entity.Utilities;

// ReSharper disable once CheckNamespace

namespace System.Linq
{
    public static class QueryableExtensions
    {
        private static readonly MethodInfo _any = GetMethod("Any");

        public static Task<bool> AnyAsync<TSource>(
            [NotNull] this IQueryable<TSource> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.NotNull(source, "source");

            cancellationToken.ThrowIfCancellationRequested();

            var provider = source.Provider as IAsyncQueryProvider;

            if (provider != null)
            {
                return provider.ExecuteAsync<bool>(
                    Expression.Call(
                        null,
                        _any.MakeGenericMethod(typeof(TSource)),
                        new[] { source.Expression }),
                    cancellationToken);
            }

            throw new InvalidOperationException(Strings.FormatIQueryableProviderNotAsync());
        }

        private static readonly MethodInfo _anyPredicate = GetMethod("Any", 1);

        public static Task<bool> AnyAsync<TSource>(
            [NotNull] this IQueryable<TSource> source,
            [NotNull] Expression<Func<TSource, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.NotNull(source, "source");
            Check.NotNull(predicate, "predicate");

            cancellationToken.ThrowIfCancellationRequested();

            var provider = source.Provider as IAsyncQueryProvider;

            if (provider != null)
            {
                return provider.ExecuteAsync<bool>(
                    Expression.Call(
                        null,
                        _anyPredicate.MakeGenericMethod(typeof(TSource)),
                        new[] { source.Expression, Expression.Quote(predicate) }
                        ),
                    cancellationToken);
            }

            throw new InvalidOperationException(Strings.FormatIQueryableProviderNotAsync());
        }

        private static readonly MethodInfo _count = GetMethod("Count");

        public static Task<int> CountAsync<TSource>(
            [NotNull] this IQueryable<TSource> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.NotNull(source, "source");

            cancellationToken.ThrowIfCancellationRequested();

            var provider = source.Provider as IAsyncQueryProvider;

            if (provider != null)
            {
                return provider.ExecuteAsync<int>(
                    Expression.Call(
                        null,
                        _count.MakeGenericMethod(typeof(TSource)),
                        new[] { source.Expression }),
                    cancellationToken);
            }

            throw new InvalidOperationException(Strings.FormatIQueryableProviderNotAsync());
        }

        private static readonly MethodInfo _first = GetMethod("First");

        public static Task<TSource> FirstAsync<TSource>(
            [NotNull] this IQueryable<TSource> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.NotNull(source, "source");

            cancellationToken.ThrowIfCancellationRequested();

            var provider = source.Provider as IAsyncQueryProvider;

            if (provider != null)
            {
                return provider.ExecuteAsync<TSource>(
                    Expression.Call(
                        null,
                        _first.MakeGenericMethod(typeof(TSource)),
                        new[] { source.Expression }),
                    cancellationToken);
            }

            throw new InvalidOperationException(Strings.FormatIQueryableProviderNotAsync());
        }

        private static readonly MethodInfo _firstPredicate = GetMethod("First", 1);

        public static Task<TSource> FirstAsync<TSource>(
            [NotNull] this IQueryable<TSource> source,
            [NotNull] Expression<Func<TSource, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.NotNull(source, "source");
            Check.NotNull(predicate, "predicate");

            cancellationToken.ThrowIfCancellationRequested();

            var provider = source.Provider as IAsyncQueryProvider;

            if (provider != null)
            {
                return provider.ExecuteAsync<TSource>(
                    Expression.Call(
                        null,
                        _firstPredicate.MakeGenericMethod(typeof(TSource)),
                        new[] { source.Expression, Expression.Quote(predicate) }
                        ),
                    cancellationToken);
            }

            throw new InvalidOperationException(Strings.FormatIQueryableProviderNotAsync());
        }

        private static readonly MethodInfo _firstOrDefault
            = GetMethod("FirstOrDefault");

        public static Task<TSource> FirstOrDefaultAsync<TSource>(
            [NotNull] this IQueryable<TSource> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.NotNull(source, "source");

            cancellationToken.ThrowIfCancellationRequested();

            var provider = source.Provider as IAsyncQueryProvider;

            if (provider != null)
            {
                return provider.ExecuteAsync<TSource>(
                    Expression.Call(
                        null,
                        _firstOrDefault.MakeGenericMethod(typeof(TSource)),
                        new[] { source.Expression }
                        ),
                    cancellationToken);
            }

            throw new InvalidOperationException(Strings.FormatIQueryableProviderNotAsync());
        }

        private static readonly MethodInfo _firstOrDefaultPredicate = GetMethod("FirstOrDefault", 1);

        public static Task<TSource> FirstOrDefaultAsync<TSource>(
            [NotNull] this IQueryable<TSource> source,
            [NotNull] Expression<Func<TSource, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.NotNull(source, "source");
            Check.NotNull(predicate, "predicate");

            cancellationToken.ThrowIfCancellationRequested();

            var provider = source.Provider as IAsyncQueryProvider;

            if (provider != null)
            {
                return provider.ExecuteAsync<TSource>(
                    Expression.Call(
                        null,
                        _firstOrDefaultPredicate.MakeGenericMethod(typeof(TSource)),
                        new[] { source.Expression, Expression.Quote(predicate) }
                        ),
                    cancellationToken);
            }

            throw new InvalidOperationException(Strings.FormatIQueryableProviderNotAsync());
        }

        private static readonly MethodInfo _single = GetMethod("Single");

        public static Task<TSource> SingleAsync<TSource>(
            [NotNull] this IQueryable<TSource> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.NotNull(source, "source");

            cancellationToken.ThrowIfCancellationRequested();

            var provider = source.Provider as IAsyncQueryProvider;

            if (provider != null)
            {
                return provider.ExecuteAsync<TSource>(
                    Expression.Call(
                        null,
                        _single.MakeGenericMethod(typeof(TSource)),
                        new[] { source.Expression }),
                    cancellationToken);
            }

            throw new InvalidOperationException(Strings.FormatIQueryableProviderNotAsync());
        }

        private static readonly MethodInfo _singlePredicate = GetMethod("Single", 1);

        public static Task<TSource> SingleAsync<TSource>(
            [NotNull] this IQueryable<TSource> source,
            [NotNull] Expression<Func<TSource, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.NotNull(source, "source");
            Check.NotNull(predicate, "predicate");

            cancellationToken.ThrowIfCancellationRequested();

            var provider = source.Provider as IAsyncQueryProvider;

            if (provider != null)
            {
                return provider.ExecuteAsync<TSource>(
                    Expression.Call(
                        null,
                        _singlePredicate.MakeGenericMethod(typeof(TSource)),
                        new[] { source.Expression, Expression.Quote(predicate) }
                        ),
                    cancellationToken);
            }

            throw new InvalidOperationException(Strings.FormatIQueryableProviderNotAsync());
        }

        private static readonly MethodInfo _singleOrDefault
            = GetMethod("SingleOrDefault");

        public static Task<TSource> SingleOrDefaultAsync<TSource>(
            [NotNull] this IQueryable<TSource> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.NotNull(source, "source");

            cancellationToken.ThrowIfCancellationRequested();

            var provider = source.Provider as IAsyncQueryProvider;

            if (provider != null)
            {
                return provider.ExecuteAsync<TSource>(
                    Expression.Call(
                        null,
                        _singleOrDefault.MakeGenericMethod(typeof(TSource)),
                        new[] { source.Expression }
                        ),
                    cancellationToken);
            }

            throw new InvalidOperationException(Strings.FormatIQueryableProviderNotAsync());
        }

        private static readonly MethodInfo _singleOrDefaultPredicate = GetMethod("SingleOrDefault", 1);

        public static Task<TSource> SingleOrDefaultAsync<TSource>(
            [NotNull] this IQueryable<TSource> source,
            [NotNull] Expression<Func<TSource, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.NotNull(source, "source");
            Check.NotNull(predicate, "predicate");

            cancellationToken.ThrowIfCancellationRequested();

            var provider = source.Provider as IAsyncQueryProvider;

            if (provider != null)
            {
                return provider.ExecuteAsync<TSource>(
                    Expression.Call(
                        null,
                        _singleOrDefaultPredicate.MakeGenericMethod(typeof(TSource)),
                        new[] { source.Expression, Expression.Quote(predicate) }
                        ),
                    cancellationToken);
            }

            throw new InvalidOperationException(Strings.FormatIQueryableProviderNotAsync());
        }

        private static readonly MethodInfo _sumDecimal = GetMethod<decimal>("Sum");

        public static Task<decimal> SumAsync(
            [NotNull] this IQueryable<decimal> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.NotNull(source, "source");

            cancellationToken.ThrowIfCancellationRequested();

            var provider = source.Provider as IAsyncQueryProvider;

            if (provider != null)
            {
                return provider.ExecuteAsync<decimal>(
                    Expression.Call(
                        null,
                        _sumDecimal,
                        new[] { source.Expression }
                        ),
                    cancellationToken);
            }

            throw new InvalidOperationException(Strings.FormatIQueryableProviderNotAsync());
        }

        private static readonly MethodInfo _sumInt = GetMethod<int>("Sum");

        public static Task<int> SumAsync(
            [NotNull] this IQueryable<int> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.NotNull(source, "source");

            var provider = source.Provider as IAsyncQueryProvider;

            if (provider != null)
            {
                return provider.ExecuteAsync<int>(
                    Expression.Call(
                        null,
                        _sumInt,
                        new[] { source.Expression }
                        ),
                    cancellationToken);
            }

            throw new InvalidOperationException(Strings.FormatIQueryableProviderNotAsync());
        }

        public static Task<List<TSource>> ToListAsync<TSource>(
            [NotNull] this IQueryable<TSource> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.NotNull(source, "source");

            cancellationToken.ThrowIfCancellationRequested();

            return source.ToAsyncEnumerable().ToList(cancellationToken);
        }

        public static Task<TSource[]> ToArrayAsync<TSource>(
            [NotNull] this IQueryable<TSource> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.NotNull(source, "source");

            cancellationToken.ThrowIfCancellationRequested();

            return source.ToAsyncEnumerable().ToArray(cancellationToken);
        }

        public static IQueryable<T> Include<T, TProperty>(
            [NotNull] this IQueryable<T> source,
            [NotNull] Expression<Func<T, TProperty>> path)
        {
            Check.NotNull(source, "source");

            throw new NotImplementedException();
        }

        private static MethodInfo GetMethod(string name, int parameterCount = 0)
        {
            return typeof(Queryable).GetTypeInfo().GetDeclaredMethods(name)
                .Single(mi => mi.GetParameters().Length == parameterCount + 1);
        }

        private static MethodInfo GetMethod<TReturn>(string name, int parameterCount = 0)
        {
            return typeof(Queryable).GetTypeInfo().GetDeclaredMethods(name)
                .Single(mi => mi.GetParameters().Length == parameterCount + 1
                              && mi.ReturnType == typeof(TReturn));
        }
    }
}
