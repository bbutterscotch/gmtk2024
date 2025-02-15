using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public static SceneController instance {  get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        } else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Tilemap", LoadSceneMode.Single);
    }

    public void LoadTitle()
    {
        SceneManager.LoadScene("Title", LoadSceneMode.Single);
    }
}
