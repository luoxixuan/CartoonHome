using System.Collections.Generic;
using UnityEngine;

namespace GalGameDialog
{
    public class SceneDecorateItem : MonoBehaviour
    {
        public GameObject TouchItemPrefab;
        
        public List<Sprite> m_lDecList; //
        public bool m_bNeedFall;        // 是否需要倒下

        [Tooltip("物体能够触发的事件的id数组")]
        public List<int> m_eventIDs = null;   //事件的id数组

        private List<GameEventConfig> m_eventList = null; //可触发事件的列表,在运行时根据在unity编辑器里配置的IDs数组赋值
        //private int m_eventCanTriggerCount = 0; //当前状态可触发的事件数量,用于判断要不要出交互item
        private SpriteRenderer m_spRenderer;
        private PolygonCollider2D m_collider;
        private Vector3 m_vEulerRotation;

        private GameObject m_gColliderGo;
        private GameObject m_touchItem;

        private bool m_onTrigger = false; //不是立即触发东西，加个状态标记下，当前有没有在屏幕上显示触发开关

        // Use this for initialization
        void Start()
        {
            m_spRenderer = this.GetComponent<SpriteRenderer>();
            m_collider = this.GetComponent<PolygonCollider2D>();

            if (null == TouchItemPrefab) //如果场景中没有预设这边就用交互物品
            {
                TouchItemPrefab = (GameObject)Resources.Load("Prefabs/TouchMe");
            }
            m_eventList = new List<GameEventConfig>();
            //添加事件，好像效率有点低，需要循环两个表
            if (m_eventIDs != null)
            {
                foreach (int id in m_eventIDs)
                {
                    foreach (GameEventConfig e in GameConfig.instance.eventConfigs)
                    {
                        if (e.eventID == id)
                        {
                            
                            m_eventList.Add(e);
                        }
                    }
                }
            }
            //m_eventCanTriggerCount = m_eventList.Count; //游戏最开始不知道有几个能触发。。
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void doDecorateItemEvent() {
            bool deleteEventTrigger = false;
            //m_eventCanTriggerCount = m_eventList.Count;
            if (null != m_eventList && m_eventList.Count > 0) {
                for (int i = 0; i < m_eventList.Count; )
                {
                    GameEventConfig e = m_eventList[i];
                    deleteEventTrigger = doEventTrigger(e);
                    if (deleteEventTrigger)
                    {
                        m_eventList.Remove(e); //需要删除的事件
                    }
                    else
                    {
                        i++;
                    }

                }
            }

            
        }

        private bool isEventTrigger(List<EventTriggerState> eventTriggers)
        {
            if (eventTriggers == null || eventTriggers.Count <= 0) return true; //Count == 0说明不需要条件
            //如果有触发条件但是玩家对应的物品状态不符合那就不触发
            //大于0表示需要这个物品才能触发,小于0表示没有这物品才能触发
            bool flag = true;
            foreach (EventTriggerState trigger in eventTriggers)
            {
                switch (trigger.triggerType)
                {
                    case TriggerType.TrigType_None:
                        Debug.Log("Unknown TriggerType.TrigType_None!");
                        break;

                    case TriggerType.TrigType_Item:
                        if ((trigger.triggerID > 0 && !GameManager.instance.PlayHasItem(trigger.triggerID, trigger.triggerNum)) || (trigger.triggerID < 0 && GameManager.instance.PlayHasItem(-trigger.triggerID, trigger.triggerNum)))
                        {
                            return false; //只要一个条件不满足就直接返回false
                        }
                        break;

                    case TriggerType.TrigType_Event: //事件触发，现在还没有
                        break;

                    case TriggerType.TrigType_Talk: //对话触发，现在还没有
                        break;

                    default:
                        Debug.Log("Unknown EventTriggerState!");
                        break;
                }
                    
            }
            return flag; //能走到这说明条件全满足了
        }

        //加个返回值判断下事件触发了需不要删除
        bool doEventTrigger(GameEventConfig gEvent)
        {
            bool deleteEventTrigger = true; //默认触发了就删掉, 除非触发失败或者事件可以多次触发
            if (!isEventTrigger(gEvent.eventTriggers)) {
                return false; //不能触发事件就返回
            }
            DecorateType eventType = gEvent.eventType;
            //Debug.Log("SceneDecorateItem::doTrigger: " + m_eDecType);
            if (eventType == DecorateType.DecType_Random)
            {
                if (null != m_lDecList && m_lDecList.Count > 0)
                {
                    int randInt = Random.Range(0, m_lDecList.Count - 1);
                    m_spRenderer.sprite = m_lDecList[randInt];
                    if (true == m_bNeedFall)
                    {
                        if (m_gColliderGo.transform.position.x <= transform.position.x)
                        {
                            m_vEulerRotation = this.transform.localEulerAngles;
                            m_vEulerRotation.z = -90.0f;
                            this.transform.localEulerAngles = m_vEulerRotation;
                        }
                        else
                        {
                            m_vEulerRotation = this.transform.localEulerAngles;
                            m_vEulerRotation.z = 90.0f;
                            this.transform.localEulerAngles = m_vEulerRotation;
                        }
                    }

                }
            }
            else if (eventType == DecorateType.DecType_Talk)
            {
                if (null == DialogManage.instance)
                {
                    return false;
                }
                else if (gEvent.eventResult > 0)
                {
                    DialogManage.instance.StartDialogByID(gEvent.eventResult);
                }
            }
            else if (eventType == DecorateType.DecType_GetItem)
            {
                if (null == GameManager.instance)
                {
                    return false;
                }
                else if (gEvent.eventResult > 0)
                {
                    GameManager.instance.AddItemToPlayer(gEvent.eventResult);
                }
            }
            else if (eventType == DecorateType.DecType_GameOver)
            {
                if (null == GameManager.instance)
                {
                    return false;
                }
                else if (gEvent.eventResult > 0)
                {
                    //GameManager.instance.GameOver(); 暂时就让他自己走吧
                }
            }

            destroyTouchItem();
            if (deleteEventTrigger) {
                deleteEventTrigger = gEvent.isRepeat == 0; //可以重复触发的事件不能删除，不能重复触发的触发完删除
            }

            return deleteEventTrigger;
        }

        // 创建一个可以点击的叹号
        private void createTouchItem()
        {
            if (null == m_touchItem && m_eventList != null && m_eventList.Count > 0)
            {
                m_touchItem = Instantiate(TouchItemPrefab);
                m_touchItem.transform.parent = gameObject.transform.parent;
                m_touchItem.transform.position = gameObject.transform.position;
                TouchItem touchItem = m_touchItem.GetComponent<TouchItem>();
                touchItem.decorateItem = this;
                m_onTrigger = true;
            }
        }

        private void destroyTouchItem()
        {
            if (m_touchItem)
            {
                GameObject.Destroy(m_touchItem);
                m_touchItem = null;
                m_onTrigger = false; //销毁后把这个状态置空
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            m_gColliderGo = collision.gameObject;
            //Debug.Log("SceneDecorateItem::OnTriggerEnter2D: "+m_gColliderGo.name);
            if (!m_onTrigger) //可以重复触发的东西，再次进来的时候激活下，已经触发的就不再继续触发
                createTouchItem();
        }

        // 出去之后销毁
        private void OnTriggerExit2D(Collider2D collision)
        {
            //Debug.Log("SceneDecorateItem::OnTriggerExit2D: " + m_gColliderGo.name);
            destroyTouchItem();
        }

        // 碰撞时每帧检测
        private void OnTriggerStay2D(Collider2D collision)
        {
            //Debug.Log("SceneDecorateItem::OnTriggerStay2D: " + m_gColliderGo.name);
            if (m_onTrigger)
            {
                if (Input.GetKeyUp(KeyCode.Backspace))
                {
                    doDecorateItemEvent(); //pc上也可以用键盘触发
                }
            }
        }

        // 设置Trigger
        public void SetTrigger(bool value)
        {
            m_collider.isTrigger = value;
        }
    }
}