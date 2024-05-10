using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AE_SkillEditor_Plus.RunTime;
using AE_SkillEditor_Plus.RunTime.Driver;
using Attack;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace AEActionSystem
{
    public class ActionSystem : MonoBehaviour
    {
        [SerializeField] private List<ActionConfig> allAction; //所有的动作
        [SerializeField] private int StartAction; //初始的动作
        [SerializeField] private bool RotateToEnemy; //动作发生时 面向敌人

        [FormerlySerializedAs("RotateToCamera")] [SerializeField]
        private bool RotateToCameraForwar; //动作发生时 面向相机

        private IThirdPersonrController _thirdPersonrController;
        private ActionConfig CurrentAction; //当前的动作
        private List<ActionConfig> PredictList; //备选动作列表
        public bool CanBeInterrupt { get; set; } //可以被中断

        private float Timer;
        private Animator Animator;

        private List<string> stateList;

        // public float WhenCanBeInterrupt { get; set; } //何时可被打断
        private AETimelineTick TimelineTick;
        private AttackDetectionManager attackDetectionManager;
        private HitBoxManager hitBoxManager;

        private void Start()
        {
            TimelineTick = new AETimelineTick();
            Animator = GetComponent<Animator>();
            PredictList = new List<ActionConfig>();

            ThirdPersonController tempThirdPersonController;
            if (TryGetComponent<ThirdPersonController>(out tempThirdPersonController))
            {
                _thirdPersonrController = tempThirdPersonController;
            }
            else
            {
                VirtualThirdPersonController tempVirtualThirdPersonController;
                TryGetComponent<VirtualThirdPersonController>(out tempVirtualThirdPersonController);
                _thirdPersonrController = tempVirtualThirdPersonController;
            }
            
            _thirdPersonrController.Init();
            EnterAction(allAction.ElementAt(StartAction));

            hitBoxManager = GetComponent<HitBoxManager>();
            hitBoxManager.Init();
            attackDetectionManager = GetComponent<AttackDetectionManager>();
            attackDetectionManager.Init();

            // allAction = new Dictionary<TagsEnum, ActionConfig>();
            // foreach (var actionConfig in AllAction)
            // {
            //     allAction.Add(actionConfig.Tag, actionConfig);
            // }
        }

        private void FixedUpdate()
        {
            // hitBoxManager.Tick();
            attackDetectionManager.Tick();
        }

        private void Update()
        {
            Tick(Time.deltaTime);
            _thirdPersonrController.OnUpdate(Time.deltaTime);
            if (CurrentAction == null) return;
            TimelineTick.Tick(Time.deltaTime, gameObject);
            var motion = CurrentAction.MotionInfo;
            //允许使用方向键控制人物
            if (motion.ControlMove)
            {
                _thirdPersonrController.ContorlMoveRotate(Time.deltaTime, motion);
            }
            else
            {
                motion.RotateToCameraForward = RotateToCameraForwar;
                _thirdPersonrController.ActionMoveRotate(Time.deltaTime, motion);
            }
        }

        private void LateUpdate()
        {
            _thirdPersonrController.OnLateUpdate(Time.deltaTime);
        }

        private void OnAnimatorMove()
        {
            if (CurrentAction == null) return;
            CurrentAction.MotionInfo.RMVelocity = Animator.velocity;
        }

        //进入动作
        public void EnterAction(ActionConfig action)
        {
            // Debug.Log(action);
            CanBeInterrupt = true;
            Timer = 0;
            PredictList.Clear();
            CurrentAction = action;
            //TODO: 处理动画
            // Debug.Log((float)CurrentAction.Duration / CurrentAction.FPS + " " + CurrentAction.name);
            TimelineTick.PlayAsset(CurrentAction, gameObject);
            // if (CurrentAction.AnimatorState)
            // {
            //     Animator.CrossFade(CurrentAction.StateName, CurrentAction.AnimFadeIn);
            // }
            // else
            // {
            //     Animator.CrossFade(CurrentAction.AnimationClip.name, CurrentAction.AnimFadeIn);
            // }
            _thirdPersonrController.EnterAction();
        }

        //加入备选列表
        public void AddToPredict(ActionConfig action)
        {
            PredictList.Add(action);
        }

        //更新动作
        public void Tick(float delta)
        {
            if (CurrentAction == null) return;
            Timer += delta;

            // if (Timer >= WhenCanBeInterrupt)
            // {
            //     // Debug.Log("能够切换动作了");
            //     CanBeInterrupt = true;
            if (CanBeInterrupt)
                if (PredictList.Count > 0) //动作预测表Count是否大于0
                {
                    ChooseNextAction();
                }

            if (Timer >= (float)CurrentAction.Duration / CurrentAction.FPS) //动作是否结束
            {
                if (CurrentAction.NextNaltureAction != null) //是否有下一个自然动作
                {
                    EnterAction(CurrentAction.NextNaltureAction);
                }
            }
            // }
            // else
            // {
            //     CanBeInterrupt = false;
            // }
        }


        //选择优先级最高的动作进入
        public void ChooseNextAction()
        {
            PredictList.Sort((ActionConfig a, ActionConfig b) => { return a.Priority - b.Priority; });
            EnterAction(PredictList[0]);
        }

        //处理输入
        public void ProcessInput(string actionName)
        {
            foreach (var item in allAction)
            {
                foreach (var key in item.WhichKeyToInterval)
                {
                    // Debug.Log(key + "--" + item.Key);
                    if (key == actionName)
                    {
                        // Debug.Log(key);
                        //判断是否可以打断当前动作
                        if (item.InterruptOtherTagList.Contains(CurrentAction))
                        {
                            //加入
                            // Debug.Log(item);
                            PredictList.Add(item);
                        }
                    }
                }
            }
        }
    }
}