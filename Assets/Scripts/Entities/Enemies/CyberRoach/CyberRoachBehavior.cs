using IA.FSM;
using System;
using System.Collections;
using UnityEngine;

namespace TOM.Enemy.CR
{
    enum CRStates
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

    [RequireComponent(typeof(CyberRoach))]
    public class CyberRoachBehavior : MonoBehaviour
    {
        private GameObject target;

        private CyberRoach cyberRoach;

        private Rigidbody rb;

        private static Vector3 arenaCenter;

        private bool isAlreadyWaiting = false;

        private bool isTargetAlive = false;

        private FSM fsm = null;

        [field: SerializeField] private CRStates CRState { set; get; } //Esta magia negra no se lo que hace, preguntar a fede

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
            fsm = new FSM(Enum.GetValues(typeof(CRStates)).Length, Enum.GetValues(typeof(Flags)).Length);
        }
        private void SetUp()
        {
            rb = GetComponent<Rigidbody>();
            cyberRoach = gameObject.GetComponent<CyberRoach>();
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


            fsm.SetRelation((int)CRStates.Spawning, (int)Flags.OnEveryEnemySpawned, (int)CRStates.Walking);
            walkingParameters.Parameters = new object[3] { rb, arenaCenter, cyberRoach.GetMovementSpeed() };
            fsm.AddState<WalkingStateCR>((int)CRStates.Walking, walkingParameters, thisGO, thisGO);



            fsm.SetRelation((int)CRStates.Walking, (int)Flags.OnArenaArrived, (int)CRStates.Pursuit);
            pursuitParameters.Parameters = new object[5] { rb, target.transform, cyberRoach.GetMovementSpeed(), cyberRoach.GetAttackRadius(), isTargetAlive };
            fsm.AddState<PursuitState>((int)CRStates.Pursuit, pursuitParameters);



            fsm.SetRelation((int)CRStates.Pursuit, (int)Flags.OnReachedTarget, (int)CRStates.Attack);
            attackParameters.Parameters = new object[3] { target.GetComponent<Player>(), cyberRoach, cyberRoach.GetType() };
            fsm.AddState<AttackState>((int)CRStates.Attack, attackParameters);



            fsm.SetRelation((int)CRStates.Attack, (int)Flags.OnBasicAttack, (int)CRStates.WaitingForBasicAttack);
            waitForBasicParameters.Parameters = new object[2] { cyberRoach.GetBasicHitCD(), this };
            fsm.AddState<WaitingStateCR>((int)CRStates.WaitingForBasicAttack, waitForBasicParameters);



            fsm.SetRelation((int)CRStates.Attack, (int)Flags.OnPowerAttack, (int)CRStates.WaitingForPowerAttack);
            waitForPowerParameters.Parameters = new object[2] { cyberRoach.GetPowerHitCD(), this };
            fsm.AddState<WaitingStateCR>((int)CRStates.WaitingForPowerAttack, waitForPowerParameters);



            fsm.SetRelation((int)CRStates.Attack, (int)Flags.OnGettingDamage, (int)CRStates.Hurting);
            hurtParameters.Parameters = new object[1] { cyberRoach.GetStunTime() };
            fsm.AddState<HurtingState>((int)CRStates.Hurting, hurtParameters);



            fsm.SetRelation((int)CRStates.Pursuit, (int)Flags.OnGettingFatalDamage, (int)CRStates.Dying);
            dyingParameters.Parameters = new object[1] { cyberRoach };
            fsm.AddState<DyingState>((int)CRStates.Dying, dyingParameters);



            fsm.SetRelation((int)CRStates.Attack, (int)Flags.OnGettingFatalDamage, (int)CRStates.Dying);
            fsm.SetRelation((int)CRStates.Pursuit, (int)Flags.OnGettingDamage, (int)CRStates.Hurting);
            fsm.SetRelation((int)CRStates.Hurting, (int)Flags.OnHurtFinish, (int)CRStates.Pursuit);



            fsm.SetCurrentStateForced((int)CRStates.Spawning);
        }
        private void FixedUpdate()
        {
            if (!GameManager.IsPaused)
            {
                if (cyberRoach.IsAlive)
                {
                    fsm.Update();
                }
            }
            //Debug.Log("Estado actual de " + rb.gameObject.name + " es " + ((CRStates)fsm.currentStateIndex).ToString());
            //Debug.Log("Posicion actual de " + rb.gameObject.name + " es " + transform.position);
        }

        //private void Update()
        //{

        //}

        private void CyberRoachSuccessfulySpawned()
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
            EnemyController.OnAllEnemiesCreated += CyberRoachSuccessfulySpawned;
        }

        public void ResetBehaviour()
        {
            fsm.SetCurrentStateForced((int)CRStates.Spawning);
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
            while (t < waitingSeconds)
            {
                t += Time.deltaTime;
                yield return null;
            }
            fsm.SetCurrentStateForced((int)CRStates.Pursuit);
            isAlreadyWaiting = false;
        }

    }

}