using System.Collections.Generic;

namespace SkillBridgeApp
{
    /// <summary>
    /// Представляет пользователя платформы SkillBridge.
    /// Содержит профиль, навыки и рейтинг.
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public double Rating { get; set; } = 0.0;
        public List<string> Skills { get; set; } = new List<string>();

        public User(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}
