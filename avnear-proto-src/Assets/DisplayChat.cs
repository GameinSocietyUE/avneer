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
    [SerializeField] private Transform message_parent;
    [SerializeField] private ChatInfo[] chatInfos;
    [SerializeField] private NetworkManager networkManager;

    [System.Serializable]
    public class ChatInfo {
        public DisplayChatMessage prefab;
        public List<DisplayChatMessage> pool;
        public int index;
    }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        DisplayAnswers.Instance.Hide();

    }

    public override void Show() {
        base.Show();
        foreach (var item in chatInfos) {
            foreach (var dM in item.pool) {
                dM.Hide();
            }
            item.index = 0;
        }
        Debug.Log($"ONCE");
        StartCoroutine(networkManager.GetChatData());
    }

    public void AddMessage(string message, Side side) {

        var info = chatInfos[(int)side];

        if ( info.index <= info.pool.Count) {
            info.pool.Add(Instantiate(info.prefab, message_parent));
        }

        var displayMessage = info.pool[info.index];
        displayMessage.Display(message);
        ++info.index;

    }
}
