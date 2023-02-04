using UnityEngine;

[RequireComponent(typeof(CarController))]
public class BodyRoll : MonoBehaviour
{
	[SerializeField]
	private CarController m_CarController;

	[SerializeField] 
	private Transform m_Body;                                //Link to car body.
	
	[SerializeField] 
	private float m_MaxAngle = 10;                           //Max tilt angle of car body.
	
	[SerializeField]
	private float m_AngleVelocityMultiplayer = 0.2f;         //Rotation angle multiplier when moving forward.
	
	[SerializeField]
	private float m_RearAngleVelocityMultiplayer = 0.4f;     //Rotation angle multiplier when moving backwards.
	
	[SerializeField] 
	private float m_MaxTiltOnSpeed = 60;                     //The speed at which the maximum tilt is reached.

	private float m_Angle;

	private void Update()
	{

		if (m_CarController.GetCurrentCarDirection() == 1)
        {
			m_Angle = -m_CarController.VelocityAngle * m_AngleVelocityMultiplayer;
		}
		else if (m_CarController.GetCurrentCarDirection() == -1)
		{
			m_Angle = Globals.LoopClamp(m_CarController.VelocityAngle + 180, -180, 180) * m_RearAngleVelocityMultiplayer;
		}
		else
		{
			m_Angle = 0;
		}

		m_Angle *= Mathf.Clamp01(Globals.MsToKph(m_CarController.CurrentSpeed) / m_MaxTiltOnSpeed);
		m_Angle = Mathf.Clamp(m_Angle, -m_MaxAngle, m_MaxAngle);
		m_Body.localRotation = Quaternion.AngleAxis(m_Angle, Vector3.forward);
	}
}
