using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodScript : ItemScript {

	private int burnTime = -1;

	//wood burns off after burning for 1 time unit, then teleports to (-99, -99)
	public void CheckIfWoodShouldBurnOff(int currentTime) {
		if (burnTime == -1) {
			burnTime = currentTime;
		} else {
			//BurnOffBranch();
			gItem.MoveAway();
		}
	}

}
