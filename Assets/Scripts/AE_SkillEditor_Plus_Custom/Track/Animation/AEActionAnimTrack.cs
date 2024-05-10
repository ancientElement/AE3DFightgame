using System;
using AE_SkillEditor_Plus.Example.BuiltTracks;
using AE_SkillEditor_Plus.RunTime;
using AE_SkillEditor_Plus.RunTime.Attribute;
using AE_SkillEditor_Plus.RunTime.Driver;
using AE_SkillEditor_Plus.RunTime.Interface;

namespace AE_SkillEditor_Plus_Custom.Animation.Track
{
    [AETrackName(Name = "自定义动作动画")]
    [AETrackColor(127f / 255, 252f / 255, 228f / 255)]
    [AEBindClip(ClipType = typeof(AEActionAnimClip))]
    [AEClipStyle(ClassName = "AE_SkillEditor_Plus.Example.BuiltTracks.CustomAnimationClip")]
    [Serializable]
    public class AEActionAnimTrack : StandardTrack,IRuntimeBehaviour, IEditorBehaviour
    {
        public AEPlayableBehaviour CreateRuntimeBehaviour(StandardClip clip) => new AEActionAnimRuntimeBehaviour(clip);

        public AEPlayableBehaviour CreateEditorBehaviour(StandardClip clip) => new AEAnimationEditorBehaviour(clip);
    }
}