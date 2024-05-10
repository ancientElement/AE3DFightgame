using System;
using AE_SkillEditor_Plus.RunTime;
using AE_SkillEditor_Plus.RunTime.Driver;
using UnityEngine;
using AEActionSystem;

namespace AE_SkillEditor_Plus_Custom.Animation.Track
{
    public class AEActionInterruptTimeBehaviour : AEPlayableBehaviour
    {
        private StandardClip Clip;

        public AEActionInterruptTimeBehaviour(StandardClip clip) : base(clip)
        {
            Clip = clip;
        }

        public override void OnExit(GameObject context, int fps, int currentFrameID)
        {
            base.OnExit(context, fps, currentFrameID);
            var actionSystem = context.GetComponent<ActionSystem>();
            actionSystem.CanBeInterrupt = true;
            // Debug.Log("CanBeInterrupt");
        }

        public override void OnEnter(GameObject context,int fps, int currentFrameID)
        {
            base.OnEnter(context,fps, currentFrameID);
            // var actionSystem = context.GetComponent<ActionSystem>();
            // actionSystem.WhenCanBeInterrupt = (float)Clip.Duration / fps;
            var actionSystem = context.GetComponent<ActionSystem>();
            actionSystem.CanBeInterrupt = false;
            // Debug.Log(actionSystem.WhenCanBeInterrupt);
        }
    }
}