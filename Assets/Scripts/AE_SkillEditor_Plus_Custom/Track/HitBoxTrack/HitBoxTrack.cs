using System;
using AE_SkillEditor_Plus.Example.BuiltTracks;
using AE_SkillEditor_Plus.Example.BuiltTracks.Effect;
using AE_SkillEditor_Plus.RunTime;
using AE_SkillEditor_Plus.RunTime.Attribute;
using AE_SkillEditor_Plus.RunTime.Driver;
using AE_SkillEditor_Plus.RunTime.Interface;

namespace AE_SkillEditor_Plus_Custom.Track
{
    [AETrackName(Name = "受击关闭")]
    [AETrackColor(176f/255, 67f/255, 65f/255)]
    [AEBindClip(ClipType = typeof(HitBoxClip))]
    [Serializable]
    public class HitBoxTrackTrack : StandardTrack, IRuntimeBehaviour, IEditorBehaviour
    {
        public AEPlayableBehaviour CreateRuntimeBehaviour(StandardClip clip) =>
            new HitBoxRuntimeBehaviour(clip);

        public AEPlayableBehaviour CreateEditorBehaviour(StandardClip clip) => new HitBoxEditorBehaviour(clip);
    }
}