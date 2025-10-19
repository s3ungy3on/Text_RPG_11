using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    // JSON 전체 구조
    public class ItemDataContainer
    {
        public CraftingConfig craftingConfig { get; set; }
        public List<ItemDataBase> items { get; set; }
        public List<PotionData> potions { get; set; }
        public Dictionary<string, RarityInfo> rarityInfo { get; set; }
    }

    // 아이템 공통 정보
    public class ItemDataBase
    {
        public int id { get; set; }
        public string name { get; set; }
        public string type { get; set; } //"무기", "방어구"
        public string rarity { get; set; }
        public int attackPower { get; set; }
        public int defensePower { get; set; }
        public int itemHp { get; set; }
        public int itemMp { get; set; }
        public List<string> equipJob { get; set; } // "전사", "마법사", "도적", "궁수"
        public int price { get; set; }
        public List<string> obtainMethods { get; set; }  // "shop", "dungeon", "quest"
        public bool crafting { get; set; } //사용X
        public CraftingRecipe craftingRecipe { get; set; } //사용X
    }

    // 물약 데이터
    public class PotionData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string rarity { get; set; }
        public int healPower { get; set; }
        public int price { get; set; }
        public List<string> obtainMethods { get; set; }
    }

    // 합성 기본 설정, 사용X
    public class CraftingConfig
    {
        public bool alwaysSuccess { get; set; }
    }

    // 합성 레시피 리스트, 사용X
    public class CraftingRecipe
    {
        public List<RequiredItem> requiredItems { get; set; }
    }

    // 레시피 필요 아이템, 사용X
    public class RequiredItem
    {
        public int itemId { get; set; }
        public int quantity { get; set; }
    }

    // 등급 정보
    public class RarityInfo
    {
        public string displayName { get; set; }
        public string color { get; set; }
        public int craftingGoldCost { get; set; }
    }
}