using System;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace AEActionSystem
{
    [Serializable]
    public class MotionInfo
    {
        public bool ControlMove; //角色是否可以被控制移动
        public bool ApplayRotate; //允许角色在动作中旋转

        public float MoveSpeedRate; //角色可以被控制移动时的移动倍率

        // public bool RotateToSpecialDiraction; //角色转向特殊方向
        public bool UseRootMotionMove; //使用RootMotion移动
        public bool UseRootMotionRotate; //使用RootMoton旋转
        [HideInInspector] public Vector3 RMVelocity;
        [HideInInspector] public float DesForwordDirection; //目标旋转方向
        [HideInInspector] public bool RotateToCameraForward; //角色转向镜头前方
    }
}