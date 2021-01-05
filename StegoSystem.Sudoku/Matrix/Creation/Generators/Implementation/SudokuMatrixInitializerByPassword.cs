using StegoSystem.Sudoku.Matrix.Generators.Helpers;
using StegoSystem.Sudoku.Matrix.InvariantTransformations;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace StegoSystem.Sudoku.Matrix.Generators
{
    public class SudokuMatrixInitializerByPassword<T>
    {
        private readonly List<IInvariantTransforation<T>> _invariantTransforations = new List<IInvariantTransforation<T>>()
        {
            new SwapColumnsInsideVerticalRegionInvariantTransforation<T>(),
            new SwapHorizontalRegionsInvariantTransforation<T>(),
            new SwapRowsInsideHorizontalRegionInvariantTransforation<T>(),
            new SwapVerticalRegionsInvariantTransforation<T>(),
        };

        public void Initialize(ref SudokuMatrix<T> matrix, string passwordString)
        {
            var password = GetHash(passwordString);

            int transformationsCount = 0;
            for (int i = 0; i < password.Length; i++)
            {
                var transformation = _invariantTransforations[password[i] % _invariantTransforations.Count];
                int[] transformationParams = new int[transformation.IndexesLength];

#if DEBUG
                System.Console.Write($"Transformation: {password[i] % _invariantTransforations.Count} -");
#endif

                for (int j = 0; j < transformationParams.Length && i + j < password.Length; j++, ++i)
                {
                    transformationParams[j] = password[i];
#if DEBUG
                    System.Console.Write($" {transformationParams[j]}");
#endif
                }

#if DEBUG
                System.Console.WriteLine();
#endif
                transformation.Transform(ref matrix, transformationParams);
                transformationsCount++;
            }
#if DEBUG
            System.Console.WriteLine("-", 20);
            System.Console.WriteLine($"Transf. count = {transformationsCount}");
            System.Console.WriteLine("-", 20);
#endif

        }

        //ok, but new method is better
        //private byte[] ConvertStringToItsHash(string s)
        //{
        //    //if range or limit, not the count!!!
        //    var primeNumbers = PrimeNumbersGenerator.Generate(s.Length / 2);            
        //    int arraysCount = (primeNumbers.Count + 1) * 2;
        //    char[][] result = new char[arraysCount][];

        //    char[] charArray = s.ToCharArray();
        //    result[0] = new char[charArray.Length];
        //    Array.Copy(charArray, result[0], charArray.Length);

        //    for (int i = 0; i < primeNumbers.Count; i++)
        //    {                    
        //        result[i + 1] = new char[(charArray.Length - 1) / primeNumbers[i]];

        //        for(int k = 0; k < result[i + 1].Length; k++)
        //        {
        //            result[i + 1][k] = charArray[primeNumbers[i] * (k + 1)];
        //        }
        //    }

        //    Array.Reverse(charArray);
        //    result[arraysCount - 1] = new char[charArray.Length];
        //    Array.Copy(charArray, result[arraysCount - 1], charArray.Length);

        //    for (int i = 0; i < primeNumbers.Count; i++)
        //    {
        //        result[i + 1] = new char[(charArray.Length -1) / primeNumbers[i]];

        //        for (int k = 0; k < result[i + 1].Length; k++)
        //        {
        //            result[i + 1][k] = charArray[primeNumbers[i] * (k + 1)];
        //        }
        //    }

        //    byte[] byteResult = new byte[32 * arraysCount];

        //    using (HashAlgorithm algorithm = SHA256.Create())
        //    {

        //        for (int i = 0; i < result.Length; i++)
        //        {
        //            var str = new string(result[i]);
        //            var bytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(str));
        //            Array.Copy(bytes, 0, byteResult, 32 * i, bytes.Length);
        //        }
        //    }

        //    return byteResult;

        //}

        private byte[] GetHash(string s)
        {
            int arraysCount;
            if (s.Length <= 30)
            {
                var primeNumbers = PrimeNumbersGenerator.Generate(s.Length / 2);
                arraysCount = 2 * s.Length - primeNumbers[(primeNumbers.Count - 1) / 2];
            }
            else
            {
                arraysCount = 60;
            }

            byte[] bytesResult = new byte[32 * arraysCount];

            using (HashAlgorithm algorithm = SHA256.Create())
            {
                var bytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(s));
                Array.Copy(bytes, 0, bytesResult, 0, bytes.Length);

                for (int i = 0; i < arraysCount - 1; i++)
                {
                    bytes = algorithm.ComputeHash(bytes);
                    Array.Copy(bytes, 0, bytesResult, 32 * (i + 1), bytes.Length);
                }
            }

            return bytesResult;
        }
    }
}
