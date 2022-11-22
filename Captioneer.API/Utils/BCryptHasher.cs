namespace Captioneer.API.Utils
{
    public static class BCryptHasher
    {
        public static string Hash(string input)
        {
            return BCrypt.Net.BCrypt.HashPassword(input);
        }

        public static bool Verify(string password, string hashedValue)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedValue);
        }
    }
}
