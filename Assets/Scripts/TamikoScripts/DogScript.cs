using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalStuff;

//extends from ItemScript
public class DogScript : ItemScript
{
	
	public int sensingRange = 15;
	public bool gotCoffee = false;
	public Sprite gotCoffeSprite;
	private CoffeeScript coffee;
	private Animator anim;
	private float moveAnimSpeed;
	private SpriteRenderer sRenderer;

	public override void Start() {
		base.Start();
		coffee = GameObject.FindGameObjectWithTag("Coffee").GetComponent<CoffeeScript>();
		sRenderer = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		moveAnimSpeed = TimeManagerScript.instance.moveAnimationSpeed;
	}


	public void SenseSticks() {

		GameObject[] items = TimeManagerScript.instance.allItemsOnGrid;
		List<ItemScript> iScripts = new List<ItemScript>();
		List<BranchScript> bScripts = new List<BranchScript>();
		for (int i = 0; i < items.Length; i++) {
			if (items[i].gameObject.GetComponent<BranchScript>()) {
				bScripts.Add(items[i].GetComponent<BranchScript>());
			}  else {
				iScripts.Add(items[i].GetComponent<ItemScript>());
			}
		}

		int count = 1;
		bool uBlocked = false, dBlocked = false, lBlocked = false, rBlocked = false;
		BranchScript br = null;
		while (count <= sensingRange) {
			//up
			if(!uBlocked && SenseSticksHelper(0, -count, gridSize, gridSize, iScripts, bScripts, ref uBlocked, ref br)) {
				if (br != null) br.EatBranch(); //eat branch if on top
				anim.SetTrigger("MoveBack");
				gItem.MoveOneUnitBySeconds(Direction.up, moveAnimSpeed);
				break;
			}
			//right
			if (!rBlocked && SenseSticksHelper(count, 0, gridSize, gridSize, iScripts, bScripts, ref rBlocked, ref br)) {
				if (br != null) br.EatBranch(); //eat branch if on top
				anim.SetTrigger("MoveRight");
				gItem.MoveOneUnitBySeconds(Direction.right, moveAnimSpeed);
				break;
			}
			//down
			if (!dBlocked && SenseSticksHelper(0, count, gridSize, gridSize, iScripts, bScripts, ref dBlocked, ref br)) {
				if (br != null) br.EatBranch(); //eat branch if on top
				anim.SetTrigger("MoveFront");
				gItem.MoveOneUnitBySeconds(Direction.down, moveAnimSpeed);
				break;
			}
			//left
			if (!lBlocked && SenseSticksHelper(-count, 0, gridSize, gridSize, iScripts, bScripts, ref lBlocked, ref br)) {
				if (br != null) br.EatBranch(); //eat branch if on top
				anim.SetTrigger("MoveLeft");
				gItem.MoveOneUnitBySeconds(Direction.left, moveAnimSpeed);
				break;
			}
			count++;
		}
		
		if(coffee != null && coffee.itemPos.x >= 0 && coffee.itemPos.y >= 0 && coffee.CompareToItemPosition(itemPos)) {
			GoCoffeeMode();
		}
		
		
	}

	private void GoCoffeeMode() {
		coffee.DrinkCoffee();
		//anim.enabled = false;
		Invoke("SetSprite", moveAnimSpeed);
		gotCoffee = true;
	}

	private void SetSprite() {
		anim.enabled = false;
		sRenderer.sprite = gotCoffeSprite;
	}
	
	private bool SenseSticksHelper(int xDisp, int yDisp, int xMax, int yMax, List<ItemScript> allItems, List<BranchScript> bScripts, ref bool blocked, ref BranchScript eatIt) {
		
		Vector2Int posCheck = new Vector2Int(itemPos.x + xDisp, itemPos.y + yDisp);
		if (posCheck.x > xMax || posCheck.x < 0 
			|| posCheck.y > yMax || posCheck.y < 0){
			blocked = true;
			return false; //break if beyond the grid 
		}
		for(int i = 0; i < allItems.Count; i++) {
			if(allItems[i].CompareToItemPosition(posCheck) && 
				!allItems[i].gameObject.GetComponent<BranchScript>() && !allItems[i].gameObject.GetComponent<CoffeeScript>()) {
				blocked = true;
				return false; //break if blocked off from a branch
			}
		}
		for (int i = 0; i < bScripts.Count; i++) {
			if (bScripts[i].CompareToItemPosition(posCheck) && !bScripts[i].isOnFire) {
				if(Mathf.Abs(xDisp) == 1 || Mathf.Abs(yDisp) == 1)  eatIt = bScripts[i];
				return true; //finds if it contains branch script and not on fire
			} else if(bScripts[i].CompareToItemPosition(posCheck) && bScripts[i].isOnFire) {
				blocked = true;
				return false; //break if blocked off with burning branch
			}
		}
		
		return false;

	}


}
