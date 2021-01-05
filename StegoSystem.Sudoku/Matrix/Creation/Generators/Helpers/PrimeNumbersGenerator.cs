using System;
using System.Collections.Generic;
using System.Linq;
namespace StegoSystem.Sudoku.Matrix.Generators.Helpers
{
    class PrimeNumbersGenerator
    {
        //small optimization
        private static List<int> _maxGeneratedList;
        private static int _maxtRequestedLimit = 0;

        internal static List<int> Generate(int limit)
        {
            if (_maxtRequestedLimit == limit && _maxtRequestedLimit > 0 && _maxGeneratedList != null)
            {
                return _maxGeneratedList;
            }

            if(_maxtRequestedLimit > limit)
            {
                return _maxGeneratedList.TakeWhile(v => v < limit).ToList();                
            }

            var result = from i in Enumerable.Range(2, limit - 1).AsParallel()
                    where Enumerable.Range(1, (int)Math.Sqrt(i)).All(j => j == 1 || i % j != 0)
                    select i;

            _maxGeneratedList = result.OrderBy(r => r).ToList();
            _maxtRequestedLimit = limit;

            return _maxGeneratedList;
        }
    }
}
