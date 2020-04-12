using UnityEngine;

public partial class FightingState : StateMachineBehaviour
{
    private float m_animationTime;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // if (stateInfo.IsTag("Attack") || stateInfo.IsTag("DashAttack") || stateInfo.IsTag("ComboFinisher"))
       // { AnimatorUtility.SetBoolParameter(animator, "IsAttacking", true); }
       // else { AnimatorUtility.SetBoolParameter(animator, "IsAttacking", false); }

        if (stateInfo.fullPathHash == fightingIdleState)
        {
            AnimatorUtility.SetBoolParameter(animator, "FightingIdle", true);
            if (animator.GetFloat(AnimatorUtility.PS.actionStancePara) > 0)
            {
                animator.SetFloat(AnimatorUtility.PS.actionStancePara, animator.GetFloat(AnimatorUtility.PS.actionStancePara) - Time.deltaTime);
            }
        }
        else if (AnimatorUtility.GetIntParameter(animator, "SkillSetNumber") == 0) { AnimatorUtility.SetBoolParameter(animator, "FightingIdle", false); }

        if (stateInfo.fullPathHash == Combo1[7] && stateInfo.normalizedTime >= 0.55f && stateInfo.normalizedTime <= 0.75f)
        {
            animator.speed = 0.50f;
        }
        else if (stateInfo.fullPathHash == Combo1[7] && stateInfo.normalizedTime >= 0.75f)
        {
            animator.speed = 1f;
        }

        if (stateInfo.fullPathHash == Combo2[6] || 
            stateInfo.fullPathHash == Combo4[4] || 
            stateInfo.fullPathHash == Combo7[2]   )
        {
            m_animationTime = stateInfo.normalizedTime;
            if (m_animationTime >= 0.43f)
            {
                if (m_animationTime <= 0.45f)
                {
                    animator.speed = 0.08f;
                }
                if (m_animationTime >= 0.45f)
                {
                    if (m_animationTime <= 0.70)
                    {
                        animator.speed = 0.5f;
                    }
                    else
                    {
                        animator.speed = 0.7f;
                    }
                }
            }
        }

        if (stateInfo.fullPathHash == Combo5[5] || stateInfo.fullPathHash == Combo6[3])
        {
            m_animationTime = stateInfo.normalizedTime;
            if (m_animationTime >= 0.60f)
            {
                if (m_animationTime <= 0.80f)
                {
                    animator.speed = 0.4f;
                }
                else
                {
                    animator.speed = 0.8f;
                }
            }
        }

        if(stateInfo.fullPathHash == Combo6[4])
        {
            m_animationTime = stateInfo.normalizedTime;
            if (m_animationTime >= 0.75f)
            {
                if (m_animationTime <= 0.78f)
                {
                    animator.speed = 0.1f;
                }
                else
                {
                    animator.speed = 0.7f;
                }
            }
        }

        if(stateInfo.fullPathHash == Combo6[7])
        {
            m_animationTime = stateInfo.normalizedTime;
            if (m_animationTime >= 0.50f)
            {
                animator.speed = 0.7f;
            }
        }

        if(stateInfo.fullPathHash == Combo6[8])
        {
            m_animationTime = stateInfo.normalizedTime;
            if (m_animationTime >= 0.42f)
            {
                if (m_animationTime <= 0.45f)
                {
                    animator.speed = 0.08f;
                }
                if (m_animationTime >= 0.45f)
                {
                    if (m_animationTime <= 0.70)
                    {
                        animator.speed = 0.5f;
                    }
                    else
                    {
                        animator.speed = 0.7f;
                    }
                }
            }
        }

        if(stateInfo.fullPathHash == Combo5[7] || stateInfo.fullPathHash == Combo7[12])
        {
            m_animationTime = stateInfo.normalizedTime;
            if (m_animationTime >= 0.26f)
            {
                if (m_animationTime <= 0.29f)
                {
                    animator.speed = 0.08f;
                }
                if (m_animationTime >= 0.29f)
                {
                    if (m_animationTime <= 0.6f) // 0.7
                    {
                        animator.speed = 0.5f;
                    }
                    else
                    {
                        animator.speed = 0.7f;
                    }
                }
            }
        }

        if (stateInfo.fullPathHash == Combo4[7])
        {
            m_animationTime = stateInfo.normalizedTime;
            if (m_animationTime >= 0.62f)
            {
                 if (m_animationTime <= 0.8f)
                 {
                     animator.speed = 0.4f;
                 }
                 else
                 {
                     animator.speed = 1f;
                 }
            }
        }

        if (stateInfo.fullPathHash == Combo2[8])
        {
            m_animationTime = stateInfo.normalizedTime;
            if (m_animationTime >= 0.45f)
            {
                if (m_animationTime <= 0.48f)
                {
                    animator.speed = 0.08f;
                }
                if (m_animationTime >= 0.48f)
                {
                    if (m_animationTime <= 0.85f) // 0.9
                    {
                        animator.speed = 0.5f;
                    }
                    else
                    {
                        animator.speed = 0.7f;
                    }
                }
            }
        }

        if (stateInfo.fullPathHash == Combo3[3] || stateInfo.fullPathHash == Combo6[9] || stateInfo.fullPathHash == (int)BasicKicks[3])
        {
            m_animationTime = stateInfo.normalizedTime;
            if (m_animationTime >= 0.42f)
            {
                if (m_animationTime <= 0.46f)
                {
                    animator.speed = 0.08f;
                }
                if(m_animationTime >= 0.46f)
                {
                    if(m_animationTime <= 0.85f) // 0.8
                    {
                        animator.speed = 0.4f;
                    }
                    else
                    {
                        animator.speed = 0.7f;
                    }
                }
            }
        }

        if(stateInfo.fullPathHash == Combo3[6] || stateInfo.fullPathHash == Combo7[4])
        {
            m_animationTime = stateInfo.normalizedTime;
            if (m_animationTime >= 0.80f)
            {
                if (m_animationTime <= 0.85f)
                {
                    animator.speed = 0.6f;
                }
                else
                {
                    animator.speed = 0.9f;
                }
            }
        }

        if (stateInfo.fullPathHash == SpecialAttacks[0])
        {
            m_animationTime = stateInfo.normalizedTime;
            if (m_animationTime >= 0.475f)
            {
                if (m_animationTime <= 0.675f)
                {
                    animator.speed = 0.35f;
                }
                else
                {
                    animator.speed = 0.75f;
                }
            }
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsTag("Attack") || stateInfo.IsTag("DashAttack") || stateInfo.IsTag("ComboFinisher"))
        {
            animator.speed = 1f;
        }
        if (stateInfo.IsTag("EC1K")) { AnimatorUtility.SetIntParameter(animator, "PreviousHitSet", 1); }
        if (stateInfo.IsTag("EC2") || stateInfo.IsTag("EC2K")) { AnimatorUtility.SetIntParameter(animator, "PreviousHitSet", 2); }
        if (stateInfo.IsTag("EC3K")) { AnimatorUtility.SetIntParameter(animator, "PreviousHitSet", 3); }
        if (stateInfo.IsTag("EC4") || stateInfo.IsTag("EC4K")) { AnimatorUtility.SetIntParameter(animator, "PreviousHitSet", 4); }

        if (stateInfo.IsTag("EC1K") || stateInfo.IsTag("EC2") || stateInfo.IsTag("EC2K") || stateInfo.IsTag("EC3K") || stateInfo.IsTag("EC4") || stateInfo.IsTag("EC4K"))
        {
            animator.SetBool("IsBeingHit", true);
            AnimatorUtility.SetIntParameter(animator, "HitSet", 0);
            AnimatorUtility.SetIntParameter(animator, "HitNumber", 0); // Debug.Log("Reset =  " + animator.name);
        }
    }

    private AttackEffects m_attackEffects;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.IsTag("Attack") || stateInfo.IsTag("DashAttack") || stateInfo.IsTag("ComboFinisher"))
        {
            animator.speed = 1f;
        }
        if (stateInfo.IsTag("EC1K") || stateInfo.IsTag("EC2") || stateInfo.IsTag("EC2K") || stateInfo.IsTag("EC3K") || stateInfo.IsTag("EC4") || stateInfo.IsTag("EC4K"))
        {
            animator.SetBool("IsBeingHit", false);
        }

        if (m_attackEffects == null) { m_attackEffects = animator.transform.Find("mixamorig:Hips").GetComponent<AttackEffects>(); }
        if (stateInfo.IsTag("Attack") || stateInfo.IsTag("DashAttack") || stateInfo.IsTag("ComboFinisher")) { m_attackEffects.HitConfirmed = false; }
        if (stateInfo.IsTag("Attack") || stateInfo.IsTag("DashAttack") || stateInfo.IsTag("ComboFinisher")) { m_attackEffects.AttackMissed = false; }
    }
}