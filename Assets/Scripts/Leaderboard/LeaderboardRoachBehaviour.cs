using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardRoachBehaviour : MonoBehaviour
{
    private Animator anim;
    private Coroutine walkCoroutine;
    private float distance = 0;
    private const float maxDistance = 7;
    int walkAnimationHash = Animator.StringToHash("CyberRoach_Walk");
    int idleAnimationHash = Animator.StringToHash("CyberRoach_Idle");
    int deathAnimationHash = Animator.StringToHash("CyberRoach_Death");

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private IEnumerator MoveFoward()
    {
        anim.Play(walkAnimationHash);
        while (distance < maxDistance)
        {
            Vector3 fowardDistance = transform.forward * Time.deltaTime * 3;
            transform.position += fowardDistance;
            distance += fowardDistance.z;
            yield return null;
        }
        yield return null;
    }

    public void Walk()
    {
        walkCoroutine = StartCoroutine(MoveFoward());
    }

    public void StopWalk()
    {
        StopCoroutine(walkCoroutine);
        anim.Play(idleAnimationHash);
    }

    public void Die()
    {
        anim.Play(deathAnimationHash);
    }

}
