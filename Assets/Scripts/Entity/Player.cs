using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    private Movement playerMovement;
    bool invulnerable = false;

    private void Start()
    {
        playerMovement = GetComponent<Movement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (invulnerable)
            return;

        if (other.CompareTag("Damager"))
        {
            SetLife(GetLife() - 20);
        }
        if(GetLife() <= 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        playerMovement.StopAllMovement();
        playerMovement.enabled = false;
        StartCoroutine(StartRespawn());
    }

    private void SetInvulnerable(bool makeInvulnerable)
    {
        invulnerable = makeInvulnerable;
    }

    private IEnumerator StartRespawn()
    {
        SetInvulnerable(true);
        yield return new WaitForSeconds(2);
        transform.position = new Vector3(0, -1.2f, 0);
        playerMovement.enabled = true;
        SetLife(GetMaxLife());
        yield return new WaitForSeconds(1);
        SetInvulnerable(false);
    }
}
