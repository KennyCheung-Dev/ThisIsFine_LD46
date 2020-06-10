using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    AudioSource audio;
    public int sceneIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void NextScene() {
        audio.Play();
        StartCoroutine(DelayNextScene());
	}
    IEnumerator DelayNextScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnPress()
    {
        audio.Play();
        StartCoroutine(DelayOnPress());
    }
    IEnumerator DelayOnPress()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneIndex);
    }
}
