using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class AttackCollection
{
    private readonly Dictionary<int, AttackInfo> m_attackSFX = new Dictionary<int, AttackInfo>();
    private readonly Dictionary<int, AttackVFX> m_attackVFX = new Dictionary<int, AttackVFX>();
    private readonly Dictionary<int, AttkInfo> m_attkSFX = new Dictionary<int, AttkInfo>();

    public Dictionary<Transform, HitVisualEffects> InitHitVFXs(GameObject root)
    {
        Dictionary<Transform, HitVisualEffects> VFX = new Dictionary<Transform, HitVisualEffects>();
        List<Transform> transforms = SkillTreeInfo.FindComponents<Transform>(root, "VFXEmitter");
        for (int i = 0; i < transforms.Count; i++)
        {
            VFX.Add(transforms[i], transforms[i].GetComponent<HitVisualEffects>());
        }
        return VFX;
    }

    public static KeyValuePair<Transform, HitVisualEffects> GetEntry(Dictionary<Transform, HitVisualEffects> dictionary, string key)
    {
        return new KeyValuePair<Transform, HitVisualEffects>(dictionary.First(x => x.Key.name == key).Key, dictionary.First(x => x.Key.name == key).Value);
    }

    public static KeyValuePair<int, CombatGruntInfo> GetEntry(Dictionary<int, CombatGruntInfo> dictionary, int key)
    {
        return new KeyValuePair<int, CombatGruntInfo>(dictionary.First(x => x.Key == key).Key, dictionary.First(x => x.Key == key).Value);
    }

    public void InitAttackVFX(Dictionary<Transform, HitVisualEffects> VFX, string tag)
    {
        if (tag == "Enemy")
        {
            m_attackVFX.Add(AnimatorUtility.EnemyAttacks[0],  new AttackVFX(GetEntry(VFX, "HitVFX_Ground_Smash(E)").Key, GetEntry(VFX, "HitVFX_Ground_Smash(E)").Value.Earth[2],   new CameraShakeInfo(0.250f,  0.175f)  ));

            m_attackVFX.Add(AnimatorUtility.EnemyAttacks[1],  new AttackVFX(GetEntry(VFX, "HitVFX_Right_Hand(E)").Key,   GetEntry(VFX, "HitVFX_Right_Hand(E)").Value.Fire[1],      new CameraShakeInfo(0.075f,  0.100f)  ));
            m_attackVFX.Add(AnimatorUtility.EnemyAttacks[2],  new AttackVFX(GetEntry(VFX, "HitVFX_Left_Hand(E)").Key,    GetEntry(VFX, "HitVFX_Left_Hand(E)").Value.Fire[1],       new CameraShakeInfo(0.075f,  0.100f)  ));
            m_attackVFX.Add(AnimatorUtility.EnemyAttacks[3],  new AttackVFX(GetEntry(VFX, "HitVFX_Right_Hand(E)").Key,   GetEntry(VFX, "HitVFX_Right_Hand(E)").Value.Fire[1],      new CameraShakeInfo(0.075f,  0.100f)  ));
            m_attackVFX.Add(AnimatorUtility.EnemyAttacks[4],  new AttackVFX(GetEntry(VFX, "HitVFX_Left_Hand(E)").Key,    GetEntry(VFX, "HitVFX_Left_Hand(E)").Value.Fire[1],       new CameraShakeInfo(0.075f,  0.100f)  ));
            m_attackVFX.Add(AnimatorUtility.EnemyAttacks[5],  new AttackVFX(GetEntry(VFX, "HitVFX_Right_Hand(E)").Key,   GetEntry(VFX, "HitVFX_Right_Hand(E)").Value.Fire[1],      new CameraShakeInfo(0.075f,  0.100f)  ));
            m_attackVFX.Add(AnimatorUtility.EnemyAttacks[6],  new AttackVFX(GetEntry(VFX, "HitVFX_Left_Hand(E)").Key,    GetEntry(VFX, "HitVFX_Left_Hand(E)").Value.Fire[1],       new CameraShakeInfo(0.075f,  0.100f)  ));
            m_attackVFX.Add(AnimatorUtility.EnemyAttacks[7],  new AttackVFX(GetEntry(VFX, "HitVFX_Right_Hand(E)").Key,   GetEntry(VFX, "HitVFX_Right_Hand(E)").Value.Fire[1],      new CameraShakeInfo(0.075f,  0.100f)  ));
            m_attackVFX.Add(AnimatorUtility.EnemyAttacks[8],  new AttackVFX(GetEntry(VFX, "HitVFX_Left_Hand(E)").Key,    GetEntry(VFX, "HitVFX_Left_Hand(E)").Value.Fire[1],       new CameraShakeInfo(0.075f,  0.100f)  ));
            m_attackVFX.Add(AnimatorUtility.EnemyAttacks[9],  new AttackVFX(GetEntry(VFX, "HitVFX_Right_Hand(E)").Key,   GetEntry(VFX, "HitVFX_Right_Hand(E)").Value.Fire[1],      new CameraShakeInfo(0.075f,  0.100f)  ));
            m_attackVFX.Add(AnimatorUtility.EnemyAttacks[10], new AttackVFX(GetEntry(VFX, "HitVFX_Left_Hand(E)").Key,    GetEntry(VFX, "HitVFX_Left_Hand(E)").Value.Fire[1],       new CameraShakeInfo(0.075f,  0.100f)  ));

            m_attackVFX.Add(AnimatorUtility.EnemyAttacks[11], new AttackVFX(GetEntry(VFX, "HitVFX_Right_Foot(E)").Key,   GetEntry(VFX, "HitVFX_Right_Foot(E)").Value.Lightning[1], new CameraShakeInfo(0.125f,  0.150f)  ));
            m_attackVFX.Add(AnimatorUtility.EnemyAttacks[12], new AttackVFX(GetEntry(VFX, "HitVFX_Right_Hand(E)").Key,   GetEntry(VFX, "HitVFX_Right_Hand(E)").Value.Light[1],     new CameraShakeInfo(0.100f,  0.100f)  ));

            m_attackVFX.Add(AnimatorUtility.EnemyAttacks[13], new AttackVFX(GetEntry(VFX, "HitVFX_Head(E)").Key,         GetEntry(VFX, "HitVFX_Head(E)").Value.Light[1],           new CameraShakeInfo(0.125f,  0.100f)  ));
            m_attackVFX.Add(AnimatorUtility.EnemyAttacks[14], new AttackVFX(GetEntry(VFX, "HitVFX_Right_Hand(E)").Key,   GetEntry(VFX, "HitVFX_Right_Hand(E)").Value.Lightning[1], new CameraShakeInfo(0.150f,  0.125f)  ));
        }
        else if (tag == "Player")
        {
            m_attackVFX.Add(FightingState.BasicPunches[0], new AttackVFX(GetEntry(VFX, "HitVFX_Right_Hand").Key,   GetEntry(VFX, "HitVFX_Right_Hand").Value.Lightning[1], new CameraShakeInfo(0.100f,  0.100f)  ));
            m_attackVFX.Add(FightingState.BasicPunches[1], new AttackVFX(GetEntry(VFX, "HitVFX_Left_Hand_1").Key,  GetEntry(VFX, "HitVFX_Left_Hand_1").Value.Neutral[0],  new CameraShakeInfo(0.075f,  0.100f)  ));
            m_attackVFX.Add(FightingState.BasicPunches[2], new AttackVFX(GetEntry(VFX, "HitVFX_Right_Hand").Key,   GetEntry(VFX, "HitVFX_Right_Hand").Value.Neutral[1],   new CameraShakeInfo(0.075f,  0.075f)  ));
            m_attackVFX.Add(FightingState.BasicPunches[3], new AttackVFX(GetEntry(VFX, "HitVFX_Right_Hand").Key,   GetEntry(VFX, "HitVFX_Right_Hand").Value.Light[1],     new CameraShakeInfo(0.100f,  0.100f)  ));
                                                                                                                                                                                                                
            m_attackVFX.Add(FightingState.BasicKicks[0], new AttackVFX(GetEntry(VFX, "HitVFX_Right_Foot").Key,     GetEntry(VFX, "HitVFX_Right_Foot").Value.Fire[1],      new CameraShakeInfo(0.075f,  0.075f)  ));
            m_attackVFX.Add(FightingState.BasicKicks[1], new AttackVFX(GetEntry(VFX, "HitVFX_Left_Foot").Key,      GetEntry(VFX, "HitVFX_Left_Foot").Value.Fire[1],       new CameraShakeInfo(0.075f,  0.075f)  ));
            m_attackVFX.Add(FightingState.BasicKicks[2], new AttackVFX(GetEntry(VFX, "HitVFX_Right_Foot").Key,     GetEntry(VFX, "HitVFX_Right_Foot").Value.Light[1],     new CameraShakeInfo(0.100f,  0.100f)  ));
            m_attackVFX.Add(FightingState.BasicKicks[3], new AttackVFX(GetEntry(VFX, "HitVFX_Left_Foot").Key,      GetEntry(VFX, "HitVFX_Left_Foot").Value.Lightning[1],  new CameraShakeInfo(0.125f,  0.125f)  ));

            m_attackVFX.Add(FightingState.Combo1[0], new AttackVFX(GetEntry(VFX, "HitVFX_Right_Hand").Key,         GetEntry(VFX, "HitVFX_Right_Hand").Value.Lightning[1], new CameraShakeInfo(0.100f,  0.100f)  ));
            m_attackVFX.Add(FightingState.Combo1[1], new AttackVFX(GetEntry(VFX, "HitVFX_Left_Hand").Key,          GetEntry(VFX, "HitVFX_Left_Hand").Value.Light[3],      new CameraShakeInfo(0.075f,  0.075f)  ));
            m_attackVFX.Add(FightingState.Combo1[2], new AttackVFX(GetEntry(VFX, "HitVFX_Right_Foot").Key,         GetEntry(VFX, "HitVFX_Right_Foot").Value.Fire[1],      new CameraShakeInfo(0.075f,  0.100f)  ));
            m_attackVFX.Add(FightingState.Combo1[3], new AttackVFX(GetEntry(VFX, "HitVFX_Left_Foot").Key,          GetEntry(VFX, "HitVFX_Left_Foot").Value.Fire[1],       new CameraShakeInfo(0.075f,  0.100f)  ));
            m_attackVFX.Add(FightingState.Combo1[4], new AttackVFX(GetEntry(VFX, "HitVFX_Right_Foot").Key,         GetEntry(VFX, "HitVFX_Right_Foot").Value.Lightning[2], new CameraShakeInfo(0.125f,  0.125f)  ));
            m_attackVFX.Add(FightingState.Combo1[5], new AttackVFX(GetEntry(VFX, "HitVFX_Left_Hand_1").Key,        GetEntry(VFX, "HitVFX_Left_Hand_1").Value.Neutral[0],  new CameraShakeInfo(0.075f,  0.100f)  ));
            m_attackVFX.Add(FightingState.Combo1[6], new AttackVFX(GetEntry(VFX, "HitVFX_Right_Foot").Key,         GetEntry(VFX, "HitVFX_Right_Foot").Value.Fire[1],      new CameraShakeInfo(0.100f,  0.100f)  ));
            m_attackVFX.Add(FightingState.Combo1[7], new AttackVFX(GetEntry(VFX, "HitVFX_Left_Foot").Key,          GetEntry(VFX, "HitVFX_Left_Foot").Value.Fire[1],       new CameraShakeInfo(0.125f,  0.175f)  ));

   
            m_attackVFX.Add(FightingState.Combo2[0], new AttackVFX(GetEntry(VFX, "HitVFX_Left_Hand_1").Key,        GetEntry(VFX, "HitVFX_Left_Hand_1").Value.Light[1],    new CameraShakeInfo(0.075f,  0.100f)  ));
            m_attackVFX.Add(FightingState.Combo2[1], new AttackVFX(GetEntry(VFX, "HitVFX_Right_Hand").Key,         GetEntry(VFX, "HitVFX_Right_Hand").Value.Lightning[1], new CameraShakeInfo(0.100f,  0.100f)  ));
            m_attackVFX.Add(FightingState.Combo2[2], new AttackVFX(GetEntry(VFX, "HitVFX_Left_Hand").Key,          GetEntry(VFX, "HitVFX_Left_Hand").Value.Light[3],      new CameraShakeInfo(0.075f,  0.075f)  ));
            m_attackVFX.Add(FightingState.Combo2[3], new AttackVFX(GetEntry(VFX, "HitVFX_Right_Elbow").Key,        GetEntry(VFX, "HitVFX_Right_Elbow").Value.Dark[1],     new CameraShakeInfo(0.075f,  0.075f)  ));
            m_attackVFX.Add(FightingState.Combo2[4], new AttackVFX(GetEntry(VFX, "HitVFX_Right_Hand").Key,         GetEntry(VFX, "HitVFX_Right_Hand").Value.Light[1],     new CameraShakeInfo(0.100f,  0.100f)  ));
            m_attackVFX.Add(FightingState.Combo2[5], new AttackVFX(GetEntry(VFX, "HitVFX_Left_Hand_1").Key,        GetEntry(VFX, "HitVFX_Left_Hand_1").Value.Neutral[0],  new CameraShakeInfo(0.075f,  0.100f)  ));
            m_attackVFX.Add(FightingState.Combo2[6], new AttackVFX(GetEntry(VFX, "HitVFX_Right_Knee").Key,         GetEntry(VFX, "HitVFX_Right_Knee").Value.Lightning[1], new CameraShakeInfo(0.175f,  0.200f)  ));
            m_attackVFX.Add(FightingState.Combo2[7], new AttackVFX(GetEntry(VFX, "HitVFX_Left_Foot").Key,          GetEntry(VFX, "HitVFX_Left_Foot").Value.Fire[1],       new CameraShakeInfo(0.075f,  0.100f)  ));
            m_attackVFX.Add(FightingState.Combo2[8], new AttackVFX(GetEntry(VFX, "HitVFX_Right_Foot").Key,         GetEntry(VFX, "HitVFX_Right_Foot").Value.Lightning[1], new CameraShakeInfo(0.175f,  0.200f)  ));

            m_attackVFX.Add(FightingState.SpecialAttacks[0], new AttackVFX(GetEntry(VFX, "HitVFX_Right_Foot").Key, GetEntry(VFX, "HitVFX_Right_Foot").Value.Light[1],     new CameraShakeInfo(0.125f,  0.150f)  ));
        }
    }

    public AttackCollection(AttackAudio clip, string tag)
    {
        if (tag == "Enemy")
        {
            m_attackSFX.Add(AnimatorUtility.EnemyAttacks[0],  new AttackInfo(new AudioClip[] { clip.GroundSmash, null },                   new float[] { 5f,  0.00f }, null, null, new AttackDetails(1, 0, 0.40f, 0.525f, -0.150f, -8.00f) ));

            m_attackSFX.Add(AnimatorUtility.EnemyAttacks[1],  new AttackInfo(new AudioClip[] { clip.HardSrike, clip.Tekken7Hits[6] },      new float[] { 8f,  2.50f }, null, null, new AttackDetails(2, 0, 0.50f, 0.675f, -0.125f, -0.25f) ));
            m_attackSFX.Add(AnimatorUtility.EnemyAttacks[2],  new AttackInfo(new AudioClip[] { clip.HardSrike, clip.Tekken7Hits[6] },      new float[] { 8f,  2.50f }, null, null, new AttackDetails(2, 1, 0.35f, 0.535f, -0.125f, -0.25f) ));
            m_attackSFX.Add(AnimatorUtility.EnemyAttacks[3],  new AttackInfo(new AudioClip[] { clip.HardSrike, clip.Tekken7Hits[6] },      new float[] { 8f,  2.50f }, null, null, new AttackDetails(2, 0, 0.35f, 0.535f, -0.125f, -0.25f) ));
            m_attackSFX.Add(AnimatorUtility.EnemyAttacks[4],  new AttackInfo(new AudioClip[] { clip.HardSrike, clip.Tekken7Hits[6] },      new float[] { 8f,  2.50f }, null, null, new AttackDetails(2, 1, 0.35f, 0.535f, -0.125f, -0.25f) ));
            m_attackSFX.Add(AnimatorUtility.EnemyAttacks[5],  new AttackInfo(new AudioClip[] { clip.HardSrike, clip.Tekken7Hits[6] },      new float[] { 8f,  2.50f }, null, null, new AttackDetails(2, 0, 0.35f, 0.535f, -0.125f, -0.25f) ));
            m_attackSFX.Add(AnimatorUtility.EnemyAttacks[6],  new AttackInfo(new AudioClip[] { clip.HardSrike, clip.Tekken7Hits[6] },      new float[] { 8f,  2.50f }, null, null, new AttackDetails(2, 1, 0.35f, 0.535f, -0.125f, -0.25f) ));
            m_attackSFX.Add(AnimatorUtility.EnemyAttacks[7],  new AttackInfo(new AudioClip[] { clip.HardSrike, clip.Tekken7Hits[6] },      new float[] { 8f,  2.50f }, null, null, new AttackDetails(2, 0, 0.35f, 0.535f, -0.125f, -0.25f) ));
            m_attackSFX.Add(AnimatorUtility.EnemyAttacks[8],  new AttackInfo(new AudioClip[] { clip.HardSrike, clip.Tekken7Hits[6] },      new float[] { 8f,  2.50f }, null, null, new AttackDetails(2, 1, 0.35f, 0.535f, -0.125f, -0.25f) ));
            m_attackSFX.Add(AnimatorUtility.EnemyAttacks[9],  new AttackInfo(new AudioClip[] { clip.HardSrike, clip.Tekken7Hits[6] },      new float[] { 8f,  2.50f }, null, null, new AttackDetails(2, 0, 0.35f, 0.535f, -0.125f, -0.25f) ));
            m_attackSFX.Add(AnimatorUtility.EnemyAttacks[10], new AttackInfo(new AudioClip[] { clip.HardSrike, clip.Tekken7Hits[6] },      new float[] { 8f,  2.50f }, null, null, new AttackDetails(2, 1, 0.35f, 0.535f, -0.125f, -0.25f) ));

            m_attackSFX.Add(AnimatorUtility.EnemyAttacks[11], new AttackInfo(new AudioClip[] { clip.FrontPushKick, clip.GenericHits[11] }, new float[] { 8f,  6.00f }, null, null, new AttackDetails(2, 2, 0.15f, 0.265f, -0.150f, -6.75f) ));
            m_attackSFX.Add(AnimatorUtility.EnemyAttacks[12], new AttackInfo(new AudioClip[] { clip.ShovelKick,    clip.BodyPunches[3] },  new float[] { 10f, 6.00f }, null, null, new AttackDetails(3, 0, 0.35f, 0.450f, -0.115f, -7.25f) ));

            m_attackSFX.Add(AnimatorUtility.EnemyAttacks[13], new AttackInfo(new AudioClip[] { clip.KneeUppercut,  clip.DeepPunches[7] },  new float[] { 8f,  4.00f }, null, null, new AttackDetails(4, 0, 0.50f, 0.60f, -0.125f, -0.60f)  ));
            m_attackSFX.Add(AnimatorUtility.EnemyAttacks[14], new AttackInfo(new AudioClip[] { clip.Jab,           clip.BodyPunches[6] },  new float[] { 8f,  5.00f }, null, null, new AttackDetails(4, 1, 0.25f, 0.35f, -0.150f, -10.0f)  ));
        }
        else if (tag == "Player")
        {
            // Disable all sound effects except hits and hitmisses. You will notice inconsistencies between the same hit sound effects. Uppercut hit sound effect in basic punches produces incorrect sound.

            m_attackSFX.Add(FightingState.BasicPunches[0], new AttackInfo(new AudioClip[] { clip.Jab, clip.BodyPunches[6] },         new float[] { 8f, 3.25f }, new AudioClip[] { clip.AttackGrunts[9], clip.GenericGrunts[0] },  new float[] { 3.5f, 3.5f }, new AttackDetails(10, 0, 0.50f, 0.67f, -0.125f, -0.750f)  ));
            m_attackSFX.Add(FightingState.BasicPunches[1], new AttackInfo(new AudioClip[] { clip.MediumPunch, clip.BodyPunches[1] }, new float[] { 8f, 2.75f }, null, null,                                                                                   new AttackDetails(10, 1, 0.50f, 0.65f, -0.125f, -0.500f)  ));
            m_attackSFX.Add(FightingState.BasicPunches[2], new AttackInfo(new AudioClip[] { clip.LightPunch, clip.BodyPunches[2] },  new float[] { 8f, 2.25f }, null, null,                                                                                   new AttackDetails(10, 2, 0.35f, 0.55f, -0.000f, -0.000f)  ));
            m_attackSFX.Add(FightingState.BasicPunches[3], new AttackInfo(new AudioClip[] { clip.LightSwing, clip.GenericHits[11] }, new float[] { 8f, 3.25f }, new AudioClip[] { clip.AttackGrunts[3], clip.GenericGrunts[2] },  new float[] { 2.5f, 3.5f }, new AttackDetails(10, 3, 0.35f, 0.50f, -0.100f, -1.500f)  ));

            m_attackSFX.Add(FightingState.BasicKicks[0], new AttackInfo(new AudioClip[] { clip.LightKick, clip.GenericHits[11] },    new float[] { 8f, 2.25f }, null, null,                                                                                   new AttackDetails(11, 0, 0.25f, 0.47f, -0.125f, -0.250f)  ));
            m_attackSFX.Add(FightingState.BasicKicks[1], new AttackInfo(new AudioClip[] { clip.LightKick2, clip.WetPunches[4] },     new float[] { 8f, 2.25f }, new AudioClip[] { clip.AttackGrunts[9], clip.GenericGrunts[0] },  new float[] { 3.5f, 3.5f }, new AttackDetails(11, 1, 0.30f, 0.43f, -0.125f, -0.250f)  ));
            m_attackSFX.Add(FightingState.BasicKicks[2], new AttackInfo(new AudioClip[] { clip.RoundHouse, clip.DeepPunches[0] },    new float[] { 8f, 3.25f }, new AudioClip[] { clip.AttackGrunts[3], clip.GenericGrunts[2] },  new float[] { 2.5f, 3.5f }, new AttackDetails(11, 2, 0.33f, 0.43f, -0.125f, -0.125f)  ));
            m_attackSFX.Add(FightingState.BasicKicks[3], new AttackInfo(new AudioClip[] { clip.HardSrike, clip.GenericHits[3] },     new float[] { 8f, 3.00f }, new AudioClip[] { clip.AttackGrunts[0], clip.AttackGrunts[1]  },  new float[] { 2.5f, 2.5f }, new AttackDetails(11, 3, 0.20f, 0.40f, -0.175f, -13.25f)  ));

            m_attackSFX.Add(FightingState.Combo1[0], new AttackInfo(new AudioClip[] { clip.Jab, clip.BodyPunches[6] },               new float[] { 8f, 3.25f }, new AudioClip[] { clip.AttackGrunts[9], clip.GenericGrunts[0] },  new float[] { 3.5f, 3.5f }, new AttackDetails(1, 0, 0.50f, 0.67f, -0.100f, -0.150f)   ));
            m_attackSFX.Add(FightingState.Combo1[1], new AttackInfo(new AudioClip[] { clip.LightPunch2, clip.BodyPunches[2] },       new float[] { 8f, 2.50f }, null, null,                                                                                   new AttackDetails(1, 1, 0.25f, 0.47f, -0.000f, -0.000f)   ));
            m_attackSFX.Add(FightingState.Combo1[2], new AttackInfo(new AudioClip[] { clip.LightKick, clip.GenericHits[11] },        new float[] { 8f, 2.25f }, new AudioClip[] { clip.GMGrunts[0], clip.GMGrunts[1] },           new float[] { 2.5f, 2.5f }, new AttackDetails(1, 2, 0.30f, 0.55f, -0.125f, -0.500f)   ));
            m_attackSFX.Add(FightingState.Combo1[3], new AttackInfo(new AudioClip[] { clip.LightKick2, clip.WetPunches[4] },         new float[] { 8f, 2.25f }, new AudioClip[] { clip.AttackGrunts[9], clip.GenericGrunts[0] },  new float[] { 3.5f, 3.5f }, new AttackDetails(1, 3, 0.30f, 0.43f, -0.125f, -0.500f)   ));
            m_attackSFX.Add(FightingState.Combo1[4], new AttackInfo(new AudioClip[] { clip.SpinningHookKick, clip.GenericHits[13] }, new float[] { 8f, 3.00f }, new AudioClip[] { clip.AttackGrunts[7], clip.AttackGrunts[1]  },  new float[] { 3.5f, 2.5f }, new AttackDetails(1, 4, 0.35f, 0.46f, -0.125f, -0.500f)   ));
            m_attackSFX.Add(FightingState.Combo1[5], new AttackInfo(new AudioClip[] { clip.MediumPunch, clip.BodyPunches[1] },       new float[] { 8f, 2.75f }, null, null,                                                                                   new AttackDetails(1, 5, 0.50f, 0.65f, -0.125f, -0.500f)   ));
            m_attackSFX.Add(FightingState.Combo1[6], new AttackInfo(new AudioClip[] { clip.HardKick2, clip.GenericHits[11] },        new float[] { 8f, 3.25f }, new AudioClip[] { clip.GMGrunts[0], clip.GMGrunts[1] },           new float[] { 2.5f, 2.5f }, new AttackDetails(1, 6, 0.25f, 0.50f, -0.000f, -0.000f)   ));
            m_attackSFX.Add(FightingState.Combo1[7], new AttackInfo(new AudioClip[] { clip.DoubleKick, clip.BodyPunches[8] },        new float[] { 8f, 3.00f }, new AudioClip[] { clip.AttackGrunts[8], clip.AttackGrunts[2]  },  new float[] { 2.5f, 2.5f }, new AttackDetails(1, 7, 0.20f, 0.30f, -0.100f, -1.500f)   ));

            m_attackSFX.Add(FightingState.Combo2[0], new AttackInfo(new AudioClip[] { clip.MediumPunch, clip.GenericHits[11] },      new float[] { 8f, 3.00f }, null, null,                                                                                   new AttackDetails(2, 0, 0.25f, 0.40f, -0.000f, -0.000f)   ));
            m_attackSFX.Add(FightingState.Combo2[1], new AttackInfo(new AudioClip[] { clip.Jab, clip.BodyPunches[6] },               new float[] { 8f, 3.25f }, new AudioClip[] { clip.GenericGrunts[0]                        }, new float[] { 5.0f       }, new AttackDetails(2, 1, 0.30f, 0.55f, -0.100f, -0.100f)   ));
            m_attackSFX.Add(FightingState.Combo2[2], new AttackInfo(new AudioClip[] { clip.LightPunch2, clip.BodyPunches[2] },       new float[] { 8f, 2.50f }, null, null,                                                                                   new AttackDetails(2, 2, 0.20f, 0.40f, -0.000f, -0.000f)   ));
            m_attackSFX.Add(FightingState.Combo2[3], new AttackInfo(new AudioClip[] { clip.ElbowStrike, clip.Combo2Hits[0] },        new float[] { 9f, 3.75f }, new AudioClip[] { clip.AttackGrunts[6]                         }, new float[] { 5.0f       }, new AttackDetails(2, 3, 0.15f, 0.28f, -0.000f, -0.000f)   ));
            m_attackSFX.Add(FightingState.Combo2[4], new AttackInfo(new AudioClip[] { clip.LightSwing, clip.GenericHits[11] },       new float[] { 12f,9.25f }, null, null,                                                                                   new AttackDetails(2, 4, 0.30f, 0.40f, -0.100f, -0.500f)   ));
            m_attackSFX.Add(FightingState.Combo2[5], new AttackInfo(new AudioClip[] { clip.MediumPunch, clip.BodyPunches[1] },       new float[] { 8f, 2.75f }, new AudioClip[] { clip.AttackGrunts[9]                         }, new float[] { 4.0f       }, new AttackDetails(2, 5, 0.55f, 0.67f, -0.000f, -0.000f)   ));
            m_attackSFX.Add(FightingState.Combo2[6], new AttackInfo(new AudioClip[] { clip.KneeUppercut, clip.DistortedPunches[0] }, new float[] { 8f, 2.25f }, new AudioClip[] { clip.AttackGrunts[3]                         }, new float[] { 8.0f       }, new AttackDetails(2, 6, 0.25f, 0.43f, -0.100f, -0.250f)   ));
            m_attackSFX.Add(FightingState.Combo2[7], new AttackInfo(new AudioClip[] { clip.LightKick2, clip.WetPunches[4] },         new float[] { 8f, 2.25f }, null, null,                                                                                   new AttackDetails(2, 7, 0.28f, 0.45f, -0.000f, -0.000f)   ));
            m_attackSFX.Add(FightingState.Combo2[8], new AttackInfo(new AudioClip[] { clip.HardSrike, clip.DistortedPunches[3] },    new float[] { 8f, 1.75f }, new AudioClip[] { clip.AttackGrunts[7]                         }, new float[] { 7.0f       }, new AttackDetails(2, 8, 0.23f, 0.42f, -0.125f, -6.000f)   ));


            m_attackSFX.Add(FightingState.SpecialAttacks[0], new AttackInfo(new AudioClip[] { clip.ShovelKick, clip.WetPunches[8] }, new float[] { 8f, 2.75f }, new AudioClip[] { clip.AttackGrunts[0], clip.AttackGrunts[1] },   new float[] { 2.5f, 2.5f }, new AttackDetails(8, 0, 0.20f, 0.30f, -0.150f, -12.00f)   ));

            
            // m_attackSFX.Add(FightingState.Combo2[1], new AttackInfo(  ));
            // m_attackSFX.Add(FightingState.Combo2[2], new AttackInfo(  ));
            // m_attackSFX.Add(FightingState.Combo2[3], new AttackInfo(  ));
            // m_attackSFX.Add(FightingState.Combo2[4], new AttackInfo(  ));
            // m_attackSFX.Add(FightingState.Combo2[5], new AttackInfo(  ));
            // m_attackSFX.Add(FightingState.Combo2[6], new AttackInfo(  ));
            // m_attackSFX.Add(FightingState.Combo2[7], new AttackInfo(  ));

            // m_attkSFX.Add(FightingState.Combo2[0], new AttkInfo(2, 0, 0.25f, 0.35f, clip.MediumPunch, 5, 0f, 0f));
            // m_attkSFX.Add(FightingState.Combo2[1], new AttkInfo(2, 1, 0.45f, 0.55f, clip.Jab, 5, 0f, 0f));
            // m_attkSFX.Add(FightingState.Combo2[2], new AttkInfo(2, 2, 0.30f, 0.40f, clip.LightPunch2, 5, 0f, 0f));
            // m_attkSFX.Add(FightingState.Combo2[3], new AttkInfo(2, 3, 0.25f, 0.35f, clip.MediumPunch, 5, 0f, 0f));
            // m_attkSFX.Add(FightingState.Combo2[4], new AttkInfo(2, 4, 0.35f, 0.45f, clip.LightSwing, 4, 0f, 0f));
            // m_attkSFX.Add(FightingState.Combo2[5], new AttkInfo(2, 5, 0.25f, 0.35f, clip.KneeUppercut, 4, 0f, 0f));
            // m_attkSFX.Add(FightingState.Combo2[6], new AttkInfo(2, 6, 0.25f, 0.35f, clip.LightKick2, 5, 0f, 0f));
            // m_attkSFX.Add(FightingState.Combo2[7], new AttkInfo(2, 7, 0.20f, 0.30f, clip.HardSrike, 4, 0f, 0f));

            m_attkSFX.Add(FightingState.Combo3[0], new AttkInfo(3, 0, 0.30f, 0.40f, clip.LightPunch2, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo3[1], new AttkInfo(3, 1, 0.50f, 0.60f, clip.MediumPunch, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo3[2], new AttkInfo(3, 2, 0.35f, 0.45f, clip.LightPunch, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo3[3], new AttkInfo(3, 3, 0.25f, 0.35f, clip.HardSrike, 4, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo3[4], new AttkInfo(3, 4, 0.35f, 0.45f, clip.MediumPunch2, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo3[5], new AttkInfo(3, 5, 0.25f, 0.35f, clip.HardKick2, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo3[6], new AttkInfo(3, 6, 0.30f, 0.40f, clip.CrescentKick, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo3[7], new AttkInfo(3, 7, 0.45f, 0.55f, clip.DoubleKick, 5, 0f, 0f));

            m_attkSFX.Add(FightingState.Combo4[0], new AttkInfo(4, 0, 0.40f, 0.50f, clip.HardKick, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo4[1], new AttkInfo(4, 1, 0.25f, 0.35f, clip.LightKick2, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo4[2], new AttkInfo(4, 2, 0.35f, 0.45f, clip.LightSwing, 4, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo4[3], new AttkInfo(4, 3, 0.35f, 0.45f, clip.LightPunch, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo4[4], new AttkInfo(4, 4, 0.25f, 0.35f, clip.KneeUppercut, 4, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo4[5], new AttkInfo(4, 5, 0.30f, 0.40f, clip.RoundHouse, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo4[6], new AttkInfo(4, 6, 0.35f, 0.45f, clip.SweepKick, 3, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo4[7], new AttkInfo(4, 7, 0.30f, 0.40f, clip.ShovelKick, 5, 0f, 0f));

            m_attkSFX.Add(FightingState.Combo5[0], new AttkInfo(5, 0, 0.20f, 0.30f, clip.LightSwing, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo5[1], new AttkInfo(5, 1, 0.25f, 0.35f, clip.DoubleKick, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo5[2], new AttkInfo(5, 2, 0.35f, 0.45f, clip.MediumPunch2, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo5[3], new AttkInfo(5, 3, 0.30f, 0.40f, clip.LightKick, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo5[4], new AttkInfo(5, 4, 0.25f, 0.35f, clip.LightKick2, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo5[5], new AttkInfo(5, 5, 0.20f, 0.30f, clip.ElbowStrike, 3, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo5[6], new AttkInfo(5, 6, 0.30f, 0.40f, clip.HardKick2, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo5[7], new AttkInfo(5, 7, 0.10f, 0.20f, clip.LaunchKick, 5, 0f, 0f));

            m_attkSFX.Add(FightingState.Combo6[0], new AttkInfo(6, 0, 0.20f, 0.30f, clip.LeftUppercut, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo6[1], new AttkInfo(6, 1, 0.45f, 0.55f, clip.Jab, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo6[2], new AttkInfo(6, 2, 0.30f, 0.40f, clip.LightPunch2, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo6[3], new AttkInfo(6, 3, 0.20f, 0.30f, clip.ElbowStrike, 3, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo6[4], new AttkInfo(6, 4, 0.50f, 0.60f, clip.ElbowStrike2, 4, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo6[5], new AttkInfo(6, 5, 0.50f, 0.60f, clip.MediumPunch, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo6[6], new AttkInfo(6, 6, 0.20f, 0.30f, clip.CEBJointattack, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo6[7], new AttkInfo(6, 7, 0.20f, 0.30f, clip.HighElbow, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo6[8], new AttkInfo(6, 8, 0.20f, 0.30f, clip.FrontPushKick, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo6[9], new AttkInfo(6, 9, 0.25f, 0.35f, clip.HardSrike, 4, 0f, 0f));

            m_attkSFX.Add(FightingState.Combo7[0], new AttkInfo(7, 0, 0.50f, 0.60f, clip.MediumPunch, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo7[1], new AttkInfo(7, 1, 0.45f, 0.55f, clip.Jab, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo7[2], new AttkInfo(7, 2, 0.25f, 0.35f, clip.KneeUppercut, 4, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo7[3], new AttkInfo(7, 3, 0.35f, 0.45f, clip.SpinningHookKick, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo7[4], new AttkInfo(7, 4, 0.30f, 0.40f, clip.CrescentKick, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo7[5], new AttkInfo(7, 5, 0.30f, 0.40f, clip.LightKick, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo7[6], new AttkInfo(7, 6, 0.25f, 0.35f, clip.LightKick2, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo7[7], new AttkInfo(7, 7, 0.40f, 0.50f, clip.LightKick, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo7[8], new AttkInfo(7, 8, 0.15f, 0.25f, clip.HardKick2, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo7[9], new AttkInfo(7, 9, 0.15f, 0.25f, clip.LightKick, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo7[10], new AttkInfo(7, 11, 0.35f, 0.45f, clip.SpinningHookKick, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo7[11], new AttkInfo(7, 12, 0.25f, 0.35f, clip.LightKick2, 5, 0f, 0f));
            m_attkSFX.Add(FightingState.Combo7[12], new AttkInfo(7, 13, 0.10f, 0.20f, clip.LaunchKick, 5, 0f, 0f));
        }
    }

    public int GetSFXCount() { return m_attackSFX.Count; }

    public AttackInfo GetSFXInfo(int stateHash)
    {
        AttackInfo attackSFXInfo;
        if (m_attackSFX.TryGetValue(stateHash, out attackSFXInfo))
        {
            return attackSFXInfo;
        }
        return default(AttackInfo);
    }

    public int GetVFXCount() { return m_attackVFX.Count; }

    public AttackVFX GetVFXInfo(int stateHash)
    {
        AttackVFX attackSFXInfo;
        if (m_attackVFX.TryGetValue(stateHash, out attackSFXInfo))
        {
            return attackSFXInfo;
        }
        Debug.Log("State does not match! ");
        return default(AttackVFX);
    }

    public int GetAttkSFXCount() { return m_attkSFX.Count; }

    public AttkInfo GetAttkSFXInfo(int stateHash)
    {
        AttkInfo attkSFXInfo;
        if (m_attkSFX.TryGetValue(stateHash, out attkSFXInfo))
        {
            return attkSFXInfo;
        }
        return default(AttkInfo);
    }
}

