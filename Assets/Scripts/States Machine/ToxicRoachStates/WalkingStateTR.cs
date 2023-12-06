using Random = UnityEngine.Random;
using System.Collections.Generic;
using UnityEngine;
using IA.FSM;
using System;

namespace TOM.Enemy.TR
{
    public class WalkingStateTR : State
    {
        private Rigidbody rb;
        private Vector3 targetPosition;
        private float speed;
        private Vector3 firstPoint;
        public override List<Action> GetBehaviours(StateParameters parameters)
        {

            SetParameters(parameters);

            List<Action> behabiours = new List<Action>();


            behabiours.Add(() =>
                {
                    Vector3 direction = firstPoint - rb.position;
                    direction.y = 0;
                    if (Vector3.Distance(rb.position, firstPoint) < 1f)
                    {
                        Transition((int)TOM.Enemy.Flags.OnArenaArrived);
                    }
                    direction.Normalize();
                    rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
                }
            );

            return behabiours;
        }

        public override List<Action> GetOnEnterBehaviours(StateParameters parameters)
        {
            List<Action> behabiours = new List<Action>();

            GameObject go = parameters.Parameters[0] as GameObject;
            BoxCollider box = go.GetComponent<BoxCollider>();

            behabiours.Add(() =>
                {
                    (go).layer = 9;//Esta fuera
                    box.size = new Vector3(box.size.x, 0.2f, box.size.z);
                    box.center = new Vector3(box.center.x, 0.09f, box.center.z);
                }
            );
            return behabiours;
        }

        public override List<Action> GetOnExitBehaviours(StateParameters parameters)
        {
            List<Action> behabiours = new List<Action>();

            GameObject go = parameters.Parameters[0] as GameObject;
            BoxCollider box = go.GetComponent<BoxCollider>();

            behabiours.Add(() =>
                {
                    (go).layer = 8;//Esta dentro
                    box.size = new Vector3(box.size.x, 1, box.size.z);
                    box.center = new Vector3(box.center.x, 0.48f, box.center.z);
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
            rb = parameters.Parameters[0] as Rigidbody;
            targetPosition = (Vector3)parameters.Parameters[1];
            speed = float.Parse(parameters.Parameters[2].ToString());
            Vector3 random = Random.insideUnitSphere * 2.5f;
            random.y = 0;
            firstPoint = targetPosition + random;
        }

        public override void Transition(int flag)
        {
            SetFlag?.Invoke(flag);
        }

    }

}