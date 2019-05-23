using UnityEngine;

namespace Cartoon.HomeDemo
{
    public class RingGuide : MonoBehaviour
    {
        [SerializeField]
        private string guideName = "";
        private bool m_enabled = false;
        private StoryBoard storyBoard;
        // Start is called before the first frame update
        void Start()
        {
            //DisableGuide();
        }

        private void Awake()
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Animator>().enabled = false;
            if ("" == guideName)
            {
                guideName = transform.parent.gameObject.name;
            }
            storyBoard = GameObject.Find("StoryBoard").GetComponent<StoryBoard>();
        }

        private void EnableGuide()
        {
            if (!m_enabled)
            {
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<Animator>().enabled = true;
                m_enabled = true;
                //gameObject.SetActive(true);

                if("Door" == transform.parent.gameObject.name)
                    transform.parent.gameObject.BroadcastMessage("EnableDoor");
            }
        }

        private void DisableGuide()
        {
            if (m_enabled)
            {
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Animator>().enabled = false;
                m_enabled = false;
                //gameObject.SetActive(false);

                storyBoard.OnGuideCompleted(guideName);

                if ("Door" == transform.parent.gameObject.name)
                    transform.parent.gameObject.BroadcastMessage("DisableDoor");
            }
        }
    }
}
