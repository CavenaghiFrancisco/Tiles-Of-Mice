using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using IA.FSM;
using System;

namespace TOM.Enemy.CR
{
    public class HurtingState : State
    {
        float timer = 0;
        Material material = null;
        Color defaultColor = default;
        float stunTime = 0;

        public override List<Action> GetBehaviours(StateParameters parameters)
        {

            List<Action> behabiours = new List<Action>();

            behabiours.Add(() =>
            {
                Debug.Log("Recibi daño!");
                if(timer<stunTime)
                {
                    timer += Time.deltaTime;
                    material.color = Color.magenta;
                }
                else
                {
                    material.color = defaultColor;
                    Transition((int)Flags.OnHurtFinish);
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
            material = parameters.Parameters[0] as Material;
            stunTime = (float)parameters.Parameters[1];
            
            timer = 0;
            defaultColor = material.color;
        }

        public override void Transition(int flag)
        {
            SetFlag?.Invoke(flag);
        }
        IEnumerator HurtCoroutine(float stunTime)
        {
            float t = 0;
            while (t < stunTime)
            {
                t += Time.deltaTime;
                yield return null;
            }
            Transition((int)Flags.OnStunFinish);
        }

    }

}