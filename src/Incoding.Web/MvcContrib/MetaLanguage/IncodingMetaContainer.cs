using System;
using System.Collections.Generic;
using System.Linq;
using Incoding.Core.Extensions;
using Incoding.Core;
using Incoding.Web.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class IncodingMetaContainer
    {
        #region Fields

        readonly List<ExecutableBase> merges = new List<ExecutableBase>();

        JquerySelectorExtend target;

        IncodingEventCanceled onEventStatus;

        bool lockTarget;

        #endregion

        #region Constructors

        public IncodingMetaContainer()
        {
            OnEventStatus = IncodingEventCanceled.None;
        }

        #endregion

        #region Properties

        public string OnBind { get; set; }

        public bool LockTarget
        {
            get { return this.lockTarget; }
            set
            {
                if (!value)
                    this.target = null;
                this.lockTarget = value;
            }
        }

        public IncodingEventCanceled OnEventStatus
        {
            get { return this.onEventStatus; }
            set
            {
                bool isAll = (this.onEventStatus == IncodingEventCanceled.PreventDefault && value == IncodingEventCanceled.StopPropagation) ||
                             (this.onEventStatus == IncodingEventCanceled.StopPropagation && value == IncodingEventCanceled.PreventDefault);
                this.onEventStatus = isAll ? IncodingEventCanceled.All : value;
            }
        }
        
        public JquerySelectorExtend Target
        {
            get { return this.target; }
            set
            {
                this.target = this.target != null
                                      ? this.target.Add(value)
                                      : value;
            }
        }

        public IncodingCallbackStatus OnCurrentStatus { get; set; }

        #endregion

        #region Api Methods

        public HtmlRouteValueDictionary AsHtmlAttributes(IHtmlHelper htmlHelper, object htmlAttributes = null)
        {
            const string dataIncodingKey = "incoding";
            var res = new HtmlRouteValueDictionary(AnonymousHelper.ToDictionary(htmlAttributes));
            res.HtmlHelper = htmlHelper;
            var callbacks = merges.Select(@base => @base.AsObject()).ToArray();

            if (!res.ContainsKey(dataIncodingKey))
                res.Add(dataIncodingKey, ObjectExtensions.ToJsonString(callbacks));
            else
            {
                var newArray = ObjectExtensions.DeserializeFromJson<ExecutableBase.Json[]>(res[dataIncodingKey].ToString()).ToList();
                newArray.AddRange(callbacks);
                res[dataIncodingKey] = ObjectExtensions.ToJsonString(newArray);
            }

            return res;
        }

        public void Add(ExecutableBase executable)
        {
            var errorMessage = executable.GetErrors();
            if (errorMessage.Any())
            {
                throw new ArgumentException("Executable {0} have problem: {1}".F(executable.GetType().Name, StringExtensions.AsString(errorMessage
                        .Select(r => "{0}-{1}".F(r.Key, r.Value)), ",")), "callback");
            }
            executable.Add("onBind", OnBind);
            executable.Add("onStatus", (int)OnCurrentStatus);
            executable.Add("target", Target.With(r => r.ToJqueryObject()));
            executable.Add("onEventStatus", (int)OnEventStatus);
            this.merges.Add(executable);
            if (!LockTarget)
                this.target = null;
        }

        public void SetFilter(ConditionalBase filter)
        {
            MaybeObject.DoEach<ExecutableActionBase>(this.merges
                    .Where(r => r["onBind"].ToString() == OnBind)
                    .OfType<ExecutableActionBase>(), @base => @base.SetFilter(filter));
        }

        public bool IsLastAction { get { return merges.LastOrDefault() is ExecutableActionBase; } }

        #endregion
    }
}