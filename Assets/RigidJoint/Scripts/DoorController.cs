using UnityEngine;

namespace Cartoon.RigidJoint
{
    public class DoorController : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private float k_motorForce = 20.0f;
        [SerializeField] private float k_motorVel = -25.0f;

        private HingeJoint m_hingeJoint;
        private JointMotor m_motor = new JointMotor();
        private JointMotor m_closeMotor = new JointMotor();
        private bool m_closed = true;

        public void open()
        {
            m_hingeJoint.motor = m_motor;

            m_hingeJoint.useMotor = true;
            m_closed = !m_closed;
        }

        public void close()
        {
            m_hingeJoint.motor = m_closeMotor;

            m_hingeJoint.useMotor = true;
            m_closed = !m_closed;
        }

        private void Start()
        {
            m_motor.targetVelocity = k_motorVel;
            m_motor.force = k_motorForce;
            m_closeMotor.targetVelocity = -k_motorVel;
            m_closeMotor.force = k_motorForce;
            m_hingeJoint = gameObject.GetComponent<HingeJoint>();

        }
    }
}
