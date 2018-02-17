namespace Incoding.Web.MvcContrib
{
    public interface IConditionalBinaryBuilder
    {
        IConditionalBuilder And { get; }

        IConditionalBuilder Or { get; }
    }
}