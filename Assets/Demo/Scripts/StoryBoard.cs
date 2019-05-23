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
#region private
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

        private GameState m_state = GameState.start;
#endregion

#region public
        public void Restart()
        {
            SceneManager.LoadScene(1);
        }
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

        public void BroadcastMsgToObj(string name, string func)
        {
            GameObject obj = GameObject.Find(name);
            if (obj != null)
            {
                obj.BroadcastMessage(func);
            }
        }
        public void SetObjState(string name, bool active)
        {
            GameObject obj = GameObject.Find(name);
            if (obj != null)
            {
                obj.SetActive(active);
            }
        }
        public void DisableObject(string name)
        {
            SetObjState(name, false);
        }
        public void SetAudio(string name, bool state)
        {
            GameObject obj = GameObject.Find(name);
            if (obj != null)
            {
                obj.GetComponent<AudioSource>().enabled = state;
            }
        }

        public void TriggerObject(string name)
        {
            BroadcastMsgToObj(name, "DoActivateTrigger");
        }

        public void DisableGameObjectGuide(string name)
        {
            BroadcastMsgToObj(name, "DisableGuide");
        }
        public void EnableGameObjectGuide(string name)
        {
            BroadcastMsgToObj(name, "EnableGuide");
        }
        public void CloseDoor(string name)
        {
            BroadcastMsgToObj(name, "Close");
        }
        public void OpenDoor(string name)
        {
            BroadcastMsgToObj(name, "Open");
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
            else if (guideName == "DoorHouse")
            {
                Debug.Log("SceneManager DoorHouse OnGuideCompleted");
                SwitchState(GameState.dust);
            }
            else if (guideName == "GuideSofa")
            {
                Debug.Log("SceneManager DoorBathroom OnGuideCompleted");
                SwitchState(GameState.night);
            }
            else if (guideName == "Kira_Back")
            {
                Debug.Log("SceneManager KiraBack OnGuideCompleted");
                SwitchState(GameState.exit);
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
            Debug.Log("SceneManager Start");
            DisableAll();
            SwitchState(GameState.start);
        }
        private void Awake()
        {
            //Debug.Log("SceneManager Awake");
            //DisableAll();
            //SwitchState(GameState.start);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
        }
        private void ExitGame()
        {
            SceneManager.LoadScene(0);
        }
        private void StartState()
        {
            Debug.Log("SceneManager StartState");
            SwitchState(GameState.bed);
        }

        private void BedState()
        {
            Debug.Log("SceneManager BedState");
            DisableAll();

            SetAudio("BGMAudio", false);
            SetAudio("Clock", true);
            SetSkyBox(SkyState.daylight);
            EnableGameObjectGuide("Clock");
            m_playables[0].SetActive(true);
        }
        private void ClockState()
        {
            Debug.Log("SceneManager ClockState");
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
            Debug.Log("SceneManager BedroomtState");
            DisableGameObjectGuide("DoorBedroom");
            EnableGameObjectGuide("DoorBathroom");
        }
        private void GirlState()
        {
            Debug.Log("SceneManager GirlState");
            DisableAll();
            m_girl.SetActive(true);
        }
        private void BathroomState()
        {
            Debug.Log("SceneManager BathroomState");
            DisableAll();
            DisableGameObjectGuide("DoorBathroom");
            EnableGameObjectGuide("DoorHouse");

            CloseDoor("DoorBathroom");
            m_playables[1].SetActive(true);
        }
        private void DustState()
        {
            Debug.Log("SceneManager DustState");
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
            Debug.Log("SceneManager NightState");
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
            Debug.Log("SceneManager ExitState");
            ExitGame();
        }

        private void DisableAll()
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
    }
}
