using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class NewWheelCollider : MonoBehaviour
{
    [SerializeField]
    private NewWheelCollidersConfig WheelConfig;

    [SerializeField, HideInInspector]
    private WheelCollider m_WheelCollider;

    private Rigidbody m_RigidBody;

    private const float minSpring = 0;
    private const float maxSpring = 60000;
    private const float minDamper = 0;
    private const float maxDamper = 10000;

    //Minimum friction
    private const float minExtremumSlip = 0.4f;
    private const float minExtremumValue = 0.7f;
    private const float minAsymptoteSlip = 0.6f;
    private const float minAsymptoteValue = 0.65f;

    //Maximum friction
    private const float maxExtremumSlip = 0.4f;
    private const float maxExtremumValue = 4.5f;
    private const float maxAsymptoteSlip = 0.6f;
    private const float maxAsymptoteValue = 4f;

    public WheelCollider WheelCollider
    {
        get => m_WheelCollider;
    }

    public void UpdateStiffness(float forward, float sideways)
    {
        var forwardFriction = WheelCollider.forwardFriction;
        var sidewaysFriction = WheelCollider.sidewaysFriction;

        forwardFriction.stiffness = forward;
        sidewaysFriction.stiffness = sideways;

        WheelCollider.forwardFriction = forwardFriction;
        WheelCollider.sidewaysFriction = sidewaysFriction;
    }

    public void UpdateConfig()
    {
        UpdateConfig(WheelConfig);
    }

    public void UpdateConfig(NewWheelCollidersConfig newConfig)
    {
        if (m_RigidBody == null)
        {
            if (!SetRigidBody())
            {
                Debug.LogError("WheelCollider without attached RigidBody");
                return;
            }
        }

        WheelConfig.ForwardFriction = newConfig.ForwardFriction;
        WheelConfig.SidewaysFriction = newConfig.SidewaysFriction;


        var springValue = Mathf.Lerp(minSpring, maxSpring, newConfig.Spring);
        var damperValue = Mathf.Lerp(minDamper, maxDamper, newConfig.Damper);

        var spring = new JointSpring();
        spring.spring = springValue;
        spring.damper = damperValue;
        spring.targetPosition = newConfig.TargetPoint;

        WheelCollider.mass = newConfig.Mass;
        WheelCollider.radius = newConfig.Radius;
        WheelCollider.wheelDampingRate = newConfig.WheelDampingRate;
        WheelCollider.suspensionDistance = newConfig.SuspensionDistance;
        WheelCollider.forceAppPointDistance = newConfig.ForceAppPointDistance;
        WheelCollider.center = newConfig.Center;
        WheelCollider.suspensionSpring = spring;

        WheelFrictionCurve forwardFriction = new WheelFrictionCurve();
        forwardFriction.extremumSlip = Mathf.Lerp(minExtremumSlip, maxExtremumSlip, newConfig.ForwardFriction);
        forwardFriction.extremumValue = Mathf.Lerp(minExtremumValue, maxExtremumValue, newConfig.ForwardFriction);
        forwardFriction.asymptoteSlip = Mathf.Lerp(minAsymptoteSlip, maxAsymptoteSlip, newConfig.ForwardFriction);
        forwardFriction.asymptoteValue = Mathf.Lerp(minAsymptoteValue, maxAsymptoteValue, newConfig.ForwardFriction);
        forwardFriction.stiffness = 1;

        WheelFrictionCurve sidewaysFriction = new WheelFrictionCurve();
        sidewaysFriction.extremumSlip = Mathf.Lerp(minExtremumSlip, maxExtremumSlip, newConfig.SidewaysFriction);
        sidewaysFriction.extremumValue = Mathf.Lerp(minExtremumValue, maxExtremumValue, newConfig.SidewaysFriction);
        sidewaysFriction.asymptoteSlip = Mathf.Lerp(minAsymptoteSlip, maxAsymptoteSlip, newConfig.SidewaysFriction);
        sidewaysFriction.asymptoteValue = Mathf.Lerp(minAsymptoteValue, maxAsymptoteValue, newConfig.SidewaysFriction);
        sidewaysFriction.stiffness = 1;

        WheelCollider.forwardFriction = forwardFriction;
        WheelCollider.sidewaysFriction = sidewaysFriction;
    }


    public bool CheckFirstEnable()
    {
        if (m_WheelCollider != null)
        {
            return false;
        }

        var sprigValue = (WheelCollider.suspensionSpring.spring - minSpring) / (maxSpring - minSpring);
        var damper = (WheelCollider.suspensionSpring.damper - minDamper) / (maxDamper - minDamper);
        var forwardFriction = (WheelCollider.forwardFriction.extremumValue - minExtremumValue) / (maxExtremumValue - minExtremumValue);
        var sidewaysFriction = (WheelCollider.sidewaysFriction.extremumValue - minExtremumValue) / (maxExtremumValue - minExtremumValue);

        WheelConfig = new NewWheelCollidersConfig();
        WheelConfig.Mass = WheelCollider.mass;
        WheelConfig.Radius = WheelCollider.radius;
        WheelConfig.WheelDampingRate = WheelCollider.wheelDampingRate;
        WheelConfig.SuspensionDistance = WheelCollider.suspensionDistance;
        WheelConfig.ForceAppPointDistance = WheelCollider.forceAppPointDistance;
        WheelConfig.Center = WheelCollider.center;
        WheelConfig.TargetPoint = WheelCollider.suspensionSpring.targetPosition;
        WheelConfig.Spring = sprigValue;
        WheelConfig.Damper = damper;
        WheelConfig.ForwardFriction = forwardFriction;
        WheelConfig.SidewaysFriction = sidewaysFriction;

        return true;
    }

    private bool SetRigidBody()
    {
        m_RigidBody = m_WheelCollider.attachedRigidbody;

        return m_RigidBody != null;
    }
}
