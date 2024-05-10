using System;
using AE_SkillEditor_Plus.Example.BuiltTracks;
using AE_SkillEditor_Plus.RunTime;
using UnityEngine;

namespace AE_SkillEditor_Plus_Custom.Animation.Track
{
    [Serializable]
    public class AEActionAnimClip : AEAnimationClip
    {
        public bool isAnimatorState;
        public string StateName;
        public string AnimationName => AnimationClip.name;
    }
}