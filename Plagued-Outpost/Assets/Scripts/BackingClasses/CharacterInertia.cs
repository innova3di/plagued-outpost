using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CharacterInertia
{
    // public GameObject character;
    protected static float m_inertia;

    public static float Inertia { get { return m_inertia; } }

    protected static float m_sprintSpeedRecord = 1;
    public static float SprintSpeedRecord { get { return m_sprintSpeedRecord; } }
    protected static bool m_isSprintRecordAssigned = true;
}

public struct MovementStateInformation
{
    private int m_stateHashValue;
    private float m_stateNormalizedTime;
    private float m_inertia;

    public int StateHashValue
    {
        get { return m_stateHashValue; }
    }
    public float StateNormalizedTime
    {
        get { return m_stateNormalizedTime; }
    }
    public float Inertia
    {
        get { return m_inertia; }
    }

    public MovementStateInformation(int stateHashValue, float stateNormalizedTime, float inertia = 0)
    {
        m_stateHashValue = stateHashValue;
        m_stateNormalizedTime = stateNormalizedTime;
        m_inertia = inertia;
    }
}

public struct DashStateInformation
{
    private int m_dashStateHashValue;
    private int m_dashBreakStateHashValue;
    private float m_dashVelocity;
    private float m_dashBreakVelocity;
    private float m_dashBreakNormalizedTime;

    public int DashStateHashValue
    {
        get { return m_dashStateHashValue; }
    }
    public int DashBreakStateHashValue
    {
        get { return m_dashBreakStateHashValue; }
    }

    public float DashVelocity
    {
        get { return m_dashVelocity; }
    }
    public float DashBreakVelocity
    {
        get { return m_dashBreakVelocity; }
    }
    public float DashBreakNormalizedTime
    {
        get { return m_dashBreakNormalizedTime; }
    }

    public DashStateInformation(int dashStateHashValue, int dashBreakStateHashValue, float dashVelocity, float dashBreakVelocity, float dashBreakNormalizedTime)
    {
        m_dashStateHashValue = dashStateHashValue;
        m_dashBreakStateHashValue = dashBreakStateHashValue;
        m_dashVelocity = dashVelocity;
        m_dashBreakVelocity = dashBreakVelocity;
        m_dashBreakNormalizedTime = dashBreakNormalizedTime;
    }

    public static float SearchDashVelocity(List<DashStateInformation> dashStateInfo, int stateHashValue)
    {
        for (int i = 0; i < dashStateInfo.Count; i++)
        {
            if (dashStateInfo[i].DashStateHashValue == stateHashValue)
            {
                return dashStateInfo[i].DashVelocity;
            }
        }
        return default(float);
    }

    public static float SearchBreakVelocity(List<DashStateInformation> dashStateInfo, int stateHashValue)
    {
        for (int i = 0; i < dashStateInfo.Count; i++)
        {
            if (dashStateInfo[i].DashBreakStateHashValue == stateHashValue)
            {
                return dashStateInfo[i].DashBreakVelocity; 
            }
        }
        return default(float);
    }
}

public struct ObstructedDashInfo
{
    public int StateHash { get; private set; }
    public float OriginalDashVelocity { get; private set; }
    public float MaxRayDistance { get; private set; }

    public ObstructedDashInfo(int stateHash, float originalVelocity, float maxRayDistance)
    {
        StateHash = stateHash;
        OriginalDashVelocity = originalVelocity;
        MaxRayDistance = maxRayDistance;
    }
}

