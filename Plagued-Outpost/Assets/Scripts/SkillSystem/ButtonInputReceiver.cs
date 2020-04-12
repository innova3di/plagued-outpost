using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using InnoVision;

public class ButtonInputReceiver
{
    private Sprite[] m_skillButtonFrame;
    private List<SkillData> m_allSkills;
    private DashButtonFrame[] m_dashButtonFrame = new DashButtonFrame[5];
    public ButtonInputReceiver(Sprite[] skillButtonFrame, List<SkillData> allSkills)
    {
        m_allSkills = allSkills;
        m_skillButtonFrame = skillButtonFrame;
        for (int i = 0; i < m_dashButtonFrame.Length; i++) { m_dashButtonFrame[i] = new DashButtonFrame(); }
    }

    private sealed class DashButtonFrame
    {
        private float m_activeTime = 0;
        public bool m_isExecuted = true;

        public void DashButtonVisualFeedBack(SkillData skillInputData, Sprite[] skillButtonFrame)
        {
            if (m_activeTime < 0.5f)
            {
                m_activeTime += Time.deltaTime;
                SkillTreeInfo.FindComponent<Image>(skillInputData, "UIFrame").sprite = skillButtonFrame[0];
            }
            else { SkillTreeInfo.FindComponent<Image>(skillInputData, "UIFrame").sprite = skillButtonFrame[1]; }
            if (m_isExecuted == true) { m_isExecuted = false; }
        }
    }
    private void DashButtonVisualFeedBack(SkillData skillInputData, KeyCode keyCode, int index)
    {
        if (skillInputData.KeyCode == keyCode && skillInputData.ButtonStructure.activeInHierarchy == true)
        {
            m_dashButtonFrame[index].DashButtonVisualFeedBack(skillInputData, m_skillButtonFrame);
        }
        else if (skillInputData.KeyCode == keyCode && skillInputData.ButtonStructure.activeInHierarchy == false)
        {
            if (m_dashButtonFrame[index].m_isExecuted == false)
            {
                m_dashButtonFrame[index] = new DashButtonFrame(); 
                m_dashButtonFrame[index].m_isExecuted = true;
            }
        }
    }
    public void SkillButtonVisualFeedBack()
    {
        foreach (SkillData skillInputData in m_allSkills)
        {
            SkillSet SS = skillInputData.SkillSet;
            if (SS != SkillSet.DASH_FORWARD && SS != SkillSet.BACK_STEP && SS != SkillSet.BACK_FLIP && SS != SkillSet.DASH_LEFT && SS != SkillSet.DASH_RIGHT)
            {
                if (Input.GetKey(skillInputData.KeyCode))
                {
                    Vector2 buttonPosition = skillInputData.ButtonStructure.transform.localPosition;
                    skillInputData.ButtonStructure.transform.localPosition = new Vector2(buttonPosition.x, skillInputData.ButtonTransform - 1.5f);
                    foreach (Transform img in SkillTreeInfo.FindComponents<Transform>(skillInputData.ButtonStructure, "UIFrame")) { img.localPosition = new Vector2(0, 0 + 0.5f); }
                    foreach (Image img in SkillTreeInfo.FindComponents<Image>(skillInputData.ButtonStructure, "UIFrame")) { img.sprite = m_skillButtonFrame[0]; }
                }
                else
                {
                    Vector2 buttonPosition = skillInputData.ButtonStructure.transform.localPosition;
                    skillInputData.ButtonStructure.transform.localPosition = new Vector2(buttonPosition.x, skillInputData.ButtonTransform);
                    foreach (Transform img in SkillTreeInfo.FindComponents<Transform>(skillInputData.ButtonStructure, "UIFrame")) { img.localPosition = new Vector2(0, 0 - 0.5f); }
                    foreach (Image img in SkillTreeInfo.FindComponents<Image>(skillInputData.ButtonStructure, "UIFrame")) { img.sprite = m_skillButtonFrame[1]; }
                }
            }
            else
            {
                DashButtonVisualFeedBack(skillInputData, KeyCode.W, 0);
                DashButtonVisualFeedBack(skillInputData, KeyCode.S, 1);
                DashButtonVisualFeedBack(skillInputData, KeyCode.Space, 2);
                DashButtonVisualFeedBack(skillInputData, KeyCode.A, 3);
                DashButtonVisualFeedBack(skillInputData, KeyCode.D, 4);
            }
        }
    }
}