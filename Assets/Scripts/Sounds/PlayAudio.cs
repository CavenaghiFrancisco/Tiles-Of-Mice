using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AkGameObj))]
public class PlayAudio : MonoBehaviour
{
    [SerializeField] private string eventName;
    [SerializeField] private bool playOnEnable;
    [SerializeField] private bool playOnStart;

    private void OnEnable()
    {
        if (playOnEnable)
        {
            AkSoundEngine.PostEvent(eventName, gameObject);
        }
    }

    private void Start()
    {
        if (playOnStart)
        {
            AkSoundEngine.PostEvent(eventName, gameObject);
        }
    }
}
