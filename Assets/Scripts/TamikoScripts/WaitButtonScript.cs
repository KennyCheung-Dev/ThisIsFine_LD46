using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitButtonScript : MonoBehaviour
{

	private Button button;
	public Text buttonText;

	private void Start() {
		button = GetComponent<Button>();
	}

	public void DisableButton() {
		button.interactable = false;
	}

	public void LoseButton() {

		buttonText.text = "Oops!";
	}

	public void WinButton() {
		DisableButton();
		buttonText.text = "Yay!";
	}

	void EnableButton() {
		button.interactable = true;
	}

	public void DisableButtonForATime(float time) {
		DisableButton();
		Invoke("EnableButton", time);
	}


}
