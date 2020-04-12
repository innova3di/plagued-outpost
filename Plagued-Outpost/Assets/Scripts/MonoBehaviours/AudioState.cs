using UnityEngine;
using System.Collections.Generic;

public class AudioState : MonoBehaviour
{
    public Camera CharacterCamera;
    public StateSFX stateSFX;

    private CombatGruntSystem m_combatGruntSystem;
    private AudioSource m_staticBaseAudioSource;

    private Animator m_animator;
    private AudioSource m_sprintWindSource;
    private bool m_isBeingKnockedDown;

    private bool m_isRolling;
    private bool m_isBreaking;
    private bool m_isDashing;
    private bool m_isBackRolling;
    private bool m_isFastSprinting;
    private bool m_isSprintLooping;

    private AudioSystem m_audioSystem = new AudioSystem();

    private bool[] m_isPlaying;

    private int CSH;
    private AnimatorStateInfo CSI;

    public void Start()
    {
        m_animator = GetComponent<Animator>();
        m_combatGruntSystem = new CombatGruntSystem(stateSFX);
        m_isPlaying = new bool[m_combatGruntSystem.CombatGruntInfo.Count];
        m_staticBaseAudioSource = transform.Find("StaticBase").GetComponent<AudioSource>();
        if (tag == "Player") { m_sprintWindSource = GetComponent<AudioSource>(); }
    }

    public void Update()
    {
        CSH = AnimatorUtility.GetCurrentStateHash(m_animator);
        CSI = AnimatorUtility.GetCurrentStateInfo(m_animator);
        if (tag == "Player")
        {
            Breaking();
            RollingCloth();
            SprintingWind();
            SprintingJump();
            DashForwardWind();
            LiverShotKnockDown();
        }
        for (int i = 0; i < m_combatGruntSystem.CombatGruntInfo.Count; i++)
        {
            KeyValuePair<int, CombatGruntInfo> entry = AttackCollection.GetEntry(m_combatGruntSystem.CombatGruntInfo, m_combatGruntSystem.CombatGruntStates[i]);
            CombatSFX(entry.Key, entry.Value, ref m_isPlaying[i]);
        }
        DropOnGround(tag);
        m_audioSystem.AudioSystemUpdate();
    }

    private int previousStateHash;
    private bool isGruntDiscarded;
    private void CombatSFX(int stateHash, CombatGruntInfo cgi, ref bool isPlaying)
    {
        if (CSH == stateHash && CSI.normalizedTime >= cgi.NormalizedTime)
        {
            if (!isPlaying)
            {
                isPlaying = true;
                // if (CSH == m_combatGruntSystem.CombatGruntStates[5] || CSH == m_combatGruntSystem.CombatGruntStates[6])
                // {
                       // Debug.Log("Non-Generic NT = " + CSI.normalizedTime + " | cgi.NT = " + cgi.NormalizedTime);
                // }
                int index = Random.Range(0, cgi.GruntClips.Length);
                m_audioSystem.AddAudioToPlay(m_staticBaseAudioSource, cgi.GruntClips[index], 1f, cgi.Volumes[index]);
            }
        }
        else if (CSH != previousStateHash && CSI.tagHash == stateHash && (CSH != m_combatGruntSystem.CombatGruntStates[6] && CSH != m_combatGruntSystem.CombatGruntStates[7] && CSH != m_combatGruntSystem.CombatGruntStates[8]) && CSI.normalizedTime >= cgi.NormalizedTime)
        {
            if (CSI.tagHash == m_combatGruntSystem.CombatGruntStates[14])
            {
                if (!isPlaying && Random.Range(0f, 1f) <= 0.25f)
                {
                    isPlaying = true;
                    // Debug.Log("Generic NT = " + CSI.normalizedTime + " | cgi.NT = " + cgi.NormalizedTime);
                    int index = Random.Range(0, cgi.GruntClips.Length); // Debug.Log("index = " + index);
                    m_audioSystem.AddAudioToPlay(m_staticBaseAudioSource, cgi.GruntClips[index], 1f, cgi.Volumes[index]);
                }
                else if (!m_staticBaseAudioSource.isPlaying && Random.Range(0f, 1f) <= 0.25f) { isPlaying = false; }
            }
            else if (CSI.tagHash != m_combatGruntSystem.CombatGruntStates[14])
            {
                if (CSI.normalizedTime <= 0.10f) { isGruntDiscarded = false; }
                if (!isGruntDiscarded && !isPlaying && Random.Range(0f, 1f) < 0.175f)
                {
                    isPlaying = true;
                    previousStateHash = CSH;

                    // if (CSI.IsTag("C2")) { Debug.Log("C2"); }

                    // Debug.Log("Generic NT = " + CSI.normalizedTime + " | cgi.NT = " + cgi.NormalizedTime);
                    int index = Random.Range(0, cgi.GruntClips.Length); // Debug.Log("index = " + index);
                    m_audioSystem.AddAudioToPlay(m_staticBaseAudioSource, cgi.GruntClips[index], 1f, cgi.Volumes[index]);
                }
                else if (Random.Range(0f, 1f) > 0.175f && !isGruntDiscarded)
                {
                    isGruntDiscarded = true; // Debug.Log("Discarded = " + isGruntDiscarded);
                }
            }
        }
        else if (isPlaying) { isPlaying = false; }
    }

    private void DropOnGround(string tag)
    {
        if (tag == "Player")
        {
            if (CSH == AnimatorUtility.PS.knockedUnconsciousState)
            {
                if (CSI.normalizedTime >= 0.60f)
                {
                    if (!m_isBeingKnockedDown)
                    {
                        m_isBeingKnockedDown = true;
                        m_audioSystem.AddAudioToPlay(m_staticBaseAudioSource, stateSFX.GroundDrop[0], 1f, 3f);
                    }
                }
            }
            else if (CSH == AnimatorUtility.PS.shoulderHitAndFallState)
            {
                if (CSI.normalizedTime >= 0.425f)
                {
                    if (!m_isBeingKnockedDown)
                    {
                        m_isBeingKnockedDown = true;
                        m_audioSystem.AddAudioToPlay(m_staticBaseAudioSource, stateSFX.GroundDrop[1], 1f, 3f);
                    }
                }
            }
            else if (CSH == AnimatorUtility.PS.rageJabHitState)
            {
                if (CSI.normalizedTime >= 0.425f)
                {
                    if (!m_isBeingKnockedDown)
                    {
                        m_isBeingKnockedDown = true;
                        m_audioSystem.AddAudioToPlay(m_staticBaseAudioSource, stateSFX.GroundDrop[0], 1f, 3f);
                    }
                }
            }
            else if (CSI.IsTag("Incapacitated") || CSI.IsTag("Recovery") || CSI.IsTag("SkillAccessible"))
            {
                if (m_isBeingKnockedDown) { m_isBeingKnockedDown = false; }
            }
        }
        else if (tag == "Enemy")
        {
            if (CSH == AnimatorUtility.ES.leftSidePushKickHitBK)
            {
                if (CSI.normalizedTime >= 0.425f)
                {
                    if (!m_isBeingKnockedDown)
                    {
                        m_isBeingKnockedDown = true;
                        m_audioSystem.AddAudioToPlay(m_staticBaseAudioSource, stateSFX.GroundDrop[1], 1f, 2f);
                    }
                }
            }
            else if (CSH == AnimatorUtility.ES.flipKickHitState)
            {
                if (CSI.normalizedTime >= 0.425f)
                {
                    if (!m_isBeingKnockedDown)
                    {
                        m_isBeingKnockedDown = true;
                        m_audioSystem.AddAudioToPlay(m_staticBaseAudioSource, stateSFX.GroundDrop[0], 1f, 2f);
                    }
                }
            }
            else if (CSI.IsTag("Recovery"))
            {
                if (m_isBeingKnockedDown) { m_isBeingKnockedDown = false; }
            }
        }
    }

    private void LiverShotKnockDown()
    {
        if (CSI.IsTag("EC2K"))
        {
            if (!m_isBeingKnockedDown)
            {
                m_isBeingKnockedDown = true;
                m_audioSystem.AddAudioToPlay(m_staticBaseAudioSource, stateSFX.LivershotShoeScrape, 1f, 7f);
            }
        }
    }

    private void Breaking()
    {
        if (AnimatorUtility.GetBoolParameter(m_animator, "Break"))
        {
            if (m_isBreaking == false)
            {
                if (m_sprintWindSource.isPlaying == true) { m_sprintWindSource.Stop(); }
                if (CharacterInertia.SprintSpeedRecord != 1)
                {
                    m_isBreaking = true;
                    m_audioSystem.AddAudioToPlay(m_staticBaseAudioSource, stateSFX.SprintBreakShoeScrape, 1f, 3); //Debug.Log("Sprint Break");
                }
                else if (CSH == AnimatorUtility.PS.dashBreakState || CSH == AnimatorUtility.PS.dashBreakStateC3 || CSH == AnimatorUtility.PS.dashBreakStateC6 ||
                          CSH == AnimatorUtility.PS.dashBreakStateC7 || CSH == AnimatorUtility.PS.sprintBreakState && (CSH != AnimatorUtility.PS.backStepState))
                {
                    m_isBreaking = true;
                    m_audioSystem.AddAudioToPlay(m_staticBaseAudioSource, stateSFX.ShoeScrape[Random.Range(0, stateSFX.ShoeScrape.Length)], 1f, 3); //Debug.Log("Break");
                }
            }
        }
        else if (CSH == AnimatorUtility.PS.backFlipState) { m_staticBaseAudioSource.Stop(); }
        else if (m_animator.GetBool(AnimatorUtility.PS.sprintTacklePara))
        {
            if (m_isBreaking == false)
            {
                if (m_sprintWindSource.isPlaying == true) { m_sprintWindSource.Stop(); }
                if (!m_staticBaseAudioSource.isPlaying)
                {
                    m_isBreaking = true;
                    m_audioSystem.AddAudioToPlay(m_staticBaseAudioSource, stateSFX.TackleShoeScrape, 1f, 3); //Debug.Log("Wind Stopped");
                }
            }
        }
        else if ( CSH == AnimatorUtility.PS.dashState   || CSH == AnimatorUtility.PS.backStepState || CSH == AnimatorUtility.PS.dashStateC3 ||
                  CSH == AnimatorUtility.PS.dashStateC6 || CSH == AnimatorUtility.PS.dashStateC7   || CSH == AnimatorUtility.PS.sprintState    )
        { m_isBreaking = false; }
    }

    private void DashForwardWind()
    {
        if (CSH == AnimatorUtility.PS.dashState || CSH == AnimatorUtility.PS.dashStateC3 || CSH == AnimatorUtility.PS.dashStateC6 || CSH == AnimatorUtility.PS.dashStateC7)
        {
            if (m_isDashing == false) { m_isDashing = true; m_audioSystem.AddAudioToPlay(m_staticBaseAudioSource, stateSFX.DashWind, 1f, 1); } 
        }
        else { m_isDashing = false; }
    }

    private void SprintingWind()
    {
        if (CSH == AnimatorUtility.PS.sprintState && m_animator.GetFloat(AnimatorUtility.PS.sprintSpeedPara) >= 1.325f)
        {
            if (m_sprintWindSource.isPlaying == false)
            {
                if (!m_isSprintLooping) { m_sprintWindSource.volume = 0f; }
                m_isSprintLooping = true;
                if (m_isBreaking == false) { m_audioSystem.AddAudioToPlay(m_sprintWindSource, m_sprintWindSource.clip, 0.5f, 4); }
            }
            else if (!m_isSprintLooping) { m_isSprintLooping = true; m_sprintWindSource.volume = 0f; }

            if (m_animator.GetFloat(AnimatorUtility.PS.sprintSpeedPara) <= 1.425f && m_sprintWindSource.volume <= 1)
            { m_sprintWindSource.volume += Time.deltaTime * 1.15f; }

            if (m_animator.GetFloat(AnimatorUtility.PS.sprintSpeedPara) >= 1.425f)
            {
                if (m_sprintWindSource.volume >= 0.25) { m_sprintWindSource.volume -= Time.deltaTime * 0.5f; }
            }
            if (CharacterCamera.fieldOfView < 70) { CharacterCamera.fieldOfView += Time.deltaTime * 5f; }
        }
        else if (CSH == AnimatorUtility.PS.sprintState && m_animator.GetFloat(AnimatorUtility.PS.sprintSpeedPara) == 1)
        {
            m_isSprintLooping = false;
        }
        else if (CharacterCamera.fieldOfView > 60 && !CharacterCamera.GetComponent<CameraVFX>().DashLayoffFOV)
        {
            m_isSprintLooping = false; CharacterCamera.fieldOfView -= Time.deltaTime * 10f;
            if (CharacterCamera.fieldOfView < 60) { CharacterCamera.fieldOfView = 60; }
        }
    }

    private void SprintingJump()
    {
        if (m_animator.GetBool(AnimatorUtility.PS.fastSprintJumpPara))
        {
            if (!m_isFastSprinting)
            {
                m_isSprintLooping = false;
                m_isFastSprinting = true;
                float VolumeScale = 0;
                if (m_sprintWindSource.isPlaying)
                {
                    if (CharacterInertia.SprintSpeedRecord <= 1.4f) { VolumeScale = 1; } else { VolumeScale = 2; }
                    m_audioSystem.AddAudioToPlay(m_staticBaseAudioSource, stateSFX.SprintJumpWind, 1f, VolumeScale);
                }
            }
            if (m_sprintWindSource.volume >= 0.25) { m_sprintWindSource.volume -= Time.deltaTime * 1.15f; }
        }
        else { m_isFastSprinting = false; }

        if (m_animator.GetBool(AnimatorUtility.PS.sprintJumpPara))
        {
            if (m_sprintWindSource.isPlaying == true) { m_sprintWindSource.Stop(); }
        }
    }

    private void RollingCloth()
    {
        if (CSH == AnimatorUtility.PS.rollState && AnimatorUtility.GetNormalizedTime(m_animator) >= 0.35f && m_isRolling == false)
        {
            if (m_staticBaseAudioSource.isPlaying == false)
            {
                m_isRolling = true;
                m_audioSystem.AddAudioToPlay(m_staticBaseAudioSource, stateSFX.ClothRoll[Random.Range(0, stateSFX.ClothRoll.Length)], 1f, 5);
            }
        }
        else if (CSH != AnimatorUtility.PS.rollState) { m_isRolling = false; }

        if (CSH == AnimatorUtility.PS.backStepState && m_isBackRolling == false)
        {
            if (m_staticBaseAudioSource.isPlaying == false)
            {
                m_isBackRolling = true;
                m_audioSystem.AddAudioToPlay(m_staticBaseAudioSource, stateSFX.BackstepRoll, 1f, 5);
            }
        }
        else if (CSH != AnimatorUtility.PS.backStepState) { m_isBackRolling = false; }
    }
}
