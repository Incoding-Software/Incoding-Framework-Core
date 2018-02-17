using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Incoding.Core;
using Incoding.Core.CQRS;
using Incoding.Core.CQRS.Common;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Quality;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public sealed class MVDExecute : QueryBase<object>
    {
        static readonly List<Func<IMessageInterception>> interceptions = new List<Func<IMessageInterception>>();

        private readonly HttpContext context;

        [UsedImplicitly, Obsolete(ObsoleteMessage.SerializeConstructor, true), ExcludeFromCodeCoverage]
        public MVDExecute() { }

        public MVDExecute(HttpContext context)
        {
            this.context = context;
        }

        public CommandComposite Instance { get; set; }

        public static void SetInterception(Func<IMessageInterception> create)
        {
            interceptions.Add(create);
        }

        protected override object ExecuteResult()
        {
            Guard.NotNull("Instance", "Instance query can't be null");
            foreach (var interception in interceptions)
            {
                foreach (var message in Instance.Parts)
                    interception().OnBefore(message);
            }

            new DefaultDispatcher().Push(Instance);

            foreach (var interception in interceptions)
            {
                foreach (var message in Instance.Parts)
                    interception().OnAfter(message);
            }

            return Instance.Parts.Count == 1 ? Instance.Parts[0].Result : Instance.Parts.Select(r => r.Result);
        }
    }
}