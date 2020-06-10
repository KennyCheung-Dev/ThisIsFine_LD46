using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalStuff;

public class MoveManager : MonoBehaviour {

	public DogScript dog;
	public RoombaScript[] allRoombas;

	public void Start() {
		GameObject[] allItems = TimeManagerScript.instance.absolutelyAllItems;
		for(int i = 0; i < allItems.Length; i++) {
			if(allItems[i].GetComponent<DogScript>()) {
				dog = allItems[i].GetComponent<DogScript>();
				break;
			}
		}
		GameObject[] roombas = GameObject.FindGameObjectsWithTag("Roomba");
		allRoombas = new RoombaScript[roombas.Length];
		for(int i = 0; i < allRoombas.Length; i++) {
			allRoombas[i] = roombas[i].GetComponent<RoombaScript>();
		}

	}

	//TODO: sort all objects based on xy grid coord?
	public void MoveAllNeeded() {
		Vector2Int max = TimeManagerScript.instance.gridSize;
		//move the dog
		if (!dog.gotCoffee && !dog.isOnFire) {
			dog.SenseSticks();
		}

		for(int i = 0; i < allRoombas.Length; i++) {
			if(allRoombas[i].itemPos.x >= 0) { //not in inventory
				allRoombas[i].MoveRoomba(max.x, max.y);
			}
		}

	}

}
