using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2.5f, -5f);

    void LateUpdate()
    {
        Debug.Log("Camera following: " + target.position + " Camera at: " + transform.position);
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.LookAt(target.position + Vector3.up);
        }
    }
}