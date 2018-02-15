using Incoding.Core.Block.IoC.Core;

namespace Incoding.Core.Block.IoC
{
    public sealed class IoCInit
    {
        #region Fields

        IIoCProvider provider;

        #endregion

        #region Properties

        public IIoCProvider Provider
        {
            get { return this.provider; }
        }

        #endregion

        #region Api Methods

        public IoCInit WithProvider(IIoCProvider iocProvider)
        {
            Guard.NotNull("iocProvider", iocProvider);

            this.provider = iocProvider;
            return this;
        }

        #endregion
    }
}