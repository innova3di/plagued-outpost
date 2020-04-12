using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class BackgroundMusic : MonoBehaviour
{
    public GameObject CameraFocus;
    public AudioMixerSnapshot MapBGM;
    public AudioMixerSnapshot EnemyNearby;
    public AudioMixerSnapshot PlayerLead;
    public AudioMixerSnapshot EnemyLead;
    public float bpm = 128;

    // public AudioMixerSnapshot bim;
    // public AudioClip[] stings;
    // public AudioSource stingSource;

    private Animator m_animator;

    private AudioSource[] m_audioSources;
    private AudioSource m_enemyNearbyAS, m_playerLeadAS, m_enemyLeadAS;
    private TransformManipulator m_transformManipulator;
    private float m_transitionIn;
    private float m_transitionOut;
    // private float m_quarterNote;

    private bool m_isMapBGMPlaying, m_isPlayerOnLead, m_isEnemyOnLead, m_playerKnockedEnemy, m_addedOnce;
    private int m_enemyKnockedPlayer;

    private IEnumerator DelayedStop(AudioSource audioSource, float secondsToFade)
    {
        float timeStart = Time.time;
        while ((Time.time - timeStart) < secondsToFade)
        {
            // audioSource.volume = 1 - (Mathf.Abs(timeStart - Time.time) / secondsToFade);
            // Debug.Log("Time elapsed = " + (Time.time - timeStart));
            yield return null;
        }
        audioSource.Stop();
        // audioSource.volume = 1;
    }

    void Start()
    {
        m_animator = transform.root.GetComponent<Animator>();
        m_audioSources = GetComponents<AudioSource>();
        m_enemyNearbyAS = m_audioSources[2];
        m_playerLeadAS = m_audioSources[3];
        m_enemyLeadAS = m_audioSources[4];
        //m_bgmAS1 = m_audioSources[1];
        //m_bgmAS2 = m_audioSources[2];
        //m_escapeAS = m_audioSources[3];
        m_transformManipulator = transform.root.GetComponentInChildren<TransformManipulator>();
        m_transitionIn = 60 / bpm;
        m_transitionOut = 60 / bpm * 32;
    }

    public void Update()
    {
        if (m_transformManipulator.RayCast.Target != null)
        {
            if ( AnimatorUtility.GetCurrentStateHash(m_transformManipulator.RayCast.Target) == AnimatorUtility.ES.flipKickHitState
                 || AnimatorUtility.GetCurrentStateHash(m_transformManipulator.RayCast.Target) == AnimatorUtility.ES.leftSidePushKickHitBK)
            {
                m_playerKnockedEnemy = true;
            }
            else if (AnimatorUtility.GetCurrentStateInfo(m_transformManipulator.RayCast.Target).IsTag("Recovery"))
            {
                m_playerKnockedEnemy = false;
            }
        }
        if ( AnimatorUtility.GetCurrentStateInfo(m_animator).IsTag("EC1K") ||
             AnimatorUtility.GetCurrentStateInfo(m_animator).IsTag("EC2K") ||
             AnimatorUtility.GetCurrentStateInfo(m_animator).IsTag("EC3K") || 
             AnimatorUtility.GetCurrentStateInfo(m_animator).IsTag("EC4K")    )
        {
            if (!m_addedOnce) { m_enemyKnockedPlayer++; m_addedOnce = true; /* Debug.Log(m_enemyKnockedPlayer); */ }
        }
        else if (AnimatorUtility.GetCurrentStateInfo(m_animator).IsTag("Incapacitated") || AnimatorUtility.GetCurrentStateInfo(m_animator).IsTag("Recovery"))
        {
            if (m_addedOnce) { m_addedOnce = false; /* Debug.Log("Knocked Added = " + m_enemyKnockedPlayer); */ }
        }
        if (m_playerKnockedEnemy)
        {
            m_enemyKnockedPlayer = 0;
        }
        MapBGMPlayer();
        CombatBGMPlayer();
    }

    private void CombatBGMPlayer()
    {
        if (m_playerKnockedEnemy)
        {
            if (!m_isPlayerOnLead && !m_enemyLeadAS.isPlaying)
            {
                m_isPlayerOnLead = true;
                m_isEnemyOnLead = false;
                m_enemyNearbyAS.Stop();
                m_playerLeadAS.Play();
                PlayerLead.TransitionTo(m_transitionIn); // Debug.Log("Player on Lead.. ");
            }
        }
        if (m_enemyKnockedPlayer >= 5)
        {
            if (!m_isEnemyOnLead)
            {
                m_isEnemyOnLead = true;
                m_isPlayerOnLead = false;
                m_enemyNearbyAS.Stop();
                m_enemyLeadAS.Play();
                EnemyLead.TransitionTo(m_transitionIn); // Debug.Log("Enemy on Lead.. ");
            }
        }
    }

    private void MapBGMPlayer()
    {
        Animator enemyAnimator = CameraFocus.GetComponentInChildren<EnemyDetector>().EnemyAnimator;
        if (enemyAnimator != null)
        {
            if (Vector3.Distance(transform.root.position, enemyAnimator.transform.root.position) > 25)
            {
                if (!m_enemyNearbyAS.isPlaying && !m_enemyLeadAS.isPlaying)
                {
                    if (!m_isMapBGMPlaying)
                    {
                        m_isMapBGMPlaying = true;
                        if (m_playerLeadAS.isPlaying)
                        { StartCoroutine(DelayedStop(m_playerLeadAS, 5f)); }
                        MapBGM.TransitionTo(m_transitionOut); // Debug.Log("Map BGM is playing.. ");
                    }
                }
            }
            else if (Vector3.Distance(transform.root.position, enemyAnimator.transform.root.position) < 25)
            {
                if (!m_enemyNearbyAS.isPlaying && !m_playerLeadAS.isPlaying && !m_enemyLeadAS.isPlaying)
                {
                    m_isMapBGMPlaying = false;
                    m_enemyNearbyAS.Play();
                    EnemyNearby.TransitionTo(m_transitionIn); // Debug.Log("Enemy is nearby.. ");
                }
                // else if (m_enemyNearbyAS.isPlaying) { Debug.Log("Tempoison playing.. "); }
                // else if (m_playerLeadAS.isPlaying) { Debug.Log("Player Lead is playing.. "); }
                // else if (m_enemyLeadAS.isPlaying) { Debug.Log("Enemy Lead is playing.. "); }
            }
        }
        // else if (enemyAnimator == null) { Debug.Log("Enemy cannot be found.. "); }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy")) { }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Enemy")) { }
    }

}
