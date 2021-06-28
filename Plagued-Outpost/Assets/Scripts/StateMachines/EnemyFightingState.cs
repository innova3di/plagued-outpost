using UnityEngine;
using System.Collections;

public class EnemyFightingState : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsTag(AnimatorUtility.ES.IsBeingAttackedStates[0])) { AnimatorUtility.SetIntParameter(animator, "PreviousHitSet", 10); }
        if (stateInfo.IsTag(AnimatorUtility.ES.IsBeingAttackedStates[1])) { AnimatorUtility.SetIntParameter(animator, "PreviousHitSet", 11); }
        if (stateInfo.IsTag(AnimatorUtility.ES.IsBeingAttackedStates[2])) { AnimatorUtility.SetIntParameter(animator, "PreviousHitSet", 1); }
        if (stateInfo.IsTag(AnimatorUtility.ES.IsBeingAttackedStates[3])) { AnimatorUtility.SetIntParameter(animator, "PreviousHitSet", 8); }



        /**
            if (stateInfo.IsTag(AnimatorUtility.ES.IsBeingAttackedStates[0]) || stateInfo.IsTag(AnimatorUtility.ES.IsBeingAttackedStates[1]) || stateInfo.IsTag(AnimatorUtility.ES.IsBeingAttackedStates[2]) || stateInfo.IsTag(AnimatorUtility.ES.IsBeingAttackedStates[3]))
            {
          
            }
        **/

        for (int i = 0; i < AnimatorUtility.ES.IsBeingAttackedStates.Length; i++)
        {
            if (stateInfo.IsTag(AnimatorUtility.ES.IsBeingAttackedStates[i]))
            {
                AnimatorUtility.SetIntParameter(animator, "HitSet", 0);
                AnimatorUtility.SetIntParameter(animator, "HitNumber", 0); // Debug.Log("Reset =  " + animator.name);
            }
        }

        if (stateInfo.fullPathHash == AnimatorUtility.ES.sprintState)
        {
            AnimatorUtility.SetBoolParameter(animator, "Sprint", true);
        }
        else if (stateInfo.fullPathHash == AnimatorUtility.ES.idleState)
        {
            AnimatorUtility.SetBoolParameter(animator, "Idle", true);
        }
    }

    private float m_animationTime;

    public static IEnumerator IdleTimer(Animator animator)
    {
        float idleTime = Random.Range(1, 10);

        AnimatorUtility.SetFloatParameter(animator, "IdleTime", idleTime);
        while (AnimatorUtility.GetFloatParameter(animator, "IdleTime") >= 0.1f)
        {
            AnimatorUtility.SetFloatParameter(animator, "IdleTime", AnimatorUtility.GetFloatParameter(animator, "Dazed") - Time.deltaTime);
            yield return null;
        }
        AnimatorUtility.SetFloatParameter(animator, "IdleTime", 0f);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsTag("Attack"))
        { AnimatorUtility.SetBoolParameter(animator, "IsAttacking", true); }
        else { AnimatorUtility.SetBoolParameter(animator, "IsAttacking", false); }



        if (stateInfo.fullPathHash == AnimatorUtility.EnemyAttacks[14])
        {
            m_animationTime = stateInfo.normalizedTime;
            if (m_animationTime <= 0.25f)
            {
                animator.speed = 0.75f;
            }
            else if (m_animationTime >= 0.25f)
            {
                if (m_animationTime <= 0.365f)
                {
                    animator.speed = 0.85f;
                }
                else if (m_animationTime >= 0.365f)
                {
                    if (m_animationTime <= 0.400f)
                    {
                        animator.speed = 0.075f;
                    }
                    else if (m_animationTime >= 0.400f)
                    {
                        if (animator.speed != 1) { animator.speed = 1f; }
                    }
                }
            }
        }
    }

    private AttackEffects m_attackEffects;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.fullPathHash == AnimatorUtility.ES.sprintState)
        {
            AnimatorUtility.SetBoolParameter(animator, "Sprint", false);
        }
        else if (stateInfo.fullPathHash == AnimatorUtility.ES.idleState)
        {
            AnimatorUtility.SetBoolParameter(animator, "Idle", false);
        }
        if (stateInfo.fullPathHash == AnimatorUtility.EnemyAttacks[14]) { animator.speed = 1f; }

        if (m_attackEffects == null) { m_attackEffects = animator.GetComponentInChildren<AttackEffects>(); }
        if (stateInfo.IsTag("Attack")) { m_attackEffects.HitConfirmed = false; }
        if (stateInfo.IsTag("Attack")) { m_attackEffects.AttackMissed = false; }
    }
}
