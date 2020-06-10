using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IndividualTile : MonoBehaviour
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

    //Variables for RayCast check mouse
    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    [SerializeField] private float tileWidthThreshold = 0.5f;
    public GameObject Table;
    public Vector2Int myTile;
    public GridManager m_gridManager;
    public bool currentlyHovered = false;
    public GameObject objectThatOccupiedMySpace;
    // Start is called before the first frame update
    void Start()
    {
        //Getting reference
        //Table = GameObject.FindGameObjectWithTag("ItemsToManage").gameObject.transform.Find("Table").gameObject;
        m_gridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();

        //Raycasts
        raycaster = GetComponent<GraphicRaycaster>();

        eventSystem = GetComponent<EventSystem>();
    }

    public void OnMouseDown()
    {
    }

    public void Update()
    {
        //Was setting hover bool to false in OnMouseExit(),
        //but apprently the mouseExit won't be checked when I have an item on hand
        //So I ll just check in update
        if (!Input.GetKey(KeyCode.Mouse0))  GetComponent<Animator>().SetBool("hover", false);

        //if (Input.GetKeyUp(KeyCode.Mouse0))
        //{
        //    pointerEventData = new PointerEventData(eventSystem);
        //    pointerEventData.position = Input.mousePosition;

        //    List<RaycastResult> results = new List<RaycastResult>();

            
        //    raycaster.Raycast(pointerEventData, results);
        //    foreach(RaycastResult result in results)
        //    {
        //        Debug.Log(result.gameObject.name);
        //    }
        //}


        Vector3 pointerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Mathf.Abs(pointerPos[0] - transform.position.x) < tileWidthThreshold && Mathf.Abs(pointerPos[1] - transform.position.y) < tileWidthThreshold)
        {
            currentlyHovered = true;
            if (Input.GetKey(KeyCode.Mouse0)) GetComponent<Animator>().SetBool("hover", true);
        } else
        {
            currentlyHovered = false;
            if (Input.GetKey(KeyCode.Mouse0)) GetComponent<Animator>().SetBool("hover", false);
        }
    }

    //Mouse Enter set animation and bool to true
    public void OnMouseEnter()
    {
        //currentlyHovered = true;
        //if (Input.GetKey(KeyCode.Mouse0)) GetComponent<Animator>().SetBool("hover", true);
    }

    //Mouse exit, set animation and bool to false
    public void OnMouseExit()
    {
        //currentlyHovered = false;
        //if (Input.GetKey(KeyCode.Mouse0)) GetComponent<Animator>().SetBool("hover", false);
    }
}
