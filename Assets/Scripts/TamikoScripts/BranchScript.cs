using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchScript : WoodScript {
	
	//Dog will "eat" branch if on it (teleports it to -99, -99)
	public void EatBranch() {
		gItem.MoveAway();
	}

}
