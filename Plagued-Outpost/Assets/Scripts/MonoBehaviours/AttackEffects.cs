using UnityEngine;
using System.Collections.Generic;

public class AttackEffects : MonoBehaviour
{
    public Camera playerCamera;
    public AttackAudio attackAudio;
    public AttackCollection attkCollection;

    private Animator m_animator;
    private BoundCollider m_boundCollider;
    private AudioSource m_audioSource;
    private AudioSystem m_audioSystem = new AudioSystem();
    private bool m_audioLock;

    public bool AttackMissed { get; set; }

    private bool m_hitConfirmed; 
    public bool HitConfirmed { get { return m_hitConfirmed; } set { m_hitConfirmed = value; } }

    private HitSystem m_hitSystem;

    private bool m_isKnockedBack;
    private AttackInfo m_attackInfo;

    private object[] m_playerComboList = new object[] { FightingState.SpecialAttacks, FightingState.BasicPunches, FightingState.BasicKicks, FightingState.Combo1, FightingState.Combo2, FightingState.Combo3, FightingState.Combo4, FightingState.Combo5, FightingState.Combo6, FightingState.Combo7 };
    private object[] m_enemyComboList = new object[] { AnimatorUtility.EnemyAttacks };
    private List<int> m_allPlayerCombos = new List<int>();
    private List<int> m_allEnemyCombos = new List<int>();

    private AnimatorStateInfo m_stateInfo;

    private Dictionary<Transform, HitVisualEffects> m_hitVFXs;

    private bool m_isHitVFXActivated;

    public static ParticleSystem CreateHitVFX(ParticleSystem ps)
    {
        ParticleSystem hp = Instantiate(ps);
        hp.gameObject.SetActive(false);
        return hp;
    }

    private void HitVFXSetup()
    {
        m_hitVFXs = attkCollection.InitHitVFXs(transform.root.gameObject);
        attkCollection.InitAttackVFX(m_hitVFXs, tag); // Debug.Log(tag + " HitVFX Init");
    }

    private void Start()
    {
        m_hitSystem = new HitSystem();
        m_boundCollider = GetComponent<BoundCollider>();
        m_animator = transform.root.GetComponent<Animator>();
        m_audioSource = gameObject.GetComponent<AudioSource>();
        if (gameObject.tag == "Player")
        {
            PlayerState.SetTransformManipulator(GetType(), GetComponent<TransformManipulator>());
            for (int i = 0; i < m_playerComboList.Length; i++) { m_allPlayerCombos.AddRange((IEnumerable<int>)m_playerComboList[i]); }
            attkCollection = new AttackCollection(attackAudio, tag);
            HitVFXSetup();
        }
        else if (gameObject.tag == "Enemy")
        {
            for (int i = 0; i < m_enemyComboList.Length; i++) { m_allEnemyCombos.AddRange((IEnumerable<int>)m_enemyComboList[i]); }
            attkCollection = new AttackCollection(attackAudio, tag);
            HitVFXSetup();
        }
    }

    private RaycastService m_rayCastService;
    private void Update()
    {
        m_stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        OnAttackSFX();
        m_audioSystem.AudioSystemUpdate();
        if (m_isKnockedBack) { m_hitSystem.HitImpact(m_attackInfo, out m_isKnockedBack, m_rayCastService, m_boundCollider); }
    }

    private void ActivateHitVFX(int statePathHash, bool isAddedEffect, AttackVFX attackVFX)
    {
        if (!isAddedEffect)
        {
            AttackVFX attkVFX = attkCollection.GetVFXInfo(statePathHash);
            attkVFX.HitParticle.transform.position = attkVFX.HitTransform.position;
            if (statePathHash == FightingState.Combo1[4])
            {
                attkVFX.HitParticle.transform.rotation = Quaternion.Euler(0, transform.root.eulerAngles.y, -75);
                // Debug.Log("Adjusted Particle Rotation.. ");
            }
            attkVFX.HitParticle.gameObject.SetActive(false);
            attkVFX.HitParticle.gameObject.SetActive(true);
        }
        else
        {
            attackVFX.HitParticle.transform.position = attackVFX.HitTransform.position;
            attackVFX.HitParticle.gameObject.SetActive(false);
            attackVFX.HitParticle.gameObject.SetActive(true);
        }
    }

    public AttackInfo ActiveAttackInfo { get; private set; }
    private void ComboAttackSFX(int statePathHash, AttackInfo attackInfo, RaycastService rayCast)
    {
        if (m_stateInfo.fullPathHash == statePathHash && m_stateInfo.normalizedTime < attackInfo.FrameStart)
        {
            ActiveAttackInfo = attackInfo;
        }
        else if (m_stateInfo.fullPathHash == statePathHash && m_stateInfo.normalizedTime > attackInfo.FrameStart && m_stateInfo.normalizedTime < attackInfo.FrameEnd && !m_audioLock)
        {
            m_audioLock = true; m_isHitVFXActivated = false;
            m_audioSystem.AddAudioToPlay(m_audioSource, attackInfo.AttkSFX[0], 1f, attackInfo.AttkVolumes[0]);
            if (attackInfo.GruntSFX != null)
            {
                int index = Random.Range(0, attackInfo.GruntSFX.Length);
                m_audioSystem.AddAudioToPlay(m_audioSource, attackInfo.GruntSFX[index], 1f, attackInfo.GruntVolumes[index]);
            }
        }
        else if (!AttackMissed && m_stateInfo.fullPathHash == statePathHash && m_stateInfo.normalizedTime > attackInfo.FrameEnd)
        {
            m_audioLock = false;
            if (m_hitConfirmed == false)
            {
                if (rayCast.CrosshairColliding)
                {
                    m_hitSystem.HitConfirm(rayCast.Target, transform, playerCamera, attackInfo, out m_hitConfirmed);
                    if (m_hitConfirmed)
                    {
                        m_rayCastService = rayCast;
                        m_isKnockedBack = true;
                        m_attackInfo = attackInfo;
                        m_audioSystem.AddAudioToPlay(m_audioSource, attackInfo.AttkSFX[1], 1f, attackInfo.AttkVolumes[1]);
                        ActivateHitVFX(statePathHash, false, default(AttackVFX));
                    }
                }
                else
                {
                    AttackMissed = true; // Debug.Log("Attack missed.. ");
                    if (rayCast.Target != null) { AnimatorUtility.SetIntParameter(rayCast.Target, "PreviousHitSet", 0); }
                    if (m_stateInfo.fullPathHash == AnimatorUtility.EnemyAttacks[0]) { ActivateHitVFX(statePathHash, false, default(AttackVFX)); }
                }
            }
            if (m_stateInfo.fullPathHash == FightingState.Combo1[7] && m_stateInfo.normalizedTime >= 0.45f && !m_isHitVFXActivated)
            {
                m_isHitVFXActivated = true;
                ActivateHitVFX(statePathHash, true, new AttackVFX((Transform)AttackCollection.GetEntry(m_hitVFXs, "HitVFX_Right_Foot").Key, ((HitVisualEffects)AttackCollection.GetEntry(m_hitVFXs, "HitVFX_Right_Foot").Value).Lightning[1], new CameraShakeInfo(0.25f, 0.15f)));
            }
        }
        else if (m_stateInfo.fullPathHash == statePathHash) { ActiveAttackInfo = default(AttackInfo); }
    }

    private void LoopCheckSFX(List<int> allCombos, AttackCollection attkSFX, RaycastService rayCast)
    {
        for (int i = 0; i < attkSFX.GetSFXCount(); i++)
        {
            AttackInfo info;
            info = attkSFX.GetSFXInfo(allCombos[i]);
            ComboAttackSFX(allCombos[i], info, rayCast);
        }
    }

    private void OnAttackSFX()
    {
        if (gameObject.tag == "Player")
        {
            if ((m_animator.GetInteger("SkillSetNumber") != 0 && m_animator.GetFloat("ComboTime") < 0.2f) || m_stateInfo.IsTag("ComboFinisher"))
            {
                LoopCheckSFX(m_allPlayerCombos, attkCollection, GetComponent<TransformManipulator>().RayCast);
            }
        }
        else if (gameObject.tag == "Enemy")
        {
            if (AnimatorUtility.GetBoolParameter(m_animator, "IsAttacking"))
            {
                LoopCheckSFX(m_allEnemyCombos, attkCollection, transform.root.GetComponentInChildren<EnemyZone>().RayCast);
            }
        }
    }
}