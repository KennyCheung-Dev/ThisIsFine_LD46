using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalStuff;

//generic base class for items
[SelectionBase]
public abstract class ItemScript : MonoBehaviour {

	public Vector2Int itemPos = new Vector2Int(-1, -1); //x, y positions of item origin
	
	[Header("For reference use only, don't edit these values in inspector")]
	[SerializeField]
	private Vector2Int[] itemPosAll; //positions which the entire object occupies relative to itemPos (also includes itemPos)
	[Header("")]

	public bool isFlammable = true; //can this be set on fire even (true by default)
	public bool isOnFire; //is the item on fire?
	public bool isFrozen = false; //can it be moved in the grid?
	public Shape itemShape; //grid shape of item
	public GameObject[] fireObject;
	[HideInInspector]
	public GridItem gItem;
	[HideInInspector]
	//public GridManager gManager;
	private GameObject border;
	


	public virtual void Start() {
		border = FindGameObjectInChildWithTag(gameObject, "ItemBorder");
		fireObject = FindGameObjectsInChildWithTag(gameObject, "Fire");
		gItem = GetComponent<GridItem>();
		SetItemPosition(itemPos);
		SetFire(isOnFire);
		if (isFrozen) FreezeItem();
	}

	//SAFE TO USE: yes
	//freezes an item (not on the grid), darkens it a little
	public void FreezeItem() {
		isFrozen = true;
		border.SetActive(false);
	}

	//SAFE TO USE: yes
	//sets the item's position (and all its occupied spots) relative to p 
	//also sets itemPos to p
	//p: position to set the items origin to
	public void SetItemPosition(Vector2Int p) {
		itemPos = p; //this now also updates itemPos
		switch (itemShape) {
			case Shape.single:
				itemPosAll = new Vector2Int[1];
				itemPosAll[0] = p;
				break;
			case Shape.vert2:
				itemPosAll = new Vector2Int[2];
				itemPosAll[0] = p;
				itemPosAll[1] = new Vector2Int(p.x, p.y + 1);
				break;
			case Shape.hor2:
				itemPosAll = new Vector2Int[2];
				itemPosAll[0] = p;
				itemPosAll[1] = new Vector2Int(p.x + 1, p.y);
				break;
			case Shape.l:
				itemPosAll = new Vector2Int[3];
				itemPosAll[0] = p;
				itemPosAll[1] = new Vector2Int(p.x, p.y + 1);
				itemPosAll[2] = new Vector2Int(p.x + 1, p.y + 1);
				break;
			case Shape.r:
				itemPosAll = new Vector2Int[3];
				itemPosAll[0] = p;
				itemPosAll[1] = new Vector2Int(p.x + 1, p.y);
				itemPosAll[2] = new Vector2Int(p.x, p.y + 1);
				break;
			case Shape.rback:
				itemPosAll = new Vector2Int[3];
				itemPosAll[0] = p;
				itemPosAll[1] = new Vector2Int(p.x + 1, p.y);
				itemPosAll[2] = new Vector2Int(p.x + 1, p.y + 1);
				break;
			case Shape.large:
				itemPosAll = new Vector2Int[4];
				itemPosAll[0] = p;
				itemPosAll[1] = new Vector2Int(p.x + 1, p.y);
				itemPosAll[2] = new Vector2Int(p.x, p.y + 1);
				itemPosAll[3] = new Vector2Int(p.x + 1, p.y + 1);
				break;
		}
	}

	//SAFE TO USE: yes
	public Vector2Int[] GetItemPosition() {
		return itemPosAll;
	}
	
	//SAFE TO USE: yes
	//checks if ANY PART of the item occupies the position at pos
	//pos: position to compare the item to
	//returns whether part of this item occupies pos or not
	public bool CompareToItemPosition(Vector2Int pos) {
		foreach(Vector2Int vi in itemPosAll) {
			if (vi == pos) return true;
		}
		return false;
	}

	//SAFE TO USE: no
	//returns a list of all positions surrounding the object for fire spread
	//xMax: max size of gridmap x, 
	//yMax: max size of gridmap y
	//returns Vector2int list of positions which fire can spread to 
	//	(might include empty positions and positions already on fire)
	public List<Vector2Int> GetSurroundingFireSpace(int xMax, int yMax) {
		List<Vector2Int> fireSpaces = new List<Vector2Int>();
		for(int i = 0; i < itemPosAll.Length; i++) {
			Vector2Int p = itemPosAll[i];
			if (p.y - 1 >= 0) fireSpaces.Add(new Vector2Int(p.x, p.y - 1)); //up
			if (p.y + 1 <= xMax) fireSpaces.Add(new Vector2Int(p.x, p.y + 1)); //down
			if (p.x - 1 >= 0) fireSpaces.Add(new Vector2Int(p.x - 1, p.y)); //left
			if (p.x + 1 <= yMax) fireSpaces.Add(new Vector2Int(p.x + 1, p.y)); //right
		}
		return fireSpaces;
	}

	//SAFE TO USE: no
	//set this thing on fire depending on b
	//b: set this item on fire or not
	public void SetFire(bool b) {
		isOnFire = b;
		foreach (GameObject f in fireObject) { 
			f.SetActive(b);
		}
	}

	
}
