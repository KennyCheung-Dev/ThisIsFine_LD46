using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class HighScore : MonoBehaviour
{
    public static HighScore Instance;
    public int[] scores;
    public GameObject highscorePrefab;
    // Start is called before the first frame update
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            scores = new int[SceneManager.sceneCountInBuildSettings - 1];
            SceneManager.sceneLoaded += ChangedScene;
        } else
        {
            Destroy(gameObject);
        }

    }
    private void ChangedScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            GameObject board = GameObject.FindGameObjectWithTag("HighScoreBoard").GetComponent<MainMenuLogic>().highscore;
            if (ShowScore())
            {
                Vector3 StartPosition = board.transform.Find("Highscore").transform.position;
                for (int i = 0; i < scores.Length; i++)
                {
                    GameObject g = Instantiate(highscorePrefab, StartPosition + new Vector3(i * .4f + .7f , 0, 0)
                        , new Quaternion(), board.transform);
                    g.GetComponent<Text>().text = (i + 1) + "\n" + scores[i] + "\n" + grabPar(i+1);
                }
            }
            else
            {
                board.SetActive(false);
            }
        }
    }
    public bool ShowScore()
    {
        foreach (int i in scores)
        {
            if (i > 0)
            {
                return true;
            }
        }
        return false;
    }
    public void UpdateScore(int score)
    {
        int position = SceneManager.GetActiveScene().buildIndex - 1;
        if (scores[position] == 0 || scores[position] > score)
        {
            scores[position] = score;
        }
    }
    public int grabPar(int pos)
    {
        switch (pos)
        {
            case 1:
                return 3;
            case 2:
                return 7;
            case 3:
                return 7;
            case 4:
                return 5;
            case 5:
                return 7;
            case 6:
                return 11;
            case 7:
                return 12;
            case 8:
                return 4;
            case 9:
                return 9;
            case 10:
                return 12;
            case 11:
                return 19;
            case 12:
                return 11;
            case 13:
                return 12;
            case 14:
                return 16;
            case 15:
                return 12;
            default:
                return 0;
        }
    }

}
