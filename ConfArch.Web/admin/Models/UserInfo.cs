namespace Kentico.Admin
{
    public class UserInfo
    {
        public UserInfo()
        {
        }


        public UserInfo(IUser user)
        {
            Name = user.Name;
            FavoriteColor = user.FavoriteColor;
            Role = user.Role;
        }


        public string Name { get; set; }


        public string FavoriteColor { get; set; }


        public string Role { get; set; }
    }
}
