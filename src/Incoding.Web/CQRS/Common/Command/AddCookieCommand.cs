using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Http;

namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using JetBrains.Annotations;

    #endregion

    [ExcludeFromCodeCoverage, UsedImplicitly]
    public class AddCookieCommand : CommandBase
    {
        #region Constructors

        public AddCookieCommand() { }
        
        public AddCookieCommand(string key, string value)
        {
            Key = key;
            Value = value;
        }

        #endregion

        #region Properties

        public string Key { get; set; }

        public string Value { get; set; }

        public string Domain { get; set; }

        public DateTime? Expires { get; set; }

        #endregion

        protected override void Execute()
        {
            Dispatcher.Query(new GetHttpContextQuery()).Response.Cookies.Append(Key, Value, new CookieOptions
                                                     {
                                                            Expires = Expires ?? DateTime.Now.AddYears(1),
                                                            Domain = Domain
                                                     });
        }
    }
}