using System;
using System.Reflection;
using AE_SkillEditor_Plus_Custom.Animation.Track;
using AE_SkillEditor_Plus.RunTime;
using UnityEditor;
using UnityEngine;

namespace AE_SkillEditor_Plus_Custom.Inspector
{
    // [CustomEditor(typeof(ScriptableObject), true)]
    public class AEClipToolTipInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var scriptType = target.GetType();
            var attribute = scriptType.GetCustomAttribute<AEToolTipAttribute>();
            if (attribute != null) EditorGUILayout.HelpBox(attribute.Tip, MessageType.Info);
        }
    }
}