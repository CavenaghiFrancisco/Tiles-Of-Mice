using System.Collections;
using UnityEngine;

public class LeaderboardBr1eBehaviour : MonoBehaviour
{
    private Animator anim;
    private Coroutine walkCoroutine;
    private float distance = 0;
    private const float maxDistance = 7;
    int attackAnimationHash = Animator.StringToHash("Attack1Loop");
    int walkAnimationHash = Animator.StringToHash("RunLoop");
    int idleAnimationHash = Animator.StringToHash("IdleLoop");

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

    public void Attack()
    {
        anim.Play(attackAnimationHash);
    }
}
