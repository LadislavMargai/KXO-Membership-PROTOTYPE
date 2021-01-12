using System;
using System.Collections.Generic;
using System.Linq;

namespace Kentico.Admin
{
    public class UserRepository : IUserRepository
    {
        private static readonly Lazy<List<IUser>> mUsers = new Lazy<List<IUser>>(() => new List<IUser>());


        private List<IUser> Users => mUsers.Value;


        public UserRepository()
        {
            if (Users.Count == 0)
            {
                Users.Add(new User { Id = 1, Name = "andy", Password = "ydna", FavoriteColor = "blue", Role = "editor", ExternalId = null });
                Users.Add(new User { Id = 2, Name = "admin", Password = "nimda", FavoriteColor = "blue", Role = "admin", ExternalId = null });
                Users.Add(new User { Id = 3, Name = "datagrid.sk@gmail.com", Password = null, FavoriteColor = "orange", Role = "", ExternalId = "105838370131173562976" });
            }
        }


        public IUser GetByUsernameAndPassword(string username, string password)
        {
            return Users.SingleOrDefault(u => (u.Name == username) && (u.Password == password) && (u.ExternalId == null));
        }


        public IUser GetByExternalId(string externalId)
        {
            return Users.SingleOrDefault(u => u.ExternalId == externalId);
        }
    }
}
