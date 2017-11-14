using System;
using System.Collections.Generic;
using System.Linq;

namespace DLib.Bot.RoemischPokern
{
    public class Player
    {
        public readonly Game game;
        static int[] averageTable = new int[] { 8, 18, 23, 25, 28, 33, 35 };

        public int[] Dices { get; set; }
        public int[] Table { get; set; }
        public bool[] Cards { get; set; }
        public bool Playing { get; set; }
        public int Points
        {
            get
            {
                int points = 0;
                foreach (var value in Table)
                    points += value;
                return points + 5 * CardCount + (RomanNumberExists ? RomanNumbers[RomanNumbers.Length - 1] : 0);
            }
        }
        public int CardCount
        {
            get
            {
                int count = 0;
                foreach (var card in Cards)
                    if (card)
                        count++;
                return count;
            }
        }
        public int FilledFields
        {
            get
            {
                int count = 0;
                foreach (var value in Table)
                    if (value != 0)
                        count++;
                return count;
            }
        }
        int[] RomanNumbers
        {
            get
            {
                if (Dices[0] == 0 && Dices[1] == 0 && Dices[2] == 0)
                    return new int[] { 0 };
                else if (Dices[2] == 4 && Dices[1] == 0 && Dices[0] == 1)
                    return new int[] { 39 };
                else if (Dices[0] > 3 || Dices[1] > 1 || Dices[2] > 3)
                    return new int[0];
                else if (Dices[0] == 1 && (Dices[1] > 0 || Dices[2] > 0))
                    return new int[] { 10 * Dices[2] + 5 * Dices[1] - Dices[0], 10 * Dices[2] + 5 * Dices[1] + Dices[0] };
                else
                    return new int[] { 10 * Dices[2] + 5 * Dices[1] + Dices[0] };
            }
        }
        bool RomanNumberExists => RomanNumbers.Length > 0;
        bool IsPlaceOver
        {
            get
            {
                var points = RomanNumbers;
                if (points.Length > 0)
                {
                    for (int i = 6; i >= 0 && (Table[i] > points[0] || Table[i] == 0); i--)
                        if (Table[i] == 0)
                            return true;
                    return false;
                }
                else
                    return false;
            }
        }

        public Player(Game game)
        {
            this.game = game;
            Dices = new int[3];
            Table = new int[7];
            Cards = new bool[] { true, true, false, false, false, false };
        }

        public Player(Game game, int[] dices, bool[] cards, int[] table)
        {
            this.game = game;
            Dices = dices.ToArray();
            Table = table.ToArray();
            Cards = cards.ToArray();
        }

        public void PlayARound()
        {
            if (!game.Finished)
            {
                Playing = true;
                while (true)
                {
                    var action = GetBestAction();
                    if (action.UseCard)
                        Cards[action.Card] = false;
                    if (action.KeepUp)
                    {
                        if (action.RemoveDices)
                            Dices[action.dice] -= action.diceCount;
                        var random = new Random();
                        for (int i = 0; i < action.diceCount; i++)
                        {
                            int r = random.Next(6);
                            Dices[r < 3 ? 0 : r == 3 ? 1 : 2]++;
                        }
                    }
                    else
                    {
                        if (action.WriteDown)
                            Table[action.index] = action.points;
                        break;
                    }
                }
                Dices = new int[3];
                game.MaxFilledFields = FilledFields;
                if (CardCount == 0)
                    game.Finished = true;
                Playing = false;
            }
        }

        public Action GetBestAction()
        {
            var actions = new List<Action>();
            if (Dices[0] + Dices[1] + Dices[2] < 6 && ((Dices[0] == 0 && Dices[1] == 0 && Dices[2] == 4) || IsPlaceOver))
                actions.Add(new Action(this, 1));
            if (Cards[0] || Cards[5])
                for (int dice = 0; dice < 3; dice++)
                    if (Dices[dice] > 0)
                        actions.Add(new Action(this, 0, !Cards[0], dice, 1));
            if ((Cards[1] || Cards[5]) && IsPlaceOver)
                for (int diceCount = 1; Dices[0] >= diceCount; diceCount++)
                    actions.Add(new Action(this, 1, !Cards[1], 0, diceCount));
            if (Cards[2] || Cards[5])
                for (int diceCount = 1; Dices[1] >= diceCount; diceCount++)
                    actions.Add(new Action(this, 1, !Cards[2], 1, diceCount));
            if (Cards[3] || Cards[5])
                for (int dice = 0; dice < 3; dice++)
                    if (Dices[dice] > 0)
                        actions.Add(new Action(this, 2, !Cards[3], dice, 1));
            foreach (var point in RomanNumbers)
                if (point != 0)
                {
                    int index = GetIndex();
                    if (index != -1)
                        actions.Add(new Action(this, point, index));
                    else if ((Cards[4] || Cards[5]))
                    {
                        index = GetIndexCard();
                        if (index != -1)
                            actions.Add(new Action(this, !Cards[4], point, index));
                    }

                    int GetIndex()
                    {
                        int x = 6, i = -1, min = int.MaxValue;
                        for (; x > 0 && (Table[x - 1] == 0 || Table[x - 1] >= point); x--) ;
                        for (; x < 6 && (Table[x + 1] == 0 || Table[x + 1] < point); x++)
                            if (Table[x] == 0 && System.Math.Abs(averageTable[x] - point) < min)
                            {
                                min = System.Math.Abs(averageTable[x] - point);
                                i = x;
                            }
                        return i;
                    }

                    int GetIndexCard()
                    {
                        int i = Table.ToList().IndexOf(point);
                        if (i > 0 && i < 7)
                        {
                            if (Table[i - 1] == 0 && Table[i + 1] == 0)
                                return System.Math.Abs(averageTable[i - 1] - point) - System.Math.Abs(averageTable[i + 1] - point) < 0 ? i - 1 : i + 1;
                            else if (Table[i - 1] == 0)
                                return i - 1;
                            else if (Table[i + 1] == 0)
                                return i + 1;
                            else
                                return -1;
                        }
                        else if (i == 0 && Table[i + 1] == 0)
                            return i + 1;
                        else if (i == 7 && Table[i - 1] == 0)
                            return i - 1;
                        else
                            return -1;
                    }
                }
            if (actions.Count == 0)
                return new Action(this);
            else
            {
                var best = new Action(this);
                var max = best.ExpectedValue();
                foreach (var action in actions)
                {
                    var expectedValue = action.ExpectedValue() - (action.UseCard ? System.Math.Sqrt((7 - game.MaxFilledFields) * (6 - CardCount)) : 0);
                    if (expectedValue > max)
                    {
                        best = action;
                        max = expectedValue;
                    }
                }
                return best;
            }
        }
    }
}