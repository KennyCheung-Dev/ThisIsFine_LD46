using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireManagerScript : MonoBehaviour
{

	//TODO: differentiate between inventory items and map items
	//public GameObject[] allItemsOnMap;
	
	//creates a list of all positions fire will spread to 
	//but doesn't set them on fire (used for fire indicator)
	//- returns positions for empty spaces as well but not occupied positions already on fire
	//returns List<Vector2Int> of position to set fire to
	//TODO: double check if actually works
	public List<Vector2Int> GetFireSpreadPositions() {
		GameObject[] allItemsOnMap = TimeManagerScript.instance.allItemsOnGrid;
		List<Vector2Int> toFire = SpreadFireHelper();
		toFire = toFire.Distinct().ToList();
		for (int i = toFire.Count - 1; i >= 0; i--) {
			for (int j = 0; j < allItemsOnMap.Length; j++) {
				ItemScript scr = allItemsOnMap[i].GetComponent<ItemScript>();
				if (scr.CompareToItemPosition(toFire[j]) && scr.isOnFire) {
					toFire.Remove(toFire[j]); //remove if the current spot is occupied and on fire
					break;
				}
			}
		}
		return toFire;
	}

	//creates a list of all positions which fire can spread to
	//then checks if any flammable objects occupy that position
	//then sets em on fire 
	public void SpreadFire() {
		GameObject[] allItemsOnMap = TimeManagerScript.instance.allItemsOnGrid;
		List<Vector2Int> toFire = SpreadFireHelper();

		toFire = toFire.Distinct().ToList();
		for(int i = 0; i < allItemsOnMap.Length; i++) {
			ItemScript scr = allItemsOnMap[i].GetComponent<ItemScript>();
			if(!scr.isOnFire && scr.isFlammable && scr.itemPos.x >= 0) { //only set to fire if it's flammable and not yet on fire
				for(int j = 0; j < toFire.Count; j++) {
					if(scr.CompareToItemPosition(toFire[j])) {
						scr.SetFire(true);
						break;
					}
				}
			}
		}
	}

	//helper method for SpreadFire() and GetFireSpreadPositions()
	private List<Vector2Int> SpreadFireHelper() {
		GameObject[] allItemsOnMap = TimeManagerScript.instance.allItemsOnGrid;
		List<Vector2Int> toFire = new List<Vector2Int>();
		for (int i = 0; i < allItemsOnMap.Length; i++) {
			ItemScript scr = allItemsOnMap[i].GetComponent<ItemScript>();
			if (scr.isOnFire) {
				toFire.AddRange(scr.GetSurroundingFireSpace(GlobalStuff.gridSize, GlobalStuff.gridSize));
			}
		}
		return toFire;
	}

	public void MoveAwayAllFiresIfNeeded(int currentTime) {
		//find all branches and wood blocks\
		GameObject[] allItems = TimeManagerScript.instance.allItemsOnGrid;
		List<WoodScript> allWood = new List<WoodScript>();
		for(int i = 0; i < allItems.Length; i++) {
			//TODO: add wood script
			if(allItems[i].GetComponent<WoodScript>()) {
				allWood.Add(allItems[i].GetComponent<WoodScript>());
			}
		}
		for(int i = 0; i < allWood.Count; i++) {
			if(allWood[i].isOnFire) {
				allWood[i].CheckIfWoodShouldBurnOff(currentTime);
			}
		}

	}

}
