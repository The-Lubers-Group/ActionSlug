using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LabLuby.FSM
{
    public class RunState : EnemyStateBase
    {
        private Transform _target;

        public RunState(bool needsExitTime, BaseEnemyOld enemy, Transform target) : base(needsExitTime, enemy)
        {
            this._target = target;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            _agent.enabled = true;
            _agent.isStopped = false;
            _animator.Play("Walk");
        }

        public override void OnLogic()
        {
            base.OnLogic();
            if (!_requestedExit)
            {
                _agent.SetDestination(_target.position);
            }
            else if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                fsm.StateCanExit();
            }
        }

    }
}

