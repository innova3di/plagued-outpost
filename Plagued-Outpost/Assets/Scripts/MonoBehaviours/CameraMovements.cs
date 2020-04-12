using UnityEngine;
using System.Collections;

public class CameraMovements : MonoBehaviour
{
    public Transform player;
    public Transform cameraFocus;

    public GameObject targetLock;
    public Transform targetLockTrans;

    public AudioClip targetLockAC;
    public AudioClip targetReleaseAC;

    public float cameraDistance;
    public float CameraMaxDistance;
    public float CameraMinDistance;

    public float xSpeed = 150.0f;
    public float ySpeed = 150.0f;

    public float yMinLimit;
    public float yMaxLimit;

    public static bool snapPlayerAndCamRotationInPlace = true;

    private Animator m_animator;
    private CameraVFX m_cameraVFX;
    private Vector3 m_cameraLookPos;
    private RaycastService m_rayCastService;

    private Notification m_notification;

    private AudioSystem m_audioSystem = new AudioSystem();

    private float m_vertical;
    private float m_horizontal;

    private Quaternion m_targetRotation;

    private bool m_recoveredFromAttack;
    private AnimatorStateInfo LSI, CSI; // NSI

    private int CSH, NSH;

    private void Start()
    {
        m_animator = player.GetComponent<Animator>();
        m_cameraVFX = GetComponent<CameraVFX>();
        m_rayCastService = player.GetComponentInChildren<TransformManipulator>().RayCast;

        cameraDistance = CameraMaxDistance;

        Vector3 angles = transform.eulerAngles;
        m_vertical = angles.x;
        m_horizontal = angles.y;

        m_notification = GameObject.Find("HUDManager").GetComponentInChildren<Notification>();

        // Make the rigid body not change rotation
        // if (GetComponent<Rigidbody>()) { GetComponent<Rigidbody>().freezeRotation = true; }
    }

    private void Update()
    {
        CameraStateBehaviour(); CameraRotate(); RotateCharacter(); CharacterZoomInAndOut(); LockOnTarget(); m_audioSystem.AudioSystemUpdate();
    }

    private void CameraStateBehaviour()
    {
        if (cameraFocus)
        {
            NSH = AnimatorUtility.GetNextStateHash(m_animator);
            CSH = AnimatorUtility.GetCurrentStateHash(m_animator);
            CSI = AnimatorUtility.GetCurrentStateInfo(m_animator);
            // NSI = AnimatorUtility.GetNextStateInfo(m_animator);

            if (NSH == FightingState.fightingIdleState || NSH == AnimatorUtility.PS.backFlipState) { LSI = CSI; }
            if (CSH == FightingState.fightingIdleState)
            {
                if (LSI.IsTag("Recovery") || LSI.IsTag("EC2") || LSI.IsTag("EC2K") || LSI.IsTag("RQMSB") || LSI.IsTag("EC4"))
                {
                    var nextStateInfo = AnimatorUtility.GetNextStateInfo(m_animator);
                    if (nextStateInfo.IsTag("Attack")) { m_recoveredFromAttack = true; }
                }
                else { m_recoveredFromAttack = false; }
            }

            if (m_recoveredFromAttack && !m_lockOnTargetToggle) { snapPlayerAndCamRotationInPlace = true; m_recoveredFromAttack = false; }

            if (Input.GetMouseButtonDown(2)) { snapPlayerAndCamRotationInPlace = false; }
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                if (CSI.IsTag("SkillAccessible") || CSI.IsTag("FightingIdle") || CSI.IsTag("BasicFightingIdle"))
                {
                    if (!m_lockOnTargetToggle) { snapPlayerAndCamRotationInPlace = true; }
                }
            }
            if (m_rayCastService.Target != null && m_enemyZone == null)
            {
                m_enemyZone = m_rayCastService.Target.GetComponentInChildren<EnemyZone>();
            }
        }
    }

    private EnemyZone m_enemyZone;
    private bool m_isCamReachedTarget /*, m_isCamReachedTargetInit */;
    private bool m_dodgeCamInit, m_recoveryCamInit;
    // private float m_angleInitValue = 0f;
    private Quaternion m_dodgeCamInitRotation;

    private void DodgeTransformation(bool debug)
    {
        if (!m_dodgeCamInit)
        {
            StartCoroutine(m_enemyZone.DazeTimer());
            snapPlayerAndCamRotationInPlace = false;
            m_dodgeCamInit = true;
            m_dodgeCamInitRotation = transform.rotation;
            Vector3 lookPosition = m_enemyZone.transform.root.position - player.position;
            lookPosition.y = 0;
            player.rotation = Quaternion.LookRotation(lookPosition);
            m_isCamReachedTarget = false;
        }
        if (m_rayCastService.TargetDistance < CommandReceiver.DODGE_MIN_DISTANCE * 1.5f) // 1.875f
        {
            if (debug) // Visual Cue for tracking enemy/player repositioning
            {
                VisualDebug.DrawSphere(m_rayCastService.Target.transform.position, Color.red, 0.25f);
                VisualDebug.DrawSphere(player.position, Color.blue, 0.25f);
            }
            m_rayCastService.Target.transform.position += ((player.forward + player.right) * 2) * Time.deltaTime * 10f;
            if (debug) // Visual Cue for tracking enemy/player repositioning
            {
                VisualDebug.DrawSphere(m_rayCastService.Target.transform.position, Color.green, 0.25f);
                VisualDebug.DrawSphere(player.position, Color.magenta, 0.25f);
            }
        }
        player.LookAt(m_enemyZone.transform.root);
        player.RotateAround(m_enemyZone.transform.root.position, Vector3.up, Time.deltaTime * 750f);
        transform.rotation = m_dodgeCamInitRotation; // lock camera rotation in place during player dodge
        transform.position = transform.rotation * new Vector3(0, 0.0f, -cameraDistance) + cameraFocus.position; // Debug.Log("Player dodging.. ");
    }

    private void CamLookAtTarget(bool debug)
    {
        m_cameraLookPos = m_enemyZone.transform.root.position - transform.position; m_cameraLookPos.y = -3f;
        m_targetRotation = Quaternion.LookRotation(m_cameraLookPos);
        if (Mathf.Abs(Quaternion.Angle(transform.rotation, m_targetRotation)) > 0.25f)
        {
            if (debug) { Debug.Log("Slerping.. "); }
            player.LookAt(m_enemyZone.transform.root);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, m_targetRotation, Time.deltaTime * 400f);
            // transform.rotation = Quaternion.Slerp(transform.rotation, m_targetRotation, Time.deltaTime * 12.5f);
        }
        else
        {
            m_isCamReachedTarget = true;
            transform.rotation = m_targetRotation;
            m_vertical = transform.eulerAngles.x;
            m_horizontal = transform.eulerAngles.y;
            if (!m_lockOnTargetToggle) { snapPlayerAndCamRotationInPlace = true; }
            if (debug) { VisualDebug.DrawSphere(player.position, Color.magenta, 0.25f); } // Debug.Log("LookAt Finished.. ");
        }
    }

    private void CameraRotate()
    {
        if (CSI.IsTag("EC1K") || CSI.IsTag("EC2K") || CSI.IsTag("EC3K") || CSI.IsTag("EC4K") || CSI.IsTag("Recovery"))
        {
            if (m_isCamReachedTarget && transform.rotation != m_targetRotation)
            {
                m_isCamReachedTarget = false; // Debug.Log(m_isCamReachedTarget);
            }
        }
        if (((CSI.IsTag("Recovery") || CSI.IsTag("Incapacitated")) || (CSH == FightingState.fightingIdleState && (LSI.IsTag("Recovery") || LSI.IsTag("Incapacitated")))) && !m_isCamReachedTarget)
        {
            if (m_enemyZone != null) { CamLookAtTarget(false); }
        }
        else if (CSH == AnimatorUtility.PS.dodgeCounterState && CSI.normalizedTime < 0.50f) { DodgeTransformation(false); }
        else if ((CSH == AnimatorUtility.PS.dodgeCounterState && CSI.normalizedTime > 0.50f) || ((CSI.IsTag("SkillAccessible") || CSI.IsTag("Attack") || CSI.IsTag("ComboFinisher")) && LSI.fullPathHash == AnimatorUtility.PS.dodgeCounterState) && !m_isCamReachedTarget)
        {
            if (!m_cameraVFX.IsShaking) { if (m_enemyZone != null) { CamLookAtTarget(false); } }
        }
        else if (!m_cameraVFX.IsShaking && !m_cameraVFX.DLI.CameraLayoffFX && !m_cameraVFX.SLI.CameraLayoffFX)
        {
            if (m_dodgeCamInit) { m_dodgeCamInit = false; }
            if (m_recoveryCamInit) { m_recoveryCamInit = false; }
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                m_vertical -= Input.GetAxis("Mouse Y") * ySpeed;
                m_horizontal += Input.GetAxis("Mouse X") * xSpeed;
            }
            m_vertical = Mathf.Clamp(m_vertical, yMinLimit, yMaxLimit);
            transform.rotation = Quaternion.Euler(m_vertical, cameraFocus.eulerAngles.y, 0);
            cameraFocus.rotation = Quaternion.Euler(0, m_horizontal, 0);
        }
        if (!m_cameraVFX.IsShaking && !m_cameraVFX.DLI.CameraLayoffFX && !m_cameraVFX.SLI.CameraLayoffFX)
        {
            transform.position = transform.rotation * new Vector3(0, 0.0f, -cameraDistance) + cameraFocus.position;
        }
    }

    private void CharacterZoomInAndOut()
    {
        if (!m_cameraVFX.DLI.CameraLayoffFX && !m_cameraVFX.SLI.CameraLayoffFX)
        {
            cameraDistance -= Input.mouseScrollDelta.y * 0.50f;
            if (cameraDistance < CameraMinDistance)
            {
                cameraDistance = CameraMinDistance;
            }
            else if (cameraDistance > CameraMaxDistance)
            {
                cameraDistance = CameraMaxDistance;
            }
        }
    }

    private void RotateCharacter()
    {
        if (snapPlayerAndCamRotationInPlace == true && !m_cameraVFX.DLI.CameraLayoffFX)
        {
            player.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            // RotationPoint.transform.Rotate(0, Input.GetAxis("Mouse X") * xSpeed * rotateSpeed, 0);
        }
    }

    private bool m_lockOnTargetToggle;
    private GameObject m_targetLock;
    private void LockOnTarget()
    {
        if (m_rayCastService.TargetDistance != 0 && m_rayCastService.TargetDistance < 17.5f)
        {
            if (Input.GetKeyDown(KeyCode.T) && !m_targetLockScaling)
            {
                m_lockOnTargetToggle = !m_lockOnTargetToggle;
                if (m_lockOnTargetToggle)
                {
                    m_notification.Push("Auto Aim: On");
                    m_targetLock = Instantiate(targetLock, targetLockTrans);
                    StartCoroutine(ScaleTargetLock(m_targetLock, 0.625f, false));
                    m_audioSystem.AddAudioToPlay(GetComponent<AudioSource>(), targetLockAC, 1f, 4f);
                }
                if (!m_lockOnTargetToggle)
                {
                    m_notification.Push("Auto Aim: Off");
                    StartCoroutine(ScaleTargetLock(m_targetLock, 0.325f, true));
                    m_audioSystem.AddAudioToPlay(GetComponent<AudioSource>(), targetReleaseAC, 1f, 4f);
                }
            }
            if (m_lockOnTargetToggle)
            {
                TargetLockLookAtCamera();
                snapPlayerAndCamRotationInPlace = false;
                LookAtTarget(m_rayCastService.Target.transform.root);
            }
            if (m_targetLock != null) { TargetLockRenderBehaviour(m_targetLock); }
            // Debug.Log("Target Lockable in Distance: " + m_rayCastService.TargetDistance);
        }
        else if (!snapPlayerAndCamRotationInPlace)
        {
            if (m_lockOnTargetToggle)
            {
                m_lockOnTargetToggle = false;
                m_notification.Push("Target Out of Range");
                StartCoroutine(ScaleTargetLock(m_targetLock, 0.325f, true));
                m_audioSystem.AddAudioToPlay(GetComponent<AudioSource>(), targetReleaseAC, 1f, 4f);
            }
        }
    }

    private void LookAtTarget(Transform target)
    {
        Vector3 lookPosition = target.position - player.position; lookPosition.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        player.rotation = Quaternion.Slerp(player.rotation, rotation, Time.deltaTime * 10f);
    }

    private void TargetLockLookAtCamera()
    {
        Vector3 lookPosition = transform.position - targetLockTrans.position;
        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        targetLockTrans.rotation = Quaternion.Slerp(targetLockTrans.rotation, rotation, Time.deltaTime * 10f);
    }

    private void TargetLockRenderBehaviour(GameObject targetLock)
    {
        if (m_targetLockScaling)
        {
            SetTargetLockAlpha(targetLock.GetComponent<Renderer>().material, 1);
        }
        else if (!m_targetLockScaling)
        {
            if (Vector3.Distance(targetLockTrans.position, transform.position) <= 25f)
            {
                float targetTransparency = Vector3.Distance(targetLockTrans.position, transform.position) / 25f;
                SetTargetLockAlpha(targetLock.GetComponent<Renderer>().material, targetTransparency);
            }
            else if (Vector3.Distance(targetLockTrans.position, transform.position) > 25f)
            {
                if (targetLock.GetComponent<Renderer>().material.color.a < 1)
                {
                    SetTargetLockAlpha(targetLock.GetComponent<Renderer>().material, 1);
                }
            }
        }
    }

    private bool m_targetLockScaling;
    private IEnumerator ScaleTargetLock(GameObject targetLock, float targetScale, bool destroy)
    {
        m_targetLockScaling = true;
        float currentScale = targetLock.transform.localScale.y;
        while (currentScale != targetScale)
        {
            Vector3 tempTargetScale = targetLock.transform.localScale;

            tempTargetScale.x = Mathf.MoveTowards(tempTargetScale.x, targetScale, Time.deltaTime * 2.25f);
            tempTargetScale.y = Mathf.MoveTowards(tempTargetScale.y, targetScale, Time.deltaTime * 2.25f);

            currentScale = tempTargetScale.y;

            targetLock.transform.localScale = tempTargetScale;

            yield return null;
        }
        m_targetLockScaling = false;
        if (destroy) { Destroy(targetLock); }
    }

    private void SetTargetLockAlpha(Material targetLockMaterial, float targetTransparency)
    {
        Color tempColor = targetLockMaterial.color;
        tempColor.a = Mathf.MoveTowards(tempColor.a, targetTransparency, Time.deltaTime * 5);
        targetLockMaterial.color = tempColor;
    }
}