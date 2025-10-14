namespace Text_RPG_11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player()
            {
                Name = "전사1",
                Job = "전사",
                Level = 1,
                Attack = 10,
                Defense = 5,
                HP = 100,
                Gold = 1000
            };

            player.DisplayInfo();
        }
    }
}
