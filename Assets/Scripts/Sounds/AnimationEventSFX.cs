using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventSFX : MonoBehaviour
{
    public void PostAudio(string name)
    {
        AkSoundEngine.PostEvent(name, gameObject);
    }
}
