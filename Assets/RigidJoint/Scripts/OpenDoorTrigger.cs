using UnityEngine;

namespace Cartoon.RigidJoint
{
    public class OpenDoorTrigger : RaycastTrigger
    {
        private DoorController m_doorController;

        private bool m_closed = true;

        protected override bool init()
        {
            if (!base.init())
            {
                return false;
            }

            m_doorController = gameObject.GetComponent<DoorController>();

            return true;
        }

        protected override bool doTrigger(RaycastHit hit)
        {
            base.doTrigger(hit);

            if (m_closed)
            {
                m_doorController.open();
            }
            else
            {
                m_doorController.close();
            }
            m_closed = !m_closed;

            return true;
        }
    }
}
