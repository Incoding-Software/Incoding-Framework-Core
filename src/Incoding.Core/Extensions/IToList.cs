using Incoding.Core.Block.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Incoding.Core.Block.Scheduler.GetSchedulersQuery;

namespace Incoding.Core.Extensions
{
    public static class ProviderExtensions
    {
        public static async Task<List<T>> ToProviderList<T>(this IQueryable<T> items)
        {
            var providerList = IoCFactory.Instance.TryResolve<IProviderExtension>();

            return await providerList.ToProviderList(items);
        }
        public static async Task<bool> ToProviderAny<T>(this IQueryable<T> items)
        {
            var providerList = IoCFactory.Instance.TryResolve<IProviderExtension>();

            return await providerList.ToProviderAny(items);
        }
        public static async Task<TResult> ToProviderMin<TSource, TResult>(this IQueryable<TSource> items, Expression<Func<TSource, TResult>> selector)
        {
            var providerList = IoCFactory.Instance.TryResolve<IProviderExtension>();

            return await providerList.ToProviderMin<TSource, TResult>(items, selector);
        }
    }
}