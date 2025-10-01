using System.Collections.Generic;

public static class FloodFill
{
    static readonly (int dr, int dc)[] Dirs = { (-1, 0), (1, 0), (0, -1), (0, 1) };

    public static List<(int r, int c)> Collect(BlockView[,] grid, int sr, int sc)
    {
        var rows = grid.GetLength(0);
        var cols = grid.GetLength(1);
        var color = grid[sr, sc].Color;

        var visited = new bool[rows, cols];
        var q = new Queue<(int r, int c)>();
        var list = new List<(int r, int c)>();

        visited[sr, sc] = true;
        q.Enqueue((sr, sc));

        while (q.Count > 0)
        {
            var (r, c) = q.Dequeue();
            list.Add((r, c));

            foreach (var (dr, dc) in Dirs)
            {
                int nr = r + dr, nc = c + dc;
                if (nr < 0 || nc < 0 || nr >= rows || nc >= cols || visited[nr, nc]) continue;
                var b = grid[nr, nc];
                if (b != null && b.Color == color)
                {
                    visited[nr, nc] = true;
                    q.Enqueue((nr, nc));
                }
            }
        }
        return list;
    }
}
