using InnoVision;
using UnityEngine;
using EventSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class CommandReceiver : CharacterInertia
{
    public CommandReceiver() { }
    public void SetCombo(SkillSet skillSetNumber, Queue<JointAttack> skillComboQueue)
    {
        if (skillSetNumber == SkillSet.SKILL_COMBO_ONE)
        {
            skillComboQueue.Enqueue(JointAttack.LEFT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
            skillComboQueue.Enqueue(JointAttack.LEFT_KICK);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
            skillComboQueue.Enqueue(JointAttack.LEFT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
        }
        if (skillSetNumber == SkillSet.SKILL_COMBO_TWO)
        {
            skillComboQueue.Enqueue(JointAttack.RIGHT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.LEFT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.RIGHT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.RIGHT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.LEFT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
            skillComboQueue.Enqueue(JointAttack.LEFT_KICK);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
        }
        if (skillSetNumber == SkillSet.SKILL_COMBO_THREE)
        {
            skillComboQueue.Enqueue(JointAttack.LEFT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.RIGHT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.LEFT_KICK);
            skillComboQueue.Enqueue(JointAttack.DASH_FORWARD);
            skillComboQueue.Enqueue(JointAttack.LEFT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
            skillComboQueue.Enqueue(JointAttack.LEFT_KICK);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
        }
        if (skillSetNumber == SkillSet.SKILL_COMBO_FOUR)
        {
            skillComboQueue.Enqueue(JointAttack.LEFT_KICK);
            skillComboQueue.Enqueue(JointAttack.RIGHT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.RIGHT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
            skillComboQueue.Enqueue(JointAttack.LEFT_KICK);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
        }
        if (skillSetNumber == SkillSet.SKILL_COMBO_FIVE)
        {
            skillComboQueue.Enqueue(JointAttack.LEFT_AND_RIGHT_KICK);
            skillComboQueue.Enqueue(JointAttack.LEFT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
            skillComboQueue.Enqueue(JointAttack.LEFT_KICK);
            skillComboQueue.Enqueue(JointAttack.LEFT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
        }
        if (skillSetNumber == SkillSet.SKILL_COMBO_SIX)
        {
            skillComboQueue.Enqueue(JointAttack.RIGHT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.LEFT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.LEFT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.RIGHT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.LEFT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.LEFT_AND_RIGHT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.RIGHT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.LEFT_KICK);
            skillComboQueue.Enqueue(JointAttack.DASH_FORWARD);
            skillComboQueue.Enqueue(JointAttack.LEFT_KICK);
        }
        if (skillSetNumber == SkillSet.SKILL_COMBO_SEVEN)
        {
            skillComboQueue.Enqueue(JointAttack.RIGHT_PUNCH);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
            skillComboQueue.Enqueue(JointAttack.LEFT_KICK);
            skillComboQueue.Enqueue(JointAttack.DASH_FORWARD);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
            skillComboQueue.Enqueue(JointAttack.LEFT_KICK);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
            skillComboQueue.Enqueue(JointAttack.LEFT_KICK);
            skillComboQueue.Enqueue(JointAttack.RIGHT_KICK);
        }
    }

    private Animator m_animator;
    private SkillTreeInfo m_sti;
    private RaycastService m_rayCast;
    private SkillSystem m_skillSystem;
    private float[] m_previousTapTime = new float[5];

    public const float DODGE_MAX_DISTANCE = 5f;
    public const float DODGE_MIN_DISTANCE = TransformManipulator.CROSSHAIR_DISTANCE * 0.625f;

    public CommandReceiver(SkillSystem skillSystem)
    {
        m_skillSystem = skillSystem;
        m_sti = skillSystem.SkillTreeInfo;
        m_animator = skillSystem.Animator;
        for (int i = 0; i < m_previousTapTime.Length; i++) { m_previousTapTime[i] = 0; }
    }

    private KeyCode m_previousKey;
    private void ActivateGlobalSkill(SkillData skillInputData, KeyCode keyCode, object index)
    {
        if (Input.GetKeyDown(skillInputData.KeyCode))
        {
            if (skillInputData.KeyCode == keyCode)
            {
                if (index != null)
                {
                    if ((Time.time - m_previousTapTime[(int)index]) < 0.3f)
                    {
                        SkillSystemEvent.ExecuteGlobalSkill(m_sti.GlobalSkillList[skillInputData.SkillSetIndex]);
                    }
                    else { m_previousTapTime[(int)index] = Time.time; }
                }
                else { SkillSystemEvent.ExecuteGlobalSkill(m_sti.GlobalSkillList[skillInputData.SkillSetIndex]); }
            }
        }
        else if (skillInputData.KeyCode == KeyCode.None)
        {
            if (Input.GetKeyDown(keyCode))
            {
                if ((Time.time - m_previousTapTime[(int)index]) < 0.3f)
                {
                    if (keyCode == KeyCode.A && m_previousKey == KeyCode.D)
                    {
                        SkillSystemEvent.ExecuteGlobalSkill(m_sti.GlobalSkillList[skillInputData.SkillSetIndex]);
                    }
                }
                else
                {
                    m_previousKey = keyCode;
                    m_previousTapTime[(int)index] = Time.time;
                }
            }
        }
    }

    public void GlobalSkillInputService()
    {
        foreach (SkillData skillInputData in m_sti.GlobalSkillList)
        {
            m_rayCast = m_skillSystem.transformManipulator.RayCast;
            int SS = (int)skillInputData.SkillSet;
            int SSN = AnimatorUtility.GetIntParameter(m_animator, "SkillSetNumber");
            for (int i = 0; i < AnimatorUtility.PS.SkillReadyState.Length; i++)
            {
                if (SS == 15 && AnimatorUtility.GetCurrentStateHash(m_animator) != AnimatorUtility.PS.kipUpState && (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.backStepState || AnimatorUtility.GetCurrentStateInfo(m_animator).IsTag("Recovery")))
                {
                    ActivateGlobalSkill(skillInputData, KeyCode.Space, null);
                }
                else if (AnimatorUtility.GetCurrentStateInfo(m_animator).IsTag(AnimatorUtility.PS.SkillReadyState[i]) && 
                         AnimatorUtility.GetCurrentStateHash(m_animator) != AnimatorUtility.PS.flipKickInitState && 
                         AnimatorUtility.GetCurrentStateHash(m_animator) != FightingState.SpecialAttacks[0] && 
                         !m_skillSystem.transformManipulator.RayCast.IsFacingTowardsObstacle)
                {
                    if (SS == 9 && (!m_rayCast.FrontColliding && !m_rayCast.FrontLeftColliding && !m_rayCast.FrontRightColliding && !m_rayCast.LeftColliding && !m_rayCast.RightColliding))
                    {
                        ActivateGlobalSkill(skillInputData, KeyCode.W, 0);
                    }
                    else if ((SS == 12 || SS == 15) && (!m_rayCast.BackColliding && !m_rayCast.BackLeftColliding && !m_rayCast.BackRightColliding && !m_rayCast.LeftColliding && !m_rayCast.RightColliding))
                    {
                        if (SS == 12) { ActivateGlobalSkill(skillInputData, KeyCode.S, 1); }
                        if (SS == 15 && (AnimatorUtility.GetCurrentStateInfo(m_animator).IsTag("QMSB") || AnimatorUtility.GetCurrentStateInfo(m_animator).IsTag("RQMSB")))
                        {
                            ActivateGlobalSkill(skillInputData, KeyCode.Space, null);
                        }
                    }
                    else if (SS == 13 && (!m_rayCast.LeftColliding && !m_rayCast.FrontLeftColliding && !m_rayCast.BackLeftColliding && !m_rayCast.FrontColliding && !m_rayCast.BackColliding))
                    {
                        ActivateGlobalSkill(skillInputData, KeyCode.A, 2);
                    }
                    else if (SS == 14 && (!m_rayCast.RightColliding && !m_rayCast.FrontRightColliding && !m_rayCast.BackRightColliding && !m_rayCast.FrontColliding && !m_rayCast.BackColliding))
                    {
                        ActivateGlobalSkill(skillInputData, KeyCode.D, 3);
                    }
                    else if (SS == 19)
                    {
                        if (m_rayCast.Target != null && m_rayCast.TargetDistance != 0)
                        {
                            if (m_rayCast.TargetDistance >= (DODGE_MIN_DISTANCE + 0.25f) && m_rayCast.TargetDistance <= DODGE_MAX_DISTANCE)
                            {
                                if (m_rayCast.TargetColliding)
                                {
                                    ActivateGlobalSkill(skillInputData, KeyCode.A, 4);
                                    ActivateGlobalSkill(skillInputData, KeyCode.D, 4);
                                }
                            }
                        }
                    }

                    if (SS == 10 && (SSN == 0 || SSN == 10 || SSN == 11))
                    {
                        ActivateGlobalSkill(skillInputData, KeyCode.Mouse0, null);
                    }
                    else if (SS == 11 && (SSN == 0 || SSN == 10 || SSN == 11))
                    {
                        ActivateGlobalSkill(skillInputData, KeyCode.Mouse1, null);
                    }
                    else if (SSN != 0 || SSN != 10 || SSN != 11)
                    {
                        if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
                        {
                            if (SS == 10)
                            {
                                ActivateGlobalSkill(skillInputData, KeyCode.Mouse0, null);
                            }
                            if (SS == 11)
                            {
                                ActivateGlobalSkill(skillInputData, KeyCode.Mouse1, null);
                            }
                        }
                    }

                    if (Input.GetKey(skillInputData.KeyCode))
                    {
                        if (skillInputData.KeyCode == KeyCode.Tab)
                        {
                            if ((SSN == 10 || SSN == 11 || SSN == 0) || (SSN != 3 && SSN != 6 && SSN != 7))
                            {
                                SkillSystemEvent.ExecuteGlobalSkill(m_sti.GlobalSkillList[skillInputData.SkillSetIndex]);
                            }
                            if (SSN == 3) { ConditionalFlipKick(m_animator, skillInputData, 4); }
                            if (SSN == 6) { ConditionalFlipKick(m_animator, skillInputData, 9); }
                            if (SSN == 7) { ConditionalFlipKick(m_animator, skillInputData, 5); }
                        }
                    }
                }
            }
            int CSH = AnimatorUtility.GetCurrentStateHash(m_animator);
            AnimatorStateInfo CSI = AnimatorUtility.GetCurrentStateInfo(m_animator);
            if ((CSI.IsTag("EC1K") && CSI.normalizedTime >= 0.85f) || CSH == AnimatorUtility.PS.unconsciousState)
            {
                if (SS == 16)
                {
                    if (m_activeRecoverySkill != skillInputData)
                    {
                        m_activeRecoverySkill = skillInputData;
                    }
                    if (!m_activeRecoverySkill.ButtonStructure.activeInHierarchy)
                    {
                        m_activeRecoverySkill.ButtonStructure.SetActive(true);
                    }
                    else if (m_activeRecoverySkill.ButtonStructure.activeInHierarchy)
                    {
                        ActivateGlobalSkill(skillInputData, KeyCode.R, null);
                    }
                }
            }
            else if ((CSI.IsTag("EC3K") && CSI.normalizedTime >= 0.85f) || CSH == AnimatorUtility.PS.writhingInPainState)
            {
                if (SS == 17)
                {
                    if (m_activeRecoverySkill != skillInputData)
                    {
                        m_activeRecoverySkill = skillInputData;
                    }
                    if (!m_activeRecoverySkill.ButtonStructure.activeInHierarchy)
                    {
                        m_activeRecoverySkill.ButtonStructure.SetActive(true);
                    }
                    else if (m_activeRecoverySkill.ButtonStructure.activeInHierarchy)
                    {
                        ActivateGlobalSkill(skillInputData, KeyCode.V, null);
                    }
                }
            }
            else if ((CSI.IsTag("EC4K") && CSI.normalizedTime >= 0.85f) || CSH == AnimatorUtility.PS.SU_UnconsciousState)
            {
                if (SS == 18)
                {
                    if (m_activeRecoverySkill != skillInputData)
                    {
                        m_activeRecoverySkill = skillInputData;
                    }
                    if (!m_activeRecoverySkill.ButtonStructure.activeInHierarchy)
                    {
                        m_activeRecoverySkill.ButtonStructure.SetActive(true);
                    }
                    else if (m_activeRecoverySkill.ButtonStructure.activeInHierarchy)
                    {
                        ActivateGlobalSkill(skillInputData, KeyCode.F, null);
                    }
                }
            }
            else if (CSI.IsTag("Incapacitated") && CSI.normalizedTime > 0.05f)
            {
                if (m_activeRecoverySkill != null)
                {
                    if (m_activeRecoverySkill.ButtonStructure.activeInHierarchy)
                    {
                        if (!m_activeRecoverySkill.IsOnCoolDown) { m_activeRecoverySkill.ButtonStructure.SetActive(false); }
                    }
                }
            }
        }
    }

    private SkillData m_activeRecoverySkill;

    private void ConditionalFlipKick(Animator animator, SkillData skillInputData, int dashNumber)
    {
        if (AnimatorUtility.GetCurrentStateInfo(m_animator).IsTag("DashAttack"))
        {
            if (AnimatorUtility.GetCurrentStateInfo(m_animator).normalizedTime > 0.7f) { SkillSystemEvent.ExecuteGlobalSkill(m_sti.GlobalSkillList[skillInputData.SkillSetIndex]); }
        }
        else if (AnimatorUtility.GetIntParameter(animator, "ComboNumber") != dashNumber - 1 && AnimatorUtility.GetIntParameter(animator, "ComboNumber") != dashNumber)
        {
            SkillSystemEvent.ExecuteGlobalSkill(m_sti.GlobalSkillList[skillInputData.SkillSetIndex]);
        }
        else if (AnimatorUtility.GetCurrentStateInfo(m_animator).IsTag("FightingIdle") && AnimatorUtility.GetIntParameter(animator, "ComboNumber") == dashNumber)
        {
            if (PlayerSkillState.DashComboExecuted == true) { SkillSystemEvent.ExecuteGlobalSkill(m_sti.GlobalSkillList[skillInputData.SkillSetIndex]); }
        }
    }
}