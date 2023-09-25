using System;
using UnityEngine;
using IA.FSM;
using System.Collections;
using System.Collections.Generic;

namespace TOM.Enemy.CR
{
    internal enum States
    {
        Spawning,                   // Unitl every enemy is spawned will be idle (on development)
        Pursuit,                    // Is in pursuit of its target
        Attack,                     // Is attacking its target
        Hurting,                    // Is being attacked
        WaitingForBasicAttack,      // Waiting for basic CoolDown
        WaitingForPowerAttack,      // Waiting for power CoolDown
        Dying,                      // Is starting its death
        Dead                        // Is Death
    }

    internal enum Flags
    {
        OnEveryEnemySpawned,        // Reach the max number of enemies
        OnReachedTarget,            // Reach the target position
        OnBasicAttack,              // Has attacked
        OnPowerAttack,              // Has attacked
        OnGettingDamage,            // Is being damaged
        OnStunFinish,               // Finish stun time
        OnHurtFinish,               // Finish waiting time
        OnGettingFatalDamage,       // Is near to death...
        OnDie                       // Died
    }

    [RequireComponent(typeof(CyberRoach))]
    public class CyberRoachBehavior : MonoBehaviour
    {
        private GameObject target;

        private CyberRoach cyberRoach;

        private Rigidbody rb;

        //private System.Action OnTimerEnd;

        private bool isAlreadyWaiting = false;

        private bool isTargetAlive = false;

        private FSM fsm = null;

        [field: SerializeField] private States state { set; get; } //Esta magia negra no se lo que hace, preguntar a fede

        StateParameters pursuitParameters;
        StateParameters attackParameters;
        StateParameters hurtParameters;
        StateParameters waitForBasicParameters;
        StateParameters waitForPowerParameters;
        StateParameters dyingParameters;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            cyberRoach = gameObject.GetComponent<CyberRoach>();
            fsm = cyberRoach.GetFSM();
            isTargetAlive = target.GetComponent<Player>().IsAlive();

            pursuitParameters = new StateParameters();
            attackParameters = new StateParameters();
            hurtParameters = new StateParameters();
            waitForBasicParameters = new StateParameters();
            waitForPowerParameters = new StateParameters();
            dyingParameters = new StateParameters();

            fsm = new FSM(Enum.GetValues(typeof(States)).Length, Enum.GetValues(typeof(Flags)).Length);


            fsm.SetRelation((int)States.Spawning, (int)Flags.OnEveryEnemySpawned, (int)States.Pursuit);
            pursuitParameters.Parameters = new object[5] { rb, target.transform, cyberRoach.GetMovementSpeed(), cyberRoach.GetAttackRadius(), isTargetAlive };
            fsm.AddState<PursuitState>((int)States.Pursuit, pursuitParameters);


            
            fsm.SetRelation((int)States.Pursuit, (int)Flags.OnReachedTarget, (int)States.Attack);
            attackParameters.Parameters = new object[3] { target.GetComponent<Player>(), cyberRoach, cyberRoach.GetType() };
            fsm.AddState<AttackState>((int)States.Attack, attackParameters);


            
            fsm.SetRelation((int)States.Attack, (int)Flags.OnBasicAttack, (int)States.WaitingForBasicAttack);
            waitForBasicParameters.Parameters = new object[2] { cyberRoach.GetBasicHitCD(), this };
            fsm.AddState<WaitingState>((int)States.WaitingForBasicAttack, waitForBasicParameters);
            


            fsm.SetRelation((int)States.Attack, (int)Flags.OnPowerAttack, (int)States.WaitingForPowerAttack);
            waitForPowerParameters.Parameters = new object[2] { cyberRoach.GetPowerHitCD(), this };
            fsm.AddState<WaitingState>((int)States.WaitingForPowerAttack, waitForPowerParameters);



            fsm.SetRelation((int)States.Attack, (int)Flags.OnGettingDamage, (int)States.Hurting);
            hurtParameters.Parameters = new object[2] { gameObject.GetComponent<MeshRenderer>()?.material, cyberRoach.GetStunTime() };
            fsm.AddState<HurtingState>((int)States.Hurting, hurtParameters);



            fsm.SetRelation((int)States.Pursuit, (int)Flags.OnGettingFatalDamage, (int)States.Dying);
            dyingParameters.Parameters = new object[1] { cyberRoach };
            fsm.AddState<DyingState>((int)States.Dying, dyingParameters);



            fsm.SetRelation((int)States.Attack, (int)Flags.OnGettingFatalDamage, (int)States.Dying);
            fsm.SetRelation((int)States.Pursuit, (int)Flags.OnGettingDamage, (int)States.Hurting);
            fsm.SetRelation((int)States.Hurting, (int)Flags.OnHurtFinish, (int)States.Pursuit);



            fsm.SetCurrentStateForced((int)States.Spawning);
        }
        private void FixedUpdate()
        {
            fsm.Update();
            //state = (States)(fsm.currentStateIndex); 
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
        public void WaitForTime(float seconds)
        {
            if (!isAlreadyWaiting)
            {
                StartCoroutine(WaitForSeconds(seconds));
            }
        }
        IEnumerator WaitForSeconds(float waitingSeconds)
        {
            Debug.Log(name + " va a esperar " + waitingSeconds + " segundos!");
            isAlreadyWaiting = true;
            float t = 0;
            while (t<waitingSeconds)
            {
                t += Time.deltaTime;
                yield return null;
            }
            fsm.SetCurrentStateForced((int)States.Pursuit);
            isAlreadyWaiting = false;
        }

    }

}