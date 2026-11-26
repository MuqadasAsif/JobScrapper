namespace JobScrapperMVC.Domain
{
    public class User
    {
        public int Id { get; set; }              // Primary Key — har user ka unique number
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }      // (asal project mein isay hash karna zaroori hota hai, plain text nahi)
    }
}
