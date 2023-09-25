using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using IA.FSM;
using System;

namespace TOM.Enemy.CR
{
    public class AttackState : State
    {
        bool wasPoweredAttack = false;

        Player target = null;
        Enemy enemy = null;
        Type enemyType = null;
        public override List<Action> GetBehaviours(StateParameters parameters)
        {

            SetParameters(parameters);

            List<Action> behabiours = new List<Action>();

            behabiours.Add(() =>
            {
                enemy.Attack(target);
                Transition((int)Flags.OnBasicAttack);
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
            object[] output = new object[1] { wasPoweredAttack };
            return output;
        }

        public override void SetParameters(StateParameters parameters)
        {
            wasPoweredAttack = false;
            target = parameters.Parameters[0] as Player;
            enemyType = parameters.Parameters[2] as Type;
            switch (enemyType.Name)
            {
                case "CyberRoach":
                    enemy = parameters.Parameters[1] as CyberRoach;
                    break;
                case "Gregorio":
                    //enemy = parameters.Parameters[1] as Gregorio;
                    break;
                case "SV23":
                    //enemy = parameters.Parameters[1] as SV23;
                    break;
                default:
                    Debug.LogError("There is no enemy with that Type.");
                    break;
            }
        }

        public override void Transition(int flag)
        {
            SetFlag?.Invoke(flag);
        }
    }

}