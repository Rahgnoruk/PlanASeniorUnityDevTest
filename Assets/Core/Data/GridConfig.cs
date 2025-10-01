using UnityEngine;

[CreateAssetMenu(fileName = "GridConfig", menuName = "Config/GridConfig")]
public class GridConfig : ScriptableObject
{
    public int rows = 6;
    public int cols = 5;

    // 128×128 sprites, but cells are 128×112 in the specificatios (to allow for some overlap for the studs)
    public float ppu = 100f;
    public float cellWidthPx = 128f;
    public float cellHeightPx = 112f;

    public float CellW => cellWidthPx / ppu;   // 1.28
    public float CellH => cellHeightPx / ppu;  // 1.12
}