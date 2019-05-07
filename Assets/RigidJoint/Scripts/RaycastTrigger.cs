using UnityEngine;

namespace Cartoon.RigidJoint
{
    public class RaycastTrigger : MonoBehaviour
    {
        public GameObject m_triggerBody; //默认用附加的物体
        public Camera m_cam;
        // Start is called before the first frame update


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
            var rigidbody = m_triggerBody.GetComponent<Rigidbody>();
            if (!rigidbody)
            {
                m_triggerBody.AddComponent<Rigidbody>();
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
                                 mainCamera.ScreenPointToRay(Input.mousePosition).direction, out hit, 100,
                                 Physics.DefaultRaycastLayers))
            {
                return;
            }

            doTrigger(hit);
        }

        private Camera FindCamera()
        {
            return m_cam;
        }
    }
}
