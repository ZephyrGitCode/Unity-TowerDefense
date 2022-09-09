using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Tooltip("Flag for tower is placeable on tile")]
    [SerializeField] bool isPlaceable;
    public bool IsPlaceable { get { return isPlaceable; } }

    public GridManager gridManager;

    Vector2Int coordinates = new Vector2Int();

    private void Awake() {
        // TODO Make Grid manager lane specific
        //gridManager = FindObjectOfType<GridManager>();
    }

    private void Start() {
        //if gridmanager exists
        if(gridManager!=null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            if(!isPlaceable)
            {
                gridManager.BlockNode(coordinates);
            }
        }
    }
}
