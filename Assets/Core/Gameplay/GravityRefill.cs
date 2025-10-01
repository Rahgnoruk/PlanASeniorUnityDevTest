using System;

public static class GravityRefill
{
    // Compacts each column downward. Calls move(oldR, oldC, newR) for moves.
    // Calls spawn(row, col) to create new blocks at the top.
    public static void Apply(BlockView[,] grid, Action<int, int, int> move, Action<int, int> spawn)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        for (int column = 0; column < cols; column++)
        {
            int write = rows - 1; // bottom
            for (int row = rows - 1; row >= 0; row--)
            {
                var block = grid[row, column];
                if (block == null) continue;

                if (write != row)
                {
                    grid[write, column] = block;
                    grid[row, column] = null;
                    move(row, column, write);
                }
                write--;
            }
            // Fill remaining cells [0..write]
            for (int row = write; row >= 0; row--)
            {
                spawn(row, column);
            }
        }
    }
}
