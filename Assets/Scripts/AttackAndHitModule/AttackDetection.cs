#define Test
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Attack
{
    public enum AttackDetectionType
    {
        Ray,
        Box,
        Sphere
    }

    [Serializable]
    public class AttackDetectionParams
    {
        [Header("射线检测")] //射线检测
        public Vector3 RayDirection;

        public int RayDetectionDensity;

        public float RayLength;

        [Header("Box检测")] //box
        public Vector3 BoxCenter;

        public Vector3 BoxHalfExtents;

        [Header("Sphere检测")] //sphere
        public Vector3 SpherePosition;

        public float SphereRadius;
    }

    [Serializable]
    public class AttackDetection
    {
        public string Name;
        public Transform TargetTransform;
        private LayerMask HitBoxLayer;
        private bool canDetection;

        public bool CanDetection
        {
            get { return canDetection; }
            set
            {
                canDetection = value;
                if (value)
                {
                    Detected.Clear();
                }
            }
        }

        public AttackDetectionType Type;
        public AttackDetectionParams Params;

//射线检测
        // public Vector3 Direction;
        // public int DetectionDensity;
        // public float Length;

        private Vector3 prePosition;

        private Vector3 lastDiraction; //上一次方向

        private List<HitBoxManager> Detected; //检测过的物体
//
// //box
//         public Vector3 center;
//         public Vector3 halfExtents;
//
// // or sphere
//         public Vector3 position;
//         public float radius;

        public void Init(LayerMask layerMask)
        {
            HitBoxLayer = layerMask;
            Detected = new List<HitBoxManager>();
        }

        public void Tick(float delta)
        {
            if (CanDetection)
            {
                switch (Type)
                {
                    case AttackDetectionType.Ray:
                        RayDetection();
                        break;
                    case AttackDetectionType.Box:
                        BoxDetection();
                        break;
                    case AttackDetectionType.Sphere:
                        SphereDetection();
                        break;
                }
            }

            prePosition = TargetTransform.position;
        }

        private void SphereDetection()
        {
            var collider = Physics.OverlapSphere(TargetTransform.TransformPoint(Params.SpherePosition),
                Params.SphereRadius, HitBoxLayer);
            for (int i = 0; i < collider.Length; i++)
            {
                HitCollider(collider[i]);
            }
        }

        private void BoxDetection()
        {
            var collider = Physics.OverlapBox(TargetTransform.TransformPoint(Params.BoxCenter),
                Params.BoxHalfExtents, TargetTransform.rotation, HitBoxLayer);
            for (int i = 0; i < collider.Length; i++)
            {
                HitCollider(collider[i]);
            }
        }

        private void RayDetection()
        {
            if (prePosition != Vector3.zero)
            {
                var temp = Params.RayLength / Params.RayDetectionDensity;
                for (int i = 0; i < Params.RayDetectionDensity; i++)
                {
                    var des = TargetTransform.position +
                              TargetTransform.TransformDirection(Params.RayDirection) * temp * i;
                    var desPre = prePosition + lastDiraction * temp * i;
                    if (Physics.Raycast(desPre, des - desPre, out RaycastHit hitInfo,
                            (des - desPre).magnitude,
                            HitBoxLayer))
                    {
                        HitCollider(hitInfo.collider);
                    }
                }
            }

            lastDiraction = TargetTransform.TransformDirection(Params.RayDirection);
        }

        private void HitCollider(Collider collider)
        {
            // if (collider == null) return;
            var ideltity = collider.GetComponent<HitBoxIdentity>();
            var manager = ideltity.Manager;
            if (Detected.Count > 0 && Detected[Detected.Count - 1] == manager) return;
            Detected.Add(manager);
            // Debug.Log(collider.gameObject.name, collider.gameObject);
            manager.GetDamage(ideltity.Name);
        }

#if UNITY_EDITOR
        public bool DebugMode;

        public void OnDrawGizmos()
        {
            if (DebugMode && TargetTransform != null)
            {
                if (Application.isPlaying && CanDetection || !Application.isPlaying)
                {
                    switch (Type)
                    {
                        case AttackDetectionType.Ray:
                            GizomRay();
                            break;
                        case AttackDetectionType.Box:
                            GizomBox();
                            break;
                        case AttackDetectionType.Sphere:
                            GizomSphere();
                            break;
                    }
                }
            }

            if (DebugMode && !Application.isPlaying && TargetTransform != null && Type == AttackDetectionType.Ray)
            {
                prePosition = TargetTransform.position;
                lastDiraction = TargetTransform.TransformDirection(Params.RayDirection);
            }
        }

        private void GizomSphere()
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.matrix = TargetTransform.localToWorldMatrix;
            Gizmos.DrawWireSphere(Params.SpherePosition, Params.SphereRadius);
        }

        private void GizomBox()
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.matrix = TargetTransform.localToWorldMatrix;
            Gizmos.DrawCube(Params.BoxCenter, Params.BoxHalfExtents);
        }

        private void GizomRay()
        {
            // if (Application.isPlaying && CanDetection && DebugMode && TargetTransform != null)
            // {
            var temp = Params.RayLength / Params.RayDetectionDensity;
            for (int i = 0; i < Params.RayDetectionDensity; i++)
            {
                var des = TargetTransform.position +
                          TargetTransform.TransformDirection(Params.RayDirection) * temp * i;
                var desPre = prePosition + lastDiraction * temp * i;
                Debug.DrawLine(desPre, des, Color.red, 5f);
            }
            // else if (!Application.isPlaying && DebugMode && TargetTransform != null)
            // {
            //     if (prePosition != Vector3.zero)
            //     {
            //         var temp = Params.RayLength / Params.RayDetectionDensity;
            //         for (int i = 0; i < Params.RayDetectionDensity; i++)
            //         {
            //             var des = TargetTransform.position +
            //                       TargetTransform.TransformDirection(Params.RayDirection) * temp * i;
            //             var desPre = prePosition + lastDiraction * temp * i;
            //             Debug.DrawLine(desPre, des, Color.red, 5f);
            //         }
            //     }
            //
            //     prePosition = TargetTransform.position;
            //     lastDiraction = TargetTransform.TransformDirection(Params.RayDirection);
            // }
        }
    }
#endif
}