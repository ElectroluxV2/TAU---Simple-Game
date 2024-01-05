using System;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

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
