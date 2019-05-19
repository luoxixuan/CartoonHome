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
    public enum SkyState
    {
        daylight = 0,
        dust = 1,
        night = 2,
    }

    public class SceneManager : MonoBehaviour
    {
#region private
        [SerializeField]
        private GameObject m_girl;
        [SerializeField]
        private GameObject m_bedgirl;
        [SerializeField]
        private GameObject m_cat;
        [SerializeField]
        private GameObject m_bathroomCam;
        [SerializeField]
        private GameObject m_bedroomCam;
        [SerializeField]
        private Material[] m_skyBox;

        private GameState m_state = GameState.start;
#endregion

#region public
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
                SetSkyBox(SkyState.dust);
            }
        }

        public void SetSkyBox(SkyState state)
        {
            RenderSettings.skybox = m_skyBox[(int)state];
        }
#endregion

#region private
        private void Start()
        {
            SwitchState(GameState.start);
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

            m_bedgirl.SetActive(true);
            m_bedroomCam.SetActive(true);
        }
        private void GirlState()
        {
            m_bedroomCam.SetActive(false);
            m_bathroomCam.SetActive(false);
            m_cat.SetActive(false);
            m_bedgirl.SetActive(false);

            m_girl.SetActive(true);
        }
        private void BathroomState()
        {
            m_girl.SetActive(false);
            m_bedroomCam.SetActive(false);
            m_cat.SetActive(false);
            m_bedgirl.SetActive(false);

            m_bathroomCam.SetActive(true);
        }
        private void CatState()
        {
            m_girl.SetActive(false);
            m_bathroomCam.SetActive(false);
            m_bedroomCam.SetActive(false);
            m_bedgirl.SetActive(false);

            TriggerObject("Doorhouse");
            m_cat.SetActive(true);
        }
#endregion
    }
}
