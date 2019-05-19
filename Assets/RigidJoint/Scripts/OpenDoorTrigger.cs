using UnityEngine;

namespace Cartoon.RigidJoint
{
    public class OpenDoorTrigger : RaycastTrigger
    {
        private DoorController m_doorController;
        private bool m_closed = true;
        private string doorName = "Door";
        private HomeDemo.SceneManager sceneManager;

        protected override bool init()
        {
            if (!base.init())
            {
                return false;
            }

            m_doorController = gameObject.GetComponent<DoorController>();

            sceneManager = GameObject.Find("SceneManager").GetComponent<HomeDemo.SceneManager>();
            doorName = transform.parent.gameObject.name;

            return true;
        }

        protected override bool doTrigger(RaycastHit hit)
        {
            base.doTrigger(hit);

            if (m_closed)
            {
                m_doorController.Open();
                if (sceneManager)
                {
                    gameObject.BroadcastMessage("DisableGuide");
                    //m_ringGuide.SetActive(false);
                    //sceneManager.OnDoorOpen(doorName);
                }
            }
            else
            {
                m_doorController.Close();
            }
            m_closed = !m_closed;

            return true;
        }
    }
}
