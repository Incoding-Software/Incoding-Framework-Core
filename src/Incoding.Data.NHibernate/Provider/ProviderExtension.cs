using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Incoding.Core.Extensions;
using NHibernate.Linq;

namespace Incoding.Data.NHibernate.Provider
{
    /// <summary>
    /// Extensions with selected DB Provider
    /// </summary>
    public class ProviderExtension : IProviderExtension
    {
        /// <summary>
        /// To List with selected Provider
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public async Task<List<T>> ToProviderList<T>(IQueryable<T> items)
        {
            return await items.ToListAsync();
        }

        /// <summary>
        /// Any with selected Provider
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> ToProviderAny<T>(IQueryable<T> items)
        {
            return await items.AnyAsync();
        }

        /// <summary>
        /// Min with selected Provider
        /// </summary>
        /// <param name="items"></param>
        /// <param name="selector"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<TResult> ToProviderMin<TSource, TResult>(IQueryable<TSource> items, Expression<Func<TSource, TResult>> selector)
        {
            return await items.MinAsync(selector);
        }
    }
}