using System.Threading;
using System.Threading.Tasks;
using NHibernate.Engine;
using NHibernate.Id;

namespace Incoding.Data
{
    ////ncrunch: no coverage start
    public class GuidStringGenerator : IIdentifierGenerator
    {
        #region IIdentifierGenerator Members

        public Task<object> GenerateAsync(ISessionImplementor session, object obj, CancellationToken cancellationToken)
        {
            return new GuidCombGenerator().GenerateAsync(session, obj, cancellationToken);
        }

        public object Generate(ISessionImplementor session, object obj)
        {
            return new GuidCombGenerator().Generate(session, obj);
        }

        #endregion
    }

    ////ncrunch: no coverage end
}