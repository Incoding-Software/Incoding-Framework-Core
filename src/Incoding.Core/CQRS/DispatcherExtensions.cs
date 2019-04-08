using System;
using Incoding.Core.CQRS.Core;
using Incoding.Core;

namespace Incoding.Core.CQRS
{
    #region << Using >>

    #endregion

    public static class DispatcherExtensions
    {
        #region Factory constructors

        public static void Push(this IDispatcher dispatcher, Action<CommandComposite> configuration)
        {
            var composite = new CommandComposite();
            configuration(composite);
            dispatcher.Push(composite);
        }

        public static void Push(this IDispatcher dispatcher, CommandBase message, MessageExecuteSetting executeSetting = null)
        {
            dispatcher.Push(composite => composite.Quote(message, executeSetting));
        }

        public static void Push(this IDispatcher dispatcher, CommandBase message, Action<MessageExecuteSetting> configurationSetting)
        {
            var setting = new MessageExecuteSetting();
            configurationSetting.Do(action => action(setting));
            dispatcher.Push(message, setting);
        }

        public static TResult Query<TResult>(this IDispatcher dispatcher, QueryBase<TResult> message, Action<MessageExecuteSetting> configurationSetting) where TResult : class
        {
            var setting = new MessageExecuteSetting();
            configurationSetting.Do(action => action(setting));
            return dispatcher.Query(message, setting);
        }

        public static T Push<T>(this IDispatcher dispatcher, CommandBase<T> message)
        {
            dispatcher.Push(message, (MessageExecuteSetting)null);
            return message.Result;
        }

        public static T Push<T>(this IDispatcher dispatcher, CommandBase message) where T : class
        {
            dispatcher.Push(message);
            return message.Result as T;
        }

        #endregion
    }
}