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
        public int Exp { get; set; }

        private int itemHP = 0;
        public int itemAttack = 0;
        public int itemDefense = 0;

        public int MaxHP { get { return HP + itemHP; } }

        public int MaxAttack { get { return Attack + itemAttack; } }
        public int MaxDefense { get { return Defense + itemDefense; } }


        public Player(string name, int level, string job, int attack, int defense, int hP, int gold, int exp = 0)
        {
            Name = name;
            Level = level;
            Job = job;
            Attack = attack;
            Defense = defense;
            HP = hP;
            Gold = gold;
            Exp = exp;
        }

        public void StatUpdate(GameManager gameManager)
        {
            itemHP = 0;
            itemAttack = 0;
            itemDefense = 0;

            if(gameManager.GameItems == null)
            {
                return;
            }

            foreach (var item in gameManager.GameItems)
            {
                if(item != null && item.IsEquipped)
                {
                    if (item is Weapon weapon)
                    {
                        itemAttack += weapon.AttackPower;
                        if(weapon.ItemHp > 0)
                        {
                            itemHP += weapon.ItemHp;
                        }
                    }
                    else if (item is Armor armor)
                    {
                        itemDefense += armor.DefensePower;
                        if(armor.ItemHp > 0)
                        {
                            itemHP += armor.ItemHp;
                        }
                    } 
                }
            }
        }
    }
}
