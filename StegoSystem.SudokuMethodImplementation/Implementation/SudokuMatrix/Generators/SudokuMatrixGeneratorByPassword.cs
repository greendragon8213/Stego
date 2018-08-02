using StegoSystem.SudokuMethodImplementation.Abstract;

namespace StegoSystem.SudokuMethodImplementation.Matrix
{
    public class SudokuMatrixGeneratorByPassword : SudokuMatrixGeneratorInitial, ISudokuMatrixGenerator
    {
        private readonly string _password;

        public SudokuMatrixGeneratorByPassword(int size, string password) : base(size)
        {
            _password = password;
        }

        public override byte[,] Generate()
        {
            var initialMatrix = base.Generate();

            //ToDo implement matrix deformation by password
            return initialMatrix;
        }
    }
}
