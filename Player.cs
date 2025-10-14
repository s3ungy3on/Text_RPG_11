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
        public string Name { get; set; }
        public int Level { get; set; }
        public string Job { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int HP { get; set; }
        public int Gold { get; set; }

        public Player(string name, int level, string job, int attack, int defense, int hp, int gold)
        {
            Name = name;
            Level = level;
            Job = job;
            Attack = attack;
            Defense = defense;
            HP = hp;
            Gold = gold;
        }
        public void DisplayInfo()
        {
            Console.WriteLine("=== 캐릭터의 정보가 표시됩니다 ===");
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
