using UnityEngine;
using System.Collections;

public class UISwtichState : StateMachineBehaviour
{
    public static readonly int switchPara = Animator.StringToHash("Switch");
    public static readonly int pressedState = Animator.StringToHash("Base Layer.Pressed");
    public static readonly int normalState = Animator.StringToHash("Base Layer.Normal");

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.fullPathHash == pressedState)
        {
            animator.SetBool(switchPara, false);
        }
    }
}
