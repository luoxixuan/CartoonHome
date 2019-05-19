using UnityEngine;

namespace Cartoon.RigidJoint
{
    public class RaycastTrigger : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_triggerBody; //默认用附加的物体
        [SerializeField]
        private Camera m_cam;
        [SerializeField]
        private string m_triggerFunc = "";
        [Range(0f, 10f)] [SerializeField]
        float maxHitDis = 10.0f;

        protected virtual bool doTrigger(RaycastHit hit)
        {
            //DO SOMETHING
            return true;
        }

        protected virtual bool init()
        {
            //DO SOMETHING
            if (!m_triggerBody)
            {
                m_triggerBody = gameObject;
            }
            return true;
        }

        private void Start()
        {
            init();
        }

        private void Update()
        {
            // Make sure the user pressed the mouse down
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }

            var mainCamera = FindCamera();

            // We need to actually hit an object
            RaycastHit hit = new RaycastHit();
            if (
                !Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition).origin,
                                 mainCamera.ScreenPointToRay(Input.mousePosition).direction, out hit, maxHitDis,
                                 Physics.DefaultRaycastLayers))
            {
                return;
            }
            if (hit.collider.gameObject.GetInstanceID() != gameObject.GetInstanceID())
            {
                return;
            }

            doTrigger(hit);
            if ("" != m_triggerFunc)
            {
                BroadcastMessage(m_triggerFunc);
            }
        }

        private Camera FindCamera()
        {
            //return m_cam;
            return m_cam ? m_cam : Camera.main;
        }
    }
}
