using System;
using AE_SkillEditor_Plus.RunTime;
using AE_SkillEditor_Plus.RunTime.Driver;
using UnityEngine;
using AEActionSystem;
using Attack;

namespace AE_SkillEditor_Plus_Custom.Track
{
    public class AttackDetectionEditorBehaviour : AEPlayableBehaviour
    {
        private StandardClip Clip;

        public AttackDetectionEditorBehaviour(StandardClip clip) : base(clip)
        {
            Clip = clip;
        }

        public override void OnExit(GameObject context, int fps, int currentFrameID)
        {
            base.OnExit(context, fps, currentFrameID);
            if (context == null) return;
            var detection = context.GetComponent<AttackDetectionManager>();
            detection.CloseDebug(Clip.Name);
        }

        public override void OnEnter(GameObject context, int fps, int currentFrameID)
        {
            base.OnEnter(context, fps, currentFrameID);
            if (context == null) return;
            var detection = context.GetComponent<AttackDetectionManager>();
            detection.OpenDebug(Clip.Name);
        }
    }
}