using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using IA.FSM;
using System;
using Random = UnityEngine.Random;

namespace TOM.Enemy.CR
{
    public class WalkingState : State
    {
        private float randomRadius = 1;
        public override List<Action> GetBehaviours(StateParameters parameters)
        {
            Rigidbody rb = parameters.Parameters[0] as Rigidbody;
            Vector3 targetPosition = (Vector3)parameters.Parameters[1];
            float speed = (float)parameters.Parameters[2];

            List<Action> behabiours = new List<Action>();

            randomRadius = UnityEngine.Random.Range(0.5f, 2);

            behabiours.Add(() =>
                {
                    Vector3 direction = targetPosition - rb.position;
                    direction.y = 0;
                    if (direction.magnitude < randomRadius)
                    {
                        Transition((int)Flags.OnArenaArrived);
                    }
                    direction.Normalize();
                    rb.AddForce(direction * speed);
                }
            );

            return behabiours;
        }

        public override List<Action> GetOnEnterBehaviours(StateParameters parameters)
        {
            return null;
        }

        public override List<Action> GetOnExitBehaviours(StateParameters parameters)
        {
            return null;
        }

        public override object[] GetOutputs()
        {
            return null;
        }

        public override void SetParameters(StateParameters parameters)
        {
        }

        public override void Transition(int flag)
        {
            SetFlag?.Invoke(flag);
        }

    }

}