using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject m_door;
    public Camera m_cam;
    // Start is called before the first frame update
    public float k_motorForce = 20.0f;
    public float k_motorVel = -25.0f;
    
    private HingeJoint m_hingeJoint;
    private JointMotor m_motor = new JointMotor();

    private void Start()
    {
        m_motor.targetVelocity = k_motorVel;
        m_motor.force = k_motorForce;
        m_door.GetComponent<Rigidbody>().isKinematic = true; //让它不能被推动
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
        // We need to hit a rigidbody that is not kinematic
        if (!hit.rigidbody || !hit.rigidbody.isKinematic)
        {
            return;
        }

        if (!m_hingeJoint)
        {
            //var go = new GameObject("Rigidbody dragger");
            Rigidbody body = m_door.GetComponent<Rigidbody>();
            m_hingeJoint = m_door.GetComponent<HingeJoint>();
            body.isKinematic = false;
        }

        m_hingeJoint.motor = m_motor;
        m_hingeJoint.useMotor = true;

        //StartCoroutine("DragObject", hit.distance);
    }


    private IEnumerator DragObject(float distance)
    {
        var oldDrag = m_hingeJoint.connectedBody.drag;
        var oldAngularDrag = m_hingeJoint.connectedBody.angularDrag;
        var mainCamera = FindCamera();
        while (Input.GetMouseButton(0))
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            m_hingeJoint.transform.position = ray.GetPoint(distance);
            yield return null;
        }
        if (m_hingeJoint.connectedBody)
        {
            m_hingeJoint.connectedBody.drag = oldDrag;
            m_hingeJoint.connectedBody.angularDrag = oldAngularDrag;
            m_hingeJoint.connectedBody = null;
        }
    }


    private Camera FindCamera()
    {
        return m_cam;
    }
}
