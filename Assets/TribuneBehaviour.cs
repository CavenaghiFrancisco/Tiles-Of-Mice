using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CROWD_STATE
{
    HIGH = 2,
    MEDIUM = 1,
    LOW = 0
}

public class TribuneBehaviour : MonoBehaviour
{
    private CROWD_STATE state;
    private float initialSpeed;
    private float amplitude = 0.5f;
    [SerializeField] private GameObject[] objects;
    private float[] initialPhases;

    void Start()
    {
        state = CROWD_STATE.HIGH;
        initialSpeed = (int)state;
        initialPhases = new float[objects.Length];

        for (int i = 0; i < objects.Length; i++)
        {
            initialPhases[i] = Random.Range(0f, 2f * Mathf.PI);
        }
    }

    void Update()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            GameObject obj = objects[i];
            Transform objTransform = obj.transform;

            float newY = Mathf.Sin(Time.time * initialSpeed + initialPhases[i]) * amplitude;
            objTransform.position = new Vector3(objTransform.position.x, newY, objTransform.position.z);
        }
    }
}
