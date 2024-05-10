using System;
using Attack;
using UnityEngine;

namespace Test
{
    public class TestHitBox: MonoBehaviour
    {
        private HitBoxManager hitBoxManager;

        private void Start()
        {
            hitBoxManager = GetComponent<HitBoxManager>();
            hitBoxManager.Init();
        }

        private void Update()
        {
            // hitBoxManager.Tick();
        }
    }
}