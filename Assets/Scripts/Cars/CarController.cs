using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
	[SerializeField]
	private Wheel m_FrontLeftWheel;
	
	[SerializeField]
	private Wheel m_FrontRightWheel;
	
	[SerializeField]
	private Wheel m_RearLeftWheel;
	
	[SerializeField]
	private Wheel m_RearRightWheel;
	
	[SerializeField]
	private Transform m_CenterOfMass;
	
	[SerializeField] 
	private List<ParticleSystem> m_BackFireParticles = new List<ParticleSystem>();

	[SerializeField]
	private Car m_Car;

	private float m_CurrentAccelerationInput;
	private float m_CurrentSteerInput;

	private float m_MaxMotorTorque;
	private float m_CurrentSteerAngle;
	private float m_CurrentBrake;
	private float m_HandBrake;
	private float m_VelocityAngle;
	private float m_CurrentMaxSlip;
	private float m_CurrentSpeed;
	private float m_EngineRPM;
	private float[] AllGearsRatio;
	private float m_CutOffTimer;

	private int m_FirstDriveWheel;
	private int m_LastDriveWheel;
	private int m_CurrentGear;

	private bool m_InCutOff;

	private Wheel[] m_Wheels;
	private InputManager m_InputManager;
	
	
    public float VelocityAngle 
	{
		get => m_VelocityAngle;
		set => m_VelocityAngle = value;
	}

    public float CurrentSpeed 
	{
		get => m_CurrentSpeed;
		set => m_CurrentSpeed = value;
	}

    public System.Action BackFireAction;

		private void AddListeners()
	{
		m_InputManager.Acceleration += AccelerationPerformed;
		m_InputManager.HandBrake += HandbrakePerformed;
		m_InputManager.Steer += SteerPerformed;
	}

	private void RemoveListeners()
	{
		m_InputManager.Acceleration -= AccelerationPerformed;
		m_InputManager.HandBrake -= HandbrakePerformed;
		m_InputManager.Steer -= SteerPerformed;
	}

	private void Awake()
    {
        m_Car.Rigidbody.centerOfMass = m_CenterOfMass.localPosition;

        m_InputManager = GetComponentInParent<InputManager>();
		      
		AddWheelsToList();

        switch (m_Car.Config.DriveType)
        {
            case DriveType.AWD:
                m_FirstDriveWheel = 0;
                m_LastDriveWheel = 3;
                break;
            case DriveType.FWD:
                m_FirstDriveWheel = 0;
                m_LastDriveWheel = 1;
                break;
            case DriveType.RWD:
                m_FirstDriveWheel = 2;
                m_LastDriveWheel = 3;
                break;
        }

        m_MaxMotorTorque = m_Car.Config.MaxMotorTorque / (m_LastDriveWheel - m_FirstDriveWheel + 1);

        AllGearsRatio = new float[m_Car.Config.GearsRatio.Length + 2];
        AllGearsRatio[0] = m_Car.Config.ReversGearRatio * m_Car.Config.MainRatio;
        AllGearsRatio[1] = 0;

        for (var i = 0; i < m_Car.Config.GearsRatio.Length; i++)
        {
            AllGearsRatio[i + 2] = m_Car.Config.GearsRatio[i] * m_Car.Config.MainRatio;
        }

        foreach (var particles in m_BackFireParticles)
        {
            BackFireAction += () => particles.Emit(2);
        }
    }

    private void AddWheelsToList()
    {
        m_Wheels = new Wheel[4] {
            m_FrontLeftWheel,
            m_FrontRightWheel,
            m_RearLeftWheel,
            m_RearRightWheel
        };
    }

    private void Start()
    {
		AddListeners();
	}

	private void Update()
	{
		UpdateControls();

		for (int i = 0; i < m_Wheels.Length; i++)
		{
			m_Wheels[i].UpdateVisual();
		}
	}

	private void FixedUpdate()
	{
		CurrentSpeed = m_Car.Rigidbody.velocity.magnitude;

		UpdateSteerAngleLogic();
		UpdateRpmAndTorqueLogic();

		m_CurrentMaxSlip = m_Wheels[0].CurrentMaxSlip();

		if (m_HandBrake > 0)
		{
			ApplyHandbrake();
		}

        foreach (var wheel in m_Wheels)
        {
			if (m_HandBrake <= 0)
			{
				wheel.WheelCollider.brakeTorque = m_CurrentBrake;
			}

			wheel.FixedUpdate();

			if (m_CurrentMaxSlip < wheel.CurrentMaxSlip())
			{
				m_CurrentMaxSlip = wheel.CurrentMaxSlip();
			}
		}
	}

	private void ApplyHandbrake()
    {
		m_RearLeftWheel.WheelCollider.brakeTorque = m_Car.Config.MaxBrakeTorque;
		m_RearRightWheel.WheelCollider.brakeTorque = m_Car.Config.MaxBrakeTorque;
		m_FrontLeftWheel.WheelCollider.brakeTorque = 0;
		m_FrontRightWheel.WheelCollider.brakeTorque = 0;
	}

	private void UpdateControls()
    {
		var targetSteerAngle = m_CurrentSteerInput * m_Car.Config.MaxSteerAngle;

		if (m_Car.Config.EnableSteerAngleMultiplier)
		{
			targetSteerAngle *= Mathf.Clamp(1 - Globals.MsToKph(CurrentSpeed) / m_Car.Config.MaxSpeedForMinAngleMultiplier, m_Car.Config.MinSteerAngleMultiplier, m_Car.Config.MaxSteerAngleMultiplier);
		}

		m_CurrentSteerAngle = Mathf.MoveTowards(m_CurrentSteerAngle, targetSteerAngle, Time.deltaTime * m_Car.Config.SteerAngleChangeSpeed);
	}

	#region Steer help logic

	private	void UpdateSteerAngleLogic()
	{
		var targetAngle = 0f;

		VelocityAngle = -Vector3.SignedAngle(m_Car.Rigidbody.velocity, transform.TransformDirection(Vector3.forward), Vector3.up);

		targetAngle = Mathf.Clamp(targetAngle + m_CurrentSteerAngle, -(m_Car.Config.MaxSteerAngle + 10), m_Car.Config.MaxSteerAngle + 10);

		m_Wheels[0].WheelCollider.steerAngle = targetAngle;
		m_Wheels[1].WheelCollider.steerAngle = targetAngle;
	}

	#endregion

	#region

	private void UpdateRpmAndTorqueLogic()
	{
		if (m_InCutOff)
		{
			if (m_CutOffTimer > 0)	
			{
				m_CutOffTimer -= Time.fixedDeltaTime;
				m_EngineRPM = Mathf.Lerp(m_EngineRPM, GetInCutOffRPM(), m_Car.Config.RpmEngineToRpmWheelsLerpSpeed * Time.fixedDeltaTime);
			}
			else
			{
				m_InCutOff = false;
			}
		}

		if (!RaceManager.Current.RaceIsActive)
		{
			if (m_InCutOff)
            {
				return;
			}

			SetStaticMode();

            var rpm = m_CurrentAccelerationInput > 0 ? m_Car.Config.MaxRPM : m_Car.Config.MinRPM;
			var speed = m_CurrentAccelerationInput > 0 ? m_Car.Config.RpmEngineToRpmWheelsLerpSpeed : m_Car.Config.RpmEngineToRpmWheelsLerpSpeed * 0.2f;
			
			m_EngineRPM = Mathf.Lerp(m_EngineRPM, rpm, speed * Time.fixedDeltaTime);
			
			if (m_EngineRPM >= m_Car.Config.CutOffRPM)
			{
				PlayBackfireWithProbability();
				m_InCutOff = true;
				m_CutOffTimer = m_Car.Config.CutOffTime;
			}
			return;
		}

		//Get drive wheel with MinRPM.
		var minRPM = 0f;

        //for (var i = m_FirstDriveWheel + 1; i <= m_LastDriveWheel; i++)
        //{
        //    minRPM += m_Wheels[i].WheelCollider.rpm;
        //}

        foreach (var wheel in m_Wheels)
		{
			if (wheel.Driving)
			{
                minRPM += wheel.WheelCollider.rpm;
            }
		}

		minRPM /= m_LastDriveWheel - m_FirstDriveWheel + 1;

		if (!m_InCutOff)
		{
            var targetRPM = Mathf.Abs((minRPM + 20) * AllGearsRatio[GetCurrentGear()]);
            targetRPM = Mathf.Clamp(targetRPM, m_Car.Config.MinRPM, m_Car.Config.MaxRPM);
			m_EngineRPM = Mathf.Lerp(m_EngineRPM, targetRPM, m_Car.Config.RpmEngineToRpmWheelsLerpSpeed * Time.fixedDeltaTime);
		}

		if (m_EngineRPM >= m_Car.Config.CutOffRPM)
		{
			PlayBackfireWithProbability();
			m_InCutOff = true;
			m_CutOffTimer = m_Car.Config.CutOffTime;
			return;
		}

		if (!Mathf.Approximately(m_CurrentAccelerationInput, 0))
		{
			//If the direction of the car is the same as Current Acceleration.
			if (GetCurrentCarDirection() * m_CurrentAccelerationInput >= 0)
			{
				m_CurrentBrake = 0;

				var motorTorqueFromRpm = m_Car.Config.MotorTorqueFromRpmCurve.Evaluate(m_EngineRPM * 0.001f);
				var motorTorque = m_CurrentAccelerationInput * (motorTorqueFromRpm * (m_MaxMotorTorque * AllGearsRatio[GetCurrentGear()]));
				
				if (Mathf.Abs(minRPM) * AllGearsRatio[GetCurrentGear()] > m_Car.Config.MaxRPM)
				{
					motorTorque = 0;
				}

				//If the rpm of the wheel is less than the max rpm engine * current ratio, then apply the current torque for wheel, else not torque for wheel.
				var maxWheelRPM = AllGearsRatio[GetCurrentGear()] * m_EngineRPM;
				
				//for (var i = m_FirstDriveWheel; i <= m_LastDriveWheel; i++)
				//{
				//	if (m_Wheels[i].WheelCollider.rpm <= maxWheelRPM)
				//	{
				//		m_Wheels[i].WheelCollider.motorTorque = motorTorque;
				//	}
				//	else
				//	{
				//		m_Wheels[i].WheelCollider.motorTorque = 0;
				//	}
				//}

				foreach (var wheel in m_Wheels)
				{
					if (wheel.Driving)
					{
                        if (wheel.WheelCollider.rpm <= maxWheelRPM)
                        {
                            wheel.WheelCollider.motorTorque = motorTorque;
                        }
                        else
                        {
                            wheel.WheelCollider.motorTorque = 0;
                        }
                    }
                    
                }
            }
			else
			{
				m_CurrentBrake = m_Car.Config.MaxBrakeTorque;
			}
		}
		else
		{
			m_CurrentBrake = 0;

			for (int i = m_FirstDriveWheel; i <= m_LastDriveWheel; i++)
			{
				m_Wheels[i].WheelCollider.motorTorque = 0;
			}
		}


        var forwardIsSlip = false;

        for (var i = m_FirstDriveWheel; i <= m_LastDriveWheel; i++)
        {
            if (m_Wheels[i].CurrentForwardSleep > m_Car.Config.MaxForwardSlipToBlockChangeGear)
            {
                forwardIsSlip = true;
                break;
            }
        }

        var prevRatio = 0f;
        var newRatio = 0f;

        if (!forwardIsSlip && m_EngineRPM > m_Car.Config.RpmToNextGear && m_CurrentGear >= 0 && m_CurrentGear < (AllGearsRatio.Length - 2))
        {
            prevRatio = AllGearsRatio[GetCurrentGear()];
			m_CurrentGear++;
            newRatio = AllGearsRatio[GetCurrentGear()];
        }
        else if (m_EngineRPM < m_Car.Config.RpmToPrevGear && m_CurrentGear > 0 && (m_EngineRPM <= m_Car.Config.MinRPM || m_CurrentGear != 1))
        {
            prevRatio = AllGearsRatio[GetCurrentGear()];
			m_CurrentGear--;
            newRatio = AllGearsRatio[GetCurrentGear()];
        }

        if (!Mathf.Approximately(prevRatio, 0) && !Mathf.Approximately(newRatio, 0))
        {
            m_EngineRPM = Mathf.Lerp(m_EngineRPM, m_EngineRPM * (newRatio / prevRatio), m_Car.Config.RpmEngineToRpmWheelsLerpSpeed * Time.fixedDeltaTime);
        }

        if (GetCurrentCarDirection() <= 0 && m_CurrentAccelerationInput < 0)
        {
			m_CurrentGear = -1;
        }
        else if (m_CurrentGear <= 0 && GetCurrentCarDirection() >= 0 && m_CurrentAccelerationInput > 0)
        {
			m_CurrentGear = 1;
        }
        else if (GetCurrentCarDirection() == 0 && m_CurrentAccelerationInput == 0)
        {
			m_CurrentGear = 0;
        }
    }

	private void SetStaticMode()
	{
		ApplyHandbrake();
		m_Car.Rigidbody.velocity = Vector3.zero;

        foreach (var wheel in m_Wheels)
		{
            wheel.WheelCollider.motorTorque = 0f;
        }
	}

	public int GetCurrentCarDirection()
	{
		return CurrentSpeed < 1 ? 0 : (VelocityAngle < 90 && VelocityAngle > -90 ? 1 : -1);
	}

	private float GetInCutOffRPM()
	{
		return m_Car.Config.CutOffRPM - m_Car.Config.CutOffOffsetRPM;
	}

	private int GetCurrentGear()
	{
		return m_CurrentGear + 1;
	}

	private void PlayBackfireWithProbability()
	{
		PlayBackfireWithProbability(m_Car.Config.ProbabilityBackfire);
	}

	private void PlayBackfireWithProbability(float probability)
	{
		if (Random.Range(0f, 1f) <= probability)
		{
			//BackFireAction.SafeInvoke();
			BackFireAction.Invoke();
		}
	}

	#endregion

	private void AccelerationPerformed(float obj)
	{
		m_CurrentAccelerationInput = obj;
	}

	private void HandbrakePerformed(float obj)
	{
		m_HandBrake = obj;
	}

	private void SteerPerformed(float obj)
	{
		m_CurrentSteerInput = obj;
	}

	private void OnDestroy()
    {
		RemoveListeners();
    }
}
