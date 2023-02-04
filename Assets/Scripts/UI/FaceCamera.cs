using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [SerializeField]
    private Camera m_Camera;

    private void LateUpdate()
    {
        transform.LookAt(m_Camera.transform.position, Vector3.up);
    }
}
