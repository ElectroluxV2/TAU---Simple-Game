using System;
using Unity.Mathematics;
using UnityEngine;
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

public class MapGeneratorBehaviour : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public GameObject playerPrefab;
    public Camera minimapCam;
    
    public int mapSizeX;
    public int mapSizeY;
    public int gridSize;

    private bool _isStartPlaced;
    private bool _isStopPlaced;

    private CartesianMap<MapCell> _cartesianMap;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        _cartesianMap = new MapGenerator().Generate(mapSizeX, mapSizeY);
        
        // Instantiate prefabs for generated map
        foreach (var (x, y, cell) in _cartesianMap.Enumerate())
        {
            // Cell type: Floor or wall
            var cellPosition = new Vector3(x * gridSize, 2f, y * gridSize);
            var cellPrefab = cell.IsFloor ? obstaclePrefabs[0] : obstaclePrefabs[1];
            
            Instantiate(cellPrefab, cellPosition, quaternion.identity);
            
            // Cell contents: start or stop
            var contentsPosition = new Vector3(x * gridSize, 2f, y * gridSize);
            var contentsPrefab = cell.Contents switch
            {
                CellContents.Empty => null,
                CellContents.Start => obstaclePrefabs[2],
                CellContents.Stop => obstaclePrefabs[3],
                _ => throw new ArgumentOutOfRangeException(),
            };

            if (!cell.IsEmptyCell)
            {
                Instantiate(contentsPrefab, contentsPosition, quaternion.identity);
            }
            
            // Player
            if (cell.IsStartCell)
            {
                Instantiate(playerPrefab, contentsPosition, Quaternion.identity);
            }
        }

        // Position minimap
        var t = minimapCam.transform;
        // ReSharper disable PossibleLossOfFraction
        t.position = new Vector3( mapSizeX / 2 * gridSize, t.position.y ,mapSizeY / 2 * gridSize);
        // ReSharper restore PossibleLossOfFraction
        minimapCam.orthographicSize = mapSizeX * 4.83f + 33.4f;
    }
}
