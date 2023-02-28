using System;
using System.IO;
using System.Security.Cryptography;

class decryptor
{
    static void Main()
    {
        string videoFilePath = "example.mp4";
        string decryptedFilePath = "decrypted.txt";

        byte[] inputData = File.ReadAllBytes(videoFilePath);

        byte[] key = new byte[32];
        byte[] iv = new byte[16];
        Array.Copy(inputData, key, 32);
        Array.Copy(inputData, 32, iv, 0, 16);

        byte[] videoData = new byte[inputData.Length - 32 - 16];
        Array.Copy(inputData, 32 + 16, videoData, 0, videoData.Length);

        using (Aes aesAlg = Aes.Create())
        {
            using (CryptoStream cryptoStream = new CryptoStream(new MemoryStream(videoData), aesAlg.CreateDecryptor(key, iv), CryptoStreamMode.Read))
            using (StreamReader reader = new StreamReader(cryptoStream))
            {
                string outputText = reader.ReadToEnd();
                File.WriteAllText(decryptedFilePath, outputText);
            }
        }
    }
}
