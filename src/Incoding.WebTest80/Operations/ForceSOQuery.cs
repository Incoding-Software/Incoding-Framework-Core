using Incoding.Core.CQRS.Core;

namespace Incoding.WebTest80.Operations;

public class ForceSOQuery : QueryBase<ForceSOQuery.C1>
{
    public class C1
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public C2 A1
        {
            get { return new C2(); }
        }
    }

    public class C2
    {
        public Guid Id { get; set; } = Guid.Empty;
        public C1 A2
        {
            get { return new C1(); }
        }
    }

    protected override C1 ExecuteResult()
    {
        return new C1();
    }
}