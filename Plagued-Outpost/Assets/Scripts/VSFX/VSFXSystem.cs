using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct StateSFX
{
    public AudioClip DashWind;
    public AudioClip BackstepRoll;
    public AudioClip SprintJumpWind;
    public AudioClip TackleShoeScrape;
    public AudioClip LivershotShoeScrape;
    public AudioClip SprintBreakShoeScrape;

    public AudioClip[] ShoeScrape;
    public AudioClip[] ClothRoll;
    public AudioClip[] GroundDrop;

    public CombatGrunts CombatGrunts;
}

[System.Serializable]
public struct CombatGrunts
{
    public AudioClip[] EnemyScream;
    public AudioClip[] GroundPound;
    public AudioClip[] RageKick;
    public AudioClip[] JabStrike;
    public AudioClip[] RageJab;
    public AudioClip[] FlipKickPain;
    public AudioClip[] LeftSidePushKickPain;


    public AudioClip[] KneeUppercutPain;
    public AudioClip[] FrontPushGalePain;



    public AudioClip[] EnemyGenericPain;



    public AudioClip[] GroundPoundPain;
    public AudioClip[] EnemyKickPain;
    public AudioClip[] EnemyJabStrikePain;
    public AudioClip[] EnemyJabPain;
    public AudioClip[] EnemyStrikePain;
    public AudioClip[] KipUp;
    public AudioClip[] RollBack;
    public AudioClip[] Retreat;
    public AudioClip[] Dodge;
}

[System.Serializable]
public struct AttackAudio
{
    public AudioClip Jab;
    public AudioClip MediumPunch;
    public AudioClip MediumPunch2;
    public AudioClip LightPunch;
    public AudioClip LightPunch2;
    public AudioClip LightSwing;
    public AudioClip HardSrike;
    public AudioClip CrescentKick;
    public AudioClip SpinningHookKick;
    public AudioClip ElbowStrike;
    public AudioClip ElbowStrike2;
    public AudioClip LeftUppercut;
    public AudioClip CEBJointattack;
    public AudioClip HighElbow;

    public AudioClip HardKick;
    public AudioClip HardKick2;
    public AudioClip LightKick;
    public AudioClip LightKick2;
    public AudioClip RoundHouse;
    public AudioClip KneeUppercut;
    public AudioClip DoubleKick;
    public AudioClip SweepKick;
    public AudioClip ShovelKick;
    public AudioClip LaunchKick;
    public AudioClip FrontPushKick;

    public AudioClip GroundSmash;

    public AudioClip[] Combo2Hits;

    public AudioClip[] Tekken6Hits;
    public AudioClip[] Tekken7Hits;

    public AudioClip[] GenericHits;
    public AudioClip[] BodyPunches;
    public AudioClip[] WetPunches;
    public AudioClip[] DistortedPunches;
    public AudioClip[] DeepPunches;

    public AudioClip[] GenericGrunts;
    public AudioClip[] AttackGrunts;
    public AudioClip[] GMGrunts;
}



public struct AttackInfo
{
    public AudioClip[] AttkSFX { get; private set; }
    public float[] AttkVolumes { get; private set; }

    public AudioClip[] GruntSFX { get; private set; }
    public float[] GruntVolumes { get; private set; }

    public int SkillSet { get; private set; }
    public int ComboNumber { get; private set; }
    public float FrameStart { get; private set; }
    public float FrameEnd { get; private set; }
    public float KnockVelocity { get; private set; }
    public float KnockDistance { get; private set; }

    public AttackInfo(AudioClip[] attkSFX, float[] attkVolumes, AudioClip[] gruntSFX, float[] gruntVolumes, AttackDetails info)
    {
        AttkSFX = attkSFX;
        AttkVolumes = attkVolumes;
        GruntSFX = gruntSFX;
        GruntVolumes = gruntVolumes;

        SkillSet      = info.SkillSet;
        ComboNumber   = info.ComboNumber;
        FrameStart    = info.FrameStart;
        FrameEnd      = info.FrameEnd;
        KnockVelocity = info.KnockVelocity;
        KnockDistance = info.KnockDistance;
    }
}

public struct AttackDetails
{
    public int SkillSet { get; private set; }
    public int ComboNumber { get; private set; }
    public float FrameStart { get; private set; }
    public float FrameEnd { get; private set; }
    public float KnockVelocity { get; private set; }
    public float KnockDistance { get; private set; }

    public AttackDetails(int skillSet, int comboNumber, float frameStart, float frameEnd, float knockVelocity, float knockDistance)
    {
        SkillSet = skillSet;
        ComboNumber = comboNumber;
        FrameStart = frameStart;
        FrameEnd = frameEnd;
        KnockVelocity = knockVelocity;
        KnockDistance = knockDistance;
    }
}

public struct CameraShakeInfo
{
    public float Power { get; private set; }
    public float Duration { get; private set; }

    public CameraShakeInfo(float power, float duration)
    {
        Power = power;
        Duration = duration;
    }
}

public struct AttackVFX
{
    public Transform HitTransform { get; private set; }
    public ParticleSystem HitParticle { get; private set; }
    public CameraShakeInfo CamShakeInfo { get; private set; }

    public AttackVFX(Transform trans, ParticleSystem ps, CameraShakeInfo camShakeInfo)
    {
        HitTransform = trans;
        HitParticle = AttackEffects.CreateHitVFX(ps);
        CamShakeInfo = camShakeInfo;
    }
}

public struct AttkInfo
{
    public int SkillSet { get; private set; }
    public int ComboNumber { get; private set; }
    public float FrameStart { get; private set; }
    public float FrameEnd { get; private set; }
    public AudioClip AudioClip { get; private set; }
    public int Volume { get; private set; }
    public float KnockVelocity { get; private set; }
    public float KnockDistance { get; private set; }

    public AttkInfo(params object[] info)
    {
        SkillSet = (int)info[0];
        ComboNumber = (int)info[1];
        FrameStart = (float)info[2];
        FrameEnd = (float)info[3];
        AudioClip = (AudioClip)info[4];
        Volume = (int)info[5];
        KnockVelocity = (float)info[6];
        KnockDistance = (float)info[7];
    }
}

public class AudioSystem 
{
    private struct AudioInformation
    {
        public AudioSource AudioSource { get; private set; }
        public AudioClip AudioClip { get; private set; }
        public float Pitch { get; private set; }
        public float VolumeScale { get; private set; }

        public AudioInformation(AudioSource audioSource, AudioClip audioClip, float pitch, float volumeScale)
        {
            AudioSource = audioSource;
            AudioClip = audioClip;
            VolumeScale = volumeScale;
            Pitch = pitch;
        }
    }
    private Queue<AudioInformation> m_audioInfo = new Queue<AudioInformation>();

    public void AddAudioToPlay(AudioSource audioSource, AudioClip audioClip, float pitch, float volumeScale)
    {
        m_audioInfo.Enqueue(new AudioInformation(audioSource, audioClip, pitch, volumeScale));
    }

    public void AudioSystemUpdate()
    {
        if (m_audioInfo.Count != 0)
        {
            AudioInformation audioInfo = m_audioInfo.Peek();
            audioInfo.AudioSource.pitch = audioInfo.Pitch;
            audioInfo.AudioSource.PlayOneShot(audioInfo.AudioClip, audioInfo.VolumeScale);
            m_audioInfo.Dequeue();
        }
    }
}