namespace SkillBridgeApp
{
    /// <summary>
    /// Представляет объявление о поиске навыка.
    /// </summary>
    public class RequestPost : BasePost
    {
        public string WantedSkill { get; set; } // что хочет получить взамен

        public RequestPost(int id, User author, string category, string description, string wantedSkill)
            : base(id, author, category, description)
        {
            WantedSkill = wantedSkill;
        }

        public override string GetDisplayText() => $"[ИЩУ] {Category}: {Description} (Хочу: {WantedSkill})";
    }
}
