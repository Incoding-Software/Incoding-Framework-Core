using System;
using System.Collections.Generic;
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

    public sealed class MVDExecute : QueryBaseAsync<object>
    {
        internal static readonly List<Func<IMessageInterception>> interceptionFuncs = new List<Func<IMessageInterception>>();
        internal static readonly List<IMessageInterception> interceptions = new List<IMessageInterception>();

        public CommandComposite Instance { get; set; }

        public static void SetInterception(Func<IMessageInterception> create)
        {
            interceptionFuncs.Add(create);
        }
        public static void SetInterception(IMessageInterception interception)
        {
            interceptions.Add(interception);
        }

        protected override async Task<object> ExecuteResult()
        {
            Guard.NotNull("Instance", "Instance query can't be null");
            foreach (var interception in interceptionFuncs)
            {
                foreach (var message in Instance.Parts)
                    await interception().OnBefore(message);
            }
            
            foreach (var interception in interceptions)
            {
                foreach (var message in Instance.Parts)
                    await interception.OnBefore(message);
            }

            await new DefaultDispatcher().PushAsyncInternal(Instance);

            foreach (var interception in interceptionFuncs)
            {
                foreach (var message in Instance.Parts)
                    await interception().OnAfter(message);
            }

            foreach (var interception in interceptions)
            {
                foreach (var message in Instance.Parts)
                    await interception.OnAfter(message);
            }

            return Instance.Parts.Count == 1 ? Instance.Parts[0].Result : Instance.Parts.Select(r => r.Result);
        }
    }
}