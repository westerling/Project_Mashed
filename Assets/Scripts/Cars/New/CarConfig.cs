using System;
using UnityEngine;

[Serializable]
public class CarConfig
{
	[Header("Information")]
	[SerializeField]
	private string m_CarName;

	[SerializeField]
	private string m_Description;

	[Header("Durability")]
    [Range(20, 200)]
    [SerializeField]
    private float m_RearDurability = 100f;

    [Range(20, 200)]
    [SerializeField]
    private float m_FrontDurability = 100f;


    [Header("Steer Settings")]
    [SerializeField]
	private float m_MaxSteerAngle = 25;

	[Header("Engine and power settings")]
	[SerializeField]
	private DriveType m_DriveType = DriveType.RWD;

	[SerializeField]
	private float m_MaxMotorTorque = 150;                      //Max motor torque engine (Without GearBox multiplier).

	[SerializeField]
	private AnimationCurve m_MotorTorqueFromRpmCurve;          //Curve motor torque (Y(0-1) motor torque, X(0-7) motor RPM).

	[SerializeField]
	private float m_MaxRPM = 7000;

	[SerializeField]
	private float m_MinRPM = 700;

	[SerializeField]
	private float m_CutOffRPM = 6800;                          //The RPM at which the cutoff is triggered.

	[SerializeField]
	private float m_CutOffOffsetRPM = 500;

	[SerializeField]
	private float m_CutOffTime = 0.1f;

	[Range(0, 1)]
	[SerializeField]
	private float m_ProbabilityBackfire = 0.2f;

	[SerializeField]
	private float m_RpmToNextGear = 6500;                      //The speed at which there is an increase in gearbox.

	[SerializeField]
	private float m_RpmToPrevGear = 4500;                      //The speed at which there is an decrease in gearbox.

	[SerializeField]
	private float m_MaxForwardSlipToBlockChangeGear = 0.5f;    //Maximum rear wheel slip for shifting gearbox.

	[SerializeField]
	private float m_RpmEngineToRpmWheelsLerpSpeed = 15;        //Lerp Speed change of RPM.

	[SerializeField]
	private float[] m_GearsRatio;                              //Forward gears ratio.

	[SerializeField]
	private float m_MainRatio;

	[SerializeField]
	private float m_ReversGearRatio;                           //Reverse gear ratio.

	[SerializeField]
	private float m_Downforce;

	[Header("Braking settings")]
    [SerializeField]
	private float m_MaxBrakeTorque = 1000;

	[Header("Helper settings")]                             //This settings block in the full version is stored in the regime settings.

	[SerializeField]
	private bool m_EnableSteerAngleMultiplier = true;
	[SerializeField]
	private float m_MinSteerAngleMultiplier = 0.05f;           //Min steer angle multiplayer to limit understeer at high speeds.
	[SerializeField]
	private float m_MaxSteerAngleMultiplier = 1f;          //Max steer angle multiplayer to limit understeer at high speeds.
	[SerializeField]
	private float m_MaxSpeedForMinAngleMultiplier = 250;       //The maximum speed at which there will be a minimum steering angle multiplier.
	[Space(10)]


    [Header("Steering")]
	[SerializeField]
	private float m_SteerAngleChangeSpeed;                     //Wheel turn speed.

    public float MaxSteerAngle
	{
		get => m_MaxSteerAngle;
	}

    public DriveType DriveType
	{
		get => m_DriveType;
	}

    public float MaxMotorTorque 
	{
		get => m_MaxMotorTorque;
	}

    public AnimationCurve MotorTorqueFromRpmCurve
	{
		get => m_MotorTorqueFromRpmCurve; 
	}

    public float MaxRPM
	{ 
		get => m_MaxRPM; 
	}

    public float MinRPM 
	{
		get => m_MinRPM;
	}

    public float CutOffRPM 
	{ 
		get => m_CutOffRPM;
	}

    public float CutOffOffsetRPM 
	{
		get => m_CutOffOffsetRPM;
	}

    public float CutOffTime
	{ 
		get => m_CutOffTime;
	}

    public float ProbabilityBackfire 
	{ 
		get => m_ProbabilityBackfire; 
	}

    public float RpmToNextGear 
	{ 
		get => m_RpmToNextGear;
	}

    public float RpmToPrevGear
	{
		get => m_RpmToPrevGear; 
	}
    public float MaxForwardSlipToBlockChangeGear
	{ 
		get => m_MaxForwardSlipToBlockChangeGear;
	}

    public float RpmEngineToRpmWheelsLerpSpeed
	{ 
		get => m_RpmEngineToRpmWheelsLerpSpeed; 
	}

    public float[] GearsRatio
	{
		get => m_GearsRatio; 
	}

    public float MainRatio 
	{
		get => m_MainRatio;
	}

    public float ReversGearRatio
	{
		get => m_ReversGearRatio;
	}

    public bool EnableSteerAngleMultiplier 
	{ 
		get => m_EnableSteerAngleMultiplier; 
	}

    public float MinSteerAngleMultiplier 
	{
		get => m_MinSteerAngleMultiplier;
	}

    public float MaxSteerAngleMultiplier
	{
		get => m_MaxSteerAngleMultiplier;
	}
    public float MaxSpeedForMinAngleMultiplier 
	{
		get => m_MaxSpeedForMinAngleMultiplier; 
	}
    
	public float SteerAngleChangeSpeed 
	{ 
		get => m_SteerAngleChangeSpeed;
	}

    public float MaxBrakeTorque 
	{
		get => m_MaxBrakeTorque; 
	}
    public string CarName 
	{ 
		get => m_CarName; 
	}
    
	public string Description 
	{ 
		get => m_Description;
	}
    public float Downforce 
	{
		get => m_Downforce; 
	}
    public float RearDurability 
	{
		get => m_RearDurability; 
	}
    public float FrontDurability 
	{
		get => m_FrontDurability; 
	}
}
