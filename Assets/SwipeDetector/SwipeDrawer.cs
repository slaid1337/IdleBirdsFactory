using UnityEngine;
using UnityEditor;

public class SwipeDrawer : MonoBehaviour
{
    

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(gameObject.transform.position, 80.0f);
    }
}