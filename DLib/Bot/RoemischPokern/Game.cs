namespace DLib.Bot.RoemischPokern
{
    public class Game
    {
        int maxFilledFields;
        public int MaxFilledFields
        {
            get => maxFilledFields;
            set
            {
                maxFilledFields = System.Math.Max(maxFilledFields, value);
                if (value >= 7)
                    Finished = true;
            }
        }
        public bool Finished { get; set; }

        public Game()
        {
            maxFilledFields = 0;
            Finished = false;
        }
    }
}
