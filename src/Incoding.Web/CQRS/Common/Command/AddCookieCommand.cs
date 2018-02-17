using System;
using System.Diagnostics.CodeAnalysis;
using Incoding.Core.CQRS.Core;
using Incoding.Web.CQRS.Common.Query;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace Incoding.Web.CQRS.Common.Command
{
    #region << Using >>

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