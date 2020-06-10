using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuLogic : MonoBehaviour
{
	public GameObject LevelSelect1, LevelSelect2, mainMenu, highscore ;
    public Image[] MainMenuImage;
    public Text[] MainMenuText;
    public RectTransform mainScreen, levels;
    public RectTransform MaskStart, MaskEnd;
    bool _scrolling = false;
    bool _levelScroll = false;
    AudioSource audio;
    float Offset;
	private GameObject LevelSelect;
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        Offset = MaskStart.position.x;
    }
    // Start is called before the first frame update
    private void Update()
    {
        if (_scrolling)
        {
            if (mainScreen.anchoredPosition.x < 400)
                mainScreen.Translate(new Vector3(.3f, 0, 0));
        } else
        {
            if (mainScreen.anchoredPosition.x > -400)
                mainScreen.Translate(new Vector3(-.3f, 0, 0));
        }
        if (_levelScroll)
        {
            if (MaskEnd.position.x > Offset)
                levels.Translate(new Vector3(-.3f, 0, 0));
            //Debug.Log(LevelSelectMask.anchoredPosition.x + " " + MaskStart.anchoredPosition.x
                //+ " " + MaskEnd.anchoredPosition.x);
        } else
        {
            if (MaskStart.position.x < Offset)
                levels.Translate(new Vector3(.3f, 0, 0));

        }

    }

    public void LevelSelectButton1()
    {
        audio.Play();
		LevelSelect = LevelSelect1;
        StartCoroutine(FadeAway());
    }

	public void LevelSelectButton2() {
		audio.Play();
		LevelSelect = LevelSelect2;
		StartCoroutine(FadeAway());
	}


	IEnumerator FadeAway()
    {
        for (float i = 1; i > 0; i -= 0.25f)
        {
            foreach (Image image in MainMenuImage)
            {
                Color temp = image.color;
                temp.a = i;
                image.color = temp;
            }
            foreach (Text text in MainMenuText)
            {
                Color temp = text.color;
                temp.a = i;
                text.color = temp;
            }
            yield return new WaitForSeconds(.1f);
        }
        mainMenu.SetActive(false);
        LevelSelect.SetActive(true);
    }

    IEnumerator FadeBack()
    {
        
        LevelSelect.SetActive(false);
        mainMenu.SetActive(true);
        for (float i = 0; i <= 1; i += 0.25f)
        {
            foreach (Image image in MainMenuImage)
            {
                Color temp = image.color;
                temp.a = i;
                image.color = temp;
            }
            foreach (Text text in MainMenuText)
            {
                Color temp = text.color;
                temp.a = i;
                text.color = temp;
            }
            yield return new WaitForSeconds(.1f);
        }
    }



    public void RollCredits()
    {
        audio.Play();
        _scrolling = true;
    }
   
    public void UnRollButton()
    {
        audio.Play();
        _scrolling = false;
    }
    public void MainMenuButton()
    {
        audio.Play();
        StartCoroutine(FadeBack());
    }
    public void QuitButton()
    {
        audio.Play();
        StartCoroutine(DelayQuit());
    }
    IEnumerator DelayQuit()
    {
        yield return new WaitForSeconds(.5f);
        Application.Quit();
    }
    public void LoadLevel(int n)
    {
        audio.Play();
        StartCoroutine(DelayLoadLevel(n));
    }
    IEnumerator DelayLoadLevel(int n)
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(n);
    }
    public void LevelSelectScrollRight()
    {
        audio.Play();
        _levelScroll = true;
    }
    public void LevelSelectScrollLeft()
    {
        audio.Play();
        _levelScroll = false;
    }
}
