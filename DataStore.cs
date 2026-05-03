using System;
using System.Collections.Generic;
using System.Linq;

namespace SkillBridgeApp
{
    /// <summary>
    /// Статический класс для хранения всех данных приложения в памяти.
    /// </summary>
    public static class DataStore
    {
        private static readonly string[] KnownCategories = { "IT", "Арт", "Языки", "Ремонт" };
        public static List<User> Users { get; set; } = new List<User>();
        public static List<BasePost> Posts { get; set; } = new List<BasePost>();
        public static List<Review> Reviews { get; set; } = new List<Review>();
        public static List<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

        public static User CurrentUser { get; set; }

        public static void Initialize()
        {
            // Тестовые данные
            Users.Add(new User(1, "Алексей", "alexey@example.com") { Skills = { "Python", "IT" }, Rating = 4.5 });
            Users.Add(new User(2, "Даша", "dasha@example.com") { Skills = { "Рисование", "Арт" }, Rating = 3.8 });
            Users.Add(new User(3, "Алина", "alina@example.com") { Skills = { "Английский", "Языки" }, Rating = 4.2 });
            Users.Add(new User(4, "Кирилл", "kirill@example.com") { Skills = { "Ремонт велосипеда" }, Rating = 3.5 });
            Users.Add(new User(5, "Саша", "sasha@example.com") { Skills = { "Готовка", "Творчество" }, Rating = 4.9 });

            // Объявления
            Posts.Add(new OfferPost(1, Users[0], "IT", "Предлагаю уроки Python для начинающих"));
            Posts.Add(new RequestPost(2, Users[1], "IT", "Ищу того, кто научит основам C#", "C#"));
            Posts.Add(new OfferPost(3, Users[2], "Языки", "Помогу с английским языком, подготовка к IELTS"));
            Posts.Add(new RequestPost(4, Users[0], "Арт", "Хочу научиться рисовать портреты", "Рисование"));
            Posts.Add(new OfferPost(5, Users[1], "Арт", "Научу рисовать акварелью"));
            Posts.Add(new RequestPost(6, Users[2], "IT", "Ищу помощь с SQL базами данных", "SQL"));
            Posts.Add(new OfferPost(7, Users[3], "Ремонт", "Ремонтирую велосипеды любой сложности"));
            Posts.Add(new RequestPost(8, Users[4], "Языки", "Ищу репетитора по французскому", "Французский"));
            Posts.Add(new OfferPost(9, Users[0], "IT", "Помогу с настройкой Linux серверов"));

            // Чат сообщения
            ChatMessages.Add(new ChatMessage(Users[0], Users[1], "Привет, Даша! Я видел твое объявление про C#. Могу помочь."));
            ChatMessages.Add(new ChatMessage(Users[1], Users[0], "Отлично, Алексей! Когда тебе удобно?"));

            // Убедимся, что есть минимум 2 взаимных совпадения для демонстрации алгоритма
            // Алексей предлагает Python, ищет Рисование
            // Даша предлагает Рисование, ищет C#
            // Здесь нужно создать такое, чтобы сработало FindMatches
            // Пример: Пользователь А предлагает X и ищет Y. Пользователь Б предлагает Y и ищет X.
            // User 1 (Алексей) offers IT (Python), requests Art (Рисование)
            // User 2 (Даша) offers Art (Рисование), requests IT (C#)
            // This setup should create a match.
        }

        /// <summary>
        /// Алгоритм поиска взаимных совпадений.
        /// </summary>
        /// <param name="currentUser">Текущий пользователь, для которого ищутся совпадения.</param>
        /// <returns>Список пар пользователей, у которых есть взаимные интересы.</returns>
        public static List<(User offeringUser, User requestingUser, OfferPost offer, RequestPost request)> FindMatches(User currentUser)
        {
            var myOffers = Posts.OfType<OfferPost>().Where(p => p.Author == currentUser && p.IsActive).ToList();
            var myRequests = Posts.OfType<RequestPost>().Where(p => p.Author == currentUser && p.IsActive).ToList();

            var result = new List<(User, User, OfferPost, RequestPost)>();
            var uniquePairs = new HashSet<string>();

            foreach (var myOffer in myOffers)
            {
                foreach (var myRequest in myRequests)
                {
                    // Совпадение: мой offer-категория = категория запроса другого пользователя,
                    // а категория моего запроса = категория offer другого пользователя.
                    var potentialMatches = Posts.OfType<OfferPost>()
                        .Where(otherOffer => otherOffer.Author != currentUser && otherOffer.IsActive)
                        .Join(
                            Posts.OfType<RequestPost>().Where(otherRequest => otherRequest.Author != currentUser && otherRequest.IsActive),
                            offer => offer.Author,
                            request => request.Author,
                            (offer, request) => new { Offer = offer, Request = request })
                        .Where(x =>
                            IsCategoryMatch(x.Request.Category, myOffer.Category, x.Request.WantedSkill) &&
                            IsCategoryMatch(x.Offer.Category, myRequest.Category, myRequest.WantedSkill))
                        .ToList();

                    foreach (var match in potentialMatches)
                    {
                        string key = $"{currentUser.Id}:{myOffer.Id}:{myRequest.Id}:{match.Offer.Author.Id}:{match.Offer.Id}:{match.Request.Id}";
                        if (uniquePairs.Add(key))
                        {
                            result.Add((currentUser, match.Offer.Author, myOffer, match.Request));
                        }
                    }
                }
            }

            return result;
        }

        private static bool IsCategoryMatch(string sourceCategory, string targetCategory, string additionalText)
        {
            if (string.Equals(sourceCategory, targetCategory, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            string inferred = InferCategory(additionalText);
            return !string.IsNullOrEmpty(inferred) &&
                   string.Equals(inferred, targetCategory, StringComparison.OrdinalIgnoreCase);
        }

        private static string InferCategory(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            string normalized = text.ToLowerInvariant();

            if (normalized.Contains("c#") || normalized.Contains("sql") || normalized.Contains("python") || normalized.Contains("linux") || normalized.Contains("it"))
                return "IT";
            if (normalized.Contains("рис") || normalized.Contains("арт") || normalized.Contains("акварел") || normalized.Contains("портрет"))
                return "Арт";
            if (normalized.Contains("англ") || normalized.Contains("франц") || normalized.Contains("язык"))
                return "Языки";
            if (normalized.Contains("ремонт") || normalized.Contains("велосипед"))
                return "Ремонт";

            return KnownCategories.FirstOrDefault(c => normalized.Contains(c.ToLowerInvariant())) ?? string.Empty;
        }
    }
}
