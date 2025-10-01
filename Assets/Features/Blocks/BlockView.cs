using UnityEngine;
public enum BlockColor
{
    Green = 0,
    Blue = 1,
    Yellow = 2,
    Brown = 3,
    Pink = 4
}
public class BlockView : MonoBehaviour
{
    [field: SerializeField] public BlockColor Color { get; private set; }
    public int Row { get; private set; }
    public int Col { get; private set; }

    [SerializeField] SpriteRenderer sr;
    [SerializeField] BoxCollider2D col2d;

    GridConfig cfg;

    public void Init(GridConfig config, BlockColor color, int row, int col, Sprite sprite)
    {
        cfg = config;
        Color = color;
        Row = row;
        Col = col;
        sr.sprite = sprite;

        // Collider matches the playable body (ignore the studs area)
        col2d.size = new Vector2(cfg.CellW, cfg.CellH);
        col2d.offset = new Vector2(0f, col2d.size.y * 0.5f);

        UpdateSorting();
    }

    public void SetCell(int row, int col, Vector3 worldPos)
    {
        Row = row; Col = col;
        transform.position = worldPos;
        UpdateSorting();
    }

    void UpdateSorting()
    {
        // Higher rows (top) draw over lower rows so they hide studs beneath
        // We negate row so row 0 (top) gets large order.
        sr.sortingLayerName = "Blocks";
        sr.sortingOrder = -Row;
    }
}
