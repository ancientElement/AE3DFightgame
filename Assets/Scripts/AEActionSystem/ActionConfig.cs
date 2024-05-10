using System.Collections.Generic;
using AE_SkillEditor_Plus.RunTime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace AEActionSystem
{
    [CreateAssetMenu(menuName = "动作系统/ActionConfig")]
    public class ActionConfig : AETimelineAsset
    {
        // public bool AnimatorState; //单独动画还是状态
        // public AnimationClip AnimationClip; //动画名称
        // public string StateName; //如果是状态请填写状态名
        // public float AnimFadeIn; //动画进入时间
        // // public TagsEnum Tag; //标签
        public List<ActionConfig> InterruptOtherTagList; //可以打断哪些标签List
        // public float WhenCanBeInterrupt; //到哪一个时间点才可以被打断
        // public float ActionDuration; //动作总时长

        public int Priority; //优先等级

        //public List<TagsEnum> ImportanctThenWhichTagList; //优先于哪些Tag
        public List<string> WhichKeyToInterval; //哪些按键触发

        public ActionConfig NextNaltureAction; //下一个自然动作
        public MotionInfo MotionInfo;
    }
}