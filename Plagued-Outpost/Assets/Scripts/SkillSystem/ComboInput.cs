using UnityEngine;
using EventSystem;
using InnoVision;

public class ComboInput 
{
    protected Transform LMB;
    protected Transform RMB;
    protected Transform LMBD;
    protected Transform RMBD;
    protected Transform DashCombo;
    protected Transform LeftAttk;
    protected Transform RightAttk;
    protected int SSN;
    protected JointAttack m_jointAttack;
    protected Combo m_combo;
    protected Animator m_animator;

    private SkillTreeInfo m_skillTreeInfo;
    private SkillTreeInfo[] m_globalSkillTree;

    protected bool m_isDynamicButtonReset;
    protected void ResetDynamicButtons()
    {
        InitializeStateEvaluator();
        LMBD.gameObject.SetActive(false);
        RMBD.gameObject.SetActive(false);
        DashCombo.gameObject.SetActive(false);
        LeftAttk.gameObject.SetActive(false);
        RightAttk.gameObject.SetActive(false);
        m_jointAttack = JointAttack.NO_INPUT;
    }

    protected ComboInput() { }
    protected void Update(SkillSystem skillSystem)
    {
        m_skillTreeInfo = skillSystem.SkillTreeInfo;
        m_combo = skillSystem.SkillCombo;
        m_animator = skillSystem.Animator;
        m_globalSkillTree = skillSystem.globalSkillTree;

        SSN = AnimatorUtility.GetIntParameter(m_animator, "SkillSetNumber");
        LMB = skillSystem.dynamicButtons.Find("LMB");
        RMB = skillSystem.dynamicButtons.Find("RMB"); 
        LMBD = skillSystem.dynamicButtons.Find("LMB").Find("Dyna");
        RMBD = skillSystem.dynamicButtons.Find("RMB").Find("Dyna");
        DashCombo = skillSystem.dynamicButtons.Find("DashCombo");
        LeftAttk = skillSystem.dynamicButtons.Find("LeftAttk");
        RightAttk = skillSystem.dynamicButtons.Find("RightAttk");
        InitializeStateEvaluator();
    }

    private StateEvaluator[] m_stateEvaluator = new StateEvaluator[2];
    private bool[] m_isUiAnimEnded = new bool[2];
    private void InitializeStateEvaluator()
    {
        for (int i = 0; i < m_stateEvaluator.Length; i++) { m_stateEvaluator[i] = new StateEvaluator(); }
    }
    private sealed class StateEvaluator
    {
        private int previousComboNum = -1;
        public bool Evaluate(Animator animator, bool condition)
        {
            if (condition == true && AnimatorUtility.GetIntParameter(animator, "ComboNumber") != previousComboNum && AnimatorUtility.GetIntParameter(animator, "SkillSetNumber") != 0)
            {
                previousComboNum = AnimatorUtility.GetIntParameter(animator, "ComboNumber");
                return condition;
            }
            return false;
        }
    }
    private void GetButtonInputs(Transform dynamicButton, JointAttack jointAttack, SkillSet basicAttack, params JointAttack[] nextMove)
    {
        if (m_combo.ComboQueue.Count != 0 && m_combo.ComboQueue.Dequeue() == jointAttack)
        {
            AnimatorUtility.SetIntParameter(m_animator, "ComboNumber", AnimatorUtility.GetIntParameter(m_animator, "ComboNumber") + 1);
            if (m_combo.ComboQueue.Count != 0 && nextMove != null)
            {
                if (m_combo.ComboQueue.Peek() == nextMove[0] || m_combo.ComboQueue.Peek() == nextMove[1])
                {
                    dynamicButton.GetComponent<Animator>().SetBool(UISwtichState.switchPara, true);
                }
            }
        }
        else if (jointAttack != JointAttack.DASH_FORWARD)
        {
            SkillSystemEvent.ExecuteGlobalSkill(m_skillTreeInfo.GlobalSkillList[SkillTreeInfo.SearchSkillSet(basicAttack, m_globalSkillTree)]);
            AnimatorUtility.SetIntParameter(m_animator, "ComboNumber", 0);
        }
    }

    private JointAttack[] nextKick = new JointAttack[2] { JointAttack.LEFT_KICK, JointAttack.RIGHT_KICK };
    private JointAttack[] nextPunch = new JointAttack[2] { JointAttack.LEFT_PUNCH, JointAttack.RIGHT_PUNCH };
    protected void JointAttackExecution()
    {
        if (Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E) && Input.GetMouseButtonDown(0))
        {
            GetButtonInputs(LMB, JointAttack.LEFT_PUNCH, SkillSet.BASIC_PUNCHES, nextKick);
        }
        else if (Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Q) && Input.GetMouseButtonDown(0))
        {
            GetButtonInputs(LMB, JointAttack.RIGHT_PUNCH, SkillSet.BASIC_PUNCHES, nextKick);
        }
        else if (Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.Q) && Input.GetMouseButtonDown(0))
        {
            GetButtonInputs(LMB, JointAttack.LEFT_AND_RIGHT_PUNCH, SkillSet.BASIC_PUNCHES, nextKick);
        }
        else if (Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E) && Input.GetMouseButtonDown(1))
        {
            GetButtonInputs(RMB, JointAttack.LEFT_KICK, SkillSet.BASIC_KICKS, nextPunch);
        }
        else if (Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Q) && Input.GetMouseButtonDown(1))
        {
            GetButtonInputs(RMB, JointAttack.RIGHT_KICK, SkillSet.BASIC_KICKS, nextPunch);
        }
        else if (Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.Q) && Input.GetMouseButtonDown(1))
        {
            GetButtonInputs(RMB, JointAttack.LEFT_AND_RIGHT_KICK, SkillSet.BASIC_KICKS, nextPunch);
        }
        else if ((!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E)))
        {
            if (Input.GetKeyDown(KeyCode.Tab)) { GetButtonInputs(null, JointAttack.DASH_FORWARD, SkillSet.NONE, null); }
            else if (Input.GetMouseButtonDown(0))
            {
                SkillSystemEvent.ExecuteGlobalSkill(m_skillTreeInfo.GlobalSkillList[SkillTreeInfo.SearchSkillSet(SkillSet.BASIC_PUNCHES, m_globalSkillTree)]);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                SkillSystemEvent.ExecuteGlobalSkill(m_skillTreeInfo.GlobalSkillList[SkillTreeInfo.SearchSkillSet(SkillSet.BASIC_KICKS, m_globalSkillTree)]);
            }
        }
    }
    protected void DynamicButtonBehaviour()
    {
        if ((SSN == 10 || SSN == 11) || m_combo.ComboQueue.Count != 0)
        {
            if (m_jointAttack == JointAttack.DASH_FORWARD)
            {
                LMBD.gameObject.SetActive(false);
                RMBD.gameObject.SetActive(false);
                DashCombo.gameObject.SetActive(true);
            }
            else if (PlayerSkillState.DashComboExecuted) { DashCombo.gameObject.SetActive(false); }

            m_isUiAnimEnded[0] = LMB.GetComponent<Animator>().GetBool(UISwtichState.switchPara) == false;
            if (m_stateEvaluator[0].Evaluate(m_animator, (m_jointAttack == JointAttack.LEFT_PUNCH || m_jointAttack == JointAttack.RIGHT_PUNCH || m_jointAttack == JointAttack.LEFT_AND_RIGHT_PUNCH) && m_isUiAnimEnded[0]))
            {
                LMBD.gameObject.SetActive(false);
                LMBD.gameObject.SetActive(true);
            }
            else if (m_jointAttack == JointAttack.LEFT_PUNCH || m_jointAttack == JointAttack.RIGHT_PUNCH || m_jointAttack == JointAttack.LEFT_AND_RIGHT_PUNCH)
            {
                RMBD.gameObject.SetActive(false);
            }

            m_isUiAnimEnded[1] = RMB.GetComponent<Animator>().GetBool(UISwtichState.switchPara) == false;
            if (m_stateEvaluator[0].Evaluate(m_animator, (m_jointAttack == JointAttack.LEFT_KICK || m_jointAttack == JointAttack.RIGHT_KICK || m_jointAttack == JointAttack.LEFT_AND_RIGHT_KICK) && m_isUiAnimEnded[1]))
            {
                RMBD.gameObject.SetActive(false);
                RMBD.gameObject.SetActive(true);
            }
            else if (m_jointAttack == JointAttack.LEFT_KICK || m_jointAttack == JointAttack.RIGHT_KICK || m_jointAttack == JointAttack.LEFT_AND_RIGHT_KICK)
            {
                LMBD.gameObject.SetActive(false);
            }

            if (m_stateEvaluator[1].Evaluate(m_animator, m_jointAttack == JointAttack.LEFT_PUNCH || m_jointAttack == JointAttack.LEFT_KICK))
            {
                LeftAttk.gameObject.SetActive(false);
                LeftAttk.gameObject.SetActive(true);
                RightAttk.gameObject.SetActive(false);
            }
            else if (m_stateEvaluator[1].Evaluate(m_animator, m_jointAttack == JointAttack.RIGHT_PUNCH || m_jointAttack == JointAttack.RIGHT_KICK))
            {
                LeftAttk.gameObject.SetActive(false);
                RightAttk.gameObject.SetActive(false);
                RightAttk.gameObject.SetActive(true);
            }
            else if (m_stateEvaluator[1].Evaluate(m_animator, m_jointAttack == JointAttack.LEFT_AND_RIGHT_PUNCH || m_jointAttack == JointAttack.LEFT_AND_RIGHT_KICK))
            {
                LeftAttk.gameObject.SetActive(false);
                RightAttk.gameObject.SetActive(false);
                LeftAttk.gameObject.SetActive(true);
                RightAttk.gameObject.SetActive(true);
            }
            else if (m_stateEvaluator[1].Evaluate(m_animator, m_jointAttack == JointAttack.DASH_FORWARD))
            {
                LeftAttk.gameObject.SetActive(false);
                RightAttk.gameObject.SetActive(false);
            }
        }
    }
}