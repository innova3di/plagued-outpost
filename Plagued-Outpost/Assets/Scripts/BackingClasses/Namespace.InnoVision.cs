using UnityEngine;
using System.Collections.Generic;

namespace InnoVision
{
    public abstract class Command
    {
        protected CommandReceiver m_commandReceiver;
        protected PlayerMovements m_playerMovements;
        protected ButtonInputReceiver m_buttonInputReceiver;

        public Command(ButtonInputReceiver buttonInputReceiver)
        {
            m_buttonInputReceiver = buttonInputReceiver;
        }

        public Command(CommandReceiver receiver)
        {
            m_commandReceiver = receiver;
        }

        public Command(PlayerMovements playerMovements)
        {
            m_playerMovements = playerMovements;
        }

        public Command() { }

        public virtual void Execute() { }
        public virtual void Execute(RaycastService rayCast) { }
    }

    public class Invoker
    {
        private Command m_command;
        public Invoker(Command command) { m_command = command; }
        public void InvokeCommand() { m_command.Execute(); }
    }

    public enum GameState { Paused = 0, Running = 1, }

    public enum SkillSet
    {
        NONE = 0,
        SKILL_COMBO_ONE = 1,
        SKILL_COMBO_TWO = 2,
        SKILL_COMBO_THREE = 3,
        SKILL_COMBO_FOUR = 4,
        SKILL_COMBO_FIVE = 5,
        SKILL_COMBO_SIX = 6,
        SKILL_COMBO_SEVEN = 7,
        FLIP_KICK = 8,
        DASH_FORWARD = 9,
        BASIC_PUNCHES = 10,
        BASIC_KICKS = 11,
        BACK_STEP = 12,
        DASH_LEFT = 13,
        DASH_RIGHT = 14,
        BACK_FLIP = 15,
        KIP_UP = 16,
        ROLL_BACK = 17,
        RETREAT = 18,
        DODGE = 19,
    }
    public enum JointAttack
    {
        NO_INPUT = 0,
        LEFT_PUNCH = 1,
        RIGHT_PUNCH = 2,
        LEFT_KICK = 3,
        RIGHT_KICK = 4,
        LEFT_AND_RIGHT_PUNCH = 5,
        LEFT_AND_RIGHT_KICK = 6,
        DASH_FORWARD = 7,
        BACK_STEP = 8,
        DASH_LEFT = 9,
        DASH_RIGHT = 10,
    }

    public class ButtonVisualFeedBack : Command
    {
        public ButtonVisualFeedBack(ButtonInputReceiver buttonInputReceiver) : base(buttonInputReceiver) { }
        public override void Execute() { m_buttonInputReceiver.SkillButtonVisualFeedBack(); }
    }

    public class GlobalSkill : Command
    {
        public GlobalSkill(CommandReceiver skillCommandReceiver) : base(skillCommandReceiver) { }
        public override void Execute() { m_commandReceiver.GlobalSkillInputService(); }
    }

    public class Combo : Command
    {
        private SkillSet m_skillSet;
        public Queue<JointAttack> ComboQueue { get; private set; }

        public Combo(Animator animator, CommandReceiver skillCommandReceiver, SkillSet skillSet) : base(skillCommandReceiver)
        {
            m_skillSet = skillSet;
            if ((int)m_skillSet != 10 && (int)m_skillSet != 11) { AnimatorUtility.SetIntParameter(animator, "ComboNumber", 0); }
            AnimatorUtility.SetIntParameter(animator, "SkillSetNumber", (int)m_skillSet);
            AnimatorUtility.SetBoolParameter(animator, "Fighting", true);
            ComboQueue = new Queue<JointAttack>();
        }
        public override void Execute() { m_commandReceiver.SetCombo(m_skillSet, ComboQueue); }
    }

    public class Movement : Command
    {
        private Animator m_animator;
        public Movement(PlayerMovements playerMovements, Animator animator) : base(playerMovements) { m_animator = animator; }

        public override void Execute()
        {
            if (PauseMenu.GetGameState == GameState.Running)
            {
                if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.idleState) { m_playerMovements.Idle(m_animator); }
                if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.walkState) { m_playerMovements.Walk(m_animator); }
                if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.jogState) { m_playerMovements.Jog(m_animator); }
                if (AnimatorUtility.GetCurrentStateHash(m_animator) == AnimatorUtility.PS.sprintState) { m_playerMovements.Sprint(m_animator); }
            }
        }
    }
}