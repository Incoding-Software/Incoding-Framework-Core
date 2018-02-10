using Incoding.CQRS;
using Incoding.Quality;

namespace Incoding.Mvc.MvcContrib.MVD.Core
{
    #region << Using >>

    #endregion

    public sealed class MVDExecute : QueryBase<object>
    {
        static readonly List<Func<IMessageInterception>> interceptions = new List<Func<IMessageInterception>>();

        private readonly HttpContextBase context;

        [UsedImplicitly, Obsolete(ObsoleteMessage.SerializeConstructor, true), ExcludeFromCodeCoverage]
        public MVDExecute() { }

        public MVDExecute(HttpContextBase context)
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