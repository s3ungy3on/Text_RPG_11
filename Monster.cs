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

        public int MaxHP { get; set; } //최대체력
        public int HP { get; set; } //체력

        public int Attack { get; set; }
        public int Defense { get; set; }

        public int RewardExp { get; set; }
        public int RewardGold { get; set; }

        public bool isDead => HP <= 0; //몬스터 사망

        public Monster(string name, int level, int maxHP, int attack, int defense, int rewardExp = 0, int rewardGold = 0)
        {
            Name = name;
            Level = level;
            MaxHP = maxHP;
            HP = MaxHP;
            Attack = attack;
            Defense = defense;
            RewardExp = rewardExp;
            RewardGold = rewardGold;
        }

        public void TakeDamage(int rawDamaage)
        {
            int dmg = Math.Max(0, rawDamaage);
            HP = Math.Max(0, HP - dmg);
        }

        //이하 몬스터 생성 메서드
        public static Monster Minion()=>
            new Monster(name:"미니언", level:2, maxHP:15, attack:5, defense:0,rewardExp:2, rewardGold:3);
        
        public static Monster Voidling()=>
            new Monster(name:"공허충",level:3, maxHP:10, attack:9,defense:1,rewardExp:3,rewardGold:4);

        public static Monster CanonMinion()=>
            new Monster(name:"대포미니언",level:5, maxHP:25, attack:8,defense:5,rewardExp:5,rewardGold:6);
    }
}
