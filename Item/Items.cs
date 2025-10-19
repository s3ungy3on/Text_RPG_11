using System;
using System.Collections;
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
        public string Rarity { get; set; } //아이템 등급
        public int Price { get; set; } // 아이템 가격
        public bool IsEquipped { get; set; } //아이템 장착 유무
        public bool IsPurchased { get; set; } // 아이템 구매 유무
        public List<string> EquipableJobs { get; set; }


        public Items(string name, int price, bool isEquipped = false, bool isPurchased = false)
        {
            Name = name;
            Price = price;
            IsEquipped = isEquipped;
            IsPurchased = isPurchased;
        }

        public abstract string ItemType();
        public abstract string ItemStats();

        public bool CanEquip(string jobName)
        {
            if (EquipableJobs == null || EquipableJobs.Count == 0)
                return true;  // null이면 모든 직업 착용 가능

            return EquipableJobs.Contains(jobName);
        }
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
            List<string> stats = new List<string>();

            if (AttackPower > 0) stats.Add($"공격력 +{AttackPower}");
            if (DefensePower > 0) stats.Add($"방어력 +{DefensePower}");
            if (ItemHp > 0) stats.Add($"체력 +{ItemHp}");
            if (ItemMp > 0) stats.Add($"마나 +{ItemMp}");

            return stats.Count > 0 ? string.Join(", ", stats) : "효과 없음";
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
            List<string> stats = new List<string>();

            if (AttackPower > 0) stats.Add($"공격력 +{AttackPower}");
            if (DefensePower > 0) stats.Add($"방어력 +{DefensePower}");
            if (ItemHp > 0) stats.Add($"체력 +{ItemHp}");
            if (ItemMp > 0) stats.Add($"마나 +{ItemMp}");

            return stats.Count > 0 ? string.Join(", ", stats) : "효과 없음";
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
    }

    //#region 재료 아이템 관련 (사용 안함)
    //public class Material : Items
    //{
    //    public int AttackPower { get; }
    //    public int DefensePower { get; }
    //    public int ItemHp { get; }
    //    public int ItemMp { get; }
    //    public bool IsStackable { get; set; }
    //    public int MaxStack { get; set; }

    //    public Material(string name, int attackPower, int defensePower, int price, int itemHp, int itemMp, bool isEquipped = false, bool isPurchased = false, bool isStackable = true, int maxStack = 99) : base(name, price, isEquipped, isPurchased)
    //    {
    //        AttackPower = attackPower;
    //        DefensePower = defensePower;
    //        ItemHp = itemHp;
    //        ItemMp = itemMp;
    //        IsStackable = isStackable;
    //        MaxStack = maxStack;
    //        Quantity = 1;
    //    }

    //    public override string ItemType()
    //    {
    //        return "재료";
    //    }

    //    public override string ItemStats()
    //    {
    //        List<string> stats = new List<string>();

    //        if (AttackPower > 0) stats.Add($"공격력 +{AttackPower}");
    //        if (DefensePower > 0) stats.Add($"방어력 +{DefensePower}");
    //        if (ItemHp > 0) stats.Add($"체력 +{ItemHp}");
    //        if (ItemMp > 0) stats.Add($"마나 +{ItemMp}");

    //        return stats.Count > 0 ? string.Join(", ", stats) : "효과 없음";
    //    }
    //}
    //#endregion
}

