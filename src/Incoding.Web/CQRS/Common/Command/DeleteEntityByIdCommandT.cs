using System.Diagnostics.CodeAnalysis;
using Incoding.Core.Data;
using JetBrains.Annotations;

namespace Incoding.Web.CQRS.Common.Command
{
    #region << Using >>

    #endregion

    public class DeleteEntityByIdCommand<TEntity> : DeleteEntityByIdCommand where TEntity : IEntity
    {
        #region Constructors

        [UsedImplicitly, ExcludeFromCodeCoverage]
        public DeleteEntityByIdCommand() { }

        public DeleteEntityByIdCommand(string id)
                : base(id, typeof(TEntity)) { }

        #endregion
    }
}