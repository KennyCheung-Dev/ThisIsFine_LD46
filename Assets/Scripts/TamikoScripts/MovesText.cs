using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovesText : MonoBehaviour
{

	private Text myText;
	public int maxMoves;

	private void Awake() {
		myText = GetComponent<Text>();
	}

	public void Updatetext(int curMoves, int maxMoves) {
		if(maxMoves > 0) {
			myText.text = "MOVES:\n" + curMoves + " / " + maxMoves;
		} else {
			myText.text = "MOVES:\n" + curMoves;
		}
	}

}
