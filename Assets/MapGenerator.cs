using Random = System.Random;

public enum CellType
{
    Floor,
    Wall,
}

public enum CellContents
{
    Empty,
    Start,
    Stop,
}

public class MapCell
{
    public CellType Type { get; set; }
    public CellContents Contents { get; set; }

    public MapCell(CellType cellType, CellContents cellContents)
    {
        Type = cellType;
        Contents = cellContents;
    }

    public bool IsFloor => Type == CellType.Floor;
    public bool IsWall => Type == CellType.Wall;
    public bool IsStartCell => Contents == CellContents.Start;
    public bool IsStopCell => Contents == CellContents.Stop;

    public bool IsEmptyCell => Contents == CellContents.Empty;
}

public class MapGenerator
{
    private static readonly Random Random = new();

    private static bool IsMapBorder(int x, int y, int mapSizeX, int mapSizeY) => x == 0 || y == 0 || x == mapSizeX - 1 || y == mapSizeY - 1;
    public CartesianMap<MapCell> Generate(int mapSizeX, int mapSizeY)
    {
        var cartesianMap = new CartesianMap<MapCell>(mapSizeX, mapSizeY);
        
        // Generate border during instantiation
        cartesianMap.Instantiate((x, y) => IsMapBorder(x, y, mapSizeX, mapSizeY)
            ? new MapCell(CellType.Wall, CellContents.Empty)
            : new MapCell(CellType.Floor, CellContents.Empty)
        );
        
        // Generate random walls within border constrained box
        foreach (var cell in cartesianMap)
        {
            if (!cell.IsFloor || !cell.IsEmptyCell || !(Random.NextDouble() > 0.70)) continue;
            
            cell.Type = CellType.Wall;
        }

        // Generate start
        foreach (var cell in cartesianMap)
        {
            if (!cell.IsFloor || !cell.IsEmptyCell) continue;
            
            cell.Contents = CellContents.Start;
            break;
        }
        
        // Generate stop
        foreach (var cell in cartesianMap.GetReverseEnumerator())
        {
            if (!cell.IsFloor || !cell.IsEmptyCell) continue;
            
            cell.Contents = CellContents.Stop;
            break;
        }

        return cartesianMap;
    }
}
