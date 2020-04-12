using UnityEngine;
using EventSystem;
using InnoVision;

public class PlayerSkillState : StateMachineBehaviour
{
    public static readonly int punchPara = Animator.StringToHash("Punch");
    public static readonly int kickPara = Animator.StringToHash("Kick");
    public static readonly int leftAttackPara = Animator.StringToHash("LeftAttack");
    public static readonly int leftAndRightAttackPara = Animator.StringToHash("LARAttack");
    public static readonly int rightAttackPara = Animator.StringToHash("RightAttack");
    public static readonly int tabPara = Animator.StringToHash("Tab");

    private static bool m_isExecuted = false;
    private static bool m_dashComboExecuted = false; public static bool DashComboExecuted { get { return m_dashComboExecuted; } }

    public static SkillSystem m_skillSystem;
    public static void SetSkillSystem(SkillSystem skillSystem)
    {
        m_skillSystem = skillSystem;
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsTag("ComboFinisher"))
        {
            m_dashComboExecuted = false; AnimatorUtility.SetFloatParameter(animator, "ComboTime", 0f);
        }
        if (stateInfo.IsTag("Attack") || stateInfo.IsTag("DashAttack")) { AnimatorUtility.SetFloatParameter(animator, "ComboTime", 0f); m_dashComboExecuted = false; }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsTag("FightingIdle") || stateInfo.IsTag("BasicFightingIdle"))
        {
            int SSN = AnimatorUtility.GetIntParameter(animator, "SkillSetNumber");
            if (SSN != 8 && SSN != 9 && SSN != 12 && SSN != 13 && SSN != 14)
            {
                if (AnimatorUtility.GetFloatParameter(animator, "ComboTime") <= m_skillSystem.SkillTreeInfo.SkillList[SkillTreeInfo.SearchSkillSet((SkillSet)SSN, m_skillSystem.comboSkillTree)].ComboTimeFrame && AnimatorUtility.GetBoolParameter(animator, "IsMoving") == false)
                {
                    AnimatorUtility.SetFloatParameter(animator, "ComboTime", AnimatorUtility.GetFloatParameter(animator, "ComboTime") + Time.deltaTime); AnimatorUtility.SetBoolParameter(animator, "FightingIdle", true); 
                }
                else
                {
                    if (!m_isExecuted)
                    {
                        AnimatorUtility.SetIntParameter(animator, "SkillSetNumber", 0);
                        AnimatorUtility.SetIntParameter(animator, "ComboNumber", 0);
                        AnimatorUtility.SetFloatParameter(animator, "ComboTime", 0f);
                        m_isExecuted = true;
                    }
                }
            }
        }
        else if (stateInfo.IsTag("Attack") || stateInfo.IsTag("ComboFinisher")) { AnimatorUtility.SetBoolParameter(animator, "FightingIdle", false); }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(punchPara, false);
        animator.SetBool(kickPara, false);
        m_isExecuted = false;
        if (stateInfo.IsTag("Attack") || stateInfo.IsTag("DashAttack"))
        {
            if (AnimatorUtility.GetBoolParameter(animator, "IsMoving"))
            {
                int SSN = AnimatorUtility.GetIntParameter(animator, "SkillSetNumber");
                if (SSN != 8 && SSN != 9 && SSN != 12 && SSN != 13 && SSN != 14)
                {
                    AnimatorUtility.SetIntParameter(animator, "SkillSetNumber", 0);
                    AnimatorUtility.SetIntParameter(animator, "ComboNumber", 0);
                }
            }
        }
        if (stateInfo.IsTag("ComboFinisher"))
        {
            if (stateInfo.fullPathHash == FightingState.Combo1[7] &&
                animator.GetInteger("SkillSetNumber") != (int)SkillSet.DASH_FORWARD && animator.GetInteger("SkillSetNumber") != (int)SkillSet.BACK_STEP &&
                animator.GetInteger("SkillSetNumber") != (int)SkillSet.BACK_FLIP && animator.GetInteger("SkillSetNumber") != (int)SkillSet.DASH_LEFT &&
                animator.GetInteger("SkillSetNumber") != (int)SkillSet.DASH_RIGHT)
            {
                AnimatorUtility.SetIntParameter(animator, "SkillSetNumber", 0);
                AnimatorUtility.SetIntParameter(animator, "ComboNumber", 0);
            }
            else if (stateInfo.fullPathHash != FightingState.Combo1[7])
            {
                AnimatorUtility.SetIntParameter(animator, "SkillSetNumber", 0);
                AnimatorUtility.SetIntParameter(animator, "ComboNumber", 0);
            }
        }
        if (stateInfo.fullPathHash == AnimatorUtility.PS.dashStateC3 || stateInfo.fullPathHash == AnimatorUtility.PS.dashStateC6 || stateInfo.fullPathHash == AnimatorUtility.PS.dashStateC7)
        {
            m_dashComboExecuted = true;
        }
    }
}
