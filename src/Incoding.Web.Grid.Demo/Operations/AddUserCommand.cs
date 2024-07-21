using System.Threading.Tasks;
using GridUI.Persistance;
using Incoding.Core.CQRS.Core;

namespace GridUI.Operations
{
    public class AddUserCommand : CommandBaseAsync
    {
        #region Properties

        public string FirstName { get; set; }

        public string LastName { get; set; }


        #endregion

        protected override async Task ExecuteAsync()
        {
            var user = new User();
            user.FirstName = FirstName;
            user.LastName = LastName;
            await Repository.SaveAsync(user);
            Result = user;
        }
    }
}