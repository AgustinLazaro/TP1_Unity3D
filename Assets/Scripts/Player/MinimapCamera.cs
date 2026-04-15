using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}