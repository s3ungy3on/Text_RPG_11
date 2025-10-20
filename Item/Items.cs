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
        public int ItemId {  get; set; } // 아이템 Id
        public string Name { get; } // 아이템 이름
        public string Rarity { get; set; } //아이템 등급
        public int Price { get; set; } // 아이템 가격
        public bool IsEquipped { get; set; } //아이템 장착 유무
        public bool IsPurchased { get; set; } // 아이템 구매 유무
        public List<string> EquipableJobs { get; set; } //아이템 착용 가능 직업


        public Items(string name, int price, bool isEquipped = false, bool isPurchased = false)
        {
            Name = name;
            Price = price;
            IsEquipped = isEquipped;
            IsPurchased = isPurchased;
        }

        public abstract string ItemType();
        public abstract string ItemStats();

        public bool CanEquip(string jobName) //착용 가능 직업 체크
        {
            if (EquipableJobs == null || EquipableJobs.Count == 0) //착용 가능 직업 데이터가 비어있거나, 리스트가 존재하지만 값이 0개라면
                return true;  // 모든 직업 착용 가능

            return EquipableJobs.Contains(jobName); //직업 제한이 있으면 리스트 안에 jobName 값이 있는지 확인
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

        public override string ItemStats() //아이템 스탯 문자열로 반환
        {
            List<string> stats = new List<string>(); //문자 저장하는 리스트

            if (AttackPower > 0) stats.Add($"공격력 +{AttackPower}"); //해당 수치가 0보다 크다면(값이 있다면) 해당 문자를 리스트에 추가
            if (DefensePower > 0) stats.Add($"방어력 +{DefensePower}");
            if (ItemHp > 0) stats.Add($"체력 +{ItemHp}");
            if (ItemMp > 0) stats.Add($"마나 +{ItemMp}");

            return stats.Count > 0 ? string.Join(" | ", stats) : "효과 없음"; //stats 리스트에 값이 하나라도 있으면 |로 이어붙여서 반환, 없다면 효과없음 반환
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

            return stats.Count > 0 ? string.Join(" | ", stats) : "효과 없음";
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

