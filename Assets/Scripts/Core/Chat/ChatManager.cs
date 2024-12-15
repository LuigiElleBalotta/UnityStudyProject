using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public int maxMessages = 25;

    public GameObject chatPanel, textObject;
    public InputField chatBox;

    [SerializeField]
    List<Message> messages = new List<Message>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (chatBox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToChatUI(chatBox.text);
                chatBox.text = "";
            }
        }
        else
        {
            if (!chatBox.isFocused && Input.GetKeyDown(KeyCode.Return))
            {
                chatBox.ActivateInputField();
            }
        }   
    }

    public void SendMessageToChatUI(string text)
    {
        // Check if we are using an emote
        if (text.StartsWith("/emote "))
        {
            // Get the player
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            player.GetComponent<UnitStats>().TryPlayEmote(CharacterAnimationBase.AnimationStates.EmoteMode, text.Replace("/emote ", ""));
        } else
        {
            if (messages.Count >= maxMessages)
            {
                Destroy(messages[0].textObject.gameObject);
                messages.Remove(messages[0]);
            }

            Message msg = new Message();
            msg.text = text;

            GameObject newText = Instantiate(textObject, chatPanel.transform);
            msg.textObject = newText.GetComponent<Text>();
            msg.textObject.text = msg.text;

            messages.Add(msg);
        }        
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
}
