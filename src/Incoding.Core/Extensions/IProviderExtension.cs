using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Incoding.Core.Extensions
{
    /// <summary>
    /// Provider Extension
    /// </summary>
    public interface IProviderExtension
    {
        /// <summary>
        /// To List by Provider
        /// </summary>
        /// <param name="items"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<List<T>> ToProviderList<T>(IQueryable<T> items);
        /// <summary>
        /// Any by Provider
        /// </summary>
        /// <param name="items"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<bool> ToProviderAny<T>(IQueryable<T> items);
        /// <summary>
        /// Min by Provider
        /// </summary>
        /// <param name="items"></param>
        /// <param name="selector"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        Task<TResult> ToProviderMin<TSource, TResult>(IQueryable<TSource> items, Expression<Func<TSource, TResult>> selector);
    }
}