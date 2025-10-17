using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    public abstract class Items
    {
        public int ItemId {  get; set; }
        public string Name { get; } // 아이템 이름
        public int Price { get; set; } // 아이템 가격
        public bool IsEquipped { get; set; } //아이템 장착 유무
        public bool IsPurchased { get; set; } // 아이템 구매 유무
        public int Quantity {  get; set; } //아이템 개수

        public Items(string name, int price, bool isEquipped = false, bool isPurchased = false, int quantity = 1)
        {
            Name = name;
            Price = price;
            IsEquipped = isEquipped;
            IsPurchased = isPurchased;
            Quantity = quantity;
        }

        public abstract string ItemType();
        public abstract string ItemStats();
    }

    public class Weapon : Items
    {
        public int AttackPower { get; }
        public int DefensePower {  get; }
        public int ItemHp { get; }
        public int ItemMp { get; }

        public Weapon(string name, int attackPower, int defensePower, int price, int itemHp, int itemMp, bool isEquipped = false, bool isPurchased = false) : base(name, price, isEquipped, isPurchased)
        {
            AttackPower = attackPower;
            DefensePower = defensePower;
            ItemHp = itemHp;
            ItemMp = itemMp;
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
        public int AttackPower { get; }
        public int DefensePower { get; }
        public int ItemHp { get; }
        public int ItemMp { get; }

        public Armor(string name, int attackPower, int defensePower, int price, int itemHp, int itemMp, bool isEquipped = false, bool isPurchased = false) : base(name, price, isEquipped, isPurchased)
        {
            AttackPower = attackPower;
            DefensePower = defensePower;
            ItemHp = itemHp;
            ItemMp = itemMp;
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

        public Potion(string name, int healPower, int price, int potionCount = 0, bool isEquipped = false, bool isPurchased = false) : base(name, price, isEquipped, isPurchased)
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

        //public void UsePotion(Player player) //포션 사용 기능
        //{
        //    //포션 개수 ui 출력도 여기서?

        //    int healAmount = Math.Min(HealPower, player.MaxHP - player.HP);
        //    player.HP += healAmount;
        //    PotionCount--;

        //    //회복 대사 출력

        //    if (PotionCount == 0)
        //    {
        //        IsPurchased = false;
        //    }
        //}
    }
}

