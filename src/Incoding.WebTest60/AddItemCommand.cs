using Incoding.Core.CQRS.Core;

namespace Incoding.WebTest60;

public class GetItemsQuery : QueryBase<string>
{
    public int UserId { get; set; }
    protected override string ExecuteResult()
    {
        var userId = UserId;
        return userId.ToString();
    }
}