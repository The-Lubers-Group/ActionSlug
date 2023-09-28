using LabLuby.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

namespace LabLuby.FSM
{
    public class AttackState : EnemyStateBase
    {
        public AttackState(
                bool needsExitTime,
                BaseEnemyOld enemy,
                Action<State<EnemyState, StateEvent>> onEnter,
                float exitTime = 0.33f) : base(needsExitTime, enemy, exitTime, onEnter) { }

        public override void OnEnter()
        {
            _agent.isStopped = true;
            base.OnEnter();
            _animator.Play("Attack");
        }
    }
}
