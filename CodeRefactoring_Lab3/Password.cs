using System.Security.Cryptography;
using System.Text;

namespace PersonalFinanceManagement
{
    public class Password
    {
        public string HashedValue { get; private set; }

        public Password(string plainTextPassword)
        {
            HashedValue = Hash(plainTextPassword);
        }

        private string Hash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in hashedBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool Verify(string plainTextPassword)
        {
            string hashed = Hash(plainTextPassword);
            return HashedValue.Equals(hashed);
        }
    }
}
