using System;
using AE_SkillEditor_Plus.RunTime;
using AE_SkillEditor_Plus.RunTime.Driver;
using UnityEngine;
using AEActionSystem;
using Attack;

namespace AE_SkillEditor_Plus_Custom.Track
{
    public class AttackDetectionRuntimeBehaviour : AEPlayableBehaviour
    {
        private StandardClip Clip;

        public AttackDetectionRuntimeBehaviour(StandardClip clip) : base(clip)
        {
            Clip = clip;
        }

        public override void OnExit(GameObject context, int fps, int currentFrameID)
        {
            base.OnExit(context, fps, currentFrameID);
            var detection = context.GetComponent<AttackDetectionManager>();
            detection.CloseDetection(Clip.Name);
        }

        public override void OnEnter(GameObject context, int fps, int currentFrameID)
        {
            base.OnEnter(context, fps, currentFrameID);
            var detection = context.GetComponent<AttackDetectionManager>();
            detection.OpenDetection(Clip.Name);
        }
    }
}