namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Linq;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingMetaLanguageDsl))]
    public class When_incoding_meta_language_dsl_more_when_without_on
    {
        #region Establish value

        static IIncodingMetaLanguageEventBuilderDsl metaBuilder;

        #endregion

        Because of = () =>
                     {
                         metaBuilder = new IncodingMetaLanguageDsl(JqueryBind.Click)
                                 .Ajax("url")
                                 .When(JqueryBind.Blur)
                                 .OnSuccess(r => r.Self().Core().Eval(Pleasure.Generator.TheSameString()))
                                 .When("load")
                                 .OnSuccess(r => r.Self().Core().Eval(Pleasure.Generator.TheSameString()));
                     };

        It should_be_action_when_blur = () => metaBuilder
                                                      .GetAll<ExecutableDirectAction>()
                                                      .SingleOrDefault(r => r["onBind"].ToString() == "blur incoding")
                                                      .ShouldNotBeNull();

        It should_be_action_when_click = () => metaBuilder
                                                       .GetAll<ExecutableAjaxAction>()
                                                       .SingleOrDefault(r => r["onBind"].ToString() == "click incoding")
                                                       .ShouldNotBeNull();

        It should_be_action_when_load = () => metaBuilder
                                                      .GetAll<ExecutableDirectAction>()
                                                      .SingleOrDefault(r => r["onBind"].ToString() == "load incoding")
                                                      .ShouldNotBeNull();

        It should_be_callback_when_blur = () => metaBuilder
                                                        .GetAll<ExecutableEval>()
                                                        .SingleOrDefault(r => r["onBind"].ToString() == "blur incoding")
                                                        .ShouldNotBeNull();

        It should_be_callback_when_load = () => metaBuilder
                                                        .GetAll<ExecutableEval>()
                                                        .SingleOrDefault(r => r["onBind"].ToString() == "load incoding")
                                                        .ShouldNotBeNull();

        It should_be_count = () => { metaBuilder.GetAll<ExecutableBase>().Count().ShouldEqual(5); };
    }
}