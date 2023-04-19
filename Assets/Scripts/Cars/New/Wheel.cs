using System;
using UnityEngine;

[Serializable]
public struct Wheel
{
	[SerializeField]
	private WheelCollider m_WheelCollider;

	[SerializeField]
	private Transform m_WheelGraphics;

	[SerializeField]
	private float m_SlipForGenerateParticle;

    [SerializeField]
	private Vector3 m_TrailOffset;

	[SerializeField]
	private bool m_Driving;

	private float m_CurrentForwardSleep;
	private float m_CurrentSidewaysSleep;
	private WheelHit m_Hit;
	private TrailRenderer Trail;

	[SerializeField]
	private NewWheelCollider m_NewWheelCollider;

	public NewWheelCollider NewWheelCollider
	{
		get
		{
			if (m_NewWheelCollider == null)
			{
				m_NewWheelCollider = WheelCollider.GetComponent<NewWheelCollider>();
			}
			if (m_NewWheelCollider == null)
			{
				m_NewWheelCollider = WheelCollider.gameObject.AddComponent<NewWheelCollider>();
				m_NewWheelCollider.CheckFirstEnable();
			}
			return m_NewWheelCollider;
		}
	}

	FXController FXController { get { return FXController.Instance; } }

    public WheelCollider WheelCollider 
	{
		get => m_WheelCollider; 
		set => m_WheelCollider = value;
	}

    public float CurrentForwardSleep 
	{
		get => m_CurrentForwardSleep; 
		set => m_CurrentForwardSleep = value; 
	}
    
	public bool Driving 
	{
		get => m_Driving; 
	}

    Vector3 HitPoint;

	const int SmoothValuesCount = 3;

	/// <summary>
	/// Update gameplay logic.
	/// </summary>
	public void FixedUpdate()
	{

		if (WheelCollider.GetGroundHit(out m_Hit))
		{
			var prevForwar = CurrentForwardSleep;
			var prevSide = m_CurrentSidewaysSleep;

			CurrentForwardSleep = (prevForwar + Mathf.Abs(m_Hit.forwardSlip)) / 2;
			m_CurrentSidewaysSleep = (prevSide + Mathf.Abs(m_Hit.sidewaysSlip)) / 2;
		}
		else
		{
			CurrentForwardSleep = 0;
			m_CurrentSidewaysSleep = 0;
		}
	}

	/// <summary>
	/// Update visual logic (Transform, FX).
	/// </summary>
	public void UpdateVisual()
	{
		UpdateTransform();

		if (WheelCollider.isGrounded && CurrentMaxSlip() > m_SlipForGenerateParticle)
		{
			//Emit particle.
			var particles = FXController.GetAspahaltParticles();
			var point = WheelCollider.transform.position;
			point.y = m_Hit.point.y;
			particles.transform.position = point;
			particles.Emit(1);

			if (Trail == null)
			{
				//Get free or create trail.
				HitPoint = WheelCollider.transform.position;
				HitPoint.y = m_Hit.point.y;
				Trail = FXController.GetTrail(HitPoint);
				Trail.transform.SetParent(WheelCollider.transform);
				Trail.transform.localPosition += m_TrailOffset;
			}
		}
		else if (Trail != null)
		{
			//Set trail as free.
			FXController.SetFreeTrail(Trail);
			Trail = null;
		}
	}

	public void UpdateTransform()
	{
		Vector3 pos;
		Quaternion quat;
		WheelCollider.GetWorldPose(out pos, out quat);
		m_WheelGraphics.position = pos;
		m_WheelGraphics.rotation = quat;
	}

	public void UpdateFrictionConfig(NewWheelCollidersConfig config)
	{
		NewWheelCollider.UpdateConfig(config);
	}

	public float CurrentMaxSlip()
	{
		return Mathf.Max(CurrentForwardSleep, m_CurrentSidewaysSleep);
	}
}
