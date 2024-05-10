using System;
using AE_SkillEditor_Plus.Example.BuiltTracks;
using AE_SkillEditor_Plus.Example.BuiltTracks.Effect;
using AE_SkillEditor_Plus.RunTime;
using AE_SkillEditor_Plus.RunTime.Attribute;
using AE_SkillEditor_Plus.RunTime.Driver;
using AE_SkillEditor_Plus.RunTime.Interface;

namespace AE_SkillEditor_Plus_Custom.Animation.Track
{
    [AETrackName(Name = "何时可被打断")]
    [AETrackColor(239f / 255, 83f / 255, 80f / 255)]
    [AEBindClip(ClipType = typeof(AEActionInterruptTimeClip))]
    [Serializable]
    public class AEActionInterruptTimeTrack : StandardTrack, IRuntimeBehaviour
    {
        public AEPlayableBehaviour CreateRuntimeBehaviour(StandardClip clip) =>
            new AEActionInterruptTimeBehaviour(clip);
    }
}