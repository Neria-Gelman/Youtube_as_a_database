using System;
using System.IO;
using System.Security.Cryptography;

class encryptor
{
    static void Main()
    {
        string inputText = "Hello, World!";
        string videoFilePath = "example.mp4";
        string encryptedFilePath = "encrypted.mp4";

        byte[] videoData = File.ReadAllBytes(videoFilePath);

        using (Aes aesAlg = Aes.Create())
        {
            byte[] key = aesAlg.Key;
            byte[] iv = aesAlg.IV;

            using (FileStream encryptedStream = new FileStream(encryptedFilePath, FileMode.Create))
            using (CryptoStream cryptoStream = new CryptoStream(encryptedStream, aesAlg.CreateEncryptor(key, iv), CryptoStreamMode.Write))
            using (StreamWriter writer = new StreamWriter(cryptoStream))
            {
                writer.Write(inputText);
            }

            byte[] encryptedData = File.ReadAllBytes(encryptedFilePath);

            // Append the key and IV to the beginning of the video data
            byte[] outputData = new byte[videoData.Length + key.Length + iv.Length];
            key.CopyTo(outputData, 0);
            iv.CopyTo(outputData, key.Length);
            videoData.CopyTo(outputData, key.Length + iv.Length);

            File.WriteAllBytes(videoFilePath, outputData);
        }
    }
}
