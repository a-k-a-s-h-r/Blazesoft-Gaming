namespace Blazesoft.Helper
{
    public class Helpers
    {
        public decimal CalculateWin(int[,] resultMatrix, decimal bet)
        {
            decimal totalWin = 0;
            int rows = resultMatrix.GetLength(0);

            for (int row = 0; row < rows; row++)
            {
                totalWin += CalculateLineWin(resultMatrix, row, 0, 0, 1, bet);
            }

            totalWin += CalculateZigZagWin(resultMatrix, bet);

            return totalWin;
        }

        private decimal CalculateLineWin(int[,] resultMatrix, int startRow, int startCol, int rowStep, int colStep, decimal bet)
        {
            decimal lineWin = 0;
            int previousSymbol = -1;
            int consecutiveCount = 0;

            int rows = resultMatrix.GetLength(0);
            int columns = resultMatrix.GetLength(1);

            int row = startRow;
            int col = startCol;

            while (row >= 0 && row < rows && col >= 0 && col < columns)
            {
                int currentSymbol = resultMatrix[row, col];

                if (currentSymbol == previousSymbol)
                {
                    consecutiveCount++;
                }
                else
                {
                    if (consecutiveCount >= 3)
                    {
                        lineWin += previousSymbol * consecutiveCount * bet;
                    }

                    consecutiveCount = 1;
                    previousSymbol = currentSymbol;
                }

                row += rowStep;
                col += colStep;
            }

            // any remaining win after the loop
            if (consecutiveCount >= 3)
            {
                lineWin += previousSymbol * consecutiveCount * bet;
            }

            return lineWin;
        }

        private decimal CalculateZigZagWin(int[,] resultMatrix, decimal bet)
        {
            decimal zigZagWin = 0;
            int rows = resultMatrix.GetLength(0);
            int columns = resultMatrix.GetLength(1);

            int currentRow = 0;
            int previousSymbol = resultMatrix[0, 0];
            int consecutiveCount = 1;

            for (int col = 1; col < columns; col++)
            {
                int nextRow = currentRow + (col % 2 == 0 ? -1 : 1);

                if (nextRow < 0 || nextRow >= rows) break;

                int currentSymbol = resultMatrix[nextRow, col];

                if (currentSymbol == previousSymbol)
                {
                    consecutiveCount++;
                }
                else
                {
                    if (consecutiveCount >= 3)
                    {
                        zigZagWin += previousSymbol * consecutiveCount * bet;
                    }
                    consecutiveCount = 1;
                    previousSymbol = currentSymbol;
                }

                currentRow = nextRow;
            }

            // Final check for any remaining consecutive symbols
            if (consecutiveCount >= 3)
            {
                zigZagWin += previousSymbol * consecutiveCount * bet;
            }

            return zigZagWin;
        }

        public List<List<int>> ConvertMatrixToList(int[,] matrix)
        {
            var result = new List<List<int>>();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                var row = new List<int>();
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    row.Add(matrix[i, j]);
                }
                result.Add(row);
            }

            return result;
        }


    }
}
