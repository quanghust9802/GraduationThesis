using System.Text.RegularExpressions;

namespace Application.Common
{
    public class PasswordRegex
    {

        public bool IsPasswordValid(string str)
        {
            // Regex pattern to match password criteria
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%&*?])[A-Za-z\d!@#$%&*?]{8,16}$";

            // Check if the password matches the pattern
            return Regex.IsMatch(str, pattern);
        }
    }
}
