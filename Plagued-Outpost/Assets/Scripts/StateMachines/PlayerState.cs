using UnityEngine;

public partial class PlayerState : StateMachineBehaviour
{
    private static float m_horizontal; public static float Horizontal { get { return m_horizontal; } }
    private static float m_vertical; public static float Vertical { get { return m_vertical; } }

    private static float m_randomActionIdleTime; public static float RandomActionIdleTime { get { return m_randomActionIdleTime; } }
    private static bool m_isShoeIdleStateAnimated = false; public static bool IsShoeIdleStateAnimated { get { return m_isShoeIdleStateAnimated; } }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.fullPathHash == FightingState.fightingIdleState)
        {
            AnimatorUtility.SetIntParameter(animator, "SkillSetNumber", 0);
            AnimatorUtility.SetIntParameter(animator, "ComboNumber", 0);
            animator.SetFloat(AnimatorUtility.PS.actionStancePara, 10f);
        }
        if (stateInfo.fullPathHash == AnimatorUtility.PS.jogState && m_isShoeIdleStateAnimated == true)
        {
            AnimatorUtility.SetBoolParameter(animator, "Idle", false);
            animator.SetBool(AnimatorUtility.PS.sideJumpPara, false);
            animator.SetBool(AnimatorUtility.PS.forwardJumpPara, false);
            AnimatorUtility.SetBoolParameter(animator, "Fighting", false);
        }
        else if (stateInfo.fullPathHash == AnimatorUtility.PS.jogState && m_isShoeIdleStateAnimated == false)
        {
            AnimatorUtility.SetBoolParameter(animator, "Idle", false);
            animator.SetBool(AnimatorUtility.PS.sideJumpPara, false);
            animator.SetBool(AnimatorUtility.PS.forwardJumpPara, false);
            animator.SetFloat(AnimatorUtility.PS.actionStancePara, 10f);
            m_randomActionIdleTime = Random.Range(3, 8);
            AnimatorUtility.SetBoolParameter(animator, "Fighting", false);
        }
        if (stateInfo.fullPathHash == AnimatorUtility.PS.idleState) {  AnimatorUtility.SetBoolParameter(animator, "Idle", true); }
        if (stateInfo.fullPathHash == AnimatorUtility.PS.shoeIdleState) { m_isShoeIdleStateAnimated = true; }
        if (stateInfo.fullPathHash != AnimatorUtility.PS.shoeIdleState && stateInfo.fullPathHash != AnimatorUtility.PS.jogState && stateInfo.fullPathHash != AnimatorUtility.PS.actionToStandingIdleState)
        {
            m_isShoeIdleStateAnimated = false; 
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.fullPathHash == AnimatorUtility.PS.sprintTackleState) { animator.SetBool(AnimatorUtility.PS.sprintTacklePara, false); }
        if (stateInfo.fullPathHash == AnimatorUtility.PS.sprintJumpState)
        {
            animator.SetBool(AnimatorUtility.PS.sprintJumpPara, false);
            animator.SetBool(AnimatorUtility.PS.sprintPara, false);
        }
        if (stateInfo.fullPathHash == AnimatorUtility.PS.fastSprintJumpState)
        {
            animator.SetBool(AnimatorUtility.PS.fastSprintJumpPara, false);
            if (Input.GetKey(KeyCode.W) == false) { animator.SetBool(AnimatorUtility.PS.sprintPara, false); }
        }
        if (stateInfo.fullPathHash == AnimatorUtility.PS.sprintState)
        {
            animator.speed = 1; animator.SetFloat(AnimatorUtility.PS.sprintSpeedPara, animator.speed);
        }
        if (stateInfo.fullPathHash == AnimatorUtility.PS.jogState) { AnimatorUtility.SetBoolParameter(animator, "Jogging", false); }
        if (stateInfo.fullPathHash == AnimatorUtility.PS.idleState) { AnimatorUtility.SetBoolParameter(animator, "Idle", false); }
    }

    private static TransformManipulator m_transformManipulator;
    public static void SetTransformManipulator(System.Type classType, TransformManipulator transformManipulator)
    {
        if (classType == typeof(AttackEffects)) { m_transformManipulator = transformManipulator; }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_vertical = Input.GetAxis("Vertical");
        m_horizontal = Input.GetAxis("Horizontal");
        if (BoundCollider.ObstacleColliding) { m_vertical = 0; m_horizontal = 0; }
        if (m_transformManipulator.RayCast.FrontColliding)     { if (m_vertical > 0)   { m_vertical = 0;   } }
        if (m_transformManipulator.RayCast.BackColliding)      { if (m_vertical < 0)   { m_vertical = 0;   } }
        if (m_transformManipulator.RayCast.RightColliding)     { if (m_horizontal > 0) { m_horizontal = 0; } }
        if (m_transformManipulator.RayCast.LeftColliding)      { if (m_horizontal < 0) { m_horizontal = 0; } }
        if (m_transformManipulator.RayCast.FrontRightColliding)
        {
            if (m_vertical > 0) { m_vertical = 0; }
            if (m_horizontal > 0) { m_horizontal = 0; }
        }
        if (m_transformManipulator.RayCast.BackLeftColliding)
        {
            if (m_vertical < 0) { m_vertical = 0; }
            if (m_horizontal < 0) { m_horizontal = 0; }
        }
        if (m_transformManipulator.RayCast.BackRightColliding)
        {
            if (m_vertical < 0) { m_vertical = 0; }
            if (m_horizontal > 0) { m_horizontal = 0; }
        }
        if (m_transformManipulator.RayCast.FrontLeftColliding)
        {
            if (m_vertical > 0) { m_vertical = 0; }
            if (m_horizontal < 0) { m_horizontal = 0; }
        }
        animator.SetFloat(AnimatorUtility.PS.verticalPara, m_vertical, 0.15f, Time.deltaTime);
        animator.SetFloat(AnimatorUtility.PS.horizontalPara, m_horizontal, 0.15f, Time.deltaTime);
        if (m_vertical == 0 && m_horizontal == 0) { AnimatorUtility.SetBoolParameter(animator, "IsMoving", false); }
        else { m_isShoeIdleStateAnimated = false; AnimatorUtility.SetBoolParameter(animator, "IsMoving", true); }
        if (m_vertical != 1) { animator.SetBool(AnimatorUtility.PS.sprintPara, false); }

        if ( stateInfo.fullPathHash == AnimatorUtility.PS.sprintBreakState   || stateInfo.fullPathHash == AnimatorUtility.PS.dashBreakState   ||
             stateInfo.fullPathHash == AnimatorUtility.PS.backStepBreakState || stateInfo.fullPathHash == AnimatorUtility.PS.dashBreakStateC3 ||
             stateInfo.fullPathHash == AnimatorUtility.PS.dashBreakStateC6   || stateInfo.fullPathHash == AnimatorUtility.PS.dashBreakStateC7    )

             { AnimatorUtility.SetBoolParameter(animator, "Break", true);  }
        else { AnimatorUtility.SetBoolParameter(animator, "Break", false); }
    }
}