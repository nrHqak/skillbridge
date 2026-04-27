using System;

namespace SkillBridgeApp
{
    /// <summary>
    /// Представляет сообщение в чате между пользователями.
    /// </summary>
    public class ChatMessage
    {
        public User From { get; set; }
        public User To { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }

        public ChatMessage(User from, User to, string text)
        {
            From = from;
            To = to;
            Text = text;
            Time = DateTime.Now;
        }
    }
}
