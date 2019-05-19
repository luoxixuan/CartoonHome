using System.Collections;
using UnityEngine;

namespace Cartoon.HomeDemo
{
    public enum GameState
    {
        start = 0,
        bedroom = 1,
        girl = 2,
        bathroom = 3,
        cat = 4,
    };

    public class SceneManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_girl;
        [SerializeField]
        private GameObject m_cat;
        [SerializeField]
        private GameObject m_bathroomCam;
        [SerializeField]
        private GameObject m_bedroomCam;

        private GameState m_state = GameState.start;

        private void Start()
        {

        }
        private void StartState()
        {
            SwitchState(GameState.bedroom);
        }
        private void BedroomtState()
        {
            m_girl.SetActive(false);
            m_bathroomCam.SetActive(false);
            m_cat.SetActive(false);

            m_bedroomCam.SetActive(true);
        }
        private void GirlState()
        {
            m_bedroomCam.SetActive(false);
            m_bathroomCam.SetActive(false);
            m_cat.SetActive(false);

            m_girl.SetActive(true);
        }
        private void BathroomState()
        {
            m_girl.SetActive(false);
            m_bedroomCam.SetActive(false);
            m_cat.SetActive(false);

            m_bathroomCam.SetActive(true);
        }
        private void CatState()
        {
            m_girl.SetActive(false);
            m_bathroomCam.SetActive(false);
            m_bedroomCam.SetActive(false);

            TriggerObject("Doorhouse");
            m_cat.SetActive(true);
        }

        public void SwitchState(GameState state)
        {
            m_state = state;
            switch (m_state)
            {
                case GameState.start:
                    StartState();
                    break;
                case GameState.bedroom:
                    BedroomtState();
                    break;
                case GameState.girl:
                    GirlState();
                    break;
                case GameState.bathroom:
                    BathroomState();
                    break;
                case GameState.cat:
                    CatState();
                    break;
                default:
                    break;
            }
        }

        public void TriggerObject(string name)
        {
            GameObject obj = GameObject.Find(name);
            if (obj != null)
            {
                obj.BroadcastMessage("DoActivateTrigger");
            }
        }

        public void OnDoorOpen(string doorName)
        {
            if (doorName == "DoorHouse")
            {
                SwitchState(GameState.cat);
            }
        }
    }
}
