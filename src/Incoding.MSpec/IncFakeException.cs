using JetBrains.Annotations;

namespace Incoding.MSpec
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using Machine.Specifications.Annotations;

    #endregion

    [UsedImplicitly, ExcludeFromCodeCoverage]
    public class IncFakeException : Exception { }
}