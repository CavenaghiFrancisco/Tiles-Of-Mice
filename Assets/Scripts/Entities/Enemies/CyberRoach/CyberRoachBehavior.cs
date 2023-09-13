using System;
using UnityEngine;
using IA.FSM;

namespace TOM.Enemy.CR
{
    internal enum States
    {
        Spawning,                   // Unitl every enemy is spawned will be idle (on development)
        Pursuit,                    // Is in pursuit of its target
        Attack,                     // Is attacking its target
        Hurting,                    // Is being attacked
        Dying                       // Is starting its death
    }

    internal enum Flags
    {
        OnEveryEnemySpawned,        // Reach the max number of enemies
        OnReachedTarget,            // Reach the target position
        OnGettingDamage,            // Is being damaged
        OnGettingFatalDamage        // Is near to death...
    }

    [RequireComponent(typeof(CyberRoach))]
    public class CyberRoachBehavior : MonoBehaviour
    {
        private GameObject target;

        private CyberRoach cyberRoach;

        private FSM fsm = null;

        private Rigidbody rb;


        StateParameters pursuitParameters;
        StateParameters attackParameters;
        StateParameters hurtParameters;
        StateParameters dyingParameters;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            cyberRoach = gameObject.GetComponent<CyberRoach>();
            fsm = cyberRoach.GetFSM();

            pursuitParameters = new StateParameters();
            attackParameters = new StateParameters();
            hurtParameters = new StateParameters();
            dyingParameters = new StateParameters();

            fsm = new FSM(Enum.GetValues(typeof(States)).Length, Enum.GetValues(typeof(Flags)).Length);

            fsm.SetRelation((int)States.Spawning, (int)Flags.OnEveryEnemySpawned, (int)States.Pursuit);
           
            pursuitParameters.Parameters = new object[4] { rb, target.transform, cyberRoach.GetMovementSpeed(), cyberRoach.GetAttackRadius()};
            fsm.AddState<PursuitState>((int)States.Pursuit, pursuitParameters);
            
            fsm.SetCurrentStateForced((int)States.Spawning);
        }
        private void CyberRoachSuccessfulySpawned()
        {
            fsm?.SetFlag((int)Flags.OnEveryEnemySpawned);
        }

        public void Initialize(GameObject target)
        {
            this.target = target;
            Awake();
            EnemyController.OnAllEnemiesCreated += CyberRoachSuccessfulySpawned;
        }

        private void OnDestroy()
        {
            EnemyController.OnAllEnemiesCreated -= CyberRoachSuccessfulySpawned;
        }

    }

}