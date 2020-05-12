using FastyTools.EventCenter;
using UnityEngine;

namespace Player
{
    public class AttackClearSignal : StateMachineBehaviour
    {
        public string[] enterSignal;

        public string[] exitSignal;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (var s in enterSignal)
            {
                animator.ResetTrigger(s);
            }

            if (stateInfo.IsName("playerAttack"))
            {
                EventCenterManager.Instance.EventTrigger(PlayerEvent.开始攻击+"");
            }
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (var s in exitSignal)
            {
                animator.ResetTrigger(s);
            }
            if (stateInfo.IsName("playerAttack"))
            {
                EventCenterManager.Instance.EventTrigger(PlayerEvent.结束攻击+"");
            }
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}