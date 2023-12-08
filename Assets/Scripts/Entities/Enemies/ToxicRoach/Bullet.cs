using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private MeshRenderer[] meshArray;
    private TrailRenderer trail;
    private BoxCollider col;

    private int damage = 10;
    public int Damage => damage;

    private bool flying = false;
    public bool Flying => flying;

    private float bulletSpeed;

    private void Awake()
    {
        meshArray = GetComponentsInChildren<MeshRenderer>();
        trail = GetComponentInChildren<TrailRenderer>();
        col = GetComponent<BoxCollider>();
    }

    public void StartFlying(int shotDamage, int speed)
    {
        flying = true;

        foreach (MeshRenderer mesh in meshArray)
        {
            mesh.enabled = true;
        }

        damage = shotDamage;
        bulletSpeed = speed;
    }

    public void StopFlying()
    {
        flying = false;
        foreach (MeshRenderer mesh in meshArray)
        {
            mesh.enabled = false;
        }

        trail.time = 0.23f;
        col.enabled = false;
    }

    private void Update()
    {
        if (!flying)
        {
            return;
        }
        transform.position += transform.forward * (Time.deltaTime * bulletSpeed);
    }

    private void OnTriggerExit(Collider other)
    {
        if (flying)
        {
            if (other.CompareTag("DeathZone"))
            {
                StopFlying();
            }
        }
    }

    private int DoDamage() => damage;
}
