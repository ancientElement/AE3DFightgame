using System;
using System.Collections.Generic;
using AEActionSystem;
using UnityEngine;

namespace Attack
{
    public class HitBoxManager : MonoBehaviour
    {
        public List<HitBox> HitBoxes;
        private Dictionary<string, int> Map;
        public LayerMask HitBoxLayerMask;

        public void OpenDetection(string name)
        {
            GetHitBox(name).CanHit = true;
        }

        public void CloseDetection(string name)
        {
            GetHitBox(name).CanHit = false;
        }

        public void Init()
        {
         
            Map = new Dictionary<string, int>(HitBoxes.Count);
            for (int i = 0; i < HitBoxes.Count; i++)
            {
                HitBoxes[i].Init(this,HitBoxLayerMask,gameObject);
                Map.Add(HitBoxes[i].Name, i);
            }
        }

        //TODO: 测试
        public AEActionConfigTrigger HitForwardWeak;
        
        // public void Tick()
        // {
        //     for (int i = 0; i < HitBoxes.Count; i++)
        //     {
        //         HitBoxes[i].Tick(gameObject);
        //     }
        // }

        public void GetDamage(string part)
        {
            //todo: 根据受击位置播放受击动画
            Debug.Log(part);
            HitForwardWeak.CustomAction?.Invoke(HitForwardWeak.Config);
        }
        
        public HitBox GetHitBox(string name) => HitBoxes[Map[name]];

#if UNITY_EDITOR
        public bool DebugMode;

        public void OpenDebug(string name)
        {
            var attack = HitBoxes.Find(x => x.Name == name);
            if (attack == null)
            {
                Debug.LogWarning("不存在受击盒子: " + name);
                return;
            }

            attack.DebugMode = true;
        }

        public void CloseDebug(string name)
        {
            var attack = HitBoxes.Find(x => x.Name == name);
            if (attack == null)
            {
                Debug.LogWarning("不存在受击盒子: " + name);
                return;
            }

            attack.DebugMode = false;
        }

        private void OnDrawGizmos()
        {
            if (HitBoxes == null) return;
            for (int i = 0; i < HitBoxes.Count; i++)
            {
                if (DebugMode) HitBoxes[i].DebugMode = true;
                else  HitBoxes[i].DebugMode = false;
                HitBoxes[i].OnDrawGizmos();
            }
        }

#endif
    }
}