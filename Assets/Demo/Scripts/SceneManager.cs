using System;
using System.Collections;
using UnityEngine;

namespace Cartoon.HomeDemo
{
    public enum GameState
    {
        start       = 0,
        bed,
        clock,
        bedroom,
        girl,
        bathroom,
        dust,
        night,
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
        private GameObject m_FieldCams;
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
                case GameState.bed:
                    BedState();
                    break;
                case GameState.clock:
                    ClockState();
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
                case GameState.dust:
                    DustState();
                    break;
                case GameState.night:
                    NightState();
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

        public void DisableGameObjectGuide(string name)
        {
            GameObject obj = GameObject.Find(name);
            if (obj != null)
            {
                obj.BroadcastMessage("DisableGuide");
            }
        }
        public void EnableGameObjectGuide(string name)
        {
            GameObject obj = GameObject.Find(name);
            if (obj != null)
            {
                obj.BroadcastMessage("EnableGuide");
            }
        }

        public void OnDoorOpen(string doorName)
        {
            if (doorName == "DoorHouse")
            {
                //SwitchState(GameState.dust);
                Debug.Log("SceneManager DoorHouse OnDoorOpen");
            }
            else if (doorName == "DoorBedroom")
            {
                //SwitchState(GameState.bedroom);
                Debug.Log("SceneManager DoorBedroom OnDoorOpen");
            }
            else if (doorName == "DoorBathroom")
            {
                //SwitchState(GameState.bathroom);
                Debug.Log("SceneManager DoorBathroom OnDoorOpen");
            }
        }
        public void OnGuideCompleted(string guideName)
        {
            if (guideName == "Clock")
            {
                Debug.Log("SceneManager Clock OnGuideCompleted");
                SwitchState(GameState.clock);
            }
            else if (guideName == "DoorHouse")
            {
                Debug.Log("SceneManager DoorHouse OnGuideCompleted");
                SwitchState(GameState.dust);
            }
            else if (guideName == "DoorBedroom")
            {
                Debug.Log("SceneManager DoorBedroom OnGuideCompleted");
                SwitchState(GameState.bedroom);
            }
            else if (guideName == "DoorBathroom")
            {
                Debug.Log("SceneManager DoorBathroom OnGuideCompleted");
                SwitchState(GameState.bathroom);
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
            DisableAll();
            SwitchState(GameState.start);
        }
        private void StartState()
        {
            EnableGameObjectGuide("Clock");
            SwitchState(GameState.bed);
        }

        private void BedState()
        {
            DisableAll();
            m_bedgirl.SetActive(true);
            //DisableAll();
            //DisableGameObjectGuide("Clock");
            //EnableGameObjectGuide("DoorBedroom");
        }
        private void ClockState()
        {
            DisableGameObjectGuide("Clock");
            EnableGameObjectGuide("DoorBedroom");
        }
        private void BedroomtState()
        {
            DisableGameObjectGuide("DoorBedroom");
            EnableGameObjectGuide("DoorBathroom");
        }
        private void GirlState()
        {
            DisableAll();
            m_girl.SetActive(true);
        }
        private void BathroomState()
        {
            DisableAll();
            m_bathroomCam.SetActive(true);
            DisableGameObjectGuide("DoorBathroom");
            EnableGameObjectGuide("DoorHouse");
        }
        private void DustState()
        {
            DisableAll();
            TriggerObject("DoorHouse");
            m_cat.SetActive(true);
            SetSkyBox(SkyState.dust);

            DisableGameObjectGuide("DoorHouse");
        }
        private void NightState()
        {
            DisableAll();
            m_cat.SetActive(true);
            SetSkyBox(SkyState.night);
        }

        private void DisableAll()
        {
            //m_FieldCams.SetActive(false);
            m_girl.SetActive(false);
            m_bedgirl.SetActive(false);
            m_cat.SetActive(false);
        }
        #endregion
    }
}
