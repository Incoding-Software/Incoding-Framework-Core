using Incoding.Core.Data;

namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.Data;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncEntityBase))]
    public class When_entities_contain_in_dictionary : Context_entity_base
    {
        #region Establish value

        static Dictionary<IncEntityBase, string> entityDictionary;

        static Guid existsId;

        static bool isExists;

        #endregion

        Establish establish = () =>
                                  {
                                      existsId = Guid.NewGuid();
                                      entityDictionary = new Dictionary<IncEntityBase, string>
                                                             {
                                                                     { new FakeEntityBase(existsId), existsId.ToString() }
                                                             };
                                  };

        Because of = () => { isExists = entityDictionary.ContainsKey(new FakeEntityBase(existsId)); };

        It should_be_contains = () => isExists.ShouldBeTrue();
    }
}