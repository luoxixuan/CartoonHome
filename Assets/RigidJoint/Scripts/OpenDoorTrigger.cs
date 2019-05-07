using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cartoon.RigidJoint
{
    public class OpenDoorTrigger : RaycastTrigger
    {
        public GameObject m_door; //这个门和触发用的物体可以不是一个物体
        // Start is called before the first frame update
        public float k_motorForce = 20.0f;
        public float k_motorVel = -25.0f;

        private HingeJoint m_hingeJoint;
        private JointMotor m_motor = new JointMotor();

        protected override bool init()
        {
            if (!base.init())
            {
                return false;
            }

            m_motor.targetVelocity = k_motorVel;
            m_motor.force = k_motorForce;
            if (!m_hingeJoint)
            {
                //var go = new GameObject("Rigidbody dragger");
                Rigidbody body = m_door.GetComponent<Rigidbody>();
                m_hingeJoint = m_door.GetComponent<HingeJoint>();
                body.isKinematic = false;
            }

            return true;
        }

        protected override bool doTrigger(RaycastHit hit)
        {
            base.doTrigger(hit);
            // We need to hit a rigidbody that is not kinematic
            if (!hit.rigidbody || !hit.rigidbody.isKinematic)
            {
                return false;
            }

            m_hingeJoint.motor = m_motor;
            m_hingeJoint.useMotor = true;

            return true;
        }
    }
}
