using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using EventSystem;
using InnoVision;

public class SkillActivator
{
    private float m_currentFillAmount;
    private float m_currentCoolDownTime;
    private int m_baseCurrentCoolDown;
    public Combo SkillCombo { get; private set; }

    private bool m_qms, m_recovery;

    public SkillActivator(SkillSystemEvent skillSystemEvent)
    {
        skillSystemEvent.ExecuteSkillEvent += ComboTrigger;
    }

    public void StateUpdate(Animator animator)
    {
        m_qms = animator.GetCurrentAnimatorStateInfo(0).IsTag("QMS");
        m_recovery = animator.GetCurrentAnimatorStateInfo(0).IsTag("Recovery");
    }

    private void ComboTrigger(SkillData skillInputData, SkillSystem skillSystem)
    {
        m_currentFillAmount = 0;
        m_currentCoolDownTime = skillInputData.CoolDownTime;
        m_baseCurrentCoolDown = (int)skillInputData.CoolDownTime - 1;
        SkillCombo = new Combo(skillSystem.Animator, new CommandReceiver(), skillInputData.SkillSet);
        new Invoker(SkillCombo).InvokeCommand();
        if (!skillInputData.ButtonStructure.activeInHierarchy) { skillInputData.ButtonStructure.SetActive(true); }
    }

    public IEnumerator CoolDown(SkillData skillInputData, Animator animator)
    {
        Image fillerImage = SkillTreeInfo.FindComponent<Image>(skillInputData, "Filler");
        Text coolDown = SkillTreeInfo.FindComponent<Text>(skillInputData, "CoolDownText");
        int SSN = (int)skillInputData.SkillSet;
        float timeElapsed = 0;
        bool skillCanceled = false;

        while (!skillCanceled && !skillInputData.IsOnCoolDown && AnimatorUtility.GetCurrentStateHash(animator) != FightingState.Combo1[0] && AnimatorUtility.GetCurrentStateHash(animator) != FightingState.Combo2[0] && AnimatorUtility.GetCurrentStateHash(animator) != FightingState.SpecialAttacks[0] && !m_qms && !m_recovery)
        {
            timeElapsed += Time.deltaTime; if (timeElapsed >= 1.5f) { skillCanceled = true; }
            yield return null;
        }
        while (!skillCanceled && m_currentFillAmount <= skillInputData.CoolDownTime)
        {
            if ((int)skillInputData.SkillSet != 10 && (int)skillInputData.SkillSet != 11)
            {
                skillInputData.IsOnCoolDown = true;
            }
            if (m_baseCurrentCoolDown == m_currentCoolDownTime - 1)
            {
                if (coolDown != null) { coolDown.text = "" + m_currentCoolDownTime; }
            }
            if (m_currentCoolDownTime <= m_baseCurrentCoolDown)
            {
                if (m_currentCoolDownTime >= 0.1)
                {
                    if (coolDown != null) { coolDown.text = "" + m_baseCurrentCoolDown; }
                }
                m_baseCurrentCoolDown -= 1;
            }
            if (fillerImage != null)
            {
                if (fillerImage.fillAmount <= 1)
                {
                    fillerImage.fillAmount = m_currentFillAmount / skillInputData.CoolDownTime;
                }
            }
            m_currentFillAmount += Time.deltaTime;
            m_currentCoolDownTime -= Time.deltaTime;
            yield return null;
        }
        if (SSN == 9 || SSN >= 12)
        {
            if (skillInputData.ButtonStructure.activeInHierarchy) { skillInputData.ButtonStructure.SetActive(false); }
        }
        if (fillerImage != null && coolDown != null)
        {
            fillerImage.fillAmount = 1;
            coolDown.text = "";
        }
        skillInputData.IsOnCoolDown = false;
    }
}
