using System.Security.Cryptography;
using System.Text;

namespace AESCryptExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // The plaintext string
            string plaintext = "Detta är ett hemligt meddelande. Bara jag kan läsa det.";

            // The password used to encrypt the string
            string password = "Hello World";

            // Encrypt the string
            Kryptering encrypted = EncryptString(plaintext, password);

            // Decrypt the encrypted string
            string decrypted = DecryptString(encrypted, password);

            // Print the original and decrypted strings
            Console.WriteLine("Original:  " + plaintext);
            Console.WriteLine("Encrypted: " + encrypted.EncryptedValue);
            Console.WriteLine("Decrypted: " + decrypted);
            Console.WriteLine("Key: " + System.Text.Encoding.Default.GetString(encrypted.Key));
            Console.WriteLine("IV: " + System.Text.Encoding.Default.GetString(encrypted.IV));
        }

        static Kryptering EncryptString(string plaintext, string password)
        {
            // Convert the plaintext string to a byte array
            byte[] plaintextBytes = System.Text.Encoding.UTF8.GetBytes(plaintext);

            //Create a new Kryptering object
            Kryptering kryptering = new Kryptering();

            // Derive a new password using the PBKDF2 algorithm and a random salt
            Rfc2898DeriveBytes passwordBytes = new Rfc2898DeriveBytes(password, 20);

            // Use the password to encrypt the plaintext
            Aes encryptor = Aes.Create();
            encryptor.Key = passwordBytes.GetBytes(32);
            encryptor.IV = passwordBytes.GetBytes(16);

            //Spara Key och IV till kryptering objektet
            kryptering.Key = encryptor.Key;
            kryptering.IV = encryptor.IV;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(plaintextBytes, 0, plaintextBytes.Length);
                }
                kryptering.EncryptedValue = Convert.ToBase64String(ms.ToArray());
            }
            return kryptering;
        }

        static string DecryptString(Kryptering encrypted, string password)
        {
            // Convert the encrypted string to a byte array
            byte[] encryptedBytes = Convert.FromBase64String(encrypted.EncryptedValue);

            // Use the password to decrypt the encrypted string
            Aes encryptor = Aes.Create();
            encryptor.Key = encrypted.Key;
            encryptor.IV = encrypted.IV;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(encryptedBytes, 0, encryptedBytes.Length);
                }
                return System.Text.Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}