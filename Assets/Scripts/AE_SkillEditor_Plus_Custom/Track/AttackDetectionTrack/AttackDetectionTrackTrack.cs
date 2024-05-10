using System;
using AE_SkillEditor_Plus.Example.BuiltTracks;
using AE_SkillEditor_Plus.Example.BuiltTracks.Effect;
using AE_SkillEditor_Plus.RunTime;
using AE_SkillEditor_Plus.RunTime.Attribute;
using AE_SkillEditor_Plus.RunTime.Driver;
using AE_SkillEditor_Plus.RunTime.Interface;

namespace AE_SkillEditor_Plus_Custom.Track
{
    [AETrackName(Name = "攻击检测开启")]
    [AETrackColor(170f / 255, 117f / 255, 159f / 255)]
    [AEBindClip(ClipType = typeof(AttackDetectionClip))]
    [Serializable]
    public class AttackDetectionTrackTrack : StandardTrack, IRuntimeBehaviour, IEditorBehaviour
    {
        public AEPlayableBehaviour CreateRuntimeBehaviour(StandardClip clip) =>
            new AttackDetectionRuntimeBehaviour(clip);

        public AEPlayableBehaviour CreateEditorBehaviour(StandardClip clip) => new AttackDetectionEditorBehaviour(clip);
    }
}