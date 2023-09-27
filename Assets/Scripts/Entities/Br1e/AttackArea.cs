using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace TOM
{
    [RequireComponent(typeof(BoxCollider))]
    public class AttackArea : MonoBehaviour
    {
        private float time = 0;
        private BoxCollider boxCollider = null;

        public System.Action<Entity> OnEnemyHit = null;

        List<TOM.Enemy.Enemy> hittedEnemyList = new List<Enemy.Enemy>();

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.enabled = false;
        }

        public void GenerateAttackArea(float durationSeconds, Vector3 initialPos)
        {
            if (!boxCollider.enabled)
            {
                StartCoroutine(WaitTime(durationSeconds));
                transform.position = initialPos;
            }
        }

        IEnumerator WaitTime(float timeToWait)
        {
            time = 0;
            boxCollider.enabled = true;
            while (time < timeToWait)
            {
                time += Time.deltaTime;
                yield return null;
            }
            boxCollider.enabled = false;
            hittedEnemyList.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Enemy")
            {

                TOM.Enemy.Enemy hittedEnemy = other.gameObject.GetComponent<TOM.Enemy.Enemy>();
                foreach (TOM.Enemy.Enemy enemy in hittedEnemyList)
                {
                    if (enemy == other)
                    {
                        return;
                    }
                }
                hittedEnemyList.Add(hittedEnemy);
                OnEnemyHit?.Invoke(hittedEnemy);
            }
        }

    }
}