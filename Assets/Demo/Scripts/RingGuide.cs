﻿using UnityEngine;

namespace Cartoon.HomeDemo
{
    public class RingGuide : MonoBehaviour
    {
        private bool m_enabled = false;
        private string guideName = "";
        private SceneManager sceneManager;
        // Start is called before the first frame update
        void Start()
        {
            //DisableGuide();
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Animator>().enabled = false;

            guideName = transform.parent.gameObject.name;
            sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        }

        private void EnableGuide()
        {
            if (!m_enabled)
            {
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<Animator>().enabled = true;
                m_enabled = true;
                //gameObject.SetActive(true);
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

                sceneManager.OnGuideCompleted(guideName);
            }
        }
    }
}