using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GridManager : MonoBehaviour
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

	//Fields
	[SerializeField] private GridLayoutGroup mGrid;
    [SerializeField] private Vector2 gridSize;
    [SerializeField] private GameObject tilePrefab;
    private GameObject itemsToManage;
    public GameObject[,] mTiles;
	private float epsilon = 0.2f;
	// Start is called before the first frame update
	private void Awake() {
		
		itemsToManage = GameObject.FindGameObjectWithTag("ItemsToManage");
        //Getting reference
        mGrid = GetComponent<GridLayoutGroup>();
        mTiles = new GameObject[(int)gridSize[0], (int)gridSize[1]];
    }

	void Start()
    {
		
		//destroy all kids
		//for (int i = transform.childCount - 1; i>= 0 ; i--) {
		//	Destroy(transform.GetChild(i).gameObject);
		//}
		

		for (int i = 0; i < gridSize[1]; i++)
		{
			for (int j = 0; j < gridSize[0]; j++)
			{
				//Debug.Log("CurrentIndex = " + j + " " + i);
				//Debug.Log("CurrentChildIndex = " + childIndex);
				mTiles[j, i] = (GameObject)Instantiate(tilePrefab, gameObject.transform);
				mTiles[j, i].GetComponent<IndividualTile>().myTile = new Vector2Int(j, i);
			}
		}
		

		//Update Grid is a method I took somewhere online,
		//it is needed for the tile position retrieval to be correct
		UpdateGrid(mGrid);

        //Method to Update All Object's newest registered location according to their index 
        UpdateObjectLocations();

        //Method to update all references on all the object and tiles according to the tile indexes
        UpdateObjectAndTilesReferences();

		


		//print(itemsToManage == null);
    }

	public void GenerateGrid() {

		mGrid = GetComponent<GridLayoutGroup>();
        mTiles = new GameObject[(int)gridSize[0], (int)gridSize[1]];

		while(transform.childCount > 0) {
			DestroyImmediate(transform.GetChild(0).gameObject);
		}

		//Instantiating tiles into a 2D array
		for (int i = 0; i < gridSize[1]; i++) {
			for (int j = 0; j < gridSize[0]; j++) {
				mTiles[j, i] = (GameObject)Instantiate(tilePrefab, gameObject.transform);
				mTiles[j, i].GetComponent<IndividualTile>().myTile = new Vector2Int(j, i);
			}
		}
		
		//UpdateGrid(mGrid);
	}

    //Taken from stackOverflow, required to retrieve the right position data
    public void UpdateGrid(LayoutGroup gridLayoutGroup)
    {
        gridLayoutGroup.CalculateLayoutInputHorizontal();
        gridLayoutGroup.CalculateLayoutInputVertical();
        gridLayoutGroup.SetLayoutHorizontal();
        gridLayoutGroup.SetLayoutVertical();
    }

    //Update all object's world position according to their index
    public void UpdateObjectLocations()
    {
        for (int i = 0; i < itemsToManage.transform.childCount; i++)
        {
            try
            {
                Vector3Int targetTile = new Vector3Int(itemsToManage.transform.GetChild(i).GetComponent<GridItem>().SingleGridOccupied[0], itemsToManage.transform.GetChild(i).GetComponent<GridItem>().SingleGridOccupied[1], 0);
                int targetTileX = itemsToManage.transform.GetChild(i).GetComponent<GridItem>().SingleGridOccupied[0];
                int targetTileY = itemsToManage.transform.GetChild(i).GetComponent<GridItem>().SingleGridOccupied[1];
                Vector2 locationOffset = itemsToManage.transform.GetChild(i).GetComponent<GridItem>().SpriteOffset;

                itemsToManage.transform.GetChild(i).GetComponent<GridItem>().MoveToTileLocation(mTiles[targetTileX, targetTileY].gameObject.transform.position + new Vector3(locationOffset[0], locationOffset[1], 0));
            } catch (Exception e)
            {
                //Debug.Log(e.ToString());
                
            }
        }
    }

    //Update all object's node reference and tiles' object reference
    public void UpdateObjectAndTilesReferences()
    {
        for (int i = 0; i < itemsToManage.transform.childCount; i++)
        {
            itemsToManage.transform.GetChild(i).GetComponent<GridItem>().UpdateItemScriptWithSameNumbers();
            itemsToManage.transform.GetChild(i).GetComponent<GridItem>().UpdateAllMyNodes();
            itemsToManage.transform.GetChild(i).GetComponent<GridItem>().validateTilePlacementAndApplyTiles();
        }
    }

    public GameObject[] GetGameObjectsOnGrid()
    {
        GameObject[] tempArray = new GameObject[0];
        List<GameObject> itemsOnGrid = new List<GameObject>();
        for (int i = 0; i < itemsToManage.transform.childCount; i++)
        {
            //Check if  that item is on grid, if on, put item in list
            if (itemsToManage.transform.GetChild(i).GetComponent<GridItem>().SingleGridOccupied[0] != -1)
            {
                itemsOnGrid.Add(itemsToManage.transform.GetChild(i).gameObject);
            }

            //initialize array
            tempArray = new GameObject[itemsOnGrid.Count];
            //Foreach also won't work here :( cz  we need  to specify each item in the array :(((( its ugly but ok
            for (int j = 0; j < itemsOnGrid.Count; j++)
            {
                tempArray[j] = itemsOnGrid[j];
            }
        }
        return tempArray;
    }

    //Freeze all objects that are on the grid
    public void FreezeObjectsOnGrid()
    {
        if (GetGameObjectsOnGrid()[0] != null)
        {
            GameObject[] tempArray = GetGameObjectsOnGrid();
            foreach (GameObject item in tempArray)
            {
                item.GetComponent<GridItem>().CanBePickedUp = false;
            }
        }
    }

	//TAMIKO: Freeze all objects on fire that are on the grid
	public void FreezeObjectsOnFire() {
		if (GetGameObjectsOnGrid().Length > 0 && GetGameObjectsOnGrid()[0] != null) {
			GameObject[] tempArray = GetGameObjectsOnGrid();
			foreach (GameObject item in tempArray) {
				if(item.GetComponent<ItemScript>().isOnFire && item.GetComponent<ItemScript>().itemPos.x >= 0) {
					item.GetComponent<GridItem>().CanBePickedUp = false;
					item.GetComponent<ItemScript>().FreezeItem();
				}
			}
		}
	}

	public Vector2 GetGridSize()
    {
        return gridSize;
    }

    public Vector3 GetTileWorldPosition(Vector2Int coordinates)
    {
        return mTiles[coordinates[0], coordinates[1]].transform.position;
    }

    public bool TileIsOccupied(Vector2Int coordinates)
    {
        return mTiles[coordinates[0], coordinates[1]].GetComponent<IndividualTile>().objectThatOccupiedMySpace != null;
    }

    private void Update()
    {
        //Debug.Log(IsTileOccupied(new Vector2Int(0, 0)));
        //Debug.Log("Kenny Checking: " + GetTileWorldPosition(new Vector2Int(0, 0)));
        //Testing  GetGameObjectOnGrid()
        //if (GetGameObjectsOnGrid()[0] != null)
        //{
        //    GameObject[] tempArray = GetGameObjectsOnGrid();
        //    foreach (GameObject item in tempArray)
        //    {
        //        Debug.Log(item.gameObject.name);
        //    }
        //}
        //FreezeObjectOnGrid();
    }
}
