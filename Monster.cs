using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    internal class Monster
    {
        public string Name { get; set; }
        public int Level { get; set; }

        public int MaxHP { get; set; }
        public int HP { get; set; }

        public int Attack { get; set; }
        public int Defense { get; set; }

        public int RewardExp { get; set; }
        public int RewardGold { get; set; }

        public bool isDead => HP <= 0;

        public Monster(string name, int level, int maxHP, int hP, int attack, int defense, int rewardExp, int rewardGold)
        {
            Name = name;
            Level = level;
            MaxHP = maxHP;
            HP = hP;
            Attack = attack;
            Defense = defense;
            RewardExp = rewardExp;
            RewardGold = rewardGold;
        }
    }
}
