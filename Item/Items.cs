using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    public abstract class Items
    {
        public string Name { get; } // 아이템 이름
        public string Desc { get; } // 아이템 설명
        public int Price { get; set; } // 아이템 가격
        public bool IsEquipped { get; set; } //아이템 장착 유무
        public bool IsPurchased { get; set; } // 아이템 구매 유무

        public Items(string name, string desc, int price, bool isEquipped = false, bool isPurchased = false)
        {
            Name = name;
            Desc = desc;
            Price = price;
            IsEquipped = isEquipped;
            IsPurchased = isPurchased;
        }

        public abstract string ItemType();
        public abstract string ItemStats();
    }

    public static class ItemDatabase
    {
        public static Items[] CreateItemsData()
        {
            return new Items[]
            {
                //무기
                new Weapon("낡은 검", "쉽게 볼 수 있는 낡은 검 입니다.", 2, 600, 0),

                //방어구
                new Armor("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", 5, 1000, 10),

                //포션
                new Potion("체력 물약", "체력을 30 회복시켜줍니다.", 30, 500),
            };
        }
    }

    public class Weapon : Items
    {
        public int AttackPower { get; }
        public int ItemHp { get; }

        public Weapon(string name, string desc, int attackPower, int price, int itemhp, bool isEquipped = false, bool isPurchased = false) : base(name, desc, price, isEquipped, isPurchased)
        {
            AttackPower = attackPower;
            ItemHp = itemhp;

        }

        public override string ItemType()
        {
            return "무기";
        }

        public override string ItemStats()
        {
            return $"공격력 +{AttackPower}";
        }
    }

    public class Armor : Items
    {
        public int DefensePower { get; }
        public int ItemHp { get; }

        public Armor(string name, string desc, int defensePower, int price, int hp, bool isEquipped = false, bool isPurchased = false) : base(name, desc, price, isEquipped, isPurchased)
        {
            DefensePower = defensePower;
            ItemHp = hp;
        }

        public override string ItemType()
        {
            return "방어구";
        }

        public override string ItemStats()
        {
            return $"방어력 +{DefensePower}";
        }
    }

    public class Potion : Items
    {
        public int HealPower { get; }
        public int PotionCount { get; set; }

        public Potion(string name, string desc, int healPower, int price, int potionCount = 0, bool isEquipped = false, bool isPurchased = false) : base(name, desc, price, isEquipped, isPurchased)
        {
            HealPower = healPower;
            PotionCount = potionCount;
        }

        public override string ItemType()
        {
            return "물약";
        }

        public override string ItemStats()
        {
            return $"체력 회복 +{HealPower}";
        }
    }
}
