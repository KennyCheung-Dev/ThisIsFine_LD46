using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FineOMeterScript : MonoBehaviour
{
    public Text concernText, level;
    Slider _slider;
    public int value = 0;
    public int maxValue = 7;
    public string[] states;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        
        StartCoroutine( UpdateValue());
    }

	public void ChangeMaxValue(int x) {
		maxValue = x;
		_slider.maxValue = maxValue;
	}

	public void ChangeValue(int x) {
		value = x;
	}

    IEnumerator UpdateValue()
    {
        int currentvalue = value;
        while (true)
        {
            _slider.value = value;
			if(value >= maxValue) {
				concernText.text = states[states.Length - 1];
			} else if (((int)((float)value / maxValue * states.Length)) < states.Length) {
                concernText.text = states[(int)((float)value / maxValue * states.Length)];
			}
            level.text = value + " / " + (maxValue);
            currentvalue = value;
            yield return new WaitUntil(() => currentvalue != value);
        }
    }
}
