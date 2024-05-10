namespace AEActionSystem
{
    public interface IThirdPersonrController
    {
        public void Init();
        public void ContorlMoveRotate(float delta, MotionInfo info);
        public void ActionMoveRotate(float delta, MotionInfo info);
        public void EnterAction();
        public void OnLateUpdate(float deltaTime);
        public void OnUpdate(float deltaTime);
    }
}