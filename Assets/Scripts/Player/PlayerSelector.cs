using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class PlayerSelector : MonoBehaviour
{
    /// <summary>
    /// Input related variables
    /// </summary>
    [SerializeField,Tooltip("Player movement speed.")]
    private float playerSpeed = 50f;
    [Tooltip("Character controller component to move the player.")]
    private CharacterController controller;
    [Tooltip("Movement input Vector2 for player movement.")]
    private Vector2 movementInput = Vector2.zero;
    [Tooltip("Navigation Input Vector2 for menu navigation.")]
    private Vector2 navigateInput = Vector2.zero;
    [Tooltip("Bool that is set true when OnSelect is called.")]
    private bool onSelectBool = false;
    [Tooltip("Bool that is set true when OnBuy is called.")]
    private bool onBuyBool = false;

    /// <summary>
    /// UI Canvas objects
    /// </summary>
    [SerializeField]
    [Tooltip("Buy canvas gameobject")]
    public GameObject buyCanvas;
    public GameObject upgradeCanvas;
    public GameObject deployCanvas;

    /// <summary>
    /// Private variable to hold the current open menu
    /// </summary>
    private GameObject menuOpen;

    /// <summary>
    /// Buy and deploy parent objects
    /// </summary>
    [Tooltip("Parent Gameobject for buying defences")]
    public GameObject buyHolder;
    private GameObject savedDefence;

    [Tooltip("Parent Gameobject for deploying units")]
    public GameObject deployHolder;
    private GameObject savedDeploy;

    /// <summary>
    /// Private variable to hold Lane Manager and given lane
    /// </summary>
    private LaneManager laneManager;
    private Lane myLane;

    [Tooltip("My bank")]
    public Bank myBank;

    /// <summary>
    /// Map Tile (Node) related variables
    /// </summary>
    [Tooltip("Grid manager populates the Tiles (Nodes) and provides Node functions")]
    private GridManager gridManager;
    [Tooltip("Path Finder for Tile (Node) Navigation and functions")]
    private PathFinder pathFinder;
    [Tooltip("Node stores selected Tile (Node)")]
    private Node selectedNode = null;
    [Tooltip("Vextor2Int Coordinates")]
    private Vector2Int coordinates;

    private void Awake() {
        laneManager = FindObjectOfType<LaneManager>();
    }

    private void Start() {
        // Returns a lane from the manager
        myLane = laneManager.AssignLane(this);

        gridManager = myLane.transform.GetComponent<GridManager>();
        pathFinder = myLane.transform.GetComponent<PathFinder>();

        controller = gameObject.GetComponent<CharacterController>();
        coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

        savedDefence = buyHolder.transform.GetChild(0).gameObject;
        savedDeploy = deployHolder.transform.GetChild(0).gameObject;
    }

    /// <summary>
    /// Unity Event from player input
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        // Updates the movement input between -1 and 1
        movementInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Unity Event from player input. Navigate Menus.
    /// </summary>
    /// <param name="context"></param>
    public void OnNavigate(InputAction.CallbackContext context)
    {
        // Updates the navigate input between -1 and 1
        navigateInput = context.ReadValue<Vector2>();

        if (menuOpen == buyCanvas)
        {
             // Navigate right
            if (navigateInput.x > .5)
            {
                // Cycle right sibling if possible, otherwise cycle around
                savedDefence = cycleSelect(savedDefence, true);
            }

            // Navigate left
            if (navigateInput.x < -.5)
            {
                // Cycle left sibling if possible, otherwise cycle around
                savedDefence = cycleSelect(savedDefence, false);
            }
        }

        if (menuOpen == deployCanvas)
        {
             // Navigate right
            if (navigateInput.x > .5)
            {
                // Cycle right sibling if possible, otherwise cycle around
                savedDeploy = cycleSelect(savedDeploy, true);
            }

            // Navigate left
            if (navigateInput.x < -.5)
            {
                // Cycle left sibling if possible, otherwise cycle around
                savedDeploy = cycleSelect(savedDeploy, false);
            }
        }
    }

    /// <summary>
    /// Unity Event from player input - Gamepad Trigger, keyboard r. Buy defence, buy unit.
    /// </summary>
    /// <param name="context"></param>
    public void OnBuy(InputAction.CallbackContext context)
    {
        onBuyBool = context.action.triggered;

        if (menuOpen)
        {
            if (menuOpen == buyCanvas)
            {
                // Purchase defence
                BuyDefence(savedDefence.GetComponent<Tower>());
                savedDefence.SetActive(false);
                CloseAllCanvas();
            }
            
            /// <summary>
            /// Deploy Unit to Enemy Spawn. Buy unit.
            /// </summary>
            if (menuOpen == deployCanvas)
            {
                if (myBank == null)
                {
                    return;
                }

                // Withdraw from the bank and spawn the unit
                if (myBank.BuyUnit(savedDeploy.GetComponent<Enemy>()) == true)
                {
                    Lane otherLane = GetOtherLane();
                    otherLane.enemySpawner.DeployUnit(savedDeploy.gameObject, otherLane);
                    savedDeploy.SetActive(false);
                    CloseAllCanvas();
                }
                // TODO
                //SpawnMessage("Not enough money to deploy unit");
            }

            // A menu is already open
            return;
        }
    }

    /// <summary>
    /// Unity Event from player input. Open Buy or Upgrade menu.
    /// </summary>
    /// <param name="context"></param>
    public void OnSelect(InputAction.CallbackContext context)
    {
        onSelectBool = context.action.triggered;

        if (menuOpen)
        {
            // A menu is already open
            return;
        }

        // Get node from coordinates
        selectedNode = gridManager.GetNode(coordinates);

        // If empty and will not block = place turret
        if(selectedNode.isWalkable && !pathFinder.WillBlockPath(coordinates)) {
            // Snap the position of the user to the selected Node
            transform.position = gridManager.GetPositionFromCoordinates(selectedNode.coordinates);
            DisplayCanvas(buyCanvas);
            savedDefence.SetActive(true);
        }

        // If occupied = upgrade turret
        if(selectedNode.isWalkable == false) {
            // Snap the position of the user to the selected Node
            transform.position = gridManager.GetPositionFromCoordinates(selectedNode.coordinates);
            DisplayCanvas(upgradeCanvas);
            savedDeploy.SetActive(true);
        }

        // Else will block path = show not allowed message
        // TODO implement SpawnMessage
        //SpawnMessage("Not allowed to place")
    }


    /// <summary>
    /// Unity Event from player input. Open Upgrade menu.
    /// </summary>
    /// <param name="context"></param>
    public void OnDeploy(InputAction.CallbackContext context)
    {
        // Open Deploy Menu
        DisplayCanvas(deployCanvas);
        savedDeploy.SetActive(true);
    }

    /// <summary>
    /// Unity Event from player input. Exit all player menus.
    /// </summary>
    /// <param name="context"></param>
    public void OnBack(InputAction.CallbackContext context)
    {
        // Triggered by controller Circle (East)
        // Close all menus
        CloseAllCanvas();
    }

    /// <summary>
    /// Update handles:
    /// - player movement
    /// </summary>
    private void Update() {
        if(menuOpen)
        {
            return;
        }
        else{
            // Move player
            Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
            controller.Move(move * Time.deltaTime * playerSpeed);

            // Get new coordinates from position, for node selection.
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }
    }

    #region Display
    /// <summary>
    /// Close all canvases, and display a given Canvas.
    /// </summary>
    /// <param name="givenCanv"></param>
    private void DisplayCanvas(GameObject givenCanv)
    {
        CloseAllCanvas();
        givenCanv.SetActive(true);
        menuOpen = givenCanv;
    }

    /// <summary>
    /// Close all display canvases.
    /// </summary>
    public void CloseAllCanvas()
    {
        buyCanvas.SetActive(false);
        deployCanvas.SetActive(false);
        upgradeCanvas.SetActive(false);
        menuOpen = null;
    }
    #endregion
    
    #region Defence
    /// <summary>
    /// Cycle next Game Object
    /// Given GameObject and isNext bool. isNext true = cycle right, else cycle left.
    /// </summary>
    /// <param name="givenObject"></param>
    /// <param name="isNext"></param>
    /// <returns></returns>
    private GameObject cycleSelect(GameObject givenObject, bool isNext)
    {
        // De-active current object
        givenObject.SetActive(false);

        // Get index of current object
        int index = givenObject.transform.GetSiblingIndex();
        Transform savedParent = givenObject.transform.parent;

        // count children
        int count = (savedParent.childCount-1);
        int newIndex = index;

        // isNext means pressing right
        if (isNext)
        {
            // Cycle to the right
            if ((index + 1) > count)
            {
                newIndex = 0;
            }
            else{
                newIndex = index+1;
            }
        }else{
            
            // means cycling left
            if ((index - 1) < 0)
            {
                newIndex = count;
            }
            else{
                newIndex = index-1;
            }
        }
        // Reveal next object
        GameObject nextObject = savedParent.GetChild(newIndex).gameObject;
        nextObject.SetActive(true);
        return nextObject;
    }

    /// <summary>
    /// Called by OnBuy from Menu. Attempt to buy tower and place on node.
    /// </summary>
    /// <param name="towerPrefab"></param>
    public void BuyDefence(Tower towerPrefab)
    {
        /// <summary>
        /// - Buy selected turret, if can afford and location is walkable and will not block path - Handled in select button
        /// - subtract cost
        /// - place turret and set space to not walkable
        /// - Close all Canvas
        /// </summary>

        if(myBank == null){
            return;
        }
        
        bool isSuccessful = myBank.CreateTower(towerPrefab, gridManager.GetPositionFromCoordinates(selectedNode.coordinates));
        if(isSuccessful == true)
        {
            Instantiate(towerPrefab, gridManager.GetPositionFromCoordinates(selectedNode.coordinates), Quaternion.identity);
            gridManager.BlockNode(coordinates);
            pathFinder.NotifiyReceivers();
        }
        else
        {
            // TODO spawn message here
            Debug.Log("Unable to buy turret");
        }
    }
    #endregion

    /// <summary>
    /// Return other lane. Called by DeployUnit.
    /// </summary>
    public Lane GetOtherLane()
    {
        foreach(Lane i_lane in laneManager.myLanes)
        {
            if (i_lane != myLane) return i_lane;
        }
        return myLane;
    }
}