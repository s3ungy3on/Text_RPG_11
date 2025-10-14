using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    internal class UIManager
    {
        public void Intro()
        {
            Console.WriteLine("스파르타 마을에 오신 용사님 환영합니다." +
                "\n용사님의 이름은 무엇인가요.\n");
            Console.Write(">>");
            string name = Console.ReadLine();
            Console.Clear();
            Console.WriteLine($"{name}이름이 맞으십니까?\n\n1. 맞습니다\n2. 아닙니다\n\n");
            string num = Console.ReadLine();
            Console.Clear();
            while (true)
            {
                if (num == "1")
                {
                    break;
                }
                else if (num == "2")
                {
                    Console.Clear();
                    Console.WriteLine("용사님의 이름을 다시 알려주십시오\n\n");
                    name = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("용사란 놈이 이거 하나 제대로 못하나\n\n");
                }
                Console.WriteLine($"{name}이름이 맞으십니까?\n\n1. 맞습니다\n2. 아닙니다\n\n");
                num = Console.ReadLine();
            }
            Console.WriteLine($"{name} 용사님 과연 이름부터가 휘황찬란하시군요\n헌데 용사님의 직업은 무엇인지요\n\n" +
                $"1. 전사, 2. 마법사, 3. 궁수");
            Console.Write(">>");
            string job = Console.ReadLine();
            while (true)
            {
                if (job == "1")
                {
                    Console.Clear(); job = "전사"; break;
                }
                else if (job == "2")
                {
                    Console.Clear(); job = "마법사"; break;
                }
                else if (job == "3")
                {
                    Console.Clear(); job = "궁수"; break;
                }
                else
                {
                    Console.Clear(); Console.WriteLine("잘못 입력하셨습니다. 다시 입력하십시오\n\n1. 전사, 2. 마법사, 3. 궁수");
                    Console.Write(">>"); job = Console.ReadLine();
                }
            }
            Console.WriteLine($"{job}시라니 정말 대단한 직업이군요.");
        }
    }
}
