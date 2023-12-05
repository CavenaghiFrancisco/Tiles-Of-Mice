using System;
using System.Collections.Generic;
using TOM;
using UnityEngine;

namespace IA.FSM
{
    public class FSM
    {
        public int currentStateIndex = 0;
        Dictionary<int, State> states;
        Dictionary<int, StateParameters> statesParameters;
        Dictionary<int, StateParameters> statesOnEnterParameters;
        Dictionary<int, StateParameters> statesOnExitParameters;
        private int[,] relations;

        public FSM(int states, int flags)
        {
            currentStateIndex = -1;
            relations = new int[states, flags];
            for (int i = 0; i < states; i++)
            {
                for (int j = 0; j < flags; j++)
                {
                    relations[i, j] = -1;
                }
            }
            this.states = new Dictionary<int, State>();
            this.statesParameters = new Dictionary<int, StateParameters>();
            this.statesOnEnterParameters = new Dictionary<int, StateParameters>();
            this.statesOnExitParameters = new Dictionary<int, StateParameters>();
        }

        public void SetCurrentStateForced(int state)
        {
            currentStateIndex = state;
            //Debug.Log("Forced to be " + ((TOM.Enemy.CR.States)state).ToString());
        }

        public void SetRelation(int sourceState, int flag, int destinationState)
        {
            relations[sourceState, flag] = destinationState;
        }

        public void SetFlag(int flag)
        {
            if (currentStateIndex >= 0 &&
                currentStateIndex < relations.GetLength(0) &&
                flag >= 0 &&
                flag < relations.GetLength(1) &&
                relations[currentStateIndex, flag] != -1)
            {

                if (statesOnExitParameters.ContainsKey(currentStateIndex) && statesOnExitParameters[currentStateIndex] != null)
                {
                    foreach (Action OnExit in states.ContainsKey(currentStateIndex) ? states[currentStateIndex]?.GetOnExitBehaviours(statesOnExitParameters[currentStateIndex]) : null)
                        OnExit?.Invoke();
                }
                currentStateIndex = relations[currentStateIndex, flag];

                if (statesOnEnterParameters.ContainsKey(currentStateIndex) && statesOnEnterParameters[currentStateIndex] != null)
                {
                    foreach (Action OnEnter in states.ContainsKey(currentStateIndex) ? states[currentStateIndex]?.GetOnEnterBehaviours(statesOnEnterParameters[currentStateIndex]) : null)
                        OnEnter?.Invoke();
                }

                //Debug.Log("I changed my state to " + ((TOM.Enemy.CR.States)currentStateIndex).ToString());
            }
        }

        public void AddState<T>(int stateIndex, StateParameters stateParams = null,
            StateParameters stateOnEnterParams = null, StateParameters stateOnExitParams = null) where T : State, new()
        {
            if (!states.ContainsKey(stateIndex))
            {
                State newState = new T();
                states.Add(stateIndex, newState);
                newState.SetFlag += SetFlag;
                statesParameters.Add(stateIndex, stateParams);
                statesOnEnterParameters.Add(stateIndex, stateOnEnterParams);
                statesOnExitParameters.Add(stateIndex, stateOnExitParams);
            }
        }

        public void Update()
        {
            if (!GameManager.IsPaused)
            {
                if (states.ContainsKey(currentStateIndex))
                {
                    foreach (Action behaviour in states[currentStateIndex].GetBehaviours(statesParameters[currentStateIndex]))
                    {
                        behaviour?.Invoke();
                    }
                }
            }
        }

        public object[] GetStateOutputs(int stateIndex)
        {
            return states[stateIndex].GetOutputs();
        }

        public void SetStateParameters(int stateIndex, StateParameters parameters)
        {
            states[stateIndex].SetParameters(parameters);
        }

    }
}