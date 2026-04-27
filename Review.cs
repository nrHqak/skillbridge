namespace SkillBridgeApp
{
    /// <summary>
    /// Представляет отзыв о пользователе.
    /// </summary>
    public class Review
    {
        public User From { get; set; }
        public User To { get; set; }
        public int Stars { get; set; } // 1–5
        public string Comment { get; set; }

        public Review(User from, User to, int stars, string comment)
        {
            From = from;
            To = to;
            Stars = stars;
            Comment = comment;
        }
    }
}
