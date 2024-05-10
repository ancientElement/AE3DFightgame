using System;
using UnityEngine;

namespace Test
{
    public class TestRaycast : MonoBehaviour
    {
        public float Length;
        
        private void FixedUpdate()
        {
            int enemyLayerMask = LayerMask.GetMask("Enemy");
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, Length, enemyLayerMask, QueryTriggerInteraction.Collide))
            {
                Debug.Log("击中了!!!" + hitInfo.collider.gameObject.name);
            }
        }

        private void OnDrawGizmos()
        {
            Debug.DrawLine(transform.position, transform.position + transform.forward * Length, Color.red);
        }
    }
}