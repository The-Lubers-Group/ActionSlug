using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace LabLuby.FSM
{
    public abstract class EnemyStateBase : State<EnemyState, StateEvent>
    {
        protected readonly BaseEnemy _baseEnemy;
        protected readonly NavMeshAgent _agent;
        protected readonly Animator _animator;
        protected bool _requestedExit;
        protected float _exitTime;

        protected readonly Action<State<EnemyState, StateEvent>> onEnter;
        protected readonly Action<State<EnemyState, StateEvent>> onLogic;
        protected readonly Action<State<EnemyState, StateEvent>> onExit;
        protected readonly Func<State<EnemyState, StateEvent>, bool> canExit;
                   
    }
}
