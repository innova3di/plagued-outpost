using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public partial class FightingState : StateMachineBehaviour
{
    public static readonly int fightingToIdle = Animator.StringToHash("Base Layer.Fight_To_Action_Idle");
    public static readonly int fightingIdleState = Animator.StringToHash("Base Layer.Fighting_Idle");

    public static readonly int[] SpecialAttacks = new int[]
    {
        Animator.StringToHash("Base Layer.Flip_Kick")
    };

    public static readonly int[] BasicPunches = new int[]
    {
        Animator.StringToHash("Base Layer.Basic_Punches.Jab"),
        Animator.StringToHash("Base Layer.Basic_Punches.Charging_Body_Punch_Left"),
        Animator.StringToHash("Base Layer.Basic_Punches.Body_Punch_Right"),
        Animator.StringToHash("Base Layer.Basic_Punches.Uppercut"),
    };
    public static readonly int[] BasicKicks = new int[]
    {
        Animator.StringToHash("Base Layer.Basic_Kicks.Power_Roundhouse_Mid_Head"),
        Animator.StringToHash("Base Layer.Basic_Kicks.Faking_Roundhouse_L"),
        Animator.StringToHash("Base Layer.Basic_Kicks.Full_Roundhouse"),
        Animator.StringToHash("Base Layer.Basic_Kicks.Left_Side_Push_Kick"),
    };
    public static readonly int[] Combo1 = new int[]
    {
        Animator.StringToHash("Base Layer.Combo1.Jab"),
        Animator.StringToHash("Base Layer.Combo1.Straight_Left_Punch"),
        Animator.StringToHash("Base Layer.Combo1.45_Degree_Kick"),
        Animator.StringToHash("Base Layer.Combo1.Faking_Roundhouse_L"),
        Animator.StringToHash("Base Layer.Combo1.Spinning_Hook_Kick"),
        Animator.StringToHash("Base Layer.Combo1.Charging_Body_Punch_Left"),
        Animator.StringToHash("Base Layer.Combo1.Power_Roundhouse_High"),
        Animator.StringToHash("Base Layer.Combo1.Double_Straight_Kick"),
    };
    public static readonly int[] Combo2 = new int[]
    {
        Animator.StringToHash("Base Layer.Combo2.Left_Hook"),
        Animator.StringToHash("Base Layer.Combo2.Jab"),
        Animator.StringToHash("Base Layer.Combo2.Straight_Left_Punch"),
        Animator.StringToHash("Base Layer.Combo2.Straight_Right_Elbow"),
        Animator.StringToHash("Base Layer.Combo2.Uppercut"),
        Animator.StringToHash("Base Layer.Combo2.Charging_Body_Punch_Left"),
        Animator.StringToHash("Base Layer.Combo2.Knee_Uppercut"),
        Animator.StringToHash("Base Layer.Combo2.Faking_Roundhouse_L"),
        Animator.StringToHash("Base Layer.Combo2.Front_Push_Gale"),
    };
    public static readonly int[] Combo3 = new int[]
    {
        Animator.StringToHash("Base Layer.Combo3.Straight_Left_Punch"),
        Animator.StringToHash("Base Layer.Combo3.Charging_Body_Punch_Left"),
        Animator.StringToHash("Base Layer.Combo3.Body_Punch_Right"),
        Animator.StringToHash("Base Layer.Combo3.Left_Side_Push_Kick"),
        Animator.StringToHash("Base Layer.Combo3.Charging_Left_Hook"),
        Animator.StringToHash("Base Layer.Combo3.Snapping_Right_Roundhouse"),
        Animator.StringToHash("Base Layer.Combo3.Crescent_Kick"),
        Animator.StringToHash("Base Layer.Combo3.Charging_Jump_Straight_kick"),
    };
    public static readonly int[] Combo4 = new int[]
    {
        Animator.StringToHash("Base Layer.Combo4.Body_Kick_Right"),
        Animator.StringToHash("Base Layer.Combo4.Faking_Roundhouse_L"),
        Animator.StringToHash("Base Layer.Combo4.Uppercut"),
        Animator.StringToHash("Base Layer.Combo4.Body_Punch_Right"),
        Animator.StringToHash("Base Layer.Combo4.Knee_Uppercut"),
        Animator.StringToHash("Base Layer.Combo4.Full_Roundhouse"),
        Animator.StringToHash("Base Layer.Combo4.Leg_Sweep"),
        Animator.StringToHash("Base Layer.Combo4.Shovel_Kick"),
    };
    public static readonly int[] Combo5 = new int[]
    {
        Animator.StringToHash("Base Layer.Combo5.Volley_Strike_Kick"),
        Animator.StringToHash("Base Layer.Combo5.Charging_Double_Kick"),
        Animator.StringToHash("Base Layer.Combo5.Charging_Left_Hook"),
        Animator.StringToHash("Base Layer.Combo5.Power_Roundhouse_Mid_Head"),
        Animator.StringToHash("Base Layer.Combo5.Faking_Roundhouse_L"),
        Animator.StringToHash("Base Layer.Combo5.Elbow_Stab"),
        Animator.StringToHash("Base Layer.Combo5.Power_Roundhouse_High"),
        Animator.StringToHash("Base Layer.Combo5.Sweeping_Gale_R"),
    };
    public static readonly int[] Combo6 = new int[]
    {
        Animator.StringToHash("Base Layer.Combo6.Left_Uppercut"),
        Animator.StringToHash("Base Layer.Combo6.Jab"),
        Animator.StringToHash("Base Layer.Combo6.Straight_Left_Punch"),
        Animator.StringToHash("Base Layer.Combo6.Elbow_Stab"),
        Animator.StringToHash("Base Layer.Combo6.Charged_Elbow_Stab"),
        Animator.StringToHash("Base Layer.Combo6.Charging_Body_Punch_Left"),
        Animator.StringToHash("Base Layer.Combo6.CEB_Joint_Attack"),
        Animator.StringToHash("Base Layer.Combo6.High_Elbow"),
        Animator.StringToHash("Base Layer.Combo6.Front_Push_Kick"),
        Animator.StringToHash("Base Layer.Combo6.Left_Side_Push_Kick"),
    };
    public static readonly int[] Combo7 = new int[]
    {
        Animator.StringToHash("Base Layer.Combo7.Charging_Body_Punch_Left"),
        Animator.StringToHash("Base Layer.Combo7.Jab"),
        Animator.StringToHash("Base Layer.Combo7.Knee_Uppercut"),
        Animator.StringToHash("Base Layer.Combo7.Spinning_Hook_Kick"),
        Animator.StringToHash("Base Layer.Combo7.Crescent_Kick"),
        Animator.StringToHash("Base Layer.Combo7.Power_Roundhouse_Mid_Head"),
        Animator.StringToHash("Base Layer.Combo7.Faking_Roundhouse_L"),
        Animator.StringToHash("Base Layer.Combo7.Power_Roundhouse_Kick_Mid_R"),
        Animator.StringToHash("Base Layer.Combo7.Power_Roundhouse_High"),
        Animator.StringToHash("Base Layer.Combo7.Power_Roundhouse_Kick_Mid_R_1"),
        Animator.StringToHash("Base Layer.Combo7.Spinning_Hook_Kick_1"),
        Animator.StringToHash("Base Layer.Combo7.Faking_Roundhouse_L_1"),
        Animator.StringToHash("Base Layer.Combo7.Sweeping_Gale_R"),
    };
}