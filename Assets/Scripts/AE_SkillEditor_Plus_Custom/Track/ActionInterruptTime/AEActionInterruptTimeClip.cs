using System;
using System.ComponentModel;
using AE_SkillEditor_Plus_Custom.Inspector;
using AE_SkillEditor_Plus.Example.BuiltTracks;
using AE_SkillEditor_Plus.RunTime;
using UnityEngine;

namespace AE_SkillEditor_Plus_Custom.Animation.Track
{
    /// <summary>
    /// 一般从第0帧开始,在Duration后可以被打断
    /// </summary>
    [Serializable]
    [AEToolTip(Tip = "一般从第0帧开始,在Duration后可以被打断,Duration最小为2")]
    public class AEActionInterruptTimeClip : StandardClip
    {
    }
}