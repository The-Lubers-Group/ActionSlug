using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace LabLuby.FSM
{
    [RequireComponent(typeof(Animator), typeof(NavMeshAgent))]

    public class BaseEnemy : MonoBehaviour
    {
        private StateMachine<EnemyState, StateEvent> _enemyFSM;
        private Animator _animator;
        private NavMeshAgent _agent;


        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _enemyFSM = new StateMachine<EnemyState, StateEvent>();

            _enemyFSM.AddState(EnemyState.Idle, new IdleState(false, this));

            _enemyFSM.SetStartState(EnemyState.Idle);
            _enemyFSM.Init();
        }

        private void Update()
        {
            _enemyFSM.OnLogic();
        }
    }


}
