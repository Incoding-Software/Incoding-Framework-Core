using System;
using System.Linq;
using System.Threading.Tasks;
using Incoding.Core;
using Incoding.Core.CQRS;
using Incoding.Core.CQRS.Common;
using Incoding.Core.CQRS.Core;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public sealed class MVDExecuteAsync : QueryBaseAsync<object>
    {
        public CommandComposite Instance { get; set; }

        public static void SetInterception(Func<IMessageInterception> create)
        {
            MVDExecute.SetInterception(create);
        }

        protected override async Task<object> ExecuteResult()
        {
            Guard.NotNull("Instance", "Instance query can't be null");
            foreach (var interception in MVDExecute.interceptions)
            {
                foreach (var message in Instance.Parts)
                    interception().OnBefore(message);
            }

            await new DefaultDispatcher().PushAsyncInternal(Instance);

            foreach (var interception in MVDExecute.interceptions)
            {
                foreach (var message in Instance.Parts)
                    interception().OnAfter(message);
            }

            return Instance.Parts.Count == 1 ? Instance.Parts[0].Result : Instance.Parts.Select(r => r.Result);
        }
    }
}