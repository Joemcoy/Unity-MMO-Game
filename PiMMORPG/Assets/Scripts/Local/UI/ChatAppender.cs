using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using Devdog.InventoryPro;

using PiMMORPG.Client;
using tFramework.Factories;

using Scripts.Network.Requests;

namespace Scripts.Local.UI
{
    [RequireComponent(typeof(NoticeUI))]
    public class ChatAppender : MonoBehaviour
    {
        public InputField messageField;
        public NoticeUI chatUI;
        
        static Queue<string> MessageQueue = new Queue<string>();

        public static void AppendMessage(string Message)
        {
            lock(MessageQueue)
                MessageQueue.Enqueue(Message);
        }

        public void Send()
        {
            //if (messageField.isFocused)
            //{
            var message = messageField.text;
            if (!string.IsNullOrEmpty(message) || message.Replace(" ", "").Length > 0)
            {
                var client = PiBaseClient.Current;

                if (client.Socket.Connected)
                {
                    var Packet = new ChatRequest { Message = message };
                    client.Socket.Send(Packet);
                }
                else
                    AppendMessage("Not connected!");
                messageField.text = string.Empty;
            }
            //}
        }

        void Update()
        {
            lock (MessageQueue)
                if (MessageQueue.Count > 0)
                {
                    var Message = MessageQueue.Dequeue();
                    chatUI.AddMessage(Message, NoticeDuration.Short);
                    chatUI.scrollRect.normalizedPosition = Vector2.zero;
                }

            if (messageField.isFocused && messageField.text.Replace(" ", "").Length > 0 && (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter)))
                Send();
        }
    }
}
