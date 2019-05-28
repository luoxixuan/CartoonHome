using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GalGameDialog
{
    using System.Collections.Generic;		//Allows us to use Lists. 

    public struct PlayerItem {
        public int itemNum { get; set; }
        public GameItemConfig itemData { get; set; }
    }

    public class GameManager : MonoBehaviour
    {

        public int m_dialog = 1;                          //播放哪个对话
        public static GameManager instance = null;        //Static instance of GameManager which allows it to be accessed by any other script.
        [HideInInspector]
        public bool playerInDialog = false;   //Boolean to check if players is in dialog, hidden in inspector but public.
        [HideInInspector]
        public bool doingSomething = false;   //Boolean to check if players is doSomething, hidden in inspector but public.
        
        private Dictionary<int,PlayerItem> m_playersItems;      //player's items.
        

        //Awake is always called before any Start functions
        void Awake()
        {
            //Check if instance already exists
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);

            //Call the InitGame function to initialize the first level 
            InitGame();
        }

        //this is called only once, and the paramter tell it to be called only after the scene was loaded
        //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
        /*
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static public void CallbackInitialization()
        {
            //register the callback to be called everytime the scene is loaded
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        //This is called each time a scene is loaded.
        static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            instance.InitGame();
        }
        */


        void loadGameConfig()
        {
            m_playersItems = new Dictionary<int, PlayerItem>();
            PlayerItem playerItem = new PlayerItem();
            foreach (GameItemConfig itemConfig in GameConfig.instance.itemConfigs)
            {
                // 把物品表也初始化一下
                playerItem.itemData = itemConfig;
                playerItem.itemNum = 0;
                m_playersItems.Add(itemConfig.itemID, playerItem);
            }
        }

        //Initializes the game for each level.
        void InitGame()
        {
            loadGameConfig();
        }

        //Update is called every frame.
        public bool dialog = true;
        void Update()
        {
            if (dialog)
            {
                dialog = false;
                playDialog();
            }
        }

        void playDialog()
        {
            DialogManage.instance.StartDialogByID(m_dialog);
        }
        public bool DelPlayerItem(int itemID)
        {
            bool flag = false;
            
            if (m_playersItems.ContainsKey(itemID))
            {
                if (m_playersItems[itemID].itemNum == 0) {
                    return false; //没这物品，删除失败
                }

                PlayerItem playerItem = m_playersItems[itemID];
                playerItem.itemNum--;
                m_playersItems[itemID] = playerItem; //蛋疼，只能改了再赋值回去
                Debug.Log("删除物品：" + playerItem.itemData.itemName);
                flag = true;
            }

            return flag;
        }

        public bool AddItemToPlayer(int itemID)
        {
            bool flag = false;

            if (m_playersItems.ContainsKey(itemID))
            {
                PlayerItem playerItem = m_playersItems[itemID];
                playerItem.itemNum++;
                m_playersItems[itemID] = playerItem; //蛋疼，只能改了再赋值回去
                flag = true;

                Debug.Log("获得物品：" + playerItem.itemData.itemName);
            }

            return flag; //如果物品表没这物品就返回失败
        }

        public bool PlayHasItem(int itemID, int num)
        {
            bool flag = false;

            if (m_playersItems.ContainsKey(itemID))
            {
                if (m_playersItems[itemID].itemNum >= num)
                {
                    flag = true;
                    Debug.Log("玩家有足够的物品：" + m_playersItems[itemID].itemData.itemName);
                }
            }

            return flag;
        }

        //GameOver is called when the player reaches 0 food points
        public void GameOver()
        {
            //Disable this GameManager.
            enabled = false;
            
        }

        //Coroutine to move enemies in sequence.
        IEnumerator DoSomeThing()
        {
            yield return new WaitForSeconds(0.1f);
        }
    }
}

