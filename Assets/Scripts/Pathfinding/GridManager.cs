using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // New - Calculate Lane size
    [Tooltip("Calculate Lane gridSize using bottom left lane tile")]
    [SerializeField] Transform bottomLeftTile;
    [Tooltip("Calculate Lane gridSize using top right lane tile")]
    [SerializeField] Transform topRightTile;
    

    [SerializeField] Vector2Int gridSize;

    [Tooltip("World grid size should match unity editor snap settings")]
    [SerializeField] int unityGridSize = 10;
    public int UnityGridSize { get { return unityGridSize; } }

    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid { get { return grid; } }

    void Awake() {
        CreateGrid();
    }

    /// <summary>
    /// Create Lane Grid from bottom left and top right tiles.
    /// </summary>
    void CreateGrid()
    {
        // xMax = distance between bottom left x and top right x tile +1
        int xMax = Mathf.RoundToInt(topRightTile.position.x / unityGridSize)+1;
        // yMax = distance between bottom left z start and top right z tile +1
        int yMax = Mathf.RoundToInt(topRightTile.position.z / unityGridSize)+1;

        for(int x = Mathf.RoundToInt(bottomLeftTile.position.x / unityGridSize); x < xMax; x++)
        {
            for(int y = Mathf.RoundToInt(bottomLeftTile.position.z / unityGridSize); y < yMax; y++)
            {
                // generate grid into dictionary
                Vector2Int coordinates = new Vector2Int(x,y);
                grid.Add(coordinates, new Node(coordinates, true));
                //Debug.Log("Coordinates: "+grid[coordinates].coordinates +" isWalkable?: "+grid[coordinates].isWalkable);
            }
        }
    }

    public Node GetNode(Vector2Int coordinates){
        // If node exists, return the value
        if (grid.ContainsKey(coordinates)){
            return grid[coordinates];
        }
        return null;
    }

    public void BlockNode(Vector2Int coordinates)
    {
        if(grid.ContainsKey(coordinates))
        {
            grid[coordinates].isWalkable=false;
        }
    }

    public void ResetNodes()
    {
        foreach(KeyValuePair<Vector2Int, Node> entry in grid)
        {
            entry.Value.connectedTo = null;
            entry.Value.isExplored = false;
            entry.Value.isPath = false;
        }
    }

    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();
        coordinates.x = Mathf.RoundToInt(position.x / unityGridSize);
        coordinates.y = Mathf.RoundToInt(position.z / unityGridSize);

        return coordinates;
    }

    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector3 position = new Vector3();
        position.x = coordinates.x * unityGridSize;
        position.z = coordinates.y * unityGridSize;
        return position;
    }

}
