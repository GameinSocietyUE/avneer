using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DisplayChat : Displayable
{
    public enum Side {
        Bot,
        User,
    }

    public static DisplayChat Instance;

    public string debug_message;
    public Side debug_side;

    public ScrollRect ScrollRect;
    public RectTransform ScrollRect_RectTransform;
    [SerializeField] private Transform message_parent;
    [SerializeField] private ChatInfo[] chatInfos;
    [SerializeField] private NetworkManager networkManager;

    public Dictionary<string, DisplayChatMessage> messagesDisplay = new Dictionary<string, DisplayChatMessage>();

    [System.Serializable]
    public class ChatInfo {
        public DisplayChatMessage prefab;
        public List<DisplayChatMessage> pool;
        public int index;
    }

    private void Awake() {
        Instance = this;
    }


    public override void Show() {
        base.Show();
        /*foreach (var item in chatInfos) {
            foreach (var dM in item.pool) {
                dM.Hide();
            }
            item.index = 0;
        }
        Debug.Log($"ONCE");
        StartCoroutine(networkManager.GetChatData());*/
    }

    public void ResetChatAndShow()
    {
        base.Show();
        foreach (var item in chatInfos)
        {
            foreach (var dM in item.pool)
            {
                dM.Hide();
            }
            item.index = 0;
        }
        Debug.Log($"ONCE");
        //StartCoroutine(networkManager.GetChatData());
        networkManager.GetChatScenario("1ee89ec4-1209-6df4-a407-f328a7e43d7c");
    }

    public void AddMessage(QuestionData message, Side side) {

        var info = chatInfos[(int)side];

        if ( info.index <= info.pool.Count) {
            info.pool.Add(Instantiate(info.prefab, message_parent));
        }

        var displayMessage = info.pool[info.index];
        displayMessage.Display(message.label);
        messagesDisplay.Add(message.id, displayMessage);
        ++info.index;
    }

    public void StopWaitingAnim(string messageId)
    {
        if (messagesDisplay.ContainsKey(messageId))
        {
            messagesDisplay[messageId].StopWaitingAnim();
        }
    }
}
