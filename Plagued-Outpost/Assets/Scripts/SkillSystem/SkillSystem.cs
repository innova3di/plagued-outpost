using InnoVision;
using UnityEngine;
using EventSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkillSystem : MonoBehaviour
{
    public TransformManipulator transformManipulator;

    private Animator m_animator; public Animator Animator { get { return m_animator; } }
    public Transform dynamicButtons;
    public Sprite[] SkillButtonFrame;

    public SkillTreeInfo[] comboSkillTree;
    public SkillTreeInfo[] globalSkillTree;

    public Combo SkillCombo { get; private set; }

    public SkillTreeInfo SkillTreeInfo { get; private set; }

    private SkillSystemEvent m_skillSystemEvent;
    private SkillActivator m_skillActivator;
    private ComboSystem m_comboSystem = new ComboSystem();

    private float m_previousSkillTriggerTime;
    private SkillData m_previousSkillData;

    private Invoker m_globalSkill;
    private Invoker m_buttonVisualFeedBack;

    private List<SkillData> m_allSkills = new List<SkillData>();

    private Notification m_notification;

    void Start()
    {
        SkillTreeInfo = new SkillTreeInfo();
        m_animator = GetComponent<Animator>();
        PlayerSkillState.SetSkillSystem(this);
        m_globalSkill = new Invoker(new GlobalSkill(new CommandReceiver(this)));
        for (int i = 0; i < comboSkillTree.Length; i++) { SkillTreeInfo.StoreComboSkillInfo(comboSkillTree[i], comboSkillTree); }
        for (int i = 0; i < globalSkillTree.Length; i++) { SkillTreeInfo.StoreGlobalSkillInfo(globalSkillTree[i], globalSkillTree); }
        m_allSkills.AddRange(SkillTreeInfo.SkillList);
        m_allSkills.AddRange(SkillTreeInfo.GlobalSkillList);
        m_buttonVisualFeedBack = new Invoker(new ButtonVisualFeedBack(new ButtonInputReceiver(SkillButtonFrame, m_allSkills)));
        SkillSystemEvent.GlobalSkillEvent += SkillInputManager;

        m_notification = GameObject.Find("HUDManager").GetComponentInChildren<Notification>();
    }

    private void ActivateSkill(SkillData skillInputData)
    {
        m_skillSystemEvent = new SkillSystemEvent();
        m_skillActivator = new SkillActivator(m_skillSystemEvent);
        m_skillSystemEvent.OnActivateSkill(skillInputData, this);
        SkillCombo = m_skillActivator.SkillCombo;
        // Debug.Log(skillInputData.SkillSet + " is Acitvated.. Time Gap = " + (Time.time - m_previousSkillTriggerTime));
        if (skillInputData.CoolDownTime > 0) { StartCoroutine(m_skillActivator.CoolDown(skillInputData, m_animator)); }

        m_comboSystem.Update(this, m_skillSystemEvent); // Debug.Log(m_skillSystemEvent.JointAttackEventCount);
        if (!m_comboSystem.IsComboSystemRunning) StartCoroutine(m_comboSystem.JointAttackInitializer(transformManipulator.RayCast));
    }

    private void SkillInputManager(SkillData skillInputData)
    {
        if (skillInputData.IsOnCoolDown == false)
        {
            if ((m_previousSkillData == null || m_previousSkillData != skillInputData))
            {
                if ((Time.time - m_previousSkillTriggerTime) > 0.25f)
                {
                    ActivateSkill(skillInputData);
                }
            }
            else if (m_previousSkillData == skillInputData)
            {
                if (skillInputData.SkillSet == SkillSet.KIP_UP || skillInputData.SkillSet == SkillSet.ROLL_BACK || skillInputData.SkillSet == SkillSet.RETREAT)
                {
                    if ((Time.time - m_previousSkillTriggerTime) > 0.50f)
                    {
                        ActivateSkill(skillInputData); // we do the checks above to fix the reappearing recovery button when it is suppose to disappear after cooldown.
                    }
                    // else if (Time.time - m_previousSkillTriggerTime < 0.25f) { Debug.Log(skillInputData.SkillSet + " is being SPAMMED.. "); }
                }
                else if ((int)skillInputData.SkillSet <= 7)
                {
                    if ((Time.time - m_previousSkillTriggerTime) > 0.25f)
                    {
                        ActivateSkill(skillInputData);
                    }
                }
                else { ActivateSkill(skillInputData); }
            }
        }
        else if (skillInputData.IsOnCoolDown && Input.GetKeyDown(skillInputData.KeyCode)) { m_notification.Push("Skill On Cooldown"); }
        m_previousSkillData = skillInputData;
        m_previousSkillTriggerTime = Time.time;
    }

    public void Update()
    {
        if (!PauseMenu.InputDeferred)
        {
            foreach (SkillData skillInputData in SkillTreeInfo.SkillList)
            {
                if ((AnimatorUtility.GetCurrentStateInfo(m_animator).IsTag("SkillAccessible") || AnimatorUtility.GetCurrentStateInfo(m_animator).IsTag("BasicFightingIdle")) && !transformManipulator.RayCast.IsFacingTowardsObstacle)
                {
                    Image img = SkillTreeInfo.FindComponent<Image>(SkillTreeInfo.SkillList[skillInputData.SkillSetIndex], "Lock"); if (img != null) { img.enabled = false; }
                    if (Input.GetKey(skillInputData.KeyCode))
                    {
                        if (skillInputData.SkillSet != SkillSet.NONE)
                        {
                            SkillInputManager(SkillTreeInfo.SkillList[skillInputData.SkillSetIndex]);
                        }
                    }
                }
                else
                {
                    if (((int)skillInputData.SkillSet == 10 || (int)skillInputData.SkillSet == 11) && !transformManipulator.RayCast.CrosshairColliding)
                    {
                        Image img = SkillTreeInfo.FindComponent<Image>(SkillTreeInfo.SkillList[skillInputData.SkillSetIndex], "Lock");
                        if (img != null)
                        {
                            if (img.enabled) { img.enabled = false; }
                        }
                    }
                    else
                    {
                        Image img = SkillTreeInfo.FindComponent<Image>(SkillTreeInfo.SkillList[skillInputData.SkillSetIndex], "Lock");
                        if (img != null)
                        {
                            if (!img.enabled) { img.enabled = true; }
                        }
                    }
                }
            }
            if (m_skillActivator != null) { m_skillActivator.StateUpdate(m_animator); }
            if (m_comboSystem.IsComboSystemRunning) { m_comboSystem.BasicAttacks(); }

            m_globalSkill.InvokeCommand();
            m_buttonVisualFeedBack.InvokeCommand();
        }
    }
}
