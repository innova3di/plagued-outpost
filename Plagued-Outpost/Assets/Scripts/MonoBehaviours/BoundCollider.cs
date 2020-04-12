using UnityEngine;

public class BoundCollider : MonoBehaviour
{
    // public TransformManipulator transformManipulator;
    // public bool WallImpact { get; private set; }
    public bool WallColliding { get; private set; }
    public static bool ObstacleColliding { get; private set; }
    public bool PlayerEnemyColliding { get; private set; }

    private Vector3 m_adjustedTransform = new Vector3(0, 0, 0);

    private Animator m_animator;
    private AnimatorStateInfo m_stateInfo;

    public void Start()
    {
        m_animator = transform.root.GetComponent<Animator>();
    }

    public void Update()
    {
        m_stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (tag == "PlayerHitBound")
        {
            if (other.gameObject.tag == "ObstacleBound") { ObstacleColliding = true; /* Debug.Log("Obstacle Touched = " + tag); */ }
        }

        if (other.gameObject.tag == "PlayerHitBound" || other.gameObject.tag == "EnemyHitBound")
        {
            PlayerEnemyColliding = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (tag == "PlayerHitBound")
        {
            if (other.gameObject.tag == "ObstacleBound")
            {
                if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.jogState || AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.idleState || AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.sprintState || AnimatorUtility.GetBoolParameter(m_animator, "FightingIdle"))
                {
                    ObstacleColliding = false;
                }
            }
        }

        if (other.gameObject.tag == "PlayerHitBound" || other.gameObject.tag == "EnemyHitBound")
        {
            if (tag != "Enemy" && other.gameObject.tag == "EnemyHitBound")
            {
                Animator targetAnimator = other.transform.root.GetComponent<Animator>();
                if (!AnimatorUtility.GetBoolParameter(targetAnimator, "IsAttacking") && !other.GetComponent<BoundCollider>().WallColliding)
                {
                    m_adjustedTransform += transform.root.forward * 0.025f; // Debug.Log(other.transform.root.tag + " Attack Pushed Back.. ");
                    other.transform.root.position += m_adjustedTransform;
                }
            }
            else if (other.gameObject.tag == "PlayerHitBound")
            {
                AnimatorStateInfo targetAnimator = other.transform.root.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                if (!targetAnimator.IsTag("Attack") && !targetAnimator.IsTag("DashAttack") && !targetAnimator.IsTag("ComboFinisher") && !other.GetComponent<BoundCollider>().WallColliding)
                {
                    m_adjustedTransform += transform.root.forward * 0.025f; // Debug.Log(other.transform.root.tag + " Attack Pushed Back.. ");
                    other.transform.root.position += m_adjustedTransform;
                }
            }
        }
        else if (other.gameObject.tag == "HigherGround")
        {
            if (other.transform.root.tag != "Road")
            {
                // m_adjustedTransform += transformManipulator.RayCast.HitDirection * 0.01f; m_adjustedTransform.y = 0;
                // transform.root.position += m_adjustedTransform; Debug.Log("Pushed Back.. ");

                if (tag == "Player")
                {
                    WallColliding = true;
                    if (m_stateInfo.IsTag("SkillAccessible"))
                    {
                        m_adjustedTransform += other.transform.forward * 0.025f; m_adjustedTransform.y = 0;
                        transform.root.position += m_adjustedTransform;
                        // else { Debug.Log(tag + " Pushed by wall.. "); }
                    }
                    else if (m_stateInfo.IsTag("EC2") || m_stateInfo.IsTag("EC4") || m_stateInfo.IsTag("Incapacitated"))
                    {
                        m_adjustedTransform += other.transform.forward * 0.025f; m_adjustedTransform.y = 0;
                        transform.root.position += m_adjustedTransform; // Debug.Log(tag + " Continously being pushed by wall.. ");
                    }
                }
                else if (tag == "Enemy")
                {
                    WallColliding = true;
                    if (m_stateInfo.fullPathHash == AnimatorUtility.ES.idleState)
                    {
                        m_adjustedTransform += other.transform.forward * 0.025f; m_adjustedTransform.y = 0;
                        transform.root.position += m_adjustedTransform;
                    }
                    else if (m_stateInfo.fullPathHash != AnimatorUtility.ES.leftSidePushKickHitBK && m_stateInfo.fullPathHash != AnimatorUtility.ES.flipKickHitState)
                    {
                        m_adjustedTransform += other.transform.forward * 0.025f; m_adjustedTransform.y = 0;
                        transform.root.position += m_adjustedTransform; // Debug.Log(tag + " Continously being pushed by wall.. ");
                    }
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (tag == "PlayerHitBound")
        {
            if (other.gameObject.tag == "ObstacleBound") { ObstacleColliding = false; /* Debug.Log("Obstacle Exited = " + tag); */ }
        }
        
        if (other.gameObject.tag == "PlayerHitBound" || other.gameObject.tag == "EnemyHitBound")
        {
            PlayerEnemyColliding = false;
            m_adjustedTransform = new Vector3(0, 0, 0);
        }
        else if (other.gameObject.tag == "HigherGround")
        {
            if (tag == "Player" || tag == "Enemy") { WallColliding = false; m_adjustedTransform = new Vector3(0, 0, 0); }
        }
    }
}
