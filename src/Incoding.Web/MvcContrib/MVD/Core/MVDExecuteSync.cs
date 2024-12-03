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
    public sealed class MVDExecuteSync : QueryBase<object>
    {
        public CommandComposite Instance { get; set; }

        public static void SetInterception(Func<IMessageInterception> create)
        {
            MVDExecute.interceptionFuncs.Add(create);
        }
        public static void SetInterception(IMessageInterception interception)
        {
            MVDExecute.interceptions.Add(interception);
        }

        protected override object ExecuteResult()
        {
            Guard.NotNull("Instance", "Instance query can't be null");
            foreach (var interception in MVDExecute.interceptionFuncs)
            {
                foreach (var message in Instance.Parts)
                    interception().OnBeforeAsync(message).GetAwaiter().GetResult();
            }
            
            foreach (var interception in MVDExecute.interceptions)
            {
                foreach (var message in Instance.Parts)
                    interception.OnBeforeAsync(message).GetAwaiter().GetResult();
            }

            new DefaultDispatcher().Push(Instance);

            foreach (var interception in MVDExecute.interceptionFuncs)
            {
                foreach (var message in Instance.Parts)
                    interception().OnAfterAsync(message).GetAwaiter().GetResult();
            }

            foreach (var interception in MVDExecute.interceptions)
            {
                foreach (var message in Instance.Parts)
                    interception.OnAfterAsync(message).GetAwaiter().GetResult();
            }

            return Instance.Parts.Count == 1 ? Instance.Parts[0].Result : Instance.Parts.Select(r => r.Result);
        }
    }
}