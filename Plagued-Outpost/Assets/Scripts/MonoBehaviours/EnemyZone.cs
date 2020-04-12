using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class EnemyZone : MonoBehaviour
{
    private Animator m_animator;
    public RaycastService RayCast = new RaycastService();

    private PauseMenu m_pauseMenu;

    private int m_stateHash;
    private float m_crossHairLength = 2.5f;

    private List<MovementStateInformation> m_movementStateInfo = new List<MovementStateInformation>();

    public void Awake() { m_pauseMenu = GameObject.Find("HUDManager").GetComponent<PauseMenu>(); }

    public void Start()
    {
        m_animator = transform.root.GetComponent<Animator>();

        m_movementStateInfo.Add(new MovementStateInformation(AnimatorUtility.EnemyAttacks[0], 0.60f, 0.115f));

        m_movementStateInfo.Add(new MovementStateInformation(AnimatorUtility.EnemyAttacks[1], 0.60f, 0.06f));
        m_movementStateInfo.Add(new MovementStateInformation(AnimatorUtility.EnemyAttacks[2], 0.50f, 0.06f));
        m_movementStateInfo.Add(new MovementStateInformation(AnimatorUtility.EnemyAttacks[3], 0.50f, 0.06f));
        m_movementStateInfo.Add(new MovementStateInformation(AnimatorUtility.EnemyAttacks[4], 0.50f, 0.06f));
        m_movementStateInfo.Add(new MovementStateInformation(AnimatorUtility.EnemyAttacks[5], 0.50f, 0.06f));
        m_movementStateInfo.Add(new MovementStateInformation(AnimatorUtility.EnemyAttacks[6], 0.50f, 0.06f));
        m_movementStateInfo.Add(new MovementStateInformation(AnimatorUtility.EnemyAttacks[7], 0.50f, 0.06f));
        m_movementStateInfo.Add(new MovementStateInformation(AnimatorUtility.EnemyAttacks[8], 0.50f, 0.06f));
        m_movementStateInfo.Add(new MovementStateInformation(AnimatorUtility.EnemyAttacks[9], 0.50f, 0.06f));
        m_movementStateInfo.Add(new MovementStateInformation(AnimatorUtility.EnemyAttacks[10], 0.50f, 0.06f));

        m_movementStateInfo.Add(new MovementStateInformation(AnimatorUtility.EnemyAttacks[12], 0.50f, 0.10f));

        m_movementStateInfo.Add(new MovementStateInformation(AnimatorUtility.EnemyAttacks[13], 0.60f, 0.10f));

        m_pauseMenu.enemyAnimator.Add(transform.root.GetInstanceID(), m_animator);
    }

    public void Update()
    {
        m_stateHash = AnimatorUtility.GetCurrentStateHash(m_animator);

        RayCast.RaycastTarget(20f, "PlayerHitBound", false);
        RayCast.CrossHairRaycast(m_crossHairLength, "PlayerHitBound", false);
        AnimatorUtility.SetBoolParameter(m_animator, "CHColliding", RayCast.CrosshairColliding);
        RayCast.LineCastOnGround(m_animator, false);

        if (m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Recovery") || AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.ES.idleState || m_stateHash == AnimatorUtility.ES.sprintState)
        {
            if (!m_animator.applyRootMotion) { m_animator.applyRootMotion = true; }
        }

        /**

        **/

        if (m_stateHash == AnimatorUtility.ES.kneeUppercutC2)
        {
            if (m_animator.applyRootMotion) { m_animator.applyRootMotion = false; }
        }
        else if (m_stateHash == AnimatorUtility.ES.kneeUppercutKnockBackC2)
        {
            if (!m_animator.applyRootMotion) { m_animator.applyRootMotion = true; }
        }




        TransformManipulator();
        AttackingBehaviour();
        Aggression();
    }

    private void TransformManipulator()
    {
        foreach (MovementStateInformation movementStateInfo in m_movementStateInfo)
        {
            if (m_stateHash == movementStateInfo.StateHashValue && AnimatorUtility.GetNormalizedTime(m_animator) < movementStateInfo.StateNormalizedTime)
            {
                if (!RayCast.Target.GetComponentInChildren<BoundCollider>().PlayerEnemyColliding)
                {
                    transform.root.position += transform.root.forward * movementStateInfo.Inertia * (Time.deltaTime * 50f);
                    // Debug.Log("Moving.. ");
                }
                // else { Debug.Log("Obstructed.. "); }
            }
        }
    }

    private void AttackingBehaviour()
    {
        if ((AnimatorUtility.GetBoolParameter(m_animator, "Idle") || AnimatorUtility.GetBoolParameter(m_animator, "Sprint")))
        {
            if (AnimatorUtility.GetBoolParameter(m_animator, "GroundPound") == true)
            {
                AnimatorUtility.SetIntParameter(m_animator, "SkillSetNumber", 1);
                AnimatorUtility.SetIntParameter(m_animator, "ComboNumber", 0);
                m_crossHairLength = 5;
            }
            if (AnimatorUtility.GetBoolParameter(m_animator, "JabStrike") == true)
            {
                AnimatorUtility.SetIntParameter(m_animator, "SkillSetNumber", 3);
                AnimatorUtility.SetIntParameter(m_animator, "ComboNumber", 0);
                m_crossHairLength = 3.5f;
            }
            else if (AnimatorUtility.GetBoolParameter(m_animator, "HeadButt") == true)
            {
                AnimatorUtility.SetIntParameter(m_animator, "SkillSetNumber", 4);
                AnimatorUtility.SetIntParameter(m_animator, "ComboNumber", 0);
                m_crossHairLength = 3;
            }
            else if (AnimatorUtility.GetBoolParameter(m_animator, "Jab") == true)
            {
                AnimatorUtility.SetIntParameter(m_animator, "SkillSetNumber", 4);
                AnimatorUtility.SetIntParameter(m_animator, "ComboNumber", 1);
                m_crossHairLength = 2.5f;
            }
        }
        if (m_stateHash == AnimatorUtility.EnemyAttacks[0])
        {
            AnimatorUtility.SetBoolParameter(m_animator, "GroundPound", false);
            AnimatorUtility.SetIntParameter(m_animator, "SkillSetNumber", 2);
            AnimatorUtility.SetIntParameter(m_animator, "ComboNumber", 0);
        }
        else if (m_stateHash == AnimatorUtility.EnemyAttacks[11])
        {
            AnimatorUtility.SetBoolParameter(m_animator, "JabStrike", true);
        }
        else if (m_stateHash == AnimatorUtility.EnemyAttacks[12])
        {
            AnimatorUtility.SetBoolParameter(m_animator, "JabStrike", false);
            AnimatorUtility.SetBoolParameter(m_animator, "HeadButt", true);
        }
        else if (m_stateHash == AnimatorUtility.EnemyAttacks[13])
        {
            AnimatorUtility.SetBoolParameter(m_animator, "HeadButt", false);
            AnimatorUtility.SetBoolParameter(m_animator, "Jab", true);
        }
        else if (m_stateHash == AnimatorUtility.EnemyAttacks[14])
        {
            AnimatorUtility.SetBoolParameter(m_animator, "Jab", false);
            AnimatorUtility.SetBoolParameter(m_animator, "GroundPound", true);
        }
        else if (m_stateHash == AnimatorUtility.ES.idleState) { m_crossHairLength = 2.5f; }
    }

    private void Aggression()
    {
        if (RayCast.Target != null && RayCast.Target.tag == "Player")
        {
            if (!RayCast.Target.GetCurrentAnimatorStateInfo(0).IsTag("Recovery"))
            {
                AnimatorUtility.SetBoolParameter(m_animator, "PlayerStanding", true); // Debug.Log("Player is standing.. ");
            }
            else if (RayCast.Target.GetCurrentAnimatorStateInfo(0).IsTag("Recovery") && RayCast.Target.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85f)
            {
                AnimatorUtility.SetBoolParameter(m_animator, "PlayerStanding", true);
            }
            else if (RayCast.Target.GetCurrentAnimatorStateInfo(0).IsTag("Recovery") && RayCast.Target.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.85f)
            {
                AnimatorUtility.SetBoolParameter(m_animator, "PlayerStanding", false);
            }

            for (int i = 0; i < AnimatorUtility.PS.IsBeingAttackedStates.Length; i++)
            {
                if (RayCast.Target.GetCurrentAnimatorStateInfo(0).IsTag(AnimatorUtility.PS.IsBeingAttackedStates[i]))
                {
                    if (AnimatorUtility.PS.IsBeingAttackedStates[i] == "Incapacitated" && RayCast.Target.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.125f)
                    {
                        AnimatorUtility.SetBoolParameter(m_animator, "PlayerStanding", true);
                    }
                    else if (AnimatorUtility.PS.IsBeingAttackedStates[i] == "Incapacitated" && RayCast.Target.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.125f)
                    {
                        AnimatorUtility.SetBoolParameter(m_animator, "PlayerStanding", false);
                    }
                    else if (AnimatorUtility.PS.IsBeingAttackedStates[i] != "Incapacitated")
                    {
                        AnimatorUtility.SetBoolParameter(m_animator, "PlayerStanding", false);
                    }
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerHitBound") { AnimatorUtility.SetBoolParameter(m_animator, "AttackZone", true); }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerHitBound") { AnimatorUtility.SetBoolParameter(m_animator, "AttackZone", false); }
    }

    public IEnumerator DazeTimer()
    {
        AnimatorUtility.SetFloatParameter(m_animator, "Dazed", 1.5f);
        while (AnimatorUtility.GetFloatParameter(m_animator, "Dazed") >= 0.1f)
        {
            AnimatorUtility.SetFloatParameter(m_animator, "Dazed", AnimatorUtility.GetFloatParameter(m_animator, "Dazed") - Time.deltaTime);
            yield return null;
        }
        AnimatorUtility.SetFloatParameter(m_animator, "Dazed", 0f);
    }

    private void LookAtPlayer(Collider other)
    {
        if (AnimatorUtility.GetFloatParameter(m_animator, "Dazed") == 0f)
        {
            Vector3 lookPosition = other.transform.root.position - transform.root.position;
            lookPosition.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPosition);
            transform.root.rotation = Quaternion.Slerp(transform.root.rotation, rotation, Time.deltaTime * 10f);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "PlayerHitBound") { LookAtPlayer(other); }
    }
}