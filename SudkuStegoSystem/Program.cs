using SudkuStegoSystem.Logic;
using SudkuStegoSystem.Logic.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudkuStegoSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = Console.ReadLine();
            while (command != "exit")
            {
                if (command == "encr test")
                {
                    EncryptionTest1();
                    Console.WriteLine("Encrypted success");
                }

                if (command == "decr test")
                {
                    DecryptionTest1();
                    Console.WriteLine("Decrypted success");
                }

                command = Console.ReadLine();
            }
        }

        private static IStegoSystem _stegoSystem = new StegoSystem();

        private static void EncryptionTest1()
        {
            string containerPath = @"D:\Космос\C# NET\Diploma.Master.Stego\Examples\1-containers\nature.bmp";
            string secretDataFilePath = @"D:\Космос\C# NET\Diploma.Master.Stego\Examples\0-secrets\test 1.txt";
            string key = "123456";
            string outputFilePath = @"D:\Космос\C# NET\Diploma.Master.Stego\Examples\3-filled_containers";

            _stegoSystem.Encrypt(containerPath, secretDataFilePath, key, outputFilePath);
        }

        private static void DecryptionTest1()
        {
            string containerPath = @"D:\Космос\C# NET\Diploma.Master.Stego\Examples\3-filled_containers\nature.bmp";
            string key = "123456";
            string outputFilePath = @"D:\Космос\C# NET\Diploma.Master.Stego\Examples\4-decrypted-secrets";

            _stegoSystem.Decrypt(containerPath, key, outputFilePath);
        }
    }
}
