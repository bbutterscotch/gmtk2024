using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioController : MonoBehaviour
{
    public static AudioController instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetParameter(EventReference sound, string parameterName, float parameterValue, Vector3 worldPosition)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(worldPosition));
        eventInstance.setParameterByName(parameterName, parameterValue);
        eventInstance.start();
        eventInstance.release(); // Release the event instance after playing it
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPosition)
    {
        RuntimeManager.PlayOneShot(sound, worldPosition);
    }
}
