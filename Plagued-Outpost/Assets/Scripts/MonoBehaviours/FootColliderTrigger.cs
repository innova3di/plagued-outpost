using UnityEngine;

public class FootColliderTrigger : MonoBehaviour
{
    public AudioClip FlipKickLaunchStep;
    public AudioClip[] SprintStepsDirt;
    public AudioClip[] JogStepsDirt;
    public AudioClip[] WalkStepsDirt;

    private Animator m_animator;
    private AudioSource m_audioSource;
    private AudioSystem m_audioSystem = new AudioSystem();

    private BoxCollider[] m_footBaseColliders;

    public void Start()
    {
        m_animator = transform.root.GetComponent<Animator>();
        m_audioSource = GetComponent<AudioSource>();
        m_footBaseColliders = GetComponents<BoxCollider>();
    }
    public void Update()
    {
        m_audioSystem.AudioSystemUpdate();

        if (transform.root.tag == "Player")
        {
            bool backStepToFlip = AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.backStepState || AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.backStepBreakState || AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.backFlipState;
            if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.walkState)
            {
                m_footBaseColliders[0].enabled = false; // Debug.Log("Whole Foot = " + m_footBaseColliders[0].enabled);
                m_footBaseColliders[1].enabled = true;  // Debug.Log("Heel = " + m_footBaseColliders[1].enabled);
            }
            else if (AnimatorUtility.GetCurrentStateHash(m_animator) == FightingState.SpecialAttacks[0] || backStepToFlip)
            {
                m_footBaseColliders[0].enabled = true;  // Debug.Log("Whole Foot = " + m_footBaseColliders[0].enabled); 
                m_footBaseColliders[1].enabled = false; // Debug.Log("Heel = " + m_footBaseColliders[1].enabled);
            }
            else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.shoeIdleState)
            {
                m_footBaseColliders[0].enabled = true;  // Debug.Log("Whole Foot = " + m_footBaseColliders[0].enabled); 
            }
            else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.jogState && ((PlayerState.Horizontal >= 1 || PlayerState.Horizontal >= -1) && PlayerState.Vertical >= 1 && PlayerState.Horizontal != 0))
            {
                m_footBaseColliders[0].enabled = true;  // Debug.Log("Whole Foot = " + m_footBaseColliders[0].enabled);
                m_footBaseColliders[1].enabled = false; // Debug.Log("Heel = " + m_footBaseColliders[1].enabled);
            }
            else
            {
                m_footBaseColliders[0].enabled = false; // Debug.Log("Whole Foot = " + m_footBaseColliders[0].enabled);
                m_footBaseColliders[1].enabled = true;  // Debug.Log("Heel = " + m_footBaseColliders[1].enabled);
            }
        }
    }

    private const float JOG_STEP_VOLUME = 6f;
    private const float DASH_STEP = 6f;
    private const float SEMISPRINT_STEP_VOLUME = 9f;
    private const float SPRINT_STEP_VOLUME = 6f;
    private const float JUMPED_STEP = 5f;
    private const float JUMPED_ATTACK_STEP = 7f;
    private const float FAST_SPRINT_JUMP_STEP = 7f;
    private const float RECOVERY_STEP_VOLUME = 7f;
    private const float ZOMBIE_STEP_VOLUME = 7f;
    private const float WALK_STEP_VOLUME = 5f;            // 10
    private const float FIGHTING_STEP_VOLUME = 5f;        // 12

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Terrain" || other.tag == "HigherGround")
        {
            if (transform.root.tag == "Enemy")
            {
                if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.ES.sprintState)
                {
                    m_audioSystem.AddAudioToPlay(m_audioSource, JogStepsDirt[Random.Range(0, JogStepsDirt.Length)], 0.75f, ZOMBIE_STEP_VOLUME); /* Debug.Log("Jog"); */
                }
            }
            else if (transform.root.tag == "Player")
            {
                MovementFootStep(); DashFootStep(); JumpFootStep(); ShoeIdleFootStep(); FlipKickLaunchFootStep(); JumpStraightkickFootStep();
                RecoveryFootStep();
            }
        }
        // else if (other.tag != "Terrain" || other.tag != "HigherGround") { Debug.Log(transform.root.tag + " Collider Tag mismatched! = " + other.name); }
    }

    private void RecoveryFootStep()
    {
        if (AnimatorUtility.GetCurrentStateInfo(m_animator).IsTag("Recovery"))
        {
            if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.kipUpState)
            {
                if (AnimatorUtility.GetCurrentStateInfo(m_animator).normalizedTime <= 0.65f)
                {
                    m_audioSystem.AddAudioToPlay(m_audioSource, SprintStepsDirt[Random.Range(0, SprintStepsDirt.Length)], 1f, RECOVERY_STEP_VOLUME);
                }

            }
            else if (AnimatorUtility.GetCurrentStateHash(m_animator) != AnimatorUtility.PS.livershotKnockdown)
            {
                if (AnimatorUtility.GetCurrentStateInfo(m_animator).normalizedTime >= 0.50f)
                {
                    m_audioSystem.AddAudioToPlay(m_audioSource, SprintStepsDirt[Random.Range(0, SprintStepsDirt.Length)], 1f, RECOVERY_STEP_VOLUME);
                }
            }
            else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.livershotKnockdown)
            {
                m_audioSystem.AddAudioToPlay(m_audioSource, WalkStepsDirt[Random.Range(0, WalkStepsDirt.Length)], 1f, FIGHTING_STEP_VOLUME);
            }
        }
        else if (AnimatorUtility.GetCurrentStateInfo(m_animator).IsTag("Incapacitated"))
        {
            m_audioSystem.AddAudioToPlay(m_audioSource, WalkStepsDirt[Random.Range(0, WalkStepsDirt.Length)], 1f, FIGHTING_STEP_VOLUME);
        }
    }

    private void MovementFootStep()
    {
        if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.walkState)
        {
            m_audioSystem.AddAudioToPlay(m_audioSource, WalkStepsDirt[Random.Range(0, WalkStepsDirt.Length)], 1f, WALK_STEP_VOLUME); /* Debug.Log("Walk"); */
        }
        if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.jogState && (PlayerState.Vertical >= 1 || (PlayerState.Vertical >= 0 && (PlayerState.Horizontal == 1 || PlayerState.Horizontal == -1)) ))
        {
            if ( (PlayerState.Vertical >= 1 && PlayerState.Horizontal == 0)                                   || 
                 (PlayerState.Vertical == 0 && (PlayerState.Horizontal >= 1 || PlayerState.Horizontal <= -1)) || 
                 (PlayerState.Vertical >= 1 && (PlayerState.Horizontal >= 1 || PlayerState.Horizontal <= -1))    )
            {
                m_audioSystem.AddAudioToPlay(m_audioSource, JogStepsDirt[Random.Range(0, JogStepsDirt.Length)], 1f, JOG_STEP_VOLUME); /* Debug.Log("Jog"); */
            }
            else if (PlayerState.Horizontal < 1 && PlayerState.Horizontal > -1 || PlayerState.Vertical < 1 && PlayerState.Vertical > -1) 
            {
                m_audioSystem.AddAudioToPlay(m_audioSource, WalkStepsDirt[Random.Range(0, WalkStepsDirt.Length)], 1f, WALK_STEP_VOLUME); /* Debug.Log("Jog Change Direction"); */
            }
        }
        else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.jogState && AnimatorUtility.GetNextStateHash(m_animator) != FightingState.SpecialAttacks[0]) 
        {
            m_audioSystem.AddAudioToPlay(m_audioSource, WalkStepsDirt[Random.Range(0, WalkStepsDirt.Length)], 1f, WALK_STEP_VOLUME); /* Debug.Log("Idle To Jog"); */
        }
        if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.sprintState)
        {
            if (m_animator.GetFloat(AnimatorUtility.PS.sprintSpeedPara) >= 1.3f)
            {
                m_audioSystem.AddAudioToPlay(m_audioSource, SprintStepsDirt[Random.Range(0, SprintStepsDirt.Length)], 1f, SPRINT_STEP_VOLUME); /* Debug.Log("Sprint"); */
            }
            else { m_audioSystem.AddAudioToPlay(m_audioSource, JogStepsDirt[Random.Range(0, JogStepsDirt.Length)], 1f, SEMISPRINT_STEP_VOLUME); } /* Debug.Log("Jog To Sprint"); */
        }
  
        if (AnimatorUtility.GetBoolParameter(m_animator, "Fighting") && AnimatorUtility.GetCurrentStateHash(m_animator) != FightingState.SpecialAttacks[0])
        {
            m_audioSystem.AddAudioToPlay(m_audioSource, WalkStepsDirt[Random.Range(0, WalkStepsDirt.Length)], 1f, FIGHTING_STEP_VOLUME); /* Debug.Log("Fighting"); */
        }
        else if (AnimatorUtility.GetCurrentStateHash(m_animator) == FightingState.fightingToIdle)
        {
            m_audioSystem.AddAudioToPlay(m_audioSource, WalkStepsDirt[Random.Range(0, WalkStepsDirt.Length)], 1f, WALK_STEP_VOLUME); /* Debug.Log("Fighting To Idle"); */
        }
    }

    private void DashFootStep()
    {
        if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.dashLeftState || AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.dashRightState)
        {
            if (AnimatorUtility.GetNormalizedTime(m_animator) > 0.60 && AnimatorUtility.GetNormalizedTime(m_animator) < 0.90f)
            {
                m_audioSystem.AddAudioToPlay(m_audioSource, SprintStepsDirt[Random.Range(0, SprintStepsDirt.Length)], 1f, DASH_STEP); /* Debug.Log("CartWheel Landed"); */
            }
        }
        if ((AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.backStepState && AnimatorUtility.GetNormalizedTime(m_animator) > 0.60f) || AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.backFlipState)
        {
            m_audioSystem.AddAudioToPlay(m_audioSource, SprintStepsDirt[Random.Range(0, SprintStepsDirt.Length)], 1f, DASH_STEP); /* Debug.Log("BackStep"); */
        }
    }

    private void JumpFootStep()
    {
        if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.sprintJumpState && AnimatorUtility.GetNormalizedTime(m_animator) < 0.80f)
        {
            m_audioSystem.AddAudioToPlay(m_audioSource, SprintStepsDirt[Random.Range(0, SprintStepsDirt.Length)], 1f, JUMPED_ATTACK_STEP); /* Debug.Log("Sprint Jump"); */
        }
        else if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.sprintJumpState)
        {
            m_audioSystem.AddAudioToPlay(m_audioSource, WalkStepsDirt[Random.Range(0, WalkStepsDirt.Length)], 1f, WALK_STEP_VOLUME); /* Debug.Log("Sprint Jump Recovery"); */
        }

        if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.fastSprintJumpState)
        {
            m_audioSystem.AddAudioToPlay(m_audioSource, SprintStepsDirt[Random.Range(0, SprintStepsDirt.Length)], 1f, FAST_SPRINT_JUMP_STEP); /* Debug.Log("Jump"); */
        }

        if ( AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.standingJumpState  || AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.forwardJumpState   ||
             AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.leftSideJumpState  || AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.rightSideJumpState ||
             AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.leftSrideJumpState || AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.rightSrideJumpState  )
        {
            m_audioSystem.AddAudioToPlay(m_audioSource, SprintStepsDirt[Random.Range(0, SprintStepsDirt.Length)], 1f, JUMPED_STEP); /* Debug.Log("Jump"); */
        }
    }

    private void FlipKickLaunchFootStep()
    {
        if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.flipKickInitState)
        {
            m_audioSystem.AddAudioToPlay(m_audioSource, JogStepsDirt[Random.Range(0, JogStepsDirt.Length)], 1f, JUMPED_ATTACK_STEP); /* Debug.Log("FlipKick Launched"); */
        }
        else if (AnimatorUtility.GetCurrentStateHash(m_animator) == FightingState.SpecialAttacks[0] && AnimatorUtility.GetNormalizedTime(m_animator) > 0.50f && AnimatorUtility.GetNormalizedTime(m_animator) < 0.80f)
        {
            m_audioSystem.AddAudioToPlay(m_audioSource, SprintStepsDirt[Random.Range(0, SprintStepsDirt.Length)], 1f, JUMPED_ATTACK_STEP); /* Debug.Log("Flipkick Landed"); */
        }
    }

    private void JumpStraightkickFootStep()
    {
        if (AnimatorUtility.GetCurrentStateHash(m_animator) == FightingState.Combo3[7] && AnimatorUtility.GetNormalizedTime(m_animator) < 0.40f)
        {
            m_audioSystem.AddAudioToPlay(m_audioSource, SprintStepsDirt[Random.Range(0, SprintStepsDirt.Length)], 1f, JUMPED_ATTACK_STEP); /* Debug.Log("Jump Straight Kick Launching"); */
        }
        else if (AnimatorUtility.GetCurrentStateHash(m_animator) == FightingState.Combo3[7] && AnimatorUtility.GetNormalizedTime(m_animator) > 0.70f && AnimatorUtility.GetNormalizedTime(m_animator) < 0.85f)
        {
            m_audioSystem.AddAudioToPlay(m_audioSource, SprintStepsDirt[Random.Range(0, SprintStepsDirt.Length)], 1f, JUMPED_ATTACK_STEP); /* Debug.Log("Jump Straight Kick Landed"); */
        }
    }

    private void ShoeIdleFootStep()
    {
        if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.shoeIdleState && AnimatorUtility.GetNormalizedTime(m_animator) > 0.25f && AnimatorUtility.GetNormalizedTime(m_animator) < 0.50f)
        {
            m_audioSystem.AddAudioToPlay(m_audioSource, JogStepsDirt[5], 1f, 3); /* Debug.Log("Shoe Idle"); */
        }
        if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.shoeIdleState && AnimatorUtility.GetNormalizedTime(m_animator) > 0.50f)
        { m_audioSystem.AddAudioToPlay(m_audioSource, WalkStepsDirt[Random.Range(0, WalkStepsDirt.Length)], 1f, 4); } /* Debug.Log("Shoe Done"); */
    }
}
