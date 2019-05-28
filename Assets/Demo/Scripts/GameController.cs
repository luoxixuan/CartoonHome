using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cartoon.HomeDemo
{
    public class GameController : MonoBehaviour
    {
        private StoryBoard m_stateController;

        public void Restart()
        {
            SceneManager.LoadScene("HomeDemo");
        }

#region EventCallback
        public void OnDoorOpen(string doorName)
        {
            if (doorName == "DoorHouse")
            {
                //SwitchState(GameState.dust);
                Debug.Log("StoryBoard DoorHouse OnDoorOpen");
            }
            else if (doorName == "DoorBedroom")
            {
                //SwitchState(GameState.bedroom);
                Debug.Log("StoryBoard DoorBedroom OnDoorOpen");
            }
            else if (doorName == "DoorBathroom")
            {
                //SwitchState(GameState.bathroom);
                Debug.Log("StoryBoard DoorBathroom OnDoorOpen");
            }
        }
        public void OnGuideCompleted(string guideName)
        {
            if (guideName == "Clock")
            {
                Debug.Log("StoryBoard Clock OnGuideCompleted");
                m_stateController.SwitchState(GameState.clock);
            }
            else if (guideName == "DoorBedroom")
            {
                Debug.Log("StoryBoard DoorBedroom OnGuideCompleted");
                m_stateController.SwitchState(GameState.bedroom);
            }
            else if (guideName == "DoorBathroom")
            {
                Debug.Log("StoryBoard DoorBathroom OnGuideCompleted");
                m_stateController.SwitchState(GameState.bathroom);
            }
            else if (guideName == "DoorHouse")
            {
                Debug.Log("StoryBoard DoorHouse OnGuideCompleted");
                m_stateController.SwitchState(GameState.dust);
            }
            else if (guideName == "GuideSofa")
            {
                Debug.Log("StoryBoard DoorBathroom OnGuideCompleted");
                m_stateController.SwitchState(GameState.night);
            }
            else if (guideName == "Kira_Back")
            {
                Debug.Log("StoryBoard KiraBack OnGuideCompleted");
                m_stateController.SwitchState(GameState.exit);
            }
        }
#endregion

#region Common
        private void Start()
        {
            Debug.Log("StoryBoard Start");
            m_stateController.SwitchState(GameState.start);
        }
        private void Awake()
        {
            m_stateController = GetComponent<StoryBoard>();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
        }
#endregion
    }
}
