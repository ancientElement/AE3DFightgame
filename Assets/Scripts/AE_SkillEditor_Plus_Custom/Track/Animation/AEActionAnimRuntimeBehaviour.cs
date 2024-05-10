using AE_SkillEditor_Plus.RunTime;
using AE_SkillEditor_Plus.RunTime.Driver;
using UnityEngine;

namespace AE_SkillEditor_Plus_Custom.Animation.Track
{
    public class AEActionAnimRuntimeBehaviour : AEPlayableBehaviour
    {
        private AEActionAnimClip Clip;

        public AEActionAnimRuntimeBehaviour(StandardClip clip) : base(clip)
        {
            Clip = clip as AEActionAnimClip;
        }

        public override void OnEnter(GameObject context, int fps, int currentFrameID)
        {
            base.OnEnter(context, fps, currentFrameID);
            var animator = context.GetComponent<Animator>();
            if (!Clip.isAnimatorState)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName(Clip.AnimationName))
                    animator.CrossFade(Clip.AnimationName, Clip.FadeIn, 0, 0);
                animator.CrossFade(Clip.AnimationName, Clip.FadeIn);
            }
            else
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName(Clip.StateName))
                    animator.CrossFade(Clip.StateName, Clip.FadeIn, 0, 0);
                animator.CrossFade(Clip.StateName, Clip.FadeIn);
            }
        }
    }
}