#define Test
using System;
using UnityEngine;

namespace Attack
{
    [Serializable]
    public class HitBox
    {
        public string Name;
        public Vector3 Box;
        public Vector3 Offset;
        public Transform TargetTransform;
        private bool canHit = true;

        public bool CanHit
        {
            get { return canHit; }
            set
            {
                if (value) Collider.enabled = true;
                else Collider.enabled = false;
                canHit = value;
            }
        }

        private BoxCollider Collider;

        public void Init(HitBoxManager manager, LayerMask layerMask, GameObject context)
        {
            Collider = TargetTransform.gameObject.AddComponent<BoxCollider>();
            var ideltity = TargetTransform.gameObject.AddComponent<HitBoxIdentity>();
            ideltity.Manager = manager;
            ideltity.Name = Name;
            // 遍历每一个层级
            for (int i = 0; i < 32; i++)
            {
                if (1 << i == layerMask.value)
                {
                    TargetTransform.gameObject.layer = i;
                }
            }
            Collider.isTrigger = true;
            Collider.center = Offset;
            Collider.size = Box;
        }

        // public void Tick(GameObject context)
        // {
        //     if (CanHit && Collider != null) Collider.center = context.transform.InverseTransformPoint(center);
        // }
        
#if UNITY_EDITOR
        public bool DebugMode;
        public void OnDrawGizmos()
        {
            if (Application.isPlaying && CanHit && DebugMode && TargetTransform != null)
            {
                Gizmos.color = new Color(0, 0, 1, 0.5f);
                Gizmos.matrix = TargetTransform.localToWorldMatrix;
                Gizmos.DrawCube(Offset, Box);
            }

            if (!Application.isPlaying && DebugMode && TargetTransform != null)
            {
                Gizmos.color = new Color(0, 0, 1, 0.5f);
                Gizmos.matrix = TargetTransform.localToWorldMatrix;
                Gizmos.DrawCube(Offset, Box);
            }
        }
#endif
    }
}