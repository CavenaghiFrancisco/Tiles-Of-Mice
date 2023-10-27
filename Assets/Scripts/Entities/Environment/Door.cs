using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private static BoxCollider doorCollider = null;

    private void Awake()
    {
        doorCollider = GetComponent<BoxCollider>();
    }

    public static void OpenDoor()
    {
        doorCollider.isTrigger = true;
    }

    public static void CloseDoor()
    {
        doorCollider.isTrigger = false;
    }
}
