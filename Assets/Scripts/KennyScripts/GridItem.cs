using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem : MonoBehaviour
{

    /*
     For Other scripts to access the tiles or Objects:

        To access a certain tiles with a Vector2Int you need to call
        GridManager Class's mTiles variable
        e.g. mGridManager.GetComponent<GridManager>().mTiles[0, 1];
        Returns a GameObject

        To check a tile's own reference on what their Vector2Int position is,
        You access the IndividualTile Class's mTile variable
        e.g. tileGameObject.GetComponent<IndividualTile>().myTile;
        Returns a Vector2Int

        To check the tiles the object that's currently occupying them,
        You access the IndividualTile Class's objectThatOccupiedMySpace variable
        e.g. tileGameObject.GetComponent<IndividualTile>().objectThatOccupiedMySpace;
        Returns a GameObject
     */
    
    [SerializeField] private bool picked = false;
    [SerializeField] private Vector3 targetTilePosition;
    [SerializeField] private Vector3 startPosition;

    private GameObject mGridManager;

    [SerializeField] public Vector2 SpriteOffset;

    [SerializeField] private GameObject itemsToManage;

	//TAMIKO: see if you can rework your vector2ints to work with the itemScripts vector2ints
	//it likely means deprecatingsome of these variables here and just using itemScript methods
	//use the shape enum declared at GlobalStuff to see available shape types and feel free to use the GlobalStuff methods as well.
	public enum tileOccupationMode {
        SingleTile = 0,
        MultipleTiles = 1
    };
    public Vector2Int SingleGridOccupied; 
    public Vector2Int[] myNodesDisplacements;
    public Vector2Int[] allMyPoints;
    
    public tileOccupationMode tileMode;
    private BoxCollider2D mBoxCollider2D;
    private Vector3 originalLocation;

    //If  this field is false, the tile is freezed
    public bool CanBePickedUp = true;

    private bool detectSuccess = true;

	//TAMIKO: kenny I added these fields because im too lazy to do your grid item stuff
	//use the methods provided in ItemScript (mainly setItemPosition() and CompareItemPosition()) 
	//so that your grid stuff updates the itemScript's positions
	[HideInInspector]
	public ItemScript iScript;

    private void Awake()
    {
        //TAMIKO: added these
        iScript = GetComponent<ItemScript>();
		mGridManager = GameObject.FindGameObjectWithTag("GridManager");

        

        //Getting reference
        mBoxCollider2D = GetComponent<BoxCollider2D>();
        itemsToManage = GameObject.FindGameObjectWithTag("ItemsToManage");
		//Record original location for later returning
		
		
	}

    // Start is called before the first frame update
	
    void Start()
    {
		originalLocation = transform.position;
		if (iScript.isFrozen) CanBePickedUp = false;

	}

    // Update is called once per frame
    void Update()
    {
        //When picked up by mouse
        if (picked)
        {
            //Follow mouse
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
            gameObject.transform.position = new Vector3(mouseWorldPos.x + SpriteOffset[0], mouseWorldPos.y + SpriteOffset[1], 0);
        }
    }

    //When pressed
    private void OnMouseDown()
    {
        //Disable collider to detect the tiles under,
        //If collider remains on, we can't detect which tile we are currently on
        if (CanBePickedUp) picked = true;
    }

    //When released
    private void OnMouseUp()
    {
        if (CanBePickedUp)
        {
            picked = false;
            //Run the whole code that detects if dropping item is successful
            DetectDroppedTiles();
        }
    }

    //Used by GridManager to update your local position.
    //This class do not call this method, movement handled by GridManager
    public void MoveToTileLocation(Vector3 position)
    {
        gameObject.transform.position = new Vector3(position.x, position.y, transform.position.z);
    }

    //Used by GridManager to update your local position.
    public void UpdateItemScriptWithSameNumbers()
    {
        try
        {
            iScript = GetComponent<ItemScript>();
        } catch (Exception e)
        {
            Debug.Log(e);
        }
        
       // Debug.Log(iScript);
        iScript.itemPos = SingleGridOccupied;
       
        //Merging : Added This 
        iScript.SetItemPosition(iScript.itemPos);
    }

    public void validateTilePlacementAndApplyTiles()
    {
        detectSuccess = true;
        CheckSubsequentPoints(SingleGridOccupied);
        if (detectSuccess)
        {
            //Clean reference for other tiles on this object
            ClearOtherTilesOwnershipWhenSuccess();
            //Update Ownership on tiles with my tile coordinate
            UpdateTileOwnership(SingleGridOccupied);
        } else
        {
            //Return and zeroes everything
            ReturnToOriginalLocation();
            ZeroAllTileDataForThisItem();
        }
    }

    //Handles all the actions when an item is dropped onto a tile
    public void DetectDroppedTiles()
    {
        //Boolean to keep track if the checking tiles is successful during the method
        detectSuccess = true;
        //Debug.Log(mGridManager.GetComponent<GridManager>().mTiles[0, 1]);
        //Loop through all tiles
        for (int i = 0; i < mGridManager.transform.childCount; i++)
        {
            //Getting reference on each individual tile 
            IndividualTile eachIndividualTile = mGridManager.transform.GetChild(i).GetComponent<IndividualTile>();
            //If this tile is the one we are hovering on
            if (eachIndividualTile.currentlyHovered == true)
            {
                //We put this tile as our occupied tile, then updateAllMyNodes to get subsequent nodes' indexes
                SingleGridOccupied = eachIndividualTile.myTile;
                //My Method to update all the subsequent tiles' coordinate
                UpdateAllMyNodes();

                //Merging : Added This 
                iScript.itemPos = eachIndividualTile.myTile;
                iScript.SetItemPosition(eachIndividualTile.myTile);

                //Under MultiNode mode
                if (tileMode == tileOccupationMode.MultipleTiles)
                {
                    //First Loop check if any of the points is occupied by another object
                    //If there is other objects currently taking the point, abort entire
                    CheckSubsequentPoints(eachIndividualTile.myTile);
                    //If went pass the check above, object can be placed

                    //Loop through all the nodes, telling each tiles that I am the object that's occupying you
                    //Only when CheckSubsequentPoints was successful, determined by detectSuccess bool
                    if (detectSuccess)
                    {
                        ClearOtherTilesOwnershipWhenSuccess();
                        UpdateTileOwnership(eachIndividualTile.myTile);
                    }
                    else
                    {
                        //Return and zeroes everything
                        ReturnToOriginalLocation();
                        ZeroAllTileDataForThisItem();
                    }
                }  else //if tileMode is single
                {
                    //Still check subsequent node even only taking 1 node
                    CheckSubsequentPoints(eachIndividualTile.myTile);
                    //Only have to tell 1 tile that i am your object
                    //Only when CheckSubsequentPoints was successful, determined by detectSuccess bool
                    if (detectSuccess)
                    {
                        ClearOtherTilesOwnershipWhenSuccess();
                        eachIndividualTile.objectThatOccupiedMySpace = gameObject;
                    }
                    else
                    {
                        //Return and zeroes everything
                        ReturnToOriginalLocation();
                        ZeroAllTileDataForThisItem();
                    }
                }
                //Update the location of all objects, also snaps them in place
                //no matter success or not
                mGridManager.GetComponent<GridManager>().UpdateObjectLocations();
                return; //Return so we don't run the next few line, where we zero and return location
            }
        }
        //This will run only if player released on a non-tile location
        ReturnToOriginalLocation();
        ZeroAllTileDataForThisItem();
    }

    //This method given your current occupied 1 node,
    //register the other nodes that is occupied by your other body parts
    public void UpdateAllMyNodes()
    {
        //Get the initial 1st node
        allMyPoints = new Vector2Int[myNodesDisplacements.Length];
        //For every subsequent preset displacements
        for (int i = 0; i < myNodesDisplacements.Length; i++)
        {
            //Get the displaced index, and store them in allMyPoints
            allMyPoints[i] = SingleGridOccupied + myNodesDisplacements[i];
        }

        //Update ItemScript's allNodes Reference
        iScript.SetItemPosition(iScript.itemPos);
    }

    //This method will be run when placement of item fails
    //Case 1: When an item is occupying one slot
    //Case 2: When you release the item in a non tile place
    //This functions resets all occupied node information on both tiles and item itself
    public void ZeroAllTileDataForThisItem()
    { 
        // :( Foreach didn't work, so have to be ugly
        //Reset all item tile index to -1
        SingleGridOccupied = new Vector2Int(-5, -5);
        for (int k = 0; k < allMyPoints.Length; k++)
        {
            allMyPoints[k] = new Vector2Int(-5, -5);
        }

		GetComponent<ItemScript>().SetItemPosition(new Vector2Int(-5, -5)); //reset the itemscript vects as well
		//Loop through all tiles and clear my name
		//You guys are adopted and I'm not your dad //meat source
		for (int m = 0; m < mGridManager.transform.childCount; m++)
        {
            IndividualTile currentTileChecking = mGridManager.transform.GetChild(m).gameObject.GetComponent<IndividualTile>();
            if (currentTileChecking.objectThatOccupiedMySpace == gameObject)
            {
                currentTileChecking.objectThatOccupiedMySpace = null;
            }
        }
    }

    //This method will run when subsequent node check is successful
    //This will first remove the ownerships information on all tiles  for current object
    //Then the next method will apply the ownership for the new placed tiles
    public void ClearOtherTilesOwnershipWhenSuccess()
    {
        //Loop through all tiles and clear my name
        for (int m = 0; m < mGridManager.transform.childCount; m++)
        {
            IndividualTile currentTileChecking = mGridManager.transform.GetChild(m).gameObject.GetComponent<IndividualTile>();
            if (currentTileChecking.objectThatOccupiedMySpace == gameObject)
            {
                currentTileChecking.objectThatOccupiedMySpace = null;
            }
        }
    }

    //Return to Original location when level starts
    public void ReturnToOriginalLocation()
    {
        transform.position = originalLocation;
    }

    //Checkng if your placement will be successful
    //Given your currently focused node, check all other nodes from your body parts
    public void CheckSubsequentPoints(Vector2Int droppedTile)
    {
        //If there is no starting position for furniture, check is not needed
        //print(droppedTile);
        if (droppedTile == new Vector2Int(-5, -5))
        {
            //Debug.Log("Skipping Check: " + gameObject.name);
            return;
        }
        //Added Ndoe Checked count to make sure all nodes are checked
        //If not all nodes are checked, fail
        //This prevents number of tiles limiting range of checking
        int nodesChecked = 0;

        for (int m = 0; m < mGridManager.transform.childCount; m++)
        {
            //Loop through all my displacements
            for (int l = 0; l < myNodesDisplacements.Length; l++)
            {
                //Getting the set of index that matters 
                Vector2Int tempDisplacedNodePosition = droppedTile + myNodesDisplacements[l];
                IndividualTile currentTileChecking = mGridManager.transform.GetChild(m).gameObject.GetComponent<IndividualTile>();
                //Check if
                //1. the tile we are checking is the tile we are trying to occupy
                //2. If there is something already occupying  it
                //3, and the owner who is occupying it is not me
                if (currentTileChecking.myTile == tempDisplacedNodePosition &&
                    currentTileChecking.objectThatOccupiedMySpace != null &&
                                currentTileChecking.objectThatOccupiedMySpace != gameObject)
                {
                    //Report the placement failure
                    Debug.Log("Something in the node");
                    Debug.Log("CurrentTile: " + currentTileChecking.myTile.ToString());
                    Debug.Log("Node Owner: " + currentTileChecking.objectThatOccupiedMySpace.ToString());

                    //Reverse bool to stop the placement from continuing on top
                    detectSuccess = false;
                    return;
                }
                if (currentTileChecking.myTile == tempDisplacedNodePosition)
                {
                    nodesChecked += 1;
                   // Debug.Log("Checking Tile: " + currentTileChecking.myTile + " GridSize: " + mGridManager.GetComponent<GridManager>().GetGridSize());
                    //Check all tiles, see if they are out of bound
                    if (currentTileChecking.myTile.x > mGridManager.GetComponent<GridManager>().GetGridSize().x - 1 ||
                        currentTileChecking.myTile.x < 0 ||
                        currentTileChecking.myTile.y > mGridManager.GetComponent<GridManager>().GetGridSize().y - 1 ||
                        currentTileChecking.myTile.y < 0)
                    {
                        //Report the placement failure
                        Debug.Log("Something in the node");
                        Debug.Log("CurrentTile: " + currentTileChecking.myTile.ToString());
                        Debug.Log("Node Owner: " + currentTileChecking.objectThatOccupiedMySpace.ToString());

                        //Reverse bool to stop the placement from continuing on top
                        detectSuccess = false;
                        return;
                    }
                }
            }
        }

        //If nodes are not fully checked, fail as well
        if (nodesChecked != myNodesDisplacements.Length)
        {
            //Reverse bool to stop the placement from continuing on top
            Debug.Log("Nodes Aren't Fully Checked");
            detectSuccess = false;
        }
    }

    //Update all tile's ownership on current tile and subsequent tiles that occupy by bodyparts
    //This will be ran when the placement is successful
    public void UpdateTileOwnership(Vector2Int droppedTile)
    {
        for (int m = 0; m < mGridManager.transform.childCount; m++)
        {
            for (int l = 0; l < myNodesDisplacements.Length; l++)
            {
                Vector2Int tempDisplacedNodePosition = droppedTile + myNodesDisplacements[l];
                IndividualTile currentTileChecking = mGridManager.transform.GetChild(m).gameObject.GetComponent<IndividualTile>();
                if (currentTileChecking.myTile == tempDisplacedNodePosition)
                {
                    currentTileChecking.objectThatOccupiedMySpace = gameObject;
                }
            }
        }
    }

    public void MoveAway()
    {
        ZeroAllTileDataForThisItem();
		iScript.SetItemPosition(new Vector2Int(-99, -99)); //TAMIKO: added this owo
		gameObject.transform.position = new Vector3(-99, -99, transform.position.z);
    }

    public void MoveOneUnitBySeconds(GlobalStuff.Direction dir, float seconds)
    {
        Vector2Int targetTile = SingleGridOccupied; //Default to  be the current tile
        if (dir == GlobalStuff.Direction.up)
        {
            targetTile = SingleGridOccupied + new Vector2Int(0, -1);
        }
        else if (dir == GlobalStuff.Direction.down)
        {
            targetTile = SingleGridOccupied + new Vector2Int(0, 1);
        }
        else if (dir == GlobalStuff.Direction.left)
        {
            targetTile = SingleGridOccupied + new Vector2Int(-1, 0);
        }
        else if (dir == GlobalStuff.Direction.right)
        {
            targetTile = SingleGridOccupied + new Vector2Int(1, 0);
        }

        //Extract targettile objects, if failed, reset to current object for no change
        GameObject targetTileObject = gameObject;
        try
        {
            targetTileObject = mGridManager.GetComponent<GridManager>().mTiles[targetTile[0], targetTile[1]];
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        //Gathering essential position data for movement
        targetTilePosition = new Vector3(targetTileObject.transform.position.x + (float)SpriteOffset[0], targetTileObject.transform.position.y + (float)SpriteOffset[1], transform.position.z);
        startPosition = new Vector3(
                gameObject.transform.position.x,
                gameObject.transform.position.y,
                gameObject.transform.position.z);

        //Set the new tile data
        SingleGridOccupied = targetTile;
        UpdateAllMyNodes();

        //Set new tile data for ItemScript
        iScript.itemPos = targetTile;
        iScript.SetItemPosition(targetTile);

        //Update the tiles' information
        ClearOtherTilesOwnershipWhenSuccess();
        UpdateTileOwnership(targetTile);

        //Start the coroutine
        StartCoroutine(Move(startPosition, targetTilePosition, seconds));
    }

    //Coroutine to move
    IEnumerator Move(Vector3 startPos, Vector3 targetPos, float moveTime)
    {

        float t = 0f;
        while (t < 1f)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            t += Time.deltaTime / moveTime;
            yield return null;
        }
    }
    public bool isPicked()
    {
        return picked;
    }
}
