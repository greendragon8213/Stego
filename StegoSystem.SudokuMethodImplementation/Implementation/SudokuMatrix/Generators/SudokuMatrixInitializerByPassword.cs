using StegoSystem.SudokuMethodImplementation.Abstract;
using System.Collections.Generic;

namespace StegoSystem.SudokuMethodImplementation.Matrix
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
            int[] password = ConvertStringToIntArray(passwordString);

            for (int i = 0; i < password.Length; i++)
            {
                var transformation = _invariantTransforations[password[i] % _invariantTransforations.Count];
                int[] transformationParams = new int[transformation.IndexesLength];

                for(int j = 0; j < transformationParams.Length && i + j < password.Length; j++)
                {
                    transformationParams[j] = password[i];
                }

                transformation.Transform(ref matrix, transformationParams);
            }
        }

        private int[] ConvertStringToIntArray(string s)
        {
            var result = new int[s.Length];

            for (int i = 0; i < s.Length; i++)
            {
                result[i] = System.Convert.ToInt32(s[i]);
            }

            return result;
        }
    }
}
