using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoardController : MonoBehaviour
{
    [Header("Wiring")]
    [SerializeField] GridConfig gridConfig;
    [SerializeField] GameController game;
    [SerializeField] Transform blocksRoot;
    [SerializeField] BlockView blockPrefab;
    [SerializeField] Sprite[] colorSprites; // index by BlockColor

    Camera cam;
    BlockView[,] bocksGrid;
    bool busy;

    void Awake() { cam = Camera.main; }

    public void BuildFresh()
    {
        ClearAll();
        bocksGrid = new BlockView[gridConfig.rows, gridConfig.cols];

        for (int row = 0; row < gridConfig.rows; row++)
        {
            for (int column = 0; column < gridConfig.cols; column++)
            {
                SpawnRandomBlockAt(row, column);
            }
        }
    }

    void ClearAll()
    {
        if (blocksRoot == null) return;
        for (int i = blocksRoot.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(blocksRoot.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        if (busy || game.State.Moves == 0) return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, cam.nearClipPlane));
            var hit = Physics2D.OverlapPoint(worldPos);
            if (hit && hit.TryGetComponent<BlockView>(out var b))
            {
                OnCellTapped(b.Row, b.Col);
            }
        }

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 screenPos = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, cam.nearClipPlane));
            var hit = Physics2D.OverlapPoint(worldPos);
            if (hit && hit.TryGetComponent<BlockView>(out var b))
            {
                OnCellTapped(b.Row, b.Col);
            }
        }
    }

    void OnCellTapped(int r, int c)
    {
        if (bocksGrid[r, c] == null) return;

        var toCollect = FloodFill.Collect(bocksGrid, r, c);

        // Spend a move (always 1 per tap)
        if (!game.SpendMoveAndCheckGameOver()) return;

        StartCoroutine(Co_CollectAndRefill(toCollect));
    }

    IEnumerator Co_CollectAndRefill(List<(int r, int c)> cells)
    {
        busy = true;

        // Remove visuals & clear grid slots
        foreach (var (row, column) in cells)
        {
            var block = bocksGrid[row, column];
            if (block != null) Destroy(block.gameObject);
            bocksGrid[row, column] = null;
        }

        game.OnScored(cells.Count);

        // Wait 1s before refill
        yield return new WaitForSeconds(1f);

        // Gravity + spawn
        GravityRefill.Apply(
            bocksGrid,
            move: (oldR, col, newR) =>
            {
                var b = bocksGrid[newR, col]; // already reassigned in Apply
                var pos = CellToWorld(newR, col);
                b.SetCell(newR, col, pos);
            },
            spawn: (row, col) => SpawnRandomBlockAt(row, col)
        );

        busy = false;
    }

    void SpawnRandomBlockAt(int row, int column)
    {
        BlockColor color = (BlockColor)Random.Range(0, colorSprites.Length);
        Vector3 pos = CellToWorld(row, column);
        BlockView block = Instantiate(blockPrefab, pos, Quaternion.identity, blocksRoot);
        block.name = $"Block_{row}_{column}_{color}";
        block.Init(gridConfig, color, row, column, colorSprites[(int)color]);
        bocksGrid[row, column] = block;
    }

    Vector3 CellToWorld(int row, int column)
    {
        float totalW = gridConfig.CellW * (gridConfig.cols - 1);
        float totalH = gridConfig.CellH * (gridConfig.rows - 1);

        float x = -totalW * 0.5f + column * gridConfig.CellW;
        float y = totalH * 0.5f - row * gridConfig.CellH;
        return blocksRoot.position + new Vector3(x, y, 0f);
    }
}
