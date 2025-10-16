using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    internal class ItemData
    {
        // JSON 전체 구조
        public class ItemDataContainer
        {
            public List<WeaponData> weapons { get; set; }
            public List<ArmorData> armors { get; set; }
            public List<PotionData> potions { get; set; }
        }

        // 아이템 공통 정보
        public class ItemDataBase
        {
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public int price { get; set; }
            public int itemHp { get; set; }
            public int itemMp { get; set; }
            public List<string> equipJob { get; set; } // "전사", "마법사", "도적", "궁수"
            public int requiredLevel { get; set; }
            public string rarity { get; set; }
            public List<string> obtainMethods { get; set; }  // "shop", "monster", "dungeon"
            public DropInfo dropInfo { get; set; }
        }

        // 드랍 정보
        public class DropInfo
        {
            public List<int> monsterIds { get; set; }  // 어떤 몬스터가 드랍하는지
            public int? dungeonId { get; set; }         // 어떤 던전에서 나오는지
            public int dropChance { get; set; }         // 드랍 확률 (%)
        }

        // 무기 데이터
        public class WeaponData : ItemDataBase
        {
            public int attackPower { get; set; }
        }

        // 방어구 데이터
        public class ArmorData : ItemDataBase
        {
            public int defensePower { get; set; }
        }

        // 물약 데이터
        public class PotionData : ItemDataBase
        {
            public int healPower { get; set; }
            public bool stackable { get; set; }
            public int maxStack { get; set; }
        }
    }



    //public class Rootobject
    //{
    //    public Weapon[] weapons { get; set; }
    //    public Armor[] armors { get; set; }
    //    public Potion[] potions { get; set; }
    //}

    //public class Weapon
    //{
    //    public int id { get; set; }
    //    public string name { get; set; }
    //    public string description { get; set; }
    //    public int attackPower { get; set; }
    //    public int price { get; set; }
    //    public int itemHp { get; set; }
    //    public int itemMp { get; set; }
    //    public string[] equipJob { get; set; }
    //    public int requiredLevel { get; set; }
    //    public string[] obtainMethods { get; set; }
    //}

    //public class Armor
    //{
    //    public int id { get; set; }
    //    public string name { get; set; }
    //    public string description { get; set; }
    //    public int defensePower { get; set; }
    //    public int price { get; set; }
    //    public int itemHp { get; set; }
    //    public int itemMp { get; set; }
    //    public string[] equipJob { get; set; }
    //    public int requiredLevel { get; set; }
    //    public string[] obtainMethods { get; set; }
    //}

    //public class Potion
    //{
    //    public int id { get; set; }
    //    public string name { get; set; }
    //    public string description { get; set; }
    //    public int healPower { get; set; }
    //    public int price { get; set; }
    //    public int requiredLevel { get; set; }
    //    public bool stackavle { get; set; }
    //    public int maxStack { get; set; }
    //    public string[] obtainMethods { get; set; }
    //    public Dropinfo dropInfo { get; set; }
    //}

    //public class Dropinfo
    //{
    //    public int dungeonId { get; set; }
    //    public int dropChance { get; set; }
    //}

}
