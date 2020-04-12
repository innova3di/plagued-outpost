using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct CameraLayoffInfo
{
    public bool CameraLayoffFX { get; set; }
    public bool LayoffRecovery { get; set; }
    public Vector3 LayoffPosition { get; set; }
    public Vector3 RecoveryPosition { get; set; }
}

public class CameraVFX : MonoBehaviour
{
    // public float slowDownAmount;
    public Animator playerAnimator;

    public bool IsShaking { get; private set; }

    private EnemyDetector m_enemyDetector;
    private Camera m_camera;
    private AnimatorStateInfo m_pStateInfo, m_eStateInfo;
    private bool m_isInit;
    private Vector3 m_camInitPos;
    private Quaternion m_camInitRot;
    private float m_power;
    private float m_duration;
    private Animator m_eAnimator;

    private void Start()
    {
        m_enemyDetector = transform.root.GetComponentInChildren<EnemyDetector>();
        m_camera = GetComponent<Camera>();
    }

    private bool m_sprintLayoff;
    public CameraLayoffInfo DLI = new CameraLayoffInfo();
    public CameraLayoffInfo SLI = new CameraLayoffInfo();

    private bool[] m_camShakeReset = new bool[2];

    private void Update()
    {
        m_pStateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        if (m_enemyDetector.EnemyAnimator != null)
        {
            m_eAnimator = m_enemyDetector.EnemyAnimator;
            m_eStateInfo = m_eAnimator.GetCurrentAnimatorStateInfo(0);
        }
        DashFieldOfView();
        LayoffEffect(AnimatorUtility.PS.dashState, ref DLI, 3.25f, 15f, 5f);
        if (m_pStateInfo.fullPathHash == AnimatorUtility.PS.rollState) { m_sprintLayoff = true; }
        if (m_sprintLayoff) { LayoffEffect(AnimatorUtility.PS.sprintState, ref SLI, 2.50f, 4.50f, 2.50f, playerAnimator.speed >= 1.325f); }

        if (m_pStateInfo.fullPathHash != FightingState.Combo1[7])
        {
            if (m_eAnimator != null) { CamShake(m_eAnimator.GetComponentInChildren<AttackEffects>(), m_eStateInfo, ref m_camShakeReset[0]); }
            CamShake(playerAnimator.GetComponentInChildren<AttackEffects>(), m_pStateInfo, ref m_camShakeReset[1]);
        }
    }

    private void CamShake(AttackEffects attkFX, AnimatorStateInfo stateInfo, ref bool camShakeReset)
    {
        if (camShakeReset && attkFX.HitConfirmed && !m_isInit)
        {
            m_isInit = true;
            m_camInitPos = transform.localPosition;
            m_camInitRot = transform.localRotation;
            AttackVFX attkVFX = attkFX.attkCollection.GetVFXInfo(stateInfo.fullPathHash);
            m_power = attkVFX.CamShakeInfo.Power;
            m_duration = attkVFX.CamShakeInfo.Duration;
            // Debug.Log(attkFX.tag + " Shake Power = " + attkVFX.CamShakeInfo.Power + ", Duration = " + attkVFX.CamShakeInfo.Duration);
            if (m_power == 0 && m_duration == 0) { m_power = 0.175f; m_duration = 0.150f; Debug.Log("AttkVFX did not returned a value.. "); }
        }
        else if (camShakeReset && !m_isInit && attkFX.AttackMissed && stateInfo.fullPathHash == AnimatorUtility.EnemyAttacks[0])
        {
            m_isInit = true;
            m_camInitPos = transform.localPosition;
            m_camInitRot = transform.localRotation;
            m_power = 0.175f; m_duration = 0.175f;
            // Debug.Log(attkFX.tag + " Attack missed init | Power = " + m_power + ", Duration = " + m_duration);
        }
        if (m_isInit && m_duration > 0) // this sometimes turns true when zombie's Groundpound is about to hit
        {
            IsShaking = true;
            transform.localPosition = m_camInitPos + Random.insideUnitSphere * m_power;
            var euler = transform.eulerAngles; euler.z = Random.Range(-1f, 1f) * m_power * 2.50f; transform.eulerAngles = euler;
            m_duration -= Time.deltaTime;
            // Debug.Log(attkFX.tag + " Shake P = " + m_power + ", D =  " + m_duration);
        }
        else if (m_duration < 0 && camShakeReset)
        {
            // if (m_duration < 0) { Debug.Log("Shaking ended.. "); }
            m_duration = 0;
            camShakeReset = false;
            IsShaking = false; m_isInit = false;
            transform.localPosition = m_camInitPos;
            transform.localRotation = m_camInitRot;
        }
        if ((((stateInfo.IsTag("Attack") && stateInfo.fullPathHash != AnimatorUtility.EnemyAttacks[0]) || stateInfo.IsTag("DashAttack") || stateInfo.IsTag("ComboFinisher")) && !attkFX.HitConfirmed))
        {
            if (!camShakeReset) { camShakeReset = true; /* Debug.Log("Holy CamShake Resetted.. "); */ }
        }
        if (stateInfo.fullPathHash == AnimatorUtility.EnemyAttacks[0] && !attkFX.AttackMissed && !attkFX.HitConfirmed)
        {
            if (!camShakeReset) { camShakeReset = true; /* Debug.Log("GP CamShake Resetted.. "); */ } // For missed GP
        }
    }

    private void LayoffEffect(int stateHash, ref CameraLayoffInfo cli, float layoffAmount, float layoffSpeed, float recoverySpeed, bool speedThreshold = true)
    {
        if (m_pStateInfo.fullPathHash == stateHash && speedThreshold && !cli.CameraLayoffFX)
        {
            cli.CameraLayoffFX = true;
            cli.RecoveryPosition = transform.localPosition;
            cli.LayoffPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - layoffAmount);
        }
        if (cli.CameraLayoffFX && !cli.LayoffRecovery && transform.localPosition != cli.LayoffPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, cli.LayoffPosition, Time.deltaTime * layoffSpeed);
            if (transform.localPosition == cli.LayoffPosition && !cli.LayoffRecovery) { cli.LayoffRecovery = true; }
        }
        else if (cli.LayoffRecovery && transform.localPosition != cli.RecoveryPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, cli.RecoveryPosition, Time.deltaTime * recoverySpeed);
            if (transform.localPosition == cli.RecoveryPosition && cli.LayoffRecovery)
            {
                m_sprintLayoff = false;
                cli.LayoffRecovery = false;
                cli.CameraLayoffFX = false;
            }
        }
    }

    public bool DashLayoffFOV { get; private set; }
    private bool m_recovery;

    private void DashFieldOfView()
    {
        if (m_pStateInfo.fullPathHash == AnimatorUtility.PS.dashState && !DashLayoffFOV)
        {
            DashLayoffFOV = true;
        }
        if (DashLayoffFOV && !m_recovery && m_camera.fieldOfView < 70)
        {
            m_camera.fieldOfView += Time.deltaTime * 30f;
            if (m_camera.fieldOfView > 70 && !m_recovery)
            {
                m_camera.fieldOfView = 70;
                m_recovery = true;
            }
        }
        else if (m_recovery && m_camera.fieldOfView > 60)
        {
            m_camera.fieldOfView -= Time.deltaTime * 20f;
            if (m_camera.fieldOfView < 60 && m_recovery)
            {
                m_camera.fieldOfView = 60;
                m_recovery = false;
                DashLayoffFOV = false;
            }
        }
    }
}
