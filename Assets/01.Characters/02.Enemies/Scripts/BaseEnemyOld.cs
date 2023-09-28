using DG.Tweening.Core.Easing;
using LabLuby.Sensors;
using LubyAdventure;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityHFSM;

namespace LabLuby.FSM
{
    [RequireComponent(typeof(Animator), typeof(NavMeshAgent))]

    public class BaseEnemyOld : MonoBehaviour
    {
        private StateMachine<EnemyState, StateEvent> _enemyFSM;
        private Animator _animator;
        private NavMeshAgent _agent;

        [SerializeField] private PlayerSensor _followPlayerSensor;
        [SerializeField] private PlayerSensor _rangeAttackPlayerSensor;


        [SerializeField]
        [Range(0.1f, 5f)]
        private float _attackCooldown = 2;

        [SerializeField] private bool _isInMeleeRange;
        //[SerializeField] private bool _isInSpitRange;
        [SerializeField] private bool _isInChasingRange;
        [SerializeField] private float _lastAttackTime;

        [SerializeField] private UnitController _player;





        private void Awake()
        {

            _player = FindAnyObjectByType<UnitController>();

            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _enemyFSM = new StateMachine<EnemyState, StateEvent>();

            _enemyFSM.AddState(EnemyState.Idle, new IdleState(false, this));
            _enemyFSM.AddState(EnemyState.Run, new RunState(true, this, _player.transform));
            _enemyFSM.AddState(EnemyState.Attack, new AttackState(true, this, OnAttack));

            _enemyFSM.AddTriggerTransition(StateEvent.DetectPlayer, new Transition<EnemyState>(EnemyState.Idle, EnemyState.Run));
            _enemyFSM.AddTriggerTransition(StateEvent.LostPlayer, new Transition<EnemyState>(EnemyState.Run, EnemyState.Idle));

            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Run,
                (transition) => _isInChasingRange
                                && Vector3.Distance(_player.transform.position, transform.position) > _agent.stoppingDistance)
            );
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Run, EnemyState.Idle,
                (transition) => !_isInChasingRange
                                || Vector3.Distance(_player.transform.position, transform.position) <= _agent.stoppingDistance)
            );


            // Attack Transitions
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Run, EnemyState.Attack,  ShouldMelee));
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Attack, ShouldMelee));
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Run, IsNotWithinIdleRange));
            _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Idle, IsWithinIdleRange));


            _enemyFSM.SetStartState(EnemyState.Idle);
            _enemyFSM.Init();
        }

        private void OnAttack(State<EnemyState, StateEvent> State)
        {
            transform.LookAt(UnitController.main.transform.position);
            _lastAttackTime = Time.time;
        }

        private void Start()
        {
            _followPlayerSensor.OnPlayerEnter += _followPlayerSensor_OnPlayerEnter;
            _followPlayerSensor.OnPlayerExit += _followPlayerSensor_OnPlayerExit;
            //_rangeAttackPlayerSensor.OnPlayerEnter += _rangeAttackPlayerSensor_OnPlayerEnter;
            //_rangeAttackPlayerSensor.OnPlayerExit += _rangeAttackPlayerSensor_OnPlayerExit;

        }


        private void _followPlayerSensor_OnPlayerEnter(Transform player)
        {
            _enemyFSM.Trigger(StateEvent.DetectPlayer);
            _isInChasingRange = true;
        }


        private void _followPlayerSensor_OnPlayerExit(Vector3 lastKnownPosition)
        {
            _enemyFSM.Trigger(StateEvent.LostPlayer);
            _isInChasingRange = false;
        }


        private bool ShouldMelee(Transition<EnemyState> Transition) =>
      _lastAttackTime + _attackCooldown <= Time.time
             && _isInMeleeRange;

        private bool IsWithinIdleRange(Transition<EnemyState> Transition) =>
      _agent.remainingDistance <= _agent.stoppingDistance;

        private bool IsNotWithinIdleRange(Transition<EnemyState> Transition) =>
            !IsWithinIdleRange(Transition);

        //private void _rangeAttackPlayerSensor_OnPlayerExit(Vector3 lastKnownPosition) => _isInSpitRange = false;

        //private void _rangeAttackPlayerSensor_OnPlayerEnter(Transform player) => _isInSpitRange = true;
 

        private void Update()
        {
            _enemyFSM.OnLogic();
        }
    }


}
