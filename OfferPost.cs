namespace SkillBridgeApp
{
    /// <summary>
    /// Представляет объявление о предложении навыка.
    /// </summary>
    public class OfferPost : BasePost
    {
        public OfferPost(int id, User author, string category, string description) 
            : base(id, author, category, description) { }

        public override string GetDisplayText() => $"[ПРЕДЛАГАЮ] {Category}: {Description}";
    }
}
