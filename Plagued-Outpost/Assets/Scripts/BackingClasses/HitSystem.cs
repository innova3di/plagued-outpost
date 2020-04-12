using UnityEngine;

public class HitSystem
{
    private float m_accumulatedVelocity;
    private float m_dampingPercentage = 1f;

    public void HitImpact(AttackInfo attackInfo, out bool m_isKnockedBack, RaycastService rayCast, BoundCollider boundCollider)
    {
        if (!AnimatorUtility.GetCurrentStateInfo(rayCast.Target).IsTag("Recovery") || AnimatorUtility.GetCurrentStateHash(rayCast.Target) == AnimatorUtility.PS.livershotKnockdown)
        {
            if (Mathf.Abs(m_accumulatedVelocity) < Mathf.Abs(attackInfo.KnockDistance) && !rayCast.Target.GetComponentInChildren<BoundCollider>().WallColliding)
            {
                // Debug.Log("Damping = " + m_dampingPercentage);
                rayCast.Target.transform.position += (rayCast.Target.transform.forward * attackInfo.KnockVelocity) * m_dampingPercentage * (Time.deltaTime * 35f);
                m_accumulatedVelocity += attackInfo.KnockVelocity * (Time.deltaTime * 35f);
                if (m_accumulatedVelocity <= (attackInfo.KnockDistance))
                {
                    m_dampingPercentage = (m_accumulatedVelocity / attackInfo.KnockDistance);
                }
                m_isKnockedBack = true; // Debug.Log("Hit Impact Function = " + rayCast.Target.GetComponentInChildren<BoundCollider>().tag);
            }
            else if (Mathf.Abs(m_accumulatedVelocity) < Mathf.Abs(attackInfo.KnockDistance) && rayCast.Target.GetComponentInChildren<BoundCollider>().WallColliding)
            {
                m_accumulatedVelocity = 0; m_dampingPercentage = 1; rayCast.Target.applyRootMotion = false; m_isKnockedBack = false;
            }
            else if (Mathf.Abs(m_accumulatedVelocity) >= Mathf.Abs(attackInfo.KnockDistance) && !rayCast.Target.GetComponentInChildren<BoundCollider>().WallColliding)
            {
                m_accumulatedVelocity = 0; m_dampingPercentage = 1; m_isKnockedBack = false;
            }
            else { m_accumulatedVelocity = 0; m_dampingPercentage = 1; m_isKnockedBack = false; }
        }
        else { m_accumulatedVelocity = 0; m_dampingPercentage = 1; m_isKnockedBack = false; }
    }
    public void HitImpact(AttkInfo attackInfo, out bool m_isKnockedBack, RaycastService rayCast)
    {
        if (Mathf.Abs(m_accumulatedVelocity) < Mathf.Abs(attackInfo.KnockDistance))
        {
            // Debug.Log("Damping = " + m_dampingPercentage);
            rayCast.Target.transform.position += (rayCast.Target.transform.forward * attackInfo.KnockVelocity) * m_dampingPercentage;
            m_accumulatedVelocity += attackInfo.KnockVelocity;
            if (m_accumulatedVelocity <= (attackInfo.KnockDistance * 0.75f))
            {
                m_dampingPercentage = ((attackInfo.KnockDistance - m_accumulatedVelocity) / attackInfo.KnockDistance) * 2.25f;
            }
            m_isKnockedBack = true;
        }
        else { m_accumulatedVelocity = 0; m_dampingPercentage = 1; m_isKnockedBack = false; }
    }

    public void HitConfirm(Animator targetAnimator, Transform attkerTransform, Camera playerCamera, AttackInfo info, out bool hitConfirm)
    {
        AnimatorUtility.SetIntParameter(targetAnimator, "HitSet", info.SkillSet);
        AnimatorUtility.SetIntParameter(targetAnimator, "HitNumber", info.ComboNumber);
        if (targetAnimator.name == "InnoVision")
        {
            CameraMovements.snapPlayerAndCamRotationInPlace = false;
            Vector3 lookPosition = attkerTransform.transform.root.position - targetAnimator.transform.position;
            lookPosition.y = 0;
            var cameraInitRotation = playerCamera.transform.rotation;
            var cameraInitPosition = playerCamera.transform.position;
            targetAnimator.transform.rotation = Quaternion.LookRotation(lookPosition);
            playerCamera.transform.rotation = cameraInitRotation;
            playerCamera.transform.position = cameraInitPosition;
        }
        else if (targetAnimator.name == "Parasite_L_Starkie")
        {
            Vector3 lookPosition = attkerTransform.transform.root.position - targetAnimator.transform.position;
            lookPosition.y = 0;
            targetAnimator.transform.rotation = Quaternion.LookRotation(lookPosition);
        }
        hitConfirm = true;
    }
}
// if (SkillSystem.CurrentStateInfo.normalizedTime > AttackSFX.CurrentFrameEnd && SkillSystem.CurrentStateInfo.normalizedTime < AttackSFX.CurrentFrameEnd + 0.05f)