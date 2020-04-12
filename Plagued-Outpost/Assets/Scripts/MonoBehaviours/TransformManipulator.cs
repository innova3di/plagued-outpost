using System.Collections.Generic;
using UnityEngine;
using InnoVision;

public class TransformManipulator : MonoBehaviour
{
    public CharacterInertia characterInertia;
    public RaycastService RayCast = new RaycastService();

    private List<MovementStateInformation> m_movementStateInfo = new List<MovementStateInformation>();
    private List<DashStateInformation> m_dashStateInfo = new List<DashStateInformation>();

    private Animator m_animator;
    private Invoker m_movement;

    private BoundCollider m_boundCollider;

    public void Start()
    {
        m_animator = transform.root.GetComponent<Animator>();
        m_movement = new Invoker(new Movement(new PlayerMovements(), m_animator));
        m_boundCollider = GetComponent<BoundCollider>();

        m_movementStateInfo.Add(new MovementStateInformation(AnimatorUtility.PS.sprintTackleState, 0.50f));
        m_movementStateInfo.Add(new MovementStateInformation(AnimatorUtility.PS.sprintBreakState, 0.60f));
        m_movementStateInfo.Add(new MovementStateInformation(AnimatorUtility.PS.sprintJumpState, 0.60f));
        m_movementStateInfo.Add(new MovementStateInformation(AnimatorUtility.PS.fastSprintJumpState, 1.0f));

        m_dashStateInfo.Add(new DashStateInformation(AnimatorUtility.PS.dashState, AnimatorUtility.PS.dashBreakState, 0.575f, 0.085f, 0.60f));
        m_dashStateInfo.Add(new DashStateInformation(AnimatorUtility.PS.dashStateC3, AnimatorUtility.PS.dashBreakStateC3, 0.555f, 0.08f, 0.60f));
        m_dashStateInfo.Add(new DashStateInformation(AnimatorUtility.PS.dashStateC6, AnimatorUtility.PS.dashBreakStateC6, 0.485f, 0.0775f, 0.60f));
        m_dashStateInfo.Add(new DashStateInformation(AnimatorUtility.PS.dashStateC7, AnimatorUtility.PS.dashBreakStateC7, 0.55f, 0.085f, 0.60f));

        m_dashStateInfo.Add(new DashStateInformation(AnimatorUtility.PS.backStepState, AnimatorUtility.PS.backStepBreakState, -0.225f, -0.15f, 0.70f));
        m_dashStateInfo.Add(new DashStateInformation(AnimatorUtility.PS.backFlipState, AnimatorUtility.PS.backFlipState, -0.15f, -0.125f, 0.60f));
        m_dashStateInfo.Add(new DashStateInformation(AnimatorUtility.PS.dashLeftState, AnimatorUtility.PS.dashLeftState, -0.215f, 0f, 0.70f));
        m_dashStateInfo.Add(new DashStateInformation(AnimatorUtility.PS.dashRightState, AnimatorUtility.PS.dashRightState, 0.215f, 0f, 0.70f));

        m_dashStateInfo.Add(new DashStateInformation(AnimatorUtility.PS.rollBackState, AnimatorUtility.PS.rollBackBreakState, -0.135f, -0.085f, 0.70f));
        m_dashStateInfo.Add(new DashStateInformation(AnimatorUtility.PS.retreatState, AnimatorUtility.PS.retreatBreakState, -0.15f, -0.1f, 0.70f));

        m_obstructedDashList.Add(new ObstructedDashInfo(AnimatorUtility.PS.dashState, DashStateInformation.SearchDashVelocity(m_dashStateInfo, AnimatorUtility.PS.dashState), RaycastService.FrontBackMaxDistance + 1));
        m_obstructedDashList.Add(new ObstructedDashInfo(AnimatorUtility.PS.dashBreakState, DashStateInformation.SearchBreakVelocity(m_dashStateInfo, AnimatorUtility.PS.dashBreakState), RaycastService.FrontBackMaxDistance - 7));
        m_obstructedDashList.Add(new ObstructedDashInfo(AnimatorUtility.PS.backStepState, DashStateInformation.SearchDashVelocity(m_dashStateInfo, AnimatorUtility.PS.backStepState), RaycastService.FrontBackMaxDistance + 1));
        m_obstructedDashList.Add(new ObstructedDashInfo(AnimatorUtility.PS.backStepBreakState, DashStateInformation.SearchBreakVelocity(m_dashStateInfo, AnimatorUtility.PS.backStepBreakState), RaycastService.FrontBackMaxDistance - 6));
        m_obstructedDashList.Add(new ObstructedDashInfo(AnimatorUtility.PS.backFlipState, DashStateInformation.SearchDashVelocity(m_dashStateInfo, AnimatorUtility.PS.backFlipState), RaycastService.BackFlipMaxDistance + 1));
        m_obstructedDashList.Add(new ObstructedDashInfo(AnimatorUtility.PS.backFlipState, DashStateInformation.SearchBreakVelocity(m_dashStateInfo, AnimatorUtility.PS.backFlipState), RaycastService.BackFlipMaxDistance - 2));
        m_obstructedDashList.Add(new ObstructedDashInfo(AnimatorUtility.PS.dashLeftState, DashStateInformation.SearchDashVelocity(m_dashStateInfo, AnimatorUtility.PS.dashLeftState), RaycastService.SideDashMaxDistance - 2));
        m_obstructedDashList.Add(new ObstructedDashInfo(AnimatorUtility.PS.dashRightState, DashStateInformation.SearchDashVelocity(m_dashStateInfo, AnimatorUtility.PS.dashRightState), RaycastService.SideDashMaxDistance - 2));
        m_obstructedDashList.Add(new ObstructedDashInfo(AnimatorUtility.PS.dashStateC3, DashStateInformation.SearchDashVelocity(m_dashStateInfo, AnimatorUtility.PS.dashStateC3), 8f + 3));
        m_obstructedDashList.Add(new ObstructedDashInfo(AnimatorUtility.PS.dashBreakStateC3, DashStateInformation.SearchBreakVelocity(m_dashStateInfo, AnimatorUtility.PS.dashBreakStateC3), 8f - 4));
        m_obstructedDashList.Add(new ObstructedDashInfo(AnimatorUtility.PS.dashStateC6, DashStateInformation.SearchDashVelocity(m_dashStateInfo, AnimatorUtility.PS.dashStateC6), 7f + 2));
        m_obstructedDashList.Add(new ObstructedDashInfo(AnimatorUtility.PS.dashBreakStateC6, DashStateInformation.SearchBreakVelocity(m_dashStateInfo, AnimatorUtility.PS.dashBreakStateC6), 7f - 4));
        m_obstructedDashList.Add(new ObstructedDashInfo(AnimatorUtility.PS.dashStateC7, DashStateInformation.SearchDashVelocity(m_dashStateInfo, AnimatorUtility.PS.dashStateC7), 8f + 2));
        m_obstructedDashList.Add(new ObstructedDashInfo(AnimatorUtility.PS.dashBreakStateC7, DashStateInformation.SearchBreakVelocity(m_dashStateInfo, AnimatorUtility.PS.dashBreakStateC7), 8f - 4));

        m_obstructedDashList.Add(new ObstructedDashInfo(AnimatorUtility.PS.rollBackState, DashStateInformation.SearchDashVelocity(m_dashStateInfo, AnimatorUtility.PS.rollBackState), 4f));
        m_obstructedDashList.Add(new ObstructedDashInfo(AnimatorUtility.PS.rollBackBreakState, DashStateInformation.SearchBreakVelocity(m_dashStateInfo, AnimatorUtility.PS.rollBackBreakState), 2f));
        m_obstructedDashList.Add(new ObstructedDashInfo(AnimatorUtility.PS.retreatState, DashStateInformation.SearchDashVelocity(m_dashStateInfo, AnimatorUtility.PS.retreatState), 5f));
        m_obstructedDashList.Add(new ObstructedDashInfo(AnimatorUtility.PS.retreatBreakState, DashStateInformation.SearchBreakVelocity(m_dashStateInfo, AnimatorUtility.PS.retreatBreakState), 2f));
    }

    public const float CROSSHAIR_DISTANCE = 2f;
    public void Update()
    {
        RayCast.RayCastColliders(false, LayerMask.GetMask(LayerMask.LayerToName(10), LayerMask.LayerToName(11)));
        RayCast.RaycastTarget(12.5f, "EnemyHitBound", false);
        RayCast.CrossHairRaycast(CROSSHAIR_DISTANCE, "EnemyHitBound", false);
        RayCast.RayCastAdvanceUpdate(m_animator, LayerMask.GetMask(LayerMask.LayerToName(10), LayerMask.LayerToName(11)), false);
        RayCast.LineCastOnGround(m_animator, false);

        m_movement.InvokeCommand();

        PlayerRootMotion();

        if (AnimatorUtility.GetBoolParameter(m_animator, "FightingIdle") || AnimatorUtility.GetBoolParameter(m_animator, "Jogging") || m_animator.GetBool("IsBeingHit"))
        {
            DashLock = false; m_obstacleDashLock = false; if (m_initDashPositionOnce) { m_initDashPositionOnce = false; }
        }
    }

    private List<ObstructedDashInfo> m_obstructedDashList = new List<ObstructedDashInfo>();
    private bool m_dashInitialized;
    private float m_obstructedVelocity;
    private bool m_breakInitialized;

    public bool DashLock { get; private set; }
    private bool m_obstacleDashLock, m_initDashPositionOnce;

    private RaycastHit m_hitInfo;

    private void AdjustVelocity(ObstructedDashInfo obstructedDashInfo, ref bool initialized)
    {
        if (AnimatorUtility.GetCurrentStateHash(m_animator) == obstructedDashInfo.StateHash)
        {
            if (RayCast.IsRayAdvanceColliding(m_animator, out m_hitInfo) || m_obstacleDashLock)
            {
                if (initialized == false)
                {
                    initialized = true;
                    m_obstructedVelocity = obstructedDashInfo.OriginalDashVelocity * (m_hitInfo.distance / obstructedDashInfo.MaxRayDistance);
                }
                // Debug.Log("Obstructed by = " + m_hitInfo.transform.tag);
                m_obstacleDashLock = true;
                Vector3 hitPoint = new Vector3(m_hitInfo.point.x, 0, m_hitInfo.point.z);
                RayCast.Character.position = Vector3.MoveTowards(RayCast.Character.position, hitPoint, (Time.deltaTime * 50f) * Mathf.Abs(m_obstructedVelocity));
            }
            else if (m_obstacleDashLock == false)
            {
                DashLock = true;
                // Debug.Log("Not Obstructed.. ");
                if (obstructedDashInfo.StateHash == AnimatorUtility.PS.dashRightState || obstructedDashInfo.StateHash == AnimatorUtility.PS.dashLeftState)
                {
                    RayCast.Character.position += RayCast.Character.right * obstructedDashInfo.OriginalDashVelocity * (Time.deltaTime * 50f);
                }
                else { RayCast.Character.position += RayCast.Character.forward * obstructedDashInfo.OriginalDashVelocity * (Time.deltaTime * 50f); }
            }
        }
    }

    private void PlayerInertia()
    {
        foreach (DashStateInformation dashInfo in m_dashStateInfo)
        {
            if (AnimatorUtility.GetCurrentStateHash(m_animator) == dashInfo.DashStateHashValue)
            {
                m_breakInitialized = false;
                if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.dashState) { AdjustVelocity(m_obstructedDashList[0], ref m_dashInitialized); }
                else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.dashStateC3) { AdjustVelocity(m_obstructedDashList[8], ref m_dashInitialized); }
                else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.dashStateC6) { AdjustVelocity(m_obstructedDashList[10], ref m_dashInitialized); }
                else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.dashStateC7) { AdjustVelocity(m_obstructedDashList[12], ref m_dashInitialized); }
                else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.backStepState) { AdjustVelocity(m_obstructedDashList[2], ref m_dashInitialized); }
                else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.rollBackState) { AdjustVelocity(m_obstructedDashList[14], ref m_dashInitialized); }
                else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.retreatState) { AdjustVelocity(m_obstructedDashList[16], ref m_dashInitialized); }
                else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.dashLeftState && AnimatorUtility.GetNormalizedTime(m_animator) <= 0.80f)
                {
                    AdjustVelocity(m_obstructedDashList[6], ref m_dashInitialized);
                }
                else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.dashRightState && AnimatorUtility.GetNormalizedTime(m_animator) <= 0.80f)
                {
                    AdjustVelocity(m_obstructedDashList[7], ref m_dashInitialized);
                }
                else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.backFlipState)
                {
                    if (AnimatorUtility.GetNormalizedTime(m_animator) <= 0.30f)
                    {
                        RayCast.Character.position += RayCast.Character.forward * -0.05f * (Time.deltaTime * 50);
                    }
                    if (AnimatorUtility.GetNormalizedTime(m_animator) >= 0.30f)
                    {
                        if (AnimatorUtility.GetNormalizedTime(m_animator) <= 0.60f)
                        {
                            AdjustVelocity(m_obstructedDashList[4], ref m_dashInitialized);
                        }
                    }
                }
            }

            if (AnimatorUtility.GetCurrentStateHash(m_animator) == dashInfo.DashBreakStateHashValue)
            {
                m_dashInitialized = false;
                if (AnimatorUtility.GetNormalizedTime(m_animator) <= dashInfo.DashBreakNormalizedTime)
                {
                    if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.dashBreakState) { AdjustVelocity(m_obstructedDashList[1], ref m_breakInitialized); }
                    else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.dashBreakStateC3) { AdjustVelocity(m_obstructedDashList[9], ref m_breakInitialized); }
                    else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.dashBreakStateC6) { AdjustVelocity(m_obstructedDashList[11], ref m_breakInitialized); }
                    else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.dashBreakStateC7) { AdjustVelocity(m_obstructedDashList[13], ref m_breakInitialized); }
                    else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.backStepBreakState) { AdjustVelocity(m_obstructedDashList[3], ref m_breakInitialized); }
                    else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.rollBackBreakState) { AdjustVelocity(m_obstructedDashList[15], ref m_breakInitialized); }
                    else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.retreatBreakState) { AdjustVelocity(m_obstructedDashList[17], ref m_breakInitialized); }
                }
                else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.backFlipState && AnimatorUtility.GetNormalizedTime(m_animator) >= 0.625f)
                {
                    if (AnimatorUtility.GetNormalizedTime(m_animator) <= 0.725f) { AdjustVelocity(m_obstructedDashList[5], ref m_breakInitialized); }
                }
            }
        }

        if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.sprintState)
        {
            RayCast.Character.position += RayCast.Character.forward * CharacterInertia.Inertia * (Time.deltaTime * 50);
        }

        if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.dodgeCounterState)
        {
            // characterInertia.character.transform.position += characterInertia.character.transform.forward * 0.25f;
            m_animator.applyRootMotion = false;
        }

        foreach (MovementStateInformation movementStateInfo in m_movementStateInfo)
        {
            if (AnimatorUtility.GetCurrentStateHash(m_animator) == movementStateInfo.StateHashValue && AnimatorUtility.GetNormalizedTime(m_animator) < movementStateInfo.StateNormalizedTime)
            {
                RayCast.Character.position += RayCast.Character.forward * CharacterInertia.Inertia * (Time.deltaTime * 50);
            }
        }

    }

    private void PlayerRootMotion()
    {
        // bool IsRayColliding = InnoVisionRaycast.IsFacingTowardsObstacle || InnoVisionRaycast.BackColliding || InnoVisionRaycast.BackRightColliding || InnoVisionRaycast.BackLeftColliding;
        /*
            if (transformManipulator.RayCast.IsOnHigherGround)
            { GetComponent<Collider>().enabled = false; }
            else if (!transformManipulator.RayCast.IsOnHigherGround)
            { GetComponent<Collider>().enabled = true; }
         */
        int SSN = AnimatorUtility.GetIntParameter(m_animator, "SkillSetNumber");
        if (
                AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.dashStateC3 || AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.dashBreakStateC3 ||
                AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.dashStateC6 || AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.dashBreakStateC6 ||
                AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.dashStateC7 || AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.dashBreakStateC7 ||
                (SSN == 9 || SSN == 12 || SSN == 13 || SSN == 14 || SSN == 15 || SSN == 17 || SSN == 18) || m_boundCollider.WallColliding
           )
        {
            if (m_animator.applyRootMotion) { m_animator.applyRootMotion = false; }
        }
        else if (!m_animator.applyRootMotion) { m_animator.applyRootMotion = true; }
        if (!m_boundCollider.WallColliding) { PlayerInertia(); }
    }
}


/*
    if (FightingState.IsFightingIdle && ((SkillSystem.CurrentStateInfo.IsTag("SkillAccessible") && m_animator.GetFloat(AnimatorUtility.PS.actionStancePara) <= 9.5f) || (SkillSystem.CurrentStateInfo.IsTag("FightingIdle") && SkillState.ComboTime >= 0.5f)))
    {
        if (m_startPosInit == true)
        {
            m_startPosInit = false;
            Debug.Log("Distance Traveled = " + Vector3.Distance(m_startPosition, characterInertia.character.transform.position));
        }
    }
*/
