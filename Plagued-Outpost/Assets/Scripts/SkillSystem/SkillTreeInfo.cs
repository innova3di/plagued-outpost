using System.Collections.Generic;
using UnityEngine;
using InnoVision;

public class SkillData
{
    public int SkillSetIndex { get; private set; }
    public GameObject ButtonStructure { get; private set; }
    public float ButtonTransform { get; private set; }
    public float ComboTimeFrame { get; private set; }
    public KeyCode KeyCode { get; private set; }
    public SkillSet SkillSet { get; private set; }

    public float CoolDownTime { get; private set; }
    public bool IsOnCoolDown { get; set; }
    public bool IsActivated { get; set; }

    public SkillData(params object[] skillTreeInformationArray)
    {
        SkillSetIndex = (int)skillTreeInformationArray[0];
        ButtonStructure = (GameObject)skillTreeInformationArray[1];
        ButtonTransform = (float)skillTreeInformationArray[2];
        ComboTimeFrame = (float)skillTreeInformationArray[3];
        CoolDownTime = (float)skillTreeInformationArray[4];
        KeyCode = (KeyCode)skillTreeInformationArray[5];
        SkillSet = (SkillSet)skillTreeInformationArray[6];
        IsOnCoolDown = false;
        IsActivated = false;
    }
}

[System.Serializable]
public class SkillTreeInfo
{
    public GameObject buttonStructure;
    public float comboTimeFrame;
    public float coolDownTime;

    public KeyCode keyCode;
    public SkillSet skillSet;

    private List<SkillData> m_skillList = new List<SkillData>();
    public List<SkillData> SkillList
    {
        get { return m_skillList; }
    }
    private List<SkillData> m_globalSkillList = new List<SkillData>();
    public List<SkillData> GlobalSkillList
    {
        get { return m_globalSkillList; }
    }

    private static object[] CreateSkillTreeObject(SkillTreeInfo skillTreeInformation, SkillTreeInfo[] skillTree)
    {
        return new object[7]
        {
            SearchSkillSet(skillTreeInformation.skillSet, skillTree),
            skillTreeInformation.buttonStructure,
            skillTreeInformation.buttonStructure.transform.localPosition.y,
            skillTreeInformation.comboTimeFrame,
            skillTreeInformation.coolDownTime,
            skillTreeInformation.keyCode,
            skillTreeInformation.skillSet
        };
    }

    public void StoreComboSkillInfo(SkillTreeInfo skillTreeInformation, SkillTreeInfo[] skillTree)
    {
        m_skillList.Add(new SkillData(CreateSkillTreeObject(skillTreeInformation, skillTree)));
    }
    public void StoreGlobalSkillInfo(SkillTreeInfo skillTreeInformation, SkillTreeInfo[] skillTree)
    {
        m_globalSkillList.Add(new SkillData(CreateSkillTreeObject(skillTreeInformation, skillTree)));
    }
    public static T FindComponent<T>(SkillData skillInputData, string tag)
    {
        foreach (Transform uiObject in skillInputData.ButtonStructure.transform)
        {
            if (uiObject.tag == tag)
            {
                return uiObject.GetComponent<T>();
            }
        }
        return default(T);
    }

    public static List<T> FindComponents<T>(GameObject gameObject, string tag)
    {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>(true);
        List<T> objs = new List<T>();
        foreach (Transform obj in transforms)
        {
            if (obj.tag == tag)
            {
                objs.Add(obj.GetComponent<T>());
            }
        }
        return objs;
    }

    public static Transform FindObject(List<Transform> objs, string name)
    {
        for (int i = 0; i < objs.Count; i++)
        {
            if (objs[i].name == name)
            {
                return objs[i];
            }
        }
        return null;
    }

    public static int SearchSkillSet(SkillSet skillSet, SkillTreeInfo[] skillTree)
    {
        int SkillTreeIndex = 0;
        foreach (SkillTreeInfo skill in skillTree)
        {
            if (skill.skillSet == skillSet) { return SkillTreeIndex; }
            else { SkillTreeIndex += 1; }
        }
        return default(int);
    }
}