using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    public class Messages
    {
        private GameManager gameManager;
        public Messages(GameManager manager)
        {
            gameManager = manager;
        }
        public static void TextTitleHlight(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void TextMagentaHlight(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void Equipped(bool isEquipped)
        {
            if (isEquipped)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("E");
                Console.ResetColor();
                Console.Write("]");
            }
        }


        public static int ReadInput(int min, int max)
        {
            while (true)
            {
                string input = Console.ReadLine();
                int number;

                if(!int.TryParse(input, out number))
                {
                    Console.WriteLine("잘못된 입력입니다. 숫자를 입력해주세요.");
                    continue;
                }

                if(number < min || number > max)
                {
                    Console.WriteLine($"잘못된 입력입니다. {min}~{max} 사이의 숫자를 입력해주세요.");
                    continue;
                }

                return number;
            }
        }

        public string PaddingKorean_Right(string str, int width)
        {
            int curWidth = 0;

            foreach (char c in str)
            {
                curWidth += c <= 127 ? 1 : 2;
            }

            int padding = width - curWidth;

            return str + new string(' ', Math.Max(0, padding));
        }
    }
}
