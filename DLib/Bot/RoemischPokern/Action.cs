using System.Linq;

namespace DLib.Bot.RoemischPokern
{
    public class Action
    {
        readonly bool joker;
        readonly int cardAction;
        public readonly int dice, diceCount, points, index;
        readonly Player player;

        public int Card
        {
            get
            {
                if (joker)
                    return 5;
                else if (cardAction == -1)
                    return -1;
                else if (cardAction == 0)
                    return 0;
                else if (cardAction == 1)
                    return dice == 0 ? 1 : 2;
                else
                    return cardAction + 1;
            }
        }
        /**
         * 0: beliebigen Würfel neu würfeln
         * 1: beliebige Anzahl an i/v neu würfeln
         * 2: ignorieren
         * 3: in benachbertes Feld aufschreiben
         */
        public bool UseCard => cardAction >= 0;
        public bool KeepUp => diceCount > 0;
        public bool RemoveDices => cardAction < 3 && cardAction >= 0;
        public bool WriteDown => index != -1;

        public Action(Player situation)
        {
            this.player = situation;
            cardAction = -1;
            joker = false;
            dice = -1;
            diceCount = 0;
            points = 0;
            index = -1;
        }

        public Action(Player situation, int diceCount)
        {
            this.player = situation;
            cardAction = -1;
            joker = false;
            dice = -1;
            this.diceCount = diceCount;
            points = 0;
            index = -1;
        }

        public Action(Player situation, int cardAction, bool joker, int dice, int diceCount)
        {
            this.player = situation;
            this.cardAction = cardAction;
            this.joker = joker;
            this.dice = dice;
            this.diceCount = diceCount;
            points = 0;
            index = -1;
        }

        public Action(Player situation, bool joker, int points, int index)
        {
            this.player = situation;
            cardAction = 3;
            this.joker = joker;
            dice = -1;
            diceCount = 0;
            this.points = points;
            this.index = index;
        }

        public Action(Player situation, int points, int index)
        {
            this.player = situation;
            cardAction = -1;
            joker = false;
            dice = -1;
            diceCount = 0;
            this.points = points;
            this.index = index;
        }

        public double ExpectedValue()
        {
            var dices = player.Dices.ToArray();
            var cards = player.Cards.ToArray();
            var table = player.Table.ToArray();
            if (UseCard)
                cards[Card] = false;
            if (KeepUp)
            {
                if (RemoveDices)
                    dices[dice] -= diceCount;
                double expected = 0;
                for (int i = 0; i <= diceCount; i++)
                    for (int v = 0; i + v <= diceCount; v++)
                        expected += new Player(player.game, new int[] { dices[0] + i, dices[1] + v, dices[2] + diceCount - i - v }, cards, table).GetBestAction().ExpectedValue() * System.Math.Pow(1 / 2f, i) * System.Math.Pow(1 / 6f, v) * System.Math.Pow(1 / 3f, diceCount - i - v);
                return expected;
            }
            else
            {
                if (WriteDown)
                    table[index] = points;
                return new Player(player.game, new int[3], cards, table).Points;
            }
        }
    }
}