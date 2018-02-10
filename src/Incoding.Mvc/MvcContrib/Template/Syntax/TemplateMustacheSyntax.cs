using Incoding.Extensions;

namespace Incoding.Mvc.MvcContrib.Template.Syntax
{
    #region << Using >>

    #endregion

    [Obsolete("Please migrate to handlebars or another template engine", false)]
    public class TemplateMustacheSyntax<TModel> : ITemplateSyntax<TModel>
    {
        #region Constructors

        public TemplateMustacheSyntax(HtmlHelper htmlHelper, string property, bool positiveConditional)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
            string typeConditional = positiveConditional ? "#" : "^";
            htmlHelper.ViewContext.Writer.Write("{{" + typeConditional + property + "}}");
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            this.htmlHelper.ViewContext.Writer.Write("{{/" + this.property + "}}");
        }

        #endregion

        #region Fields

        readonly HtmlHelper htmlHelper;

        readonly string property;

        #endregion

        #region ITemplateSyntax<TModel> Members

        [ExcludeFromCodeCoverage]
        public ITemplateSyntax<TModel> Up()
        {
            throw new NotImplementedException();
        }

        public string For(string field)
        {
            return "{{" + field + "}}";
        }

        public string For(Expression<Func<TModel, object>> field)
        {
            return For(ReflectionExtensions.GetMemberName(field));
        }

        public string For(Expression<Func<TModel, bool>> field)
        {
            return "{{" + ReflectionExtensions.GetMemberName(field) + "}}";
        }

        public MvcHtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, string isFalse)
        {
            return StringExtensions.ToMvcHtmlString((IsInline(field, isTrue).ToHtmlString() + NotInline(field, isFalse).ToHtmlString()));
        }

        public MvcHtmlString Inline(Expression<Func<TModel, object>> field, MvcHtmlString isTrue, MvcHtmlString isFalse)
        {
            return Inline(field, (string) isTrue.ToHtmlString(), (string) isFalse.ToHtmlString());
        }

        public MvcHtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, MvcHtmlString isFalse)
        {
            return Inline(field, isTrue, (string) isFalse.ToHtmlString());
        }

        public MvcHtmlString Inline(Expression<Func<TModel, object>> field, MvcHtmlString isTrue, string isFalse)
        {
            return Inline(field, (string) isTrue.ToHtmlString(), isFalse);
        }

        public MvcHtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, Func<object, HelperResult> isFalse)
        {
            return Inline(field, (string) isTrue.Invoke(null).ToHtmlString(), (string) isFalse.Invoke(null).ToHtmlString());
        }

        public MvcHtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, MvcHtmlString isFalse)
        {
            return Inline(field, (string) isTrue.Invoke(null).ToHtmlString(), (string) isFalse.ToHtmlString());
        }

        public MvcHtmlString Inline(Expression<Func<TModel, object>> field, MvcHtmlString isTrue, Func<object, HelperResult> isFalse)
        {
            return Inline(field, (string) isTrue.ToHtmlString(), (string) isFalse.Invoke(null).ToHtmlString());
        }

        public MvcHtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, Func<object, HelperResult> isFalse)
        {
            return Inline(field, isTrue, (string) isFalse.Invoke(null).ToHtmlString());
        }

        public MvcHtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, string isFalse)
        {
            return Inline(field, (string) isTrue.Invoke(null).ToHtmlString(), isFalse);
        }

        public MvcHtmlString IsInline(Expression<Func<TModel, object>> field, MvcHtmlString content)
        {
            string memberName = ReflectionExtensions.GetMemberName(field);
            return ("{{#" + memberName + "}}" + content.ToHtmlString() + "{{/" + memberName + "}}").ToMvcHtmlString();
        }

        public MvcHtmlString NotInline(Expression<Func<TModel, object>> field, MvcHtmlString content)
        {
            string memberName = ReflectionExtensions.GetMemberName(field);
            return ("{{^" + memberName + "}}" + content.ToHtmlString() + "{{/" + memberName + "}}").ToMvcHtmlString();
        }

        public MvcHtmlString IsInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content)
        {
            return IsInline(field, new MvcHtmlString(content.Invoke(null).ToHtmlString()));
        }

        public MvcHtmlString NotInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content)
        {
            return NotInline(field, new MvcHtmlString(content.Invoke(null).ToHtmlString()));
        }

        public MvcHtmlString ForRaw(string field)
        {
            return ("{{{" + field + "}}}").ToMvcHtmlString();
        }

        public MvcHtmlString ForRaw(Expression<Func<TModel, object>> field)
        {
            return ForRaw(ReflectionExtensions.GetMemberName(field));
        }

        public ITemplateSyntax<TNewModel> ForEach<TNewModel>(Expression<Func<TModel, IEnumerable<TNewModel>>> field)
        {
            return new TemplateMustacheSyntax<TNewModel>(this.htmlHelper, ReflectionExtensions.GetMemberName(field), true);
        }

        public IDisposable Is(Expression<Func<TModel, object>> field)
        {
            return new TemplateMustacheSyntax<TModel>(this.htmlHelper, ReflectionExtensions.GetMemberName(field), true);
        }

        public IDisposable Not(Expression<Func<TModel, object>> field)
        {
            return new TemplateMustacheSyntax<TModel>(this.htmlHelper, ReflectionExtensions.GetMemberName(field), false);
        }

        public MvcHtmlString IsInline(Expression<Func<TModel, object>> field, string content)
        {
            return IsInline(field, MvcHtmlString.Create(content));
        }

        public MvcHtmlString NotInline(Expression<Func<TModel, object>> field, string content)
        {
            return NotInline(field, MvcHtmlString.Create(content));
        }

        #endregion
    }
}