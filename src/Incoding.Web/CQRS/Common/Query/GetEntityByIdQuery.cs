using System;
using System.Diagnostics.CodeAnalysis;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Data;
using Incoding.Core.Quality;
using JetBrains.Annotations;

namespace Incoding.Web.CQRS.Common.Query
{
    #region << Using >>

    #endregion

    public class GetEntityByIdQuery<T> : QueryBase<T> where T : class, IEntity, new()
    {
        #region Constructors

        [UsedImplicitly, Obsolete(ObsoleteMessage.SerializeConstructor), ExcludeFromCodeCoverage]
        public GetEntityByIdQuery() { }

        ////ncrunch: no coverage start
        public GetEntityByIdQuery(object id)
        {
            Id = id;
        }

        #endregion

        ////ncrunch: no coverage end
        #region Properties

        public object Id { get; set; }

        #endregion

        #region Override

        protected override T ExecuteResult()
        {
            return Repository.GetById<T>(Id);
        }

        #endregion
    }
}