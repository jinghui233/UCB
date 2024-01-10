using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class CompressionHelper
    {
        void Demo()
        {
            void Test2()
            {
                string filePath = @"e:\users\SuD02\source\repos\WindowsFormsApp1\ConsoleApp1\RVITA.zip";
                string textPath = @"e:\users\SuD02\source\repos\WindowsFormsApp1\ConsoleApp1\data.txt";
                CompressionHelper.FileToText(filePath, textPath);
                CompressionHelper.TextToFile(textPath, filePath);
            }
        }
        public static void FileToText(string filePath, string textPath)
        {
            // 压缩文件到字节流  
            byte[] compressedBytes = CompressionHelper.CompressFileToByteArray(filePath);
            // 将字节流保存为Base64编码的文本文件  
            CompressionHelper.ByteArrayToTextFile(textPath, compressedBytes);
        }
        public static void TextToFile(string textPath, string filePath)
        {
            // 从文本文件读取Base64编码的字节流  
            byte[] bytesFromFile = CompressionHelper.TextFileToByteArray(textPath);
            // 解压缩字节流到文件  
            CompressionHelper.ByteArrayToDecompressedFile(filePath, bytesFromFile);
        }
        public static byte[] CompressFileToByteArray(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    fileStream.CopyTo(gzipStream);
                }
                return memoryStream.ToArray();
            }
        }

        public static void ByteArrayToTextFile(string textFilePath, byte[] byteArray)
        {
            string base64String = Convert.ToBase64String(byteArray);
            File.WriteAllText(textFilePath, base64String);
        }

        public static byte[] TextFileToByteArray(string textFilePath)
        {
            string base64String = File.ReadAllText(textFilePath);
            return Convert.FromBase64String(base64String);
        }

        public static void ByteArrayToDecompressedFile(string outputFilePath, byte[] byteArray)
        {
            using (var fileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
            using (var memoryStream = new MemoryStream(byteArray))
            using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            {
                gzipStream.CopyTo(fileStream);
            }
        }
    }
}