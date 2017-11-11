using System;
using System.Collections.Generic;
using System.Linq;

namespace DLib.Math
{
    public static class Calculator
    {
        static (string op, int priority, string replacement, int paramCount, int segmentation, Func<double[], double> methode)[] Functions = new(string, int, string, int, int, Func<double[], double>)[] {
            ("+",  3, "Add",              2, 0,  a => a[0] + a[1]),
            ("-",  3, "Sub",              2, 0,  a => a[0] - a[1]),
            ("*",  2, "Mul",              2, 0,  a => a[0] * a[1]),
            ("/",  2, "Div",              2, 0,  a => a[0] / a[1]),
            ("%",  2, "Mod",              2, 0,  a => a[0] % a[1]),
            ("^",  1, "Pow",              2, 0,  a => System.Math.Pow(a[0], a[1])),
            ("√",  1, "Sqrt",             2, 0,  a => System.Math.Pow(a[1], 1 / a[0])),
            (null, 1, "Sin",              1, 0,  a => System.Math.Sin(a[0])),
            (null, 1, "Cos",              1, 0,  a => System.Math.Cos(a[0])),
            (null, 1, "Tan",              1, 0,  a => System.Math.Tan(a[0])),
            (null, 1, "ASin",             1, 0,  a => System.Math.Asin(a[0])),
            (null, 1, "ACos",             1, 0,  a => System.Math.Acos(a[0])),
            (null, 1, "ATan",             1, 0,  a => System.Math.Atan(a[0])),
            ("!",  1, "Fac",              1, -1, a => Extra.Factorial((ulong)a[0])),
            ("%",  1, "Per",              1, -1, a => a[0] / 100),
            ("p",  1, "Pos",              1, 1,  a => a[0]),
            ("m",  1, "Neg",              1, 1,  a => -a[0]),
            (null, 1, "Log",              2, 0,  a => System.Math.Log(a[1], a[0])),
            (null, 1, "Ln",               1, 0,  a => System.Math.Log(a[0])),
            ("π",  1, System.Math.PI.ToString(), 0, 0,  null),
            ("e",  1, System.Math.E.ToString(),  0, 0,  null)
        };

        public static double Solve(string termAsString)
        {
            var term = VorzeichenKorrektur(StringTermToList(termAsString));
            //zero paramters
            for (int i = 0; i < term.Count; i++)
                if (IsOperatorWithCond(term[i], j => Functions[j].paramCount == 0))
                {
                    string repl = GetReplacementFromOp(term[i]);
                    term.RemoveAt(i);
                    term.Insert(i, repl);
                }
            //pre one parameter
            for (int i = 0; i < term.Count; i++)
                if (IsOperatorWithCond(term[i], j => Functions[j].paramCount == 1 && Functions[j].segmentation == -1))
                {
                    string repl = GetReplacementFromOp(term[i]);
                    term.RemoveAt(i);
                    if (term[i - 1] == ")")
                        term.Insert(CounterBracketIndex(term, i - 1), repl);
                    else
                    {
                        term.Insert(i, ")");
                        Insert(ref term, i - 1, repl, "(");
                    }
                }
            //post one parameter
            for (int i = 0; i < term.Count; i++)
                if (IsOperatorWithCond(term[i], j => Functions[j].paramCount == 1 && Functions[j].segmentation == 1))
                {
                    string repl = GetReplacementFromOp(term[i]);
                    term.RemoveAt(i);
                    term.Insert(i, repl);
                    if (IsReplacement(term[i + 1]))
                    {
                        term.Insert(i + 1, "(");
                        term.Insert(CounterBracketIndex(term, i + 3), ")");
                    }
                    else if (term[i + 1] != "(")
                    {
                        term.Insert(i + 1, "(");
                        term.Insert(i + 3, ")");
                    }
                }
            //two parameters
            for (int p = 1, max = MaxPriorityOfOpWithCond(i => Functions[i].paramCount == 2 && Functions[i].op != null); p <= max; p++)
                for (int i = 0; i < term.Count; i++)
                    if (IsOperatorWithCond(term[i], j => Functions[j].paramCount == 2 && Functions[j].op != null && Functions[j].priority == p))
                    {
                        string repl = GetReplacementFromOp(term[i]);
                        term.RemoveAt(i);
                        term.Insert(i, ";");
                        if (IsReplacement(term[i + 1]))
                            term.Insert(CounterBracketIndex(term, i + 2), ")");
                        else if (term[i + 1] != "(")
                            term.Insert(i + 2, ")");
                        else
                            term.RemoveAt(i + 1);
                        if (term[i - 1] == ")")
                        {
                            int i2 = CounterBracketIndex(term, i - 1);
                            if (IsReplacement(term[i2 - 1]))
                                Insert(ref term, i2 - 1, repl, "(");
                            else
                            {
                                term.Insert(i2, repl);
                                term.RemoveAt(i);
                            }
                        }
                        else
                            Insert(ref term, i - 1, repl, "(");
                    }
            return SolveFunction(SimplifyBrackets(term));
        }

        static double SolveFunction(List<string> term)
        {
            if (term.Count == 1)
                return Convert.ToDouble(term[0]);
            var results = new List<double>();
            int begin = 2, i = 0;
            for (int k = 0; i < term.Count; i++)
                if (term[i] == "(")
                    k++;
                else if (term[i] == ")")
                    k--;
                else if (k == 1 && term[i] == ";")
                {
                    results.Add(SolveFunction(term.GetRange(begin, i - begin)));
                    begin = i + 1;
                }
            results.Add(SolveFunction(term.GetRange(begin, i - 1 - begin)));
            return GetMethodeFromFunc(term[0])(results.ToArray());
        }

        static List<string> SimplifyBrackets(List<string> term)
        {
            for (int i = 0; i < term.Count;)
                if (term[i] == "(" && (i - 1 < 0 || !IsReplacement(term[i - 1])))
                {
                    term.RemoveAt(CounterBracketIndex(term, i));
                    term.RemoveAt(i);
                }
                else
                    i++;
            return term;
        }

        static Func<double[], double> GetMethodeFromFunc(string func)
        {
            for (int i = 0; i < Functions.Length; i++)
                if (Functions[i].replacement == func)
                    return Functions[i].methode;
            return null;
        }

        static List<string> StringTermToList(string term)
        {
            var list = new List<string>();
            var array = term.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                string item = "";
                for (; i < array.Length && !(IsOperatorWithCond(array[i].ToString(), j => Functions[j].paramCount > 0 && Functions[j].op != null) || new string[] { "(", ")", ";" }.Contains(array[i].ToString())); item += array[i], i++) ;
                if (item != "")
                    list.Add(item);
                if (i < array.Length)
                    list.Add(array[i].ToString());
            }
            return list;
        }

        static List<string> VorzeichenKorrektur(List<string> term)
        {
            for (int i = 0; i < term.Count; i++)
                if ((term[i] == "+" || term[i] == "-") && (i - 1 < 0 || !(Extra.IsNumeric(term[i - 1]) || term[i - 1] == ")" || term[i - 1] == "!" || term[i - 1] == "%")))
                {
                    if (term[i] == "+")
                        term.RemoveAt(i--);
                    else
                        term[i] = "m";
                }
            return term;
        }

        static bool IsOperatorWithCond(string op, Func<int, bool> Cond) => Cond1AccomplishsCond2(i => Functions[i].op == op, Cond);

        static bool IsReplacement(string repl) => Cond1AccomplishsCond2(i => Functions[i].replacement == repl, i => Functions[i].paramCount > 0);

        static bool Cond1AccomplishsCond2(Func<int, bool> Cond1, Func<int, bool> Cond2)
        {
            for (int i = 0; i < Functions.Length; i++)
                if (Cond1(i))
                    return Cond2(i);
            return false;
        }

        static string GetReplacementFromOp(string op)
        {
            for (int i = 0; i < Functions.Length; i++)
                if (Functions[i].op == op)
                    return Functions[i].replacement;
            return "error";
        }

        static int MaxPriorityOfOpWithCond(Func<int, bool> Cond)
        {
            int max = 0;
            for (int i = 0; i < Functions.Length; i++)
                if (Cond(i))
                    max = System.Math.Max(max, Functions[i].priority);
            return max;
        }

        static int CounterBracketIndex(List<string> term, int index)
        {
            if (term[index] == "(")
            {
                index++;
                for (int j = 1; j > 0; index++)
                    if (term[index] == "(")
                        j++;
                    else if (term[index] == ")")
                        j--;
                return index - 1;
            }
            else if (term[index] == ")")
            {
                index--;
                for (int j = 1; j > 0; index--)
                    if (term[index] == ")")
                        j++;
                    else if (term[index] == "(")
                        j--;
                return index + 1;
            }
            else
                return -1;
        }

        static void Insert<T>(ref List<T> list, int index, params T[] items)
        {
            for (int i = 0; i < items.Length; i++)
                list.Insert(index + i, items[i]);
        }
    }
}
