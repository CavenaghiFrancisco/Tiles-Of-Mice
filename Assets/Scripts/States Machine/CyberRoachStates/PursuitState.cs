using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IA.FSM;

namespace TOM.Enemy.CR
{
    public class PursuitState : State
    {
        public static bool ForceState;
        public override List<Action> GetBehaviours(StateParameters parameters)
        {
            //gameObject.transform, target, cyberRoach.GetMovementSpeed(), cyberRoach.GetAttackRadius()
            Rigidbody rb = parameters.Parameters[0] as Rigidbody;
            Transform target = parameters.Parameters[1] as Transform;
            int speed = (int)parameters.Parameters[2];
            int radius = (int)parameters.Parameters[3];

            List<Action> behabiours = new List<Action>();
            behabiours.Add(() =>
            {
                Vector3 direction = (target.position - rb.position).normalized;
                rb.MovePosition(direction * Time.deltaTime * speed);
                if(radius<Vector3.Distance(target.position,rb.position))
                {
                    Debug.Log("Alcance a Br1e!");
                    Transition((int)Flags.OnReachedTarget);
                }
                else
                {
                    Debug.Log("Sigo persiguiendo a Br1e!");
                }
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
