using System.Collections;
using UnityEngine;
using EventSystem;
using InnoVision;

public class ComboSystem : ComboInput
{
    private SkillSystemEvent m_skillSystemEvent;
    public bool IsComboSystemRunning { get; private set; }

    public ComboSystem() { }
    public void Update(SkillSystem skillSystem, SkillSystemEvent skillSystemEvent)
    {
        Update(skillSystem);
        m_skillSystemEvent = skillSystemEvent;
        m_skillSystemEvent.JointAttackEvent += JointAttackService;
    }

    public IEnumerator JointAttackInitializer(RaycastService rayCast)
    {
        SSN = AnimatorUtility.GetIntParameter(m_animator, "SkillSetNumber");
        while (SSN == 1 || SSN == 2 || SSN == 3 || SSN == 4 || SSN == 5 || SSN == 6 || SSN == 7 || SSN == 10 || SSN == 11)
        {
            if (!PauseMenu.InputDeferred)
            {
                IsComboSystemRunning = true;
                SSN = AnimatorUtility.GetIntParameter(m_animator, "SkillSetNumber");
                m_skillSystemEvent.ExecuteJointAttack();
                if (BoundCollider.ObstacleColliding || rayCast.IsFacingTowardsObstacle) { ResetDynamicButtons(); break; }
                GetButtonInputs();
            }
            yield return null;
        }
        ResetDynamicButtons(); IsComboSystemRunning = false;
    }

    private void JointAttackService()
    {
        if (SSN != 10 && SSN != 11)
        {
            JointAttackExecution(); m_isDynamicButtonReset = false;
            if (m_combo.ComboQueue.Count != 0) { m_jointAttack = m_combo.ComboQueue.Peek(); }
            DynamicButtonBehaviour();
        }
        else if (SSN == 10 || SSN == 11)
        {
            if (!m_isDynamicButtonReset)
            {
                m_previousStateHash = 0;
                m_isDynamicButtonReset = true;
                ResetDynamicButtons();
            }
        }
    }

    private int m_previousStateHash;
    private void UpdateBasicAttack(int comboNumber, JointAttack jointAttack, int currentStateHash)
    {
        AnimatorUtility.SetIntParameter(m_animator, "ComboNumber", comboNumber);
        ResetDynamicButtons();
        m_jointAttack = jointAttack;
        m_previousStateHash = currentStateHash;
        DynamicButtonBehaviour();
    }

    private void GetButtonInputs()
    {
        if (Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
        {
            m_animator.SetBool(PlayerSkillState.leftAttackPara, true);
        }
        else { m_animator.SetBool(PlayerSkillState.leftAttackPara, false); }
        if (Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Q))
        {
            m_animator.SetBool(PlayerSkillState.rightAttackPara, true);
        }
        else { m_animator.SetBool(PlayerSkillState.rightAttackPara, false); }
        if (Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.Q))
        {
            m_animator.SetBool(PlayerSkillState.leftAndRightAttackPara, true);
        }
        else { m_animator.SetBool(PlayerSkillState.leftAndRightAttackPara, false); }

        if (Input.GetMouseButtonDown(0)) { m_animator.SetBool(PlayerSkillState.punchPara, true); }
        if (Input.GetMouseButtonDown(1)) { m_animator.SetBool(PlayerSkillState.kickPara, true); }

        if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.Tab) && !PlayerSkillState.DashComboExecuted)
        {
            m_animator.SetBool(PlayerSkillState.tabPara, true);
        }
        else { m_animator.SetBool(PlayerSkillState.tabPara, false); }
    }

    public void BasicAttacks()
    {
        if (SSN == 10)
        {
            if (AnimatorUtility.GetCurrentStateHash(m_animator) == FightingState.BasicPunches[0] && AnimatorUtility.GetCurrentStateHash(m_animator) != m_previousStateHash)
            {
                UpdateBasicAttack(0, JointAttack.LEFT_PUNCH, AnimatorUtility.GetCurrentStateHash(m_animator));
            }
            else if (AnimatorUtility.GetCurrentStateHash(m_animator) == FightingState.BasicPunches[1] && AnimatorUtility.GetCurrentStateHash(m_animator) != m_previousStateHash)
            {
                UpdateBasicAttack(1, JointAttack.RIGHT_PUNCH, AnimatorUtility.GetCurrentStateHash(m_animator));
            }
            else if (AnimatorUtility.GetCurrentStateHash(m_animator) == FightingState.BasicPunches[2] && AnimatorUtility.GetCurrentStateHash(m_animator) != m_previousStateHash)
            {
                UpdateBasicAttack(2, JointAttack.RIGHT_PUNCH, AnimatorUtility.GetCurrentStateHash(m_animator));
            }
            else if (AnimatorUtility.GetCurrentStateHash(m_animator) == FightingState.BasicPunches[3] && AnimatorUtility.GetCurrentStateHash(m_animator) != m_previousStateHash)
            {
                UpdateBasicAttack(3, JointAttack.NO_INPUT, AnimatorUtility.GetCurrentStateHash(m_animator));
            }
        }
        else if (SSN == 11)
        {
            if (AnimatorUtility.GetCurrentStateHash(m_animator) == FightingState.BasicKicks[0] && AnimatorUtility.GetCurrentStateHash(m_animator) != m_previousStateHash)
            {
                UpdateBasicAttack(0, JointAttack.LEFT_KICK, AnimatorUtility.GetCurrentStateHash(m_animator));
            }
            else if (AnimatorUtility.GetCurrentStateHash(m_animator) == FightingState.BasicKicks[1] && AnimatorUtility.GetCurrentStateHash(m_animator) != m_previousStateHash)
            {
                UpdateBasicAttack(1, JointAttack.RIGHT_KICK, AnimatorUtility.GetCurrentStateHash(m_animator));
            }
            else if (AnimatorUtility.GetCurrentStateHash(m_animator) == FightingState.BasicKicks[2] && AnimatorUtility.GetCurrentStateHash(m_animator) != m_previousStateHash)
            {
                UpdateBasicAttack(2, JointAttack.LEFT_KICK, AnimatorUtility.GetCurrentStateHash(m_animator));
            }
            else if (AnimatorUtility.GetCurrentStateHash(m_animator) == FightingState.BasicKicks[3] && AnimatorUtility.GetCurrentStateHash(m_animator) != m_previousStateHash)
            {
                UpdateBasicAttack(3, JointAttack.NO_INPUT, AnimatorUtility.GetCurrentStateHash(m_animator));
            }
        }
    }
}