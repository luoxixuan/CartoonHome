using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GalGameDialog
{
    public class DialogManage : MonoBehaviour
    {
        public static DialogManage instance;
        private bool m_bIsStartDialog;              // 是否开始对话
        private List<DialogNodeConfig> m_arrDialogArr;    // 对话List
        private DialogNodeConfig m_curDialogNode;         // 当前对话Node
        private int m_iCurDialogItemIndex;          // 当前对话Item索引

        private Text m_tTalkText;
        private Image m_imgIconImage;

        // Use this for initialization
        void Start()
        {
            instance = this;
            m_tTalkText = transform.Find("TalkText").GetComponent<Text>();
            m_imgIconImage = transform.Find("IconImage").GetComponent<Image>();
            LoadDialogs();

            gameObject.SetActive(false);
            m_bIsStartDialog = false;
        }

        public void clickToNextDialog()
        {
            nextDialog();
        }

        private void LoadDialogs()
        {
            m_arrDialogArr = GameConfig.instance.dialogConfigs;
        }

        //下一段话
        private void nextDialog()
        {
            // 开始对话
            if (true == m_bIsStartDialog)
            {
                // 有下一对话
                if (m_iCurDialogItemIndex < m_curDialogNode.DialogArr.Count)
                {
                    ShowDialogText(m_curDialogNode.DialogArr[m_iCurDialogItemIndex]);
                }
                else
                {
                    EndDialog();
                }
            }
        }

        // Update is called once per frame
        // TODO 对话触发时应该剥夺人物行动能力
        void Update()
        {
            // 暂时做的键盘
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                //nextDialog();
                EndDialog();
            }
        }

        // 根据名称 获取对话Node
        public DialogNodeConfig GetDialogNodeByID(int dialogID)
        {
            //Debug.Log("DialogManage::GetDialogNodeByName: " + name);
            if (dialogID != 0 && null != m_arrDialogArr)
            {
                for (int i = 0; i < m_arrDialogArr.Count; i++)
                {
                    //Debug.Log("DialogManage::GetDialogNodeByName遍历输出json对话名字：" + m_arrDialogArr[i].DialogName);
                    if (m_arrDialogArr[i].DialogID == dialogID)
                    {
                        //Debug.Log("DialogManage::GetDialogNodeByName: 取到了对话内容，第一句话是" + m_arrDialogArr[i].DialogArr[0].DialogText);
                        return m_arrDialogArr[i];
                    }
                }
            }
            return null;
        }

        // 根据名称 开始对话
        public void StartDialogByID(int dialogID)
        {
            //Debug.Log("DialogManage::StartDialogByName：" + name);
            DialogNodeConfig tempNode = GetDialogNodeByID(dialogID);
            if (null == tempNode)
            {
                //Debug.Log("DialogManage::StartDialogByName：" + name + " tempNode is null");
                return;
            }

            if (null == tempNode.DialogArr || tempNode.DialogArr.Count <= 0)
            {
                //Debug.Log("DialogManage::StartDialogByName：" + name + " tempNode.DialogArr is no string");
                return;
            }

            gameObject.SetActive(true);
            m_curDialogNode = tempNode;
            m_iCurDialogItemIndex = 0;
            ShowDialogText(tempNode.DialogArr[m_iCurDialogItemIndex]);
            m_bIsStartDialog = true;
        }

        public void EndDialog()
        {
            m_bIsStartDialog = false;
            m_iCurDialogItemIndex = 0;
            m_curDialogNode = null;
            gameObject.SetActive(false);
            //Debug.Log("DialogManage::EndDialog");
        }

        // 显示单个对话Item
        public void ShowDialogText(DialogItemConfig item)
        {
            m_tTalkText.text = item.Name + " : " + item.DialogText;
            Sprite sp = Resources.Load<Sprite>(item.IconPath);
            m_imgIconImage.sprite = sp;

            m_iCurDialogItemIndex++;
        }
    }
}