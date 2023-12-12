using IA.FSM;
using System;
using System.Collections;
using UnityEngine;

namespace TOM.Enemy.TR
{
    enum TRStates
    {
        Spawning,                   // Unitl every enemy is spawned will be idle (on development)
        Walking,
        Pursuit,                    // Is in pursuit of its target
        Attack,                     // Is attacking its target
        Hurting,                    // Is being attacked
        WaitingForBasicAttack,      // Waiting for basic CoolDown
        WaitingForPowerAttack,      // Waiting for power CoolDown
        Dying,                      // Is starting its death
        Dead                        // Is Death
    }

    [RequireComponent(typeof(ToxicRoach))]
    public class ToxicRoachBehavior : MonoBehaviour
    {
        private GameObject target;

        private ToxicRoach toxicRoach;

        private Rigidbody rb;

        private Animator anim;

        private static Vector3 arenaCenter;

        private bool isAlreadyWaiting = false;

        private bool isTargetAlive = false;

        private FSM fsm = null;

        [field: SerializeField] private TRStates TRState { set; get; } //Esta magia negra no se lo que hace, preguntar a fede

        private StateParameters pursuitParameters;
        private StateParameters walkingParameters;
        private StateParameters attackParameters;
        private StateParameters hurtParameters;
        private StateParameters waitForBasicParameters;
        private StateParameters waitForPowerParameters;
        private StateParameters dyingParameters;

        private StateParameters thisGO;

        private void Awake()
        {
            fsm = new FSM(Enum.GetValues(typeof(TRStates)).Length, Enum.GetValues(typeof(Flags)).Length);
        }
        private void SetUp()
        {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            toxicRoach = gameObject.GetComponent<ToxicRoach>();
            isTargetAlive = target.GetComponent<Player>().IsAlive;

            pursuitParameters = new StateParameters();
            walkingParameters = new StateParameters();
            attackParameters = new StateParameters();
            hurtParameters = new StateParameters();
            waitForBasicParameters = new StateParameters();
            waitForPowerParameters = new StateParameters();
            dyingParameters = new StateParameters();

            thisGO = new StateParameters();
            thisGO.Parameters = new object[1] { gameObject };


            fsm.SetRelation((int)TRStates.Spawning, (int)Flags.OnEveryEnemySpawned, (int)TRStates.Walking);
            walkingParameters.Parameters = new object[3] { rb, arenaCenter, toxicRoach.GetMovementSpeed() };
            fsm.AddState<WalkingStateTR>((int)TRStates.Walking, walkingParameters, thisGO, thisGO);



            fsm.SetRelation((int)TRStates.Walking, (int)Flags.OnArenaArrived, (int)TRStates.Pursuit);
            pursuitParameters.Parameters = new object[5] { rb, target.transform, toxicRoach.GetMovementSpeed(), toxicRoach.GetAttackRadius(), isTargetAlive };
            fsm.AddState<PursuitState>((int)TRStates.Pursuit, pursuitParameters);



            fsm.SetRelation((int)TRStates.Pursuit, (int)Flags.OnReachedTarget, (int)TRStates.Attack);
            attackParameters.Parameters = new object[3] { target.GetComponent<Player>(), toxicRoach, toxicRoach.GetType() };
            fsm.AddState<AttackState>((int)TRStates.Attack, attackParameters);



            fsm.SetRelation((int)TRStates.Attack, (int)Flags.OnBasicAttack, (int)TRStates.WaitingForBasicAttack);
            waitForBasicParameters.Parameters = new object[4] { toxicRoach.GetBasicHitCD(), this, target, rb };
            fsm.AddState<WaitingStateTR>((int)TRStates.WaitingForBasicAttack, waitForBasicParameters, new StateParameters());



            fsm.SetRelation((int)TRStates.Attack, (int)Flags.OnPowerAttack, (int)TRStates.WaitingForPowerAttack);
            waitForPowerParameters.Parameters = new object[4] { toxicRoach.GetPowerHitCD(), this, target, rb };
            fsm.AddState<WaitingStateTR>((int)TRStates.WaitingForPowerAttack, waitForPowerParameters);



            fsm.SetRelation((int)TRStates.Attack, (int)Flags.OnGettingDamage, (int)TRStates.Hurting);
            hurtParameters.Parameters = new object[1] { toxicRoach.GetStunTime() };
            fsm.AddState<HurtingState>((int)TRStates.Hurting, hurtParameters);



            fsm.SetRelation((int)TRStates.Pursuit, (int)Flags.OnGettingFatalDamage, (int)TRStates.Dying);
            dyingParameters.Parameters = new object[1] { toxicRoach };
            fsm.AddState<DyingState>((int)TRStates.Dying, dyingParameters);



            fsm.SetRelation((int)TRStates.Attack, (int)Flags.OnGettingFatalDamage, (int)TRStates.Dying);
            fsm.SetRelation((int)TRStates.Pursuit, (int)Flags.OnGettingDamage, (int)TRStates.Hurting);
            fsm.SetRelation((int)TRStates.Hurting, (int)Flags.OnHurtFinish, (int)TRStates.Pursuit);



            fsm.SetCurrentStateForced((int)TRStates.Spawning);
        }
        private void FixedUpdate()
        {
            if (!GameManager.IsPaused)
            {
                if (toxicRoach.IsAlive)
                {
                    fsm.Update();
                }
            }
        }

        private void ToxicRoachSuccessfulySpawned()
        {
            fsm?.SetFlag((int)Flags.OnEveryEnemySpawned);
        }

        public static void SetArenaCenter(Vector3 newCenter)
        {
            arenaCenter = newCenter;
        }

        public void Initialize(GameObject target)
        {
            this.target = target;
            SetUp();
            EnemyController.OnAllEnemiesCreated += ToxicRoachSuccessfulySpawned;
        }

        public void ResetBehaviour()
        {
            fsm.SetCurrentStateForced((int)TRStates.Spawning);
            isAlreadyWaiting = false;
        }

        private void OnDestroy()
        {
            EnemyController.OnAllEnemiesCreated -= ToxicRoachSuccessfulySpawned;
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
            anim.Play("CyberRoach_Idle");
            Debug.Log(name + " va a esperar " + waitingSeconds + " segundos!");
            isAlreadyWaiting = true;
            float t = 0;
            while (t < waitingSeconds)
            {
                t += Time.deltaTime;
                yield return null;
            }
            fsm.SetCurrentStateForced((int)TRStates.Pursuit);
            isAlreadyWaiting = false;
            Debug.Log(name + " deja de esperar!");
            anim.Play("CyberRoach_Walk");
        }

    }

}