using UnityEngine;
using UnityEngine.SceneManagement;

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
        exit,
    };
    public enum SkyState
    {
        daylight = 0,
        dust = 1,
        night = 2,
    }

    public class StoryBoard : MonoBehaviour
    {
#region Modify On Editor
        [SerializeField]
        private GameObject m_girl;
        [SerializeField]
        private GameObject m_bedgirl;
        [SerializeField]
        private GameObject m_backgirl;
        [SerializeField]
        private GameObject m_cat;
        [SerializeField]
        private Material[] m_skyBox;
        [SerializeField]
        private GameObject[] m_playables;
        [SerializeField]
        private bool m_LockCursor = true;                   // Whether the cursor should be hidden and locked.
#endregion

#region Private Value
        private GameState m_state = GameState.start;
#endregion

#region Interface
        // 目前只能按顺序切换
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
                case GameState.exit:
                    ExitState();
                    break;
                default:
                    break;
            }
        }
        public void DisableAll()
        {
            m_girl.SetActive(false);
            m_bedgirl.SetActive(false);
            m_cat.SetActive(false);

            if (m_state != GameState.bed)
            {
                Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = !m_LockCursor;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
#endregion

#region Utility
        private void BroadcastMsgToObj(string name, string func)
        {
            GameObject obj = GameObject.Find(name);
            if (obj != null)
            {
                obj.BroadcastMessage(func);
            }
        }
        private void SetObjState(string name, bool active)
        {
            GameObject obj = GameObject.Find(name);
            if (obj != null)
            {
                obj.SetActive(active);
            }
        }
        private void DisableObject(string name)
        {
            SetObjState(name, false);
        }
        private void SetAudio(string name, bool state)
        {
            GameObject obj = GameObject.Find(name);
            if (obj != null)
            {
                obj.GetComponent<AudioSource>().enabled = state;
            }
        }

        private void TriggerObject(string name)
        {
            BroadcastMsgToObj(name, "DoActivateTrigger");
        }

        private void DisableGameObjectGuide(string name)
        {
            BroadcastMsgToObj(name, "DisableGuide");
        }
        private void EnableGameObjectGuide(string name)
        {
            BroadcastMsgToObj(name, "EnableGuide");
        }
        private void CloseDoor(string name)
        {
            BroadcastMsgToObj(name, "Close");
        }
        private void OpenDoor(string name)
        {
            BroadcastMsgToObj(name, "Open");
        }

        private void SetSkyBox(SkyState state)
        {
            RenderSettings.skybox = m_skyBox[(int)state];
        }
#endregion

#region StateExecutor
        private void ExitGame()
        {
            SceneManager.LoadScene("Start");
        }
        private void StartState()
        {
            Debug.Log("GameStateController StartState");
            DisableAll();
            SwitchState(GameState.bed);
        }

        private void BedState()
        {
            Debug.Log("GameStateController BedState");
            DisableAll();

            SetAudio("BGMAudio", false);
            SetAudio("Clock", true);
            SetSkyBox(SkyState.daylight);
            EnableGameObjectGuide("Clock");
            m_playables[0].SetActive(true);
        }
        private void ClockState()
        {
            Debug.Log("GameStateController ClockState");
            DisableGameObjectGuide("Clock");
            EnableGameObjectGuide("DoorBedroom");

            //EnableObject("Kira_A");
            SetAudio("Clock", false);
            SetAudio("BGMAudio", true);
            m_girl.SetActive(true);
            DisableObject("Kira_Bed");
            DisableObject("PlayableBedroomCam");
        }
        private void BedroomtState()
        {
            Debug.Log("GameStateController BedroomtState");
            DisableGameObjectGuide("DoorBedroom");
            EnableGameObjectGuide("DoorBathroom");
        }
        private void GirlState()
        {
            Debug.Log("GameStateController GirlState");
            DisableAll();
            m_girl.SetActive(true);
        }
        private void BathroomState()
        {
            Debug.Log("GameStateController BathroomState");
            DisableAll();
            DisableGameObjectGuide("DoorBathroom");
            EnableGameObjectGuide("DoorHouse");

            CloseDoor("DoorBathroom");
            m_playables[1].SetActive(true);
        }
        private void DustState()
        {
            Debug.Log("GameStateController DustState");
            DisableAll();
            TriggerObject("DoorHouse");
            //m_cat.SetActive(true);
            m_playables[2].SetActive(true);
            SetSkyBox(SkyState.dust);

            DisableGameObjectGuide("DoorHouse");
            EnableGameObjectGuide("GuideSofa");
        }
        private void NightState()
        {
            Debug.Log("GameStateController NightState");
            //DisableAll();
            m_cat.SetActive(true);
            m_playables[3].SetActive(true);
            SetSkyBox(SkyState.night);
            SetObjState("HouseLights", false);

            m_backgirl.SetActive(true);
            EnableGameObjectGuide("Kira_Back");
        }
        private void ExitState()
        {
            Debug.Log("GameStateController ExitState");
            ExitGame();
        }
#endregion
    }
}
