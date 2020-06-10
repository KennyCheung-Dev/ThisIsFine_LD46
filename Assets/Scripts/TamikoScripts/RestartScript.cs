using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour
{
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void RestartLevel() {
        audio.Play();
        StartCoroutine(DelayRestart());
    }
    IEnumerator DelayRestart()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoHome()
    {
        audio.Play();
        StartCoroutine(DelayHome());
    }
    IEnumerator DelayHome()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(0);
    }
}
