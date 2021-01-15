using System;
using System.Collections.Generic;
using System.Linq;

namespace Kentico.Admin
{
    public class UserRepository : IUserRepository
    {
        private static readonly Lazy<List<IUser>> mUsers = new Lazy<List<IUser>>(() => new List<IUser>
        {
            new User { Id = 1, Name = "andy", Password = "ydna", FavoriteColor = "blue", Role = "moderator", ExternalId = null },
            new User { Id = 2, Name = "admin", Password = "nimda", FavoriteColor = "blue", Role = "admin", ExternalId = null },
            new User { Id = 3, Name = "datagrid.sk@gmail.com", Password = null, FavoriteColor = "orange", Role = "voyer", ExternalId = "105838370131173562976" }
        });


        private List<IUser> Users => mUsers.Value;


        public IUser GetByUsernameAndPassword(string username, string password)
        {
            return Users.SingleOrDefault(u => (u.Name == username) && (u.Password == password) && (u.ExternalId == null));
        }


        public IUser GetByUserName(string userName)
        {
            return Users.SingleOrDefault(u => u.Name == userName);
        }


        public IUser GetByExternalId(string externalId)
        {
            return Users.SingleOrDefault(u => u.ExternalId == externalId);
        }
    }
}
