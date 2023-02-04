using System;
using UnityEngine;

[Serializable]
public struct NewWheelCollidersConfig
{
    [Header("General")]
	[SerializeField]
	private float m_Mass;

	[SerializeField]
	private float m_Radius;

	[SerializeField]
	private float m_WheelDampingRate;

	[SerializeField]
	private float m_SuspensionDistance;

	[SerializeField]
	private float m_ForceAppPointDistance;

	[SerializeField]
	private Vector3 m_Center;

    [Header("Suspension")]
	[Range(0, 1), SerializeField]
	private float m_Spring;

	[Range(0, 1), SerializeField]
	private float m_Damper;

	[SerializeField]
	private float m_TargetPoint;

    [Header("Physics")]
	[Range(0, 1), SerializeField]
	private float m_ForwardFriction;

	[Range(0, 1), SerializeField]
	private float m_SidewaysFriction;

    public float Mass { get => m_Mass; set => m_Mass = value; }
    public float Radius { get => m_Radius; set => m_Radius = value; }
    public float WheelDampingRate { get => m_WheelDampingRate; set => m_WheelDampingRate = value; }
    public float SuspensionDistance { get => m_SuspensionDistance; set => m_SuspensionDistance = value; }
    public float ForceAppPointDistance { get => m_ForceAppPointDistance; set => m_ForceAppPointDistance = value; }
    public Vector3 Center { get => m_Center; set => m_Center = value; }
    public float Spring { get => m_Spring; set => m_Spring = value; }
    public float Damper { get => m_Damper; set => m_Damper = value; }
    public float TargetPoint { get => m_TargetPoint; set => m_TargetPoint = value; }
    public float ForwardFriction { get => m_ForwardFriction; set => m_ForwardFriction = value; }
    public float SidewaysFriction { get => m_SidewaysFriction; set => m_SidewaysFriction = value; }
}
