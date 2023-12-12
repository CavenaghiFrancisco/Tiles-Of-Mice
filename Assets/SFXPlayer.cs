using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public void PlaySFX(string name)
    {
        AkSoundEngine.PostEvent(name, this.gameObject);
    }
}
