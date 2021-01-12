namespace Kentico.Admin
{
    public interface IUser
    {
        string ExternalId { get; set; }


        string FavoriteColor { get; set; }
        

        int Id { get; set; }
        

        string Name { get; set; }
        

        string Password { get; set; }
        
        
        string Role { get; set; }
    }
}