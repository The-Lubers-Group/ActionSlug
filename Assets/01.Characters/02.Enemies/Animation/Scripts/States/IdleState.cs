using UnityEngine;

namespace LabLuby.FSM
{
    public class IdleState : EnemyStateBase
    {
        private float _animationLoopCount = 0;

        public IdleState(bool needsExitTime, BaseEnemyOld enemy) : base(needsExitTime, enemy) { }

        public override void OnEnter()
        {
            base.OnEnter();
            _agent.isStopped = true;
            _animator.Play("Idle_A");
        }

        public override void OnLogic()
        {
            AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);

            if(state.normalizedTime >= _animationLoopCount +1)
            {
                float value = Random.value;
                if(value < 0.95f)
                {
                    if(!state.IsName("Idle_A"))
                    {
                        _animationLoopCount = 0;
                    }
                    else
                    {
                        _animationLoopCount++;
                    }
                    _animator.Play("Idle_A");
                }
                else if (value < 0.975f)
                {
                    if (!state.IsName("Idle_B"))
                    {
                        _animationLoopCount = 0;
                    }
                    else
                    {
                        _animationLoopCount++;
                    }
                    _animator.Play("Idle_B");
                }
                else
                {
                    if (!state.IsName("Idle_C"))
                    {
                        _animationLoopCount = 0;
                    }
                    else
                    {
                        _animationLoopCount++;
                    }
                    _animator.Play("Idle_C");
                }
            }
            base.OnLogic();
        }
    }
}
