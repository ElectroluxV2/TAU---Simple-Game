using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Build;
using UnityEngine;
using Random = System.Random;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] ObstaclePrefabs;

    public GameObject playerPrefab;

    [SerializeField]
    private Camera MinimapCam;
    
    public int MapSizeX, MapSizeY;
    public int GridSize;
    private static readonly Random Random = new Random();

    private bool IsStartPlaced = false;
    private bool IsStopPlaced = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        for (var x = 0; x < MapSizeX; x++)
        {
            for (var y = 0; y < MapSizeY; y++)
            {

                var obstaclePrefabIndex = IsMapBorder(x, y) || IsInsideObstacle(x, y)
                    ? 1
                    : 0;
                
                var activeCell = Instantiate(ObstaclePrefabs[obstaclePrefabIndex]);
                activeCell.transform.position = new Vector3(x * GridSize, 0f, y * GridSize);

                if (obstaclePrefabIndex == 0 && Random.NextDouble() > .90 && IsStartPlaced == false)
                {
                    IsStartPlaced = true;
                    Instantiate(ObstaclePrefabs[2], new Vector3(x * GridSize, 2f, y * GridSize), quaternion.identity);
                    Instantiate(playerPrefab, new Vector3(x * GridSize, 2f, y * GridSize), Quaternion.identity);
                }

                if (obstaclePrefabIndex == 0 && Random.NextDouble() > .90 && IsStopPlaced == false)
                {
                    IsStopPlaced = true;
                    Instantiate(ObstaclePrefabs[3], new Vector3(x * GridSize, 2f, y * GridSize), quaternion.identity);
                }

                if (x == MapSizeX / 2 && y == MapSizeY / 2)
                {
                    MinimapCam.transform.position = new Vector3(x * GridSize, MinimapCam.transform.position.y ,y * GridSize);
                    MinimapCam.orthographicSize = MapSizeX * 4.83f + 33.4f;
                }
                
            }
        }
        
    }

    private bool IsMapBorder(int x, int y) => x == 0 || y == 0 || x == MapSizeX - 1 || y == MapSizeY - 1;
    private bool IsInsideObstacle(int x, int y) => Random.NextDouble() > 0.80;

    // Update is called once per frame
    void Update()
    {
        
    }
}
