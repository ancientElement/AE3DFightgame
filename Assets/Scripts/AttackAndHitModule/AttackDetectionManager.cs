using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Attack
{
    public class AttackDetectionManager : MonoBehaviour
    {
        public List<AttackDetection> AttackDetections;
        private Dictionary<string, int> Map;
        public LayerMask DetectionLayerMask;
        public AttackDetection GetAttackDetections(string name) => AttackDetections[Map[name]];

        public void Init()
        {
            Map = new Dictionary<string, int>(AttackDetections.Count);
            for (int i = 0; i < AttackDetections.Count; i++)
            {
                AttackDetections[i].Init(DetectionLayerMask);
                Map.Add(AttackDetections[i].Name, i);
            }
        }

        public void OpenDetection(string name)
        {
            GetAttackDetections(name).CanDetection = true;
        }

        public void CloseDetection(string name)
        {
            GetAttackDetections(name).CanDetection = false;
        }

        public void Tick()
        {
            for (int i = 0; i < AttackDetections.Count; i++)
            {
                AttackDetections[i].Tick(Time.deltaTime);
            }
        }

#if UNITY_EDITOR
        public bool DebugMode;

        public void OpenDebug(string name)
        {
            var attack = AttackDetections.Find(x => x.Name == name);
            if (attack == null)
            {
                Debug.LogWarning("不存在攻击部位: " + name);
                return;
            }

            attack.DebugMode = true;
        }

        public void CloseDebug(string name)
        {
            var attack = AttackDetections.Find(x => x.Name == name);
            if (attack == null)
            {
                Debug.LogWarning("不存在攻击部位: " + name);
                return;
            }

            attack.DebugMode = false;
        }

        private void OnDrawGizmos()
        {
            if (AttackDetections == null) return;
            for (int i = 0; i < AttackDetections.Count; i++)
            {
                if (Application.isPlaying && DebugMode) AttackDetections[i].DebugMode = true;
                else if (Application.isPlaying && !DebugMode) AttackDetections[i].DebugMode = false;
                AttackDetections[i].OnDrawGizmos();
            }
        }
#endif
    }
}