public static class StaticUserStore
{
    private static List<User> users = new List<User>
    {
        new User { Email = "player@gmail.com", UserName = "player", Password = "player123", Roles = new[] { "player" }, Scopes = new[] { "b_game" } },
        new User { Email = "admin@gmail.com", UserName = "admin", Password = "admin123", Roles = new[] { "admin" }, Scopes = new[] { "b_game", "vip_chararacter_personalize" } }
    };

    public static (string Email, string UserName, IEnumerable<string> Roles, IEnumerable<string> Scopes) ValidateUser(string emailOrUserName, string password)
    {
        var user = users.FirstOrDefault(u => (u.Email == emailOrUserName || u.UserName == emailOrUserName) && u.Password == password);

        if (user != null)
        {
            return (user.Email, user.UserName, user.Roles, user.Scopes);
        }

        return (null, null, null, null);
    }

    private class User
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<string> Scopes { get; set; }
    }
}
