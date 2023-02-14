using BCrypt.Net;

namespace UtilityService.Utils
{
    public static class BCryptHasher
    {
        public static string? Hash(string input)
        {
            try
            {
                return BCrypt.Net.BCrypt.HashPassword(input);
            }
            catch (Exception e)
            {
                LoggerManager.GetInstance().LogError(e.Message);
                return null;
            }
        }

        public static bool Verify(string password, string hashedValue)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedValue);
            }
            catch (Exception e)
            {
                LoggerManager.GetInstance().LogError(e.Message);
                return false;
            }
        }
    }
}
