namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language.Condionals.Builder
{
    public interface IConditionalBinaryBuilder
    {
        IConditionalBuilder And { get; }

        IConditionalBuilder Or { get; }
    }
}