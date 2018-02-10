using System;
using System.Linq.Expressions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Condionals.Builder;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Executables
{
    #region << Using >>

    #endregion

    public interface IExecutableSetting
    {
        [Obsolete("Use If with expression")]
        IExecutableSetting If(Action<IConditionalBuilder> configuration);

        IExecutableSetting If(Expression<Func<bool>> expression);

        IExecutableSetting TimeOut(double millisecond);

        IExecutableSetting TimeOut(TimeSpan time);

        [Obsolete("Please use fixed intervalId")]
        IExecutableSetting Interval(double millisecond, out string intervalId);

        [Obsolete("Please use fixed intervalId")]
        IExecutableSetting Interval(TimeSpan time, out string intervalId);

        IExecutableSetting Interval(TimeSpan time, string intervalId);

        IExecutableSetting Interval(double time, string intervalId);
    }
}