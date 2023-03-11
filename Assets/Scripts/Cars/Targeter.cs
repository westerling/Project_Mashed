using UnityEngine;

public class Targeter : MonoBehaviour
{

    [SerializeField]
    private LineRenderer m_LineRenderer;

    [SerializeField]
    private bool m_Active;

    [SerializeField]
    private Transform m_StartPos;

    private GameObject m_Target;

    private enum m_Materials
    {
        NonTarget = 0,
        Target = 1
    }

    public bool Active 
    {
        get => m_Active; 
        set => m_Active = value;
    }

    public GameObject Target 
    {
        get => m_Target; 
        set => m_Target = value; 
    }

    void Update()
    {
        SetVisualTargeter();
    }

    private void SetVisualTargeter()
    {
        m_LineRenderer.SetPosition(0, m_StartPos.position);

        if (Physics.Raycast(transform.position, transform.forward, out var hit, Globals.CarLayerMask))
        {
            if (hit.collider)
            {
                if (hit.transform.TryGetComponent(out Car car))
                {
                    Debug.Log("hit car");
                    m_LineRenderer.SetPosition(1, hit.point);
                    Target = car.gameObject;
                    m_LineRenderer.material = m_LineRenderer.materials[(int)m_Materials.Target];
                    return;
                }
            }
        }

        SetDeadEndPosition();
    }

    private void SetDeadEndPosition()
    {
        m_LineRenderer.SetPosition(1, m_StartPos.transform.position + transform.forward * 10);
        m_LineRenderer.material = m_LineRenderer.materials[(int)m_Materials.NonTarget];
    }
}
