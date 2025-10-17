using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    public class ItemData
    {
        // JSON 전체 구조
        public class ItemDataContainer
        {
            public CraftingConfig craftingConfig {  get; set; }
            public List<ItemDataBase> items { get; set; }
            public List<PotionData> potions { get; set; }
            public Dictionary<string, RarityInfo> rarityInfo { get; set; }
        }

        // 아이템 공통 정보
        public class ItemDataBase
        {
            public int id { get; set; }
            public string name { get; set; }
            public string type {  get; set; }
            public string rarity { get; set; }
            public int attackPower { get; set; }
            public int defensePower { get; set; }
            public int itemHp { get; set; }
            public int itemMp { get; set; }
            public List<string> equipJob { get; set; } // "전사", "마법사", "도적", "궁수"
            public int price { get; set; }

            public List<string> obtainMethods { get; set; }  // "shop", "monster", "dungeon"
            public bool crafting { get; set; }
            public CraftingRecipe craftingRecipe { get; set; }
        }

        public class PotionData // 물약 데이터
        {
            public int id { get; set; }
            public string name { get; set; }
            public string rarity { get; set; }
            public int healPower { get; set; }
            public bool stackable { get; set; }
            public int maxStack { get; set; }
            public int price { get; set; }
            public List<string> obtainMethods { get; set; }
        }

        public class CraftingConfig //합성 기본 설정
        {
            public bool alwaysSuccess {  get; set; }
        }

        public class CraftingRecipe //합성 레시피 리스트
        {
            public List<RequiredItem> requiredItems { get; set; }
        }

        public class RequiredItem //레시피 필요 아이템
        {
            public int itemId {  get; set; }
            public int quantity { get; set; }
        }

        public class RarityInfo //등급 정보
        {
            public string displayName {  get; set; }
            public string color { get; set; }
            public int craftingGoldCost { get; set; }
        }
    }
}
