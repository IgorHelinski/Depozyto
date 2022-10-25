using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Depozyto.Controllers
{
    public class EncriptController : Controller
    {
        public static string Encrypt(string text)
        {
            string encryptionText = string.Empty;
            string encryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            byte[] clearBytes = Encoding.Unicode.GetBytes(text);

            using (Aes encryptor = Aes.Create())
            {
                using (Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }))
                {
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                }

                using MemoryStream ms = new MemoryStream();

                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }

                encryptionText = Convert.ToBase64String(ms.ToArray());
            }

            return encryptionText;
        }
        public static string Decrypt(string text)
        {
            string decryptionText = string.Empty;
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            text = text.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(text);

            using (Aes encryptor = Aes.Create())
            {
                using (Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }))
                {
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                }

                using MemoryStream ms = new MemoryStream();

                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }

                decryptionText = Encoding.Unicode.GetString(ms.ToArray());
            }
            return decryptionText;
        }
    }
}

