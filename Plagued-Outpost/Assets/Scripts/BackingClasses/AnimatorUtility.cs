using UnityEngine;
using System.Collections.Generic;

public static class AnimatorUtility
{
    public struct PS
    {
        public static readonly int walkingPara = Animator.StringToHash("Walking");
        public static readonly int sprintPara = Animator.StringToHash("Sprint");
        public static readonly int horizontalPara = Animator.StringToHash("Horizontal");
        public static readonly int verticalPara = Animator.StringToHash("Vertical");
        public static readonly int actionStancePara = Animator.StringToHash("ActionStance");

        public static readonly int sprintTacklePara = Animator.StringToHash("SprintTackle");

        public static readonly int sprintSpeedPara = Animator.StringToHash("SprintSpeed");
        public static readonly int standingJumpPara = Animator.StringToHash("StandingJump");
        public static readonly int forwardJumpPara = Animator.StringToHash("ForwardJump");
        public static readonly int sideJumpPara = Animator.StringToHash("SideJump");
        public static readonly int sprintJumpPara = Animator.StringToHash("SprintJump");
        public static readonly int fastSprintJumpPara = Animator.StringToHash("FastSprintJump");

        public static readonly int standingJumpState = Animator.StringToHash("Base Layer.Standing_Jump");
        public static readonly int forwardJumpState = Animator.StringToHash("Base Layer.Forward_Jump");
        public static readonly int leftSideJumpState = Animator.StringToHash("Base Layer.Left_Jump");
        public static readonly int rightSideJumpState = Animator.StringToHash("Base Layer.Right_Jump");
        public static readonly int leftSrideJumpState = Animator.StringToHash("Base Layer.Left_Stride_Jump");
        public static readonly int rightSrideJumpState = Animator.StringToHash("Base Layer.Right_Stride_Jump");

        public static readonly int idleState = Animator.StringToHash("Base Layer.Idle");
        public static readonly int walkState = Animator.StringToHash("Base Layer.Walk");
        public static readonly int rollState = Animator.StringToHash("Base Layer.Roll");
        public static readonly int jogState = Animator.StringToHash("Base Layer.Jog");
        public static readonly int sprintTackleState = Animator.StringToHash("Base Layer.Sprint_Tackle");
        public static readonly int sprintBreakState = Animator.StringToHash("Base Layer.Sprint_Break");
        public static readonly int sprintJumpState = Animator.StringToHash("Base Layer.Sprint_Jump");
        public static readonly int fastSprintJumpState = Animator.StringToHash("Base Layer.Fast_Sprint_Jump");

        public static readonly int dashState = Animator.StringToHash("Base Layer.Dash");
        public static readonly int dashBreakState = Animator.StringToHash("Base Layer.Dash_Break");
        public static readonly int dashStateC3 = Animator.StringToHash("Base Layer.Combo3.Dash");
        public static readonly int dashStateC6 = Animator.StringToHash("Base Layer.Combo6.Dash");
        public static readonly int dashStateC7 = Animator.StringToHash("Base Layer.Combo7.Dash");
        public static readonly int backStepState = Animator.StringToHash("Base Layer.Back_Step");
        public static readonly int rollBackState = Animator.StringToHash("Base Layer.Roll_Back");
        public static readonly int retreatState = Animator.StringToHash("Base Layer.Retreat");
        public static readonly int dashBreakStateC3 = Animator.StringToHash("Base Layer.Combo3.Dash_Break");
        public static readonly int dashBreakStateC6 = Animator.StringToHash("Base Layer.Combo6.Dash_Break");
        public static readonly int dashBreakStateC7 = Animator.StringToHash("Base Layer.Combo7.Dash_Break");
        public static readonly int backStepBreakState = Animator.StringToHash("Base Layer.Back_Step_Break");
        public static readonly int rollBackBreakState = Animator.StringToHash("Base Layer.Roll_Back_Break");
        public static readonly int retreatBreakState = Animator.StringToHash("Base Layer.Retreat_Break");
        public static readonly int shoeIdleState = Animator.StringToHash("Base Layer.Shoe_Idle");
        public static readonly int actionToStandingIdleState = Animator.StringToHash("Base Layer.Action_To_Standing_Idle");
        public static readonly int dodgeCounterState = Animator.StringToHash("Base Layer.Dodge_Counter");
        public static readonly int dashCounterState = Animator.StringToHash("Base Layer.Dash_Counter");
        public static readonly int dashLeftState = Animator.StringToHash("Base Layer.Dash_Left");
        public static readonly int dashRightState = Animator.StringToHash("Base Layer.Dash_Right");
        public static readonly int backFlipState = Animator.StringToHash("Base Layer.Back_Flip");
        public static readonly int flipKickInitState = Animator.StringToHash("Base Layer.Flip_Kick_Init");

        public static readonly int sprintState = Animator.StringToHash("Base Layer.Sprint");

        public static readonly int knockedUnconsciousState = Animator.StringToHash("Base Layer.Knocked_Unconscious");
        public static readonly int pushKickHitState = Animator.StringToHash("Base Layer.Push_Kick_Hit");
        public static readonly int livershotKnockdown = Animator.StringToHash("Base Layer.Livershot_Knockdown");
        public static readonly int shoulderHitAndFallState = Animator.StringToHash("Base Layer.Shoulder_Hit_And_Fall");
        public static readonly int rageJabHitState = Animator.StringToHash("Base Layer.Rage_Jab_Hit");

        public static readonly int kipUpState = Animator.StringToHash("Base Layer.Kip_Up");

        public static readonly int unconsciousState = Animator.StringToHash("Base Layer.Unconscious");
        public static readonly int SU_UnconsciousState = Animator.StringToHash("Base Layer.SU_Unconscious");
        public static readonly int writhingInPainState = Animator.StringToHash("Base Layer.Writhing_In_Pain");

        public static string[] IsBeingAttackedStates = new string[] { "EC1K", "EC2", "EC2K", "EC3K", "EC4", "EC4K", "Incapacitated" };
        public static string[] SkillReadyState = new string[] { "SkillAccessible", "FightingIdle", "BasicFightingIdle", "Attack", "DashAttack", "ComboFinisher", "QMSB", "RQMSB" };
    }

    public struct ES
    {
        public static readonly int aggressivePara = Animator.StringToHash("Aggressive");
        public static readonly int idleState = Animator.StringToHash("Base Layer.Idle");
        public static readonly int sprintState = Animator.StringToHash("Base Layer.Sprint");
        public static readonly int screamState = Animator.StringToHash("Base Layer.Zombie_Scream");
        public static readonly int flipKickHitState = Animator.StringToHash("Base Layer.S1_Hits.Flip_Kick_Hit");
        public static readonly int leftSidePushKickHitBK = Animator.StringToHash("Base Layer.BK_Hits.Left_Side_Push_Kick_Hit_BK");



        public static readonly int kneeUppercutC2 = Animator.StringToHash("Base Layer.C2_Hits.Knee_Uppercut_C2");
        public static readonly int kneeUppercutKnockBackC2 = Animator.StringToHash("Base Layer.C2_Hits.Knee_Uppercut_Knock_Back_C2");


        public static readonly int frontPushGaleC2 = Animator.StringToHash("Base Layer.C2_Hits.Front_Push_Gale_Hit_C2");


        public static string[] IsBeingAttackedStates = new string[] { "BP", "BK", "C1", "C2", "S1" };
    }



    public static int GetCurrentStateHash(Animator animator) { return animator.GetCurrentAnimatorStateInfo(0).fullPathHash; }
    public static float GetNormalizedTime(Animator animator) { return animator.GetCurrentAnimatorStateInfo(0).normalizedTime; }

    public static int GetNextStateHash(Animator animator) { return animator.GetNextAnimatorStateInfo(0).fullPathHash; }
    public static AnimatorStateInfo GetCurrentStateInfo(Animator animator) { return animator.GetCurrentAnimatorStateInfo(0); }
    public static AnimatorStateInfo GetNextStateInfo(Animator animator) { return animator.GetNextAnimatorStateInfo(0); }

    public static void SetIntParameter(Animator animator, string parameterName, int value)
    {
        animator.SetInteger(Animator.StringToHash(parameterName), value);
    }
    public static void SetFloatParameter(Animator animator, string parameterName, float value)
    {
        animator.SetFloat(Animator.StringToHash(parameterName), value);
    }
    public static void SetBoolParameter(Animator animator, string parameterName, bool value)
    {
        animator.SetBool(Animator.StringToHash(parameterName), value);
    }
    public static int GetIntParameter(Animator animator, string parameterName)
    {
        return animator.GetInteger(Animator.StringToHash(parameterName));
    }
    public static float GetFloatParameter(Animator animator, string parameterName)
    {
        return animator.GetFloat(Animator.StringToHash(parameterName));
    }
    public static bool GetBoolParameter(Animator animator, string parameterName)
    {
        return animator.GetBool(Animator.StringToHash(parameterName));
    }

    public static readonly int[] EnemyAttacks = new int[]
    {
        Animator.StringToHash("Base Layer.Attack.Ground_Pound"),
        Animator.StringToHash("Base Layer.Attack.Zombie_Init_Strike_R"),
        Animator.StringToHash("Base Layer.Attack.Zombie_Strike_L"),
        Animator.StringToHash("Base Layer.Attack.Zombie_Strike_R"),
        Animator.StringToHash("Base Layer.Attack.Zombie_Strike_L_0"),
        Animator.StringToHash("Base Layer.Attack.Zombie_Strike_R_0"),
        Animator.StringToHash("Base Layer.Attack.Zombie_Strike_L_1"),
        Animator.StringToHash("Base Layer.Attack.Zombie_Strike_R_1"),
        Animator.StringToHash("Base Layer.Attack.Zombie_Strike_L_2"),
        Animator.StringToHash("Base Layer.Attack.Zombie_Strike_R_2"),
        Animator.StringToHash("Base Layer.Attack.Zombie_Strike_L_3"),
        Animator.StringToHash("Base Layer.Attack.Zombie_Push_Kick"),
        Animator.StringToHash("Base Layer.Attack.Zombie_Jab_Strike"),
        Animator.StringToHash("Base Layer.Attack.Zombie_HeadButt"),
        Animator.StringToHash("Base Layer.Attack.Zombie_Jab"),
    };
}

public class CombatGruntSystem
{
    public readonly int[] CombatGruntStates = new int[]
    {
        // For Enemy
        AnimatorUtility.ES.screamState,
        AnimatorUtility.EnemyAttacks[0],
        AnimatorUtility.EnemyAttacks[9],
        AnimatorUtility.EnemyAttacks[12],
        AnimatorUtility.EnemyAttacks[14],
        AnimatorUtility.ES.flipKickHitState,
        AnimatorUtility.ES.leftSidePushKickHitBK,
        AnimatorUtility.ES.kneeUppercutC2,
        AnimatorUtility.ES.frontPushGaleC2,


        Animator.StringToHash(AnimatorUtility.ES.IsBeingAttackedStates[0]),
        Animator.StringToHash(AnimatorUtility.ES.IsBeingAttackedStates[1]),
        Animator.StringToHash(AnimatorUtility.ES.IsBeingAttackedStates[2]),
        Animator.StringToHash(AnimatorUtility.ES.IsBeingAttackedStates[3]),
        

        // For Player
        AnimatorUtility.PS.knockedUnconsciousState,
        AnimatorUtility.PS.pushKickHitState,
        AnimatorUtility.PS.shoulderHitAndFallState,
        AnimatorUtility.PS.rageJabHitState,
        Animator.StringToHash(AnimatorUtility.PS.IsBeingAttackedStates[1]),
        AnimatorUtility.PS.kipUpState,
        AnimatorUtility.PS.rollBackState,
        AnimatorUtility.PS.retreatState,
        AnimatorUtility.PS.dodgeCounterState,
    };

    public Dictionary<int, CombatGruntInfo> CombatGruntInfo = new Dictionary<int, CombatGruntInfo>();

    public CombatGruntSystem(StateSFX stateSFX)
    {
        CombatGruntInfo.Add(CombatGruntStates[0],   new CombatGruntInfo(stateSFX.CombatGrunts.EnemyScream,          0.30f,  new float[] { 0.75f        } ));
        CombatGruntInfo.Add(CombatGruntStates[1],   new CombatGruntInfo(stateSFX.CombatGrunts.GroundPound,          0f,     new float[] { 1.25f        } ));
        CombatGruntInfo.Add(CombatGruntStates[2],   new CombatGruntInfo(stateSFX.CombatGrunts.RageKick,             0.50f,  new float[] { 6.00f        } ));
        CombatGruntInfo.Add(CombatGruntStates[3],   new CombatGruntInfo(stateSFX.CombatGrunts.JabStrike,            0f,     new float[] { 1.00f        } ));
        CombatGruntInfo.Add(CombatGruntStates[4],   new CombatGruntInfo(stateSFX.CombatGrunts.RageJab,              0f,     new float[] { 2.25f        } ));
        CombatGruntInfo.Add(CombatGruntStates[5],   new CombatGruntInfo(stateSFX.CombatGrunts.FlipKickPain,         0.10f,  new float[] { 2.00f        } ));
        CombatGruntInfo.Add(CombatGruntStates[6],   new CombatGruntInfo(stateSFX.CombatGrunts.LeftSidePushKickPain, 0f,     new float[] { 2.00f        } ));

        CombatGruntInfo.Add(CombatGruntStates[7],   new CombatGruntInfo(stateSFX.CombatGrunts.KneeUppercutPain,     0f,     new float[] { 2.00f        } ));
        CombatGruntInfo.Add(CombatGruntStates[8],   new CombatGruntInfo(stateSFX.CombatGrunts.FrontPushGalePain,    0f,     new float[] { 2.00f        } ));

        CombatGruntInfo.Add(CombatGruntStates[9],   new CombatGruntInfo(stateSFX.CombatGrunts.EnemyGenericPain,     0f,     new float[] { 1.50f, 1.50f, 1.50f, 1.50f } ));
        CombatGruntInfo.Add(CombatGruntStates[10],  new CombatGruntInfo(stateSFX.CombatGrunts.EnemyGenericPain,     0f,     new float[] { 1.50f, 1.50f, 1.50f, 1.50f } ));
        CombatGruntInfo.Add(CombatGruntStates[11],  new CombatGruntInfo(stateSFX.CombatGrunts.EnemyGenericPain,     0f,     new float[] { 1.50f, 1.50f, 1.50f, 1.50f } ));
        CombatGruntInfo.Add(CombatGruntStates[12],  new CombatGruntInfo(stateSFX.CombatGrunts.EnemyGenericPain,     0f,     new float[] { 1.25f, 1.25f, 1.25f, 1.25f } ));

        CombatGruntInfo.Add(CombatGruntStates[13],  new CombatGruntInfo(stateSFX.CombatGrunts.GroundPoundPain,      0.10f,  new float[] { 2.25f         } ));
        CombatGruntInfo.Add(CombatGruntStates[14],  new CombatGruntInfo(stateSFX.CombatGrunts.EnemyKickPain,        0.10f,  new float[] { 3.75f         } ));
        CombatGruntInfo.Add(CombatGruntStates[15],  new CombatGruntInfo(stateSFX.CombatGrunts.EnemyJabStrikePain,   0f,     new float[] { 1.50f         } ));
        CombatGruntInfo.Add(CombatGruntStates[16],  new CombatGruntInfo(stateSFX.CombatGrunts.EnemyJabPain,         0f,     new float[] { 2.25f         } ));
        CombatGruntInfo.Add(CombatGruntStates[17],  new CombatGruntInfo(stateSFX.CombatGrunts.EnemyStrikePain,      0f,     new float[] { 2.00f, 2.00f  } ));
        CombatGruntInfo.Add(CombatGruntStates[18],  new CombatGruntInfo(stateSFX.CombatGrunts.KipUp,                0.375f, new float[] { 5.00f         } ));
        CombatGruntInfo.Add(CombatGruntStates[19],  new CombatGruntInfo(stateSFX.CombatGrunts.RollBack,             0.50f,  new float[] { 3.75f         } ));
        CombatGruntInfo.Add(CombatGruntStates[20],  new CombatGruntInfo(stateSFX.CombatGrunts.Retreat,              0.55f,  new float[] { 3.00f         } ));
        CombatGruntInfo.Add(CombatGruntStates[21],  new CombatGruntInfo(stateSFX.CombatGrunts.Dodge,                0.25f,  new float[] { 5.00f, 5.00f, } ));
    }
}

public struct CombatGruntInfo
{
    public AudioClip[] GruntClips { get; private set; }
    public float NormalizedTime { get; private set; }
    public float[] Volumes { get; private set; }

    public CombatGruntInfo(AudioClip[] gruntClip, float normalizedTime, float[] volume)
    {
        GruntClips = gruntClip;
        NormalizedTime = normalizedTime;
        Volumes = volume;
    }
}



public static class VisualDebug
{
    public static void DrawSphere(Vector3 spherePosition, Color color, float scale)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = spherePosition;
        sphere.GetComponent<Renderer>().material.color = color;
        sphere.transform.localScale = new Vector3(scale, scale, scale);
    }
}
