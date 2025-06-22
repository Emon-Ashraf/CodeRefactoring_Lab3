using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PersonalFinanceManagement
{

    public static class Storage
    {
        private const string UserDataFile = "userData.json"; // JSON file to store user data

        public static List<User> LoadUserData()
        {
            if (File.Exists(UserDataFile))
            {
                string json = File.ReadAllText(UserDataFile);
                return JsonSerializer.Deserialize<List<User>>(json);
            }
            return new List<User>();
        }

        public static void SaveUserData(List<User> users)
        {
            string json = JsonSerializer.Serialize(users);
            File.WriteAllText(UserDataFile, json);
        }

        // Method to encrypt sensitive information like passwords
        public static string Encrypt(string text)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes("YourEncryptionKey123"); // Replace with a strong encryption key
                aesAlg.IV = new byte[16]; // Set initialization vector (IV)

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

        // Method to decrypt encrypted information
        public static string Decrypt(string cipherText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes("YourEncryptionKey123"); // Replace with a strong encryption key
                aesAlg.IV = new byte[16]; // Set initialization vector (IV)

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        // Methods to manage wallets data
        public static void SaveWalletData(User user, Wallet wallet)
        {
            List<User> users = LoadUserData();
            User existingUser = users.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null)
            {
                existingUser.AddWallet(wallet);
                SaveUserData(users);
            }
        }

        public static void RemoveWalletData(User user, Wallet wallet)
        {
            List<User> users = LoadUserData();
            User existingUser = users.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null)
            {
                existingUser.RemoveWallet(wallet);
                SaveUserData(users);
            }
        }

        // Other methods to handle more complex storage requirements can be added here

        // ADD THIS:
        public static List<User> users = new List<User>();

        public static User Authenticate(string email, string password)
        {
            return users.Find(user => user.Email == email && user.Authenticate(password));
        }
    }
}
