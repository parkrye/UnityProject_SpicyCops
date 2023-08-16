using UnityEngine;

namespace Im
{
    public class Test : MonoBehaviour
    {
        [SerializeField]Vector3 center;
        [SerializeField]Vector3 size;
        void MyCollisions()
        {
            Physics.OverlapBox(center + transform.position, size / 2, Quaternion.identity);

        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(center + transform.position, size);
        }
    }
}