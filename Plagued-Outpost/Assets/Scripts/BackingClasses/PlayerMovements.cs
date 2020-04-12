using UnityEngine;

public class PlayerMovements : CharacterInertia
{
    public void Idle(Animator animator)
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            animator.SetBool(AnimatorUtility.PS.walkingPara, true);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool(AnimatorUtility.PS.standingJumpPara, true);
        }
    }
    public void Walk(Animator animator)
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            animator.SetBool(AnimatorUtility.PS.walkingPara, true);
        }
        else { animator.SetBool(AnimatorUtility.PS.walkingPara, false); }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool(AnimatorUtility.PS.standingJumpPara, true);
        }
        else { animator.SetBool(AnimatorUtility.PS.standingJumpPara, false); }
    }
    public void Jog(Animator animator)
    {
        AnimatorUtility.SetBoolParameter(animator, "Jogging", true);
        m_sprintSpeedRecord = 1; m_isSprintRecordAssigned = true;
        if (AnimatorUtility.GetBoolParameter(animator, "IsMoving") == false)
        {
            if (animator.GetFloat(AnimatorUtility.PS.actionStancePara) > 0)
            {
                animator.SetFloat(AnimatorUtility.PS.actionStancePara, animator.GetFloat(AnimatorUtility.PS.actionStancePara) - Time.deltaTime);
                if (animator.GetFloat(AnimatorUtility.PS.actionStancePara) < PlayerState.RandomActionIdleTime && PlayerState.IsShoeIdleStateAnimated == false)
                {
                    if(AnimatorUtility.GetIntParameter(animator, "SkillSetNumber") == 0) {  AnimatorUtility.SetBoolParameter(animator, "Idle", true); }
                }
            }
        }
        else { if (animator.GetFloat(AnimatorUtility.PS.actionStancePara) != 10) { animator.SetFloat(AnimatorUtility.PS.actionStancePara, 10f); } }
        if (Input.GetKey(KeyCode.LeftControl)) { animator.SetBool(AnimatorUtility.PS.walkingPara, true); }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (PlayerState.Vertical > 0) { animator.SetBool(AnimatorUtility.PS.sprintPara, true); }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (PlayerState.Vertical > 0) { animator.SetBool(AnimatorUtility.PS.forwardJumpPara, true); }
            if (PlayerState.Vertical == 0 && PlayerState.Horizontal != 0) { animator.SetBool(AnimatorUtility.PS.sideJumpPara, true); }
            if (AnimatorUtility.GetBoolParameter(animator, "IsMoving") == false) { animator.SetBool(AnimatorUtility.PS.standingJumpPara, true); }
        }
        else { animator.SetBool(AnimatorUtility.PS.standingJumpPara, false); }
    }
    public void Sprint(Animator animator)
    {
        if (Input.GetKey(KeyCode.W) && animator.GetBool(AnimatorUtility.PS.fastSprintJumpPara) == false) 
        {
            if (animator.speed < 1.5f) 
            {
                if (m_isSprintRecordAssigned == false) { animator.speed = m_sprintSpeedRecord * 0.85f; m_isSprintRecordAssigned = true; }
                animator.speed += 0.0020f; m_inertia = 1.25f * animator.speed / 60;
                animator.SetFloat(AnimatorUtility.PS.sprintSpeedPara, animator.speed);
            }
            if (animator.speed > 1.5f) { m_inertia = 2.5f * animator.speed / 60; }
        }
        if (Input.GetKeyUp(KeyCode.W) && (animator.GetBool(AnimatorUtility.PS.fastSprintJumpPara) == false && animator.GetBool(AnimatorUtility.PS.sprintJumpPara) == false) && animator.speed > 1.2f)
        {
            animator.SetBool(AnimatorUtility.PS.sprintTacklePara, true); m_inertia = 5.55f * animator.speed / 90;
            animator.SetFloat(AnimatorUtility.PS.sprintSpeedPara, animator.speed);
        }
        else if(Input.GetKeyUp(KeyCode.W) && (animator.GetBool(AnimatorUtility.PS.fastSprintJumpPara) == false && animator.GetBool(AnimatorUtility.PS.sprintJumpPara) == false) )
        {
            AnimatorUtility.SetBoolParameter(animator, "Break", true); m_inertia = 2.775f * animator.speed / 100;
            animator.SetFloat(AnimatorUtility.PS.sprintSpeedPara, animator.speed);
        }
        if (Input.GetKeyDown(KeyCode.Space) && animator.GetBool(AnimatorUtility.PS.sprintPara) == true && animator.GetBool(AnimatorUtility.PS.fastSprintJumpPara) == false)
        {
            if (animator.speed > 1.2)
            {
                animator.SetBool(AnimatorUtility.PS.fastSprintJumpPara, true);
                m_sprintSpeedRecord = animator.speed; m_inertia = 5.55f * animator.speed / 60;
                m_isSprintRecordAssigned = false;
            }
            else { animator.SetBool(AnimatorUtility.PS.sprintJumpPara, true); m_inertia = 5.55f * animator.speed / 120; }
            animator.speed = 1; animator.SetFloat(AnimatorUtility.PS.sprintSpeedPara, animator.speed);
        }
    }
}