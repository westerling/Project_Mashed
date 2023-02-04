using UnityEngine;

[System.Serializable]

public class Stats : MonoBehaviour
{

    [SerializeField]
    private string m_ModelName;

    [Range(50, 200), SerializeField]
    private int m_Speed;

    [Range(10, 120), SerializeField]
    private int m_ReverseSpeed;

    [Range(1, 100), SerializeField]
    private int m_Acceleration;

    [Space(10)]
    [Range(10, 45), SerializeField]
    private int m_SteeringAngle;

    [Range(0.1f, 1f), SerializeField]
    private float m_SteeringSpeed;

    [Space(10)]
    [Range(100, 600), SerializeField]
    private int m_BrakeForce;

    [Range(1, 10), SerializeField]
    private int m_Deceleration;

    [Range(1, 10), SerializeField]
    private int m_HandbrakeGrip;

    public int Speed { get => m_Speed; set => m_Speed = value; }
    public int ReverseSpeed { get => m_ReverseSpeed; set => m_ReverseSpeed = value; }
    public int Acceleration { get => m_Acceleration; set => m_Acceleration = value; }
    public int SteeringAngle { get => m_SteeringAngle; set => m_SteeringAngle = value; }
    public float SteeringSpeed { get => m_SteeringSpeed; set => m_SteeringSpeed = value; }
    public int BrakeForce { get => m_BrakeForce; set => m_BrakeForce = value; }
    public int Deceleration { get => m_Deceleration; set => m_Deceleration = value; }
    public int HandbrakeGrip { get => m_HandbrakeGrip; set => m_HandbrakeGrip = value; }
    public string ModelName { get => m_ModelName; }

}
