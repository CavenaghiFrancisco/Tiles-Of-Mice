using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using IA.FSM;
using System;

namespace TOM.Enemy.CR
{
    public class PursuitState : State
    {
        private Rigidbody thisRB = null;
        private Transform br1eTransform = null;
        private int speed = 0;
        private float radius = 0;
        private bool isTargetAlive = false;
        public override List<Action> GetBehaviours(StateParameters parameters)
        {
            SetParameters(parameters);

            List<Action> behabiours = new List<Action>();
            
            behabiours.Add(() =>
            {
                Vector3 direction = (br1eTransform.position - thisRB.position).normalized;
                thisRB.MovePosition(thisRB.position + direction * Time.fixedDeltaTime * speed);
                if (Vector3.Distance(br1eTransform.position, thisRB.position) < radius)
                {
                    Transition((int)Flags.OnReachedTarget);
                }
            }
            );

            return behabiours;
        }

        public override List<Action> GetOnEnterBehaviours(StateParameters parameters)
        {
            List<Action> behabiours = new List<Action>();
            behabiours.Add(() =>
            {
                Debug.Log(thisRB.name + " esta persiguiendo a Br1e!");
            }
            );

            return behabiours;
        }

        public override List<Action> GetOnExitBehaviours(StateParameters parameters)
        {
            List<Action> behabiours = new List<Action>();
            behabiours.Add(() =>
            {
                Debug.Log(thisRB.name + " alcanzo a Br1e!");
            }
            );

            return behabiours;
        }

        public override object[] GetOutputs()
        {
            return null;
        }

        public override void SetParameters(StateParameters parameters)
        {
            thisRB = parameters.Parameters[0] as Rigidbody;
            br1eTransform = parameters.Parameters[1] as Transform;
            speed = (int)parameters.Parameters[2];
            radius = (float)parameters.Parameters[3];
        }

        public override void Transition(int flag)
        {
            SetFlag?.Invoke(flag);
        }
    }
}
