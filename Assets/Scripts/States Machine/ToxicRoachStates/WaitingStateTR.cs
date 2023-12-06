using System.Collections.Generic;
using TOM.Enemy.TR;
using IA.FSM;
using System;

namespace TOM.Enemy
{
    public class WaitingStateTR : State
    {
        bool hasStarted = false;
        public override List<Action> GetBehaviours(StateParameters parameters)
        {
            float timer = (float)parameters.Parameters[0];
            ToxicRoachBehavior crBehavior = parameters.Parameters[1] as ToxicRoachBehavior;
            hasStarted = false;
            List<Action> behabiours = new List<Action>();

            behabiours.Add(() =>
            {
                if (!hasStarted)
                {
                    crBehavior.WaitForTime(timer);
                    hasStarted = true;
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