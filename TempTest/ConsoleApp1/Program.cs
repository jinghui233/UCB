// See https://aka.ms/new-console-template for more information
using ConsoleApp1;

Console.WriteLine("Hello, World!");
string textPath = @"C:\Users\静回\OneDrive\Projects\C#\UCB\UCB\TempTest\ConsoleApp1\CompressedFileStream.txt";
string filePath = @"C:\Users\静回\OneDrive\Projects\C#\UCB\UCB\TempTest\ConsoleApp1\CompressedFile.zip";
CompressionHelper.TextToFile(textPath, filePath);