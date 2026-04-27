using System;

namespace SkillBridgeApp
{
    /// <summary>
    /// Базовый абстрактный класс для объявлений о навыках.
    /// </summary>
    public abstract class BasePost
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        public BasePost(int id, User author, string category, string description)
        {
            Id = id;
            Author = author;
            Category = category;
            Description = description;
            CreatedAt = DateTime.Now;
            IsActive = true;
        }

        public abstract string GetDisplayText(); // полиморфизм
    }
}
