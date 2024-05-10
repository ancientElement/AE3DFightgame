using System;
using UnityEngine.Events;

namespace AEActionSystem
{
    [Serializable]
    public class AEActionConfigTrigger
    {
        public ActionConfig Config;
        public UnityEvent<ActionConfig> CustomAction; //落到地面
    }
}