namespace StackLandsLike.Cards
{
    public interface ICreatureCard : ICard
    {
        public int health { get; }
        
        public int maxHealth { get; }
        
        public int attack { get; set; }
        
        public int defense { get; set; }
    }
}