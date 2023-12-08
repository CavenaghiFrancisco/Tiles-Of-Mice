using System.Collections.Generic;
using TOM.Enemy.TR;
using IA.FSM;
using System;

namespace TOM.Enemy
{
    public class WaitingStateTR : State
    {
        bool hasStarted = false;
        float timer;
        ToxicRoachBehavior trBehavior;
        UnityEngine.Transform target;
        UnityEngine.Rigidbody rb;
        public override List<Action> GetBehaviours(StateParameters parameters)
        {
            List<Action> behabiours = new List<Action>();
            SetParameters(parameters);

            behabiours.Add(() =>
            {

                rb.transform.LookAt(new UnityEngine.Vector3(target.position.x, rb.transform.position.y, target.position.z));
                
                if (!hasStarted)
                {
                    trBehavior.WaitForTime(timer);
                    hasStarted = true;
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
                hasStarted = false;
            }
            );
            return behabiours;
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
            timer = (float)parameters.Parameters[0];
            trBehavior = parameters.Parameters[1] as ToxicRoachBehavior;
            target = (parameters.Parameters[2] as UnityEngine.GameObject).transform;
            rb = parameters.Parameters[3] as UnityEngine.Rigidbody;
        }

        public override void Transition(int flag)
        {
            SetFlag?.Invoke(flag);
        }

    }

}