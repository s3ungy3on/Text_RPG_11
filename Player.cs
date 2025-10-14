using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Text_RPG_11
{
    internal class Player
    {
        public void DisplayInfo()
        {
            Console.WriteLine("=== 플레이어 정보 ===");
            Console.WriteLine($"이름: {Name}");
            Console.WriteLine($"직업: {Job}");
            Console.WriteLine($"레벨: {Level:00}"); 
            Console.WriteLine($"공격력: {Attack}");
            Console.WriteLine($"방어력: {Defense}");
            Console.WriteLine($"체력: {HP}");
            Console.WriteLine($"골드: {Gold}");
            Console.WriteLine("====================");
        }
    }
}
