using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using IA.FSM;
using System;

namespace TOM.Enemy.CR
{
    public class DyingState : State
    {
        public override List<Action> GetBehaviours(StateParameters parameters)
        {
            CyberRoach cr = parameters.Parameters[0] as CyberRoach;

            List<Action> behabiours = new List<Action>();

            behabiours.Add(() =>
            {
                cr.Die();
                Transition((int)Flags.OnDie);
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