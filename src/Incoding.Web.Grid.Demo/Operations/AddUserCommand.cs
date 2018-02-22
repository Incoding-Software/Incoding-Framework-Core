using GridUI.Persistance;
using Incoding.Core.CQRS.Core;

namespace GridUI.Operations
{
    public class AddUserCommand : CommandBase
    {
        #region Properties

        public string FirstName { get; set; }

        public string LastName { get; set; }


        #endregion

        protected override void Execute()
        {
            var user = new User();
            user.FirstName = FirstName;
            user.LastName = LastName;
            Repository.Save(user);
            Result = user;
        }
    }
}