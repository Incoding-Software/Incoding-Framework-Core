using Incoding.Core.Data;

namespace Incoding.CQRS
{
    #region << Using >>

    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;

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