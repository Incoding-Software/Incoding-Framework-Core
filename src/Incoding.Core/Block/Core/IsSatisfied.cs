namespace Incoding.Core.Block.Core
{
    public delegate bool IsSatisfied<in TInstance>(TInstance instance);
}