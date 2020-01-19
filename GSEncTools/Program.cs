/*
 * GSDecryptor: decrypts a given Gyakuten Saiban data file
 * confirmed to support MDT and BIN files
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Mono.Options;

namespace GSDecryptor
{
    class Program
    {
        static byte[] EncryptBytes(byte[] decryptedBytes)
        {
            byte[] encryptedBytes = null;
            using (Aes aes = Aes.Create())
            {
                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes("u8DurGE2", Encoding.UTF8.GetBytes("6BBGizHE"))
                {
                    IterationCount = 1000
                };

                aes.Key = pbkdf2.GetBytes(16);
                aes.IV = pbkdf2.GetBytes(16);

                try
                {
                    encryptedBytes = aes.CreateEncryptor().TransformFinalBlock(decryptedBytes, 0, decryptedBytes.Length);
                }
                catch (CryptographicException e)
                {
                    Console.WriteLine($"GSEncTools: {e.Message}");
                    Console.WriteLine("Failed to encrypt file");
                    System.Environment.Exit(-1);
                }
            }

            return encryptedBytes;
        }


        static byte[] DecryptBytes(byte[] encryptedBytes)
        {
            byte[] decryptedBytes = null;
            using (Aes aes = Aes.Create())
            {
                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes("u8DurGE2", Encoding.UTF8.GetBytes("6BBGizHE"))
                {
                    IterationCount = 1000
                };

                aes.Key = pbkdf2.GetBytes(16);
                aes.IV = pbkdf2.GetBytes(16);

                try
                {
                    decryptedBytes = aes.CreateDecryptor().TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                }
                catch (CryptographicException e)
                {
                    Console.WriteLine($"GSEncTools: {e.Message}");
                    Console.WriteLine("Is the given file really encrypted?");
                    System.Environment.Exit(-1);
                }
            }

            return decryptedBytes;
        }

        static void Main(string[] args)
        {
            bool shouldDecrypt = false;
            bool shouldEncrypt = false;
            bool shouldShowHelp = false;
            var options = new OptionSet
            {
                { "d|decrypt", "decrypt a file", d => shouldDecrypt = d != null },
                { "e|encrypt", "encrypt a file", e => shouldEncrypt = e != null },
                { "h|help", "show the help message", h => shouldShowHelp = h != null}
            };

            List<string> extra = null;
            try
            {
                extra = options.Parse(args);
            }
            catch (OptionException e)
            {
                Console.WriteLine($"GsEncTools: {e.Message}");
                Console.WriteLine("Try passing \"--help\" as an argument to learn about available options");
                return;
            }

            if (shouldShowHelp)
            {
                Console.WriteLine("GsEncTools: decrypt or encrypt a given Gyakuten Saiban data file");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            string dataFilename = null;
            byte[] dataBytes = null;
            if (extra.Count > 0)
            {
                dataFilename = extra[0];
            }
            else
            {
                Console.WriteLine($"GsEncTools: Input the path to a {(shouldEncrypt ? "n encrypted" : "decrypted")} file");
                return;
            }

            // Read decrypted or encrypted file
            try
            {
                dataBytes = File.ReadAllBytes(dataFilename);
            }
            catch (IOException e)
            {
                Console.WriteLine($"GsEncTools: {e.Message}");
                Console.WriteLine("Does the given file exist?");
            }

            if (!(shouldDecrypt ^ shouldEncrypt))
            {
                // shouldDecrypt and shouldEncrypt are False
                string dataExtension = Path.GetExtension(dataFilename);
                shouldEncrypt = (dataExtension == ".new");
            }
            if (!shouldEncrypt)
            {
                File.WriteAllBytes(dataFilename + ".dec", DecryptBytes(dataBytes));
            }
            else
            {
                File.WriteAllBytes(dataFilename + ".enc", EncryptBytes(dataBytes));
            }
        }
    }
}
