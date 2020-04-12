using UnityEngine;

namespace EventSystem
{
    public class SkillSystemEvent
    {
        public SkillSystemEvent() { }

        public delegate void SkillExecution(SkillData skillInputData, SkillSystem skillSystem);
        public event SkillExecution ExecuteSkillEvent;

        public void OnActivateSkill(SkillData skillInputData, SkillSystem skillSystem)
        {
            if (ExecuteSkillEvent != null)
            {
                ExecuteSkillEvent(skillInputData, skillSystem);
            }
        }

        public delegate void ComboExecution();
        public event ComboExecution JointAttackEvent;
        public int JointAttackEventCount { get { return JointAttackEvent.GetInvocationList().Length; } }
        public void ExecuteJointAttack()
        {
            if (JointAttackEvent != null)
            {
                JointAttackEvent();
            }
        }

        public delegate void GlobalSkills(SkillData skillInputData);
        public static event GlobalSkills GlobalSkillEvent;

        public static void ExecuteGlobalSkill(SkillData skillInputData)
        {
            if (GlobalSkillEvent != null)
            {
                GlobalSkillEvent(skillInputData);
            }
        }
    }

    public class HitSystemEvent
    {
        public HitSystemEvent() { }

        public delegate void OnHit(Animator animator);
        public event OnHit OnHitEvent;

        public void OnAttackHit(Animator animator)
        {
            if (OnHitEvent != null)
            {
                OnHitEvent(animator);
            }
        }
    }
}

