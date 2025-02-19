using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    public static AudioController instance { get; private set; }

        private Dictionary<EventReference, EventInstance> activeInstances = new Dictionary<EventReference, EventInstance>();

    [SerializeField] private EventReference music;
    [SerializeField] private EventReference menuMusic;
    private EventInstance musicEventInstance;
    private EventInstance menuMusicEventInstance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Audio Manager in the scene.");
            Destroy(gameObject);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        SetMusicBasedOnScene(SceneManager.GetActiveScene().name);
    }
    
    private void Update()
    {

    }



    private void SetMusicBasedOnScene(string sceneName)
    {
        if (sceneName == "Tilemap")
        {
            InitializeMusic(music);
        }
        else if (sceneName == "Title")
        {
            InitializeMusic(menuMusic);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetMusicBasedOnScene(scene.name);
    }


    private void InitializeMusic (EventReference musicEventReference)
    {
        StopMusic();
        musicEventInstance = RuntimeManager.CreateInstance(musicEventReference);
        SetAllParameters(0);
        musicEventInstance.start();
    }

    private void StopMusic ()
    {
        if (musicEventInstance.isValid())
        {
            musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicEventInstance.release(); // Release the event instance to free resources
        }
    }

    public void SetMusicParameter(string parameterName, float value)
    {
        if (musicEventInstance.isValid())
        {
            musicEventInstance.setParameterByName(parameterName, value);
        }
        else
        {
            Debug.LogWarning("Music Event Instance is not valid.");
        }
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPosition)
    {
        RuntimeManager.PlayOneShot(sound, worldPosition);
    }

    public void SetAllParameters(float parameterValue)
    {
        SetMusicParameter("Apiary", parameterValue);
        SetMusicParameter("Armoury", parameterValue);
        SetMusicParameter("Beekeeper", parameterValue);
        SetMusicParameter("Forest", parameterValue);
        SetMusicParameter("Garden", parameterValue);
        SetMusicParameter("Meadow", parameterValue);
        SetMusicParameter("Nursery", parameterValue);
        SetMusicParameter("Park", parameterValue);
        SetMusicParameter("Super", parameterValue);
        SetMusicParameter("Woodland", parameterValue);

        //musicEventInstance.setParameterByName("Apiary", parameterValue);
        //musicEventInstance.setParameterByName("Armoury", parameterValue);
        //musicEventInstance.setParameterByName("Beekeeper", parameterValue);
        //musicEventInstance.setParameterByName("Forest", parameterValue);
        //musicEventInstance.setParameterByName("Garden", parameterValue);
        //musicEventInstance.setParameterByName("Meadow", parameterValue);
        //musicEventInstance.setParameterByName("Nursery", parameterValue);
        //musicEventInstance.setParameterByName("Park", parameterValue);
        //musicEventInstance.setParameterByName("Super", parameterValue);
        //musicEventInstance.setParameterByName("Woodland", parameterValue);
    }
}
