namespace Depozyto.Models
{
    public class UserModel
    {
        //Dane obecnie zalogowanego urzytkownika

        public bool loggedIn { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public bool Blocked { get; set; }



    }
}
