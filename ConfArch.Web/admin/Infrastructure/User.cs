namespace Kentico.Admin
{
    public class User : IUser
    {
        public int Id { get; set; }


        public string Name { get; set; }


        public string Password { get; set; }


        public string FavoriteColor { get; set; }


        public string Role { get; set; }


        public string ExternalId { get; set; }
    }
}
