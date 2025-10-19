using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Text_RPG_11.ItemData;

namespace Text_RPG_11
{
    public static class ItemDatabase
    {
        private static string itemDataPath = "Data/items.json";
        private static ItemDataContainer cachedItems;

        private static Dictionary<int, ItemDataBase> itemOriginals = new Dictionary<int, ItemDataBase>();
        private static Dictionary<int, PotionData> potionOriginals = new Dictionary<int, PotionData>();

        static ItemDatabase()
        {
            cachedItems = LoadItems();
        }

        private static ItemDataContainer LoadItems()
        {
            try
            {
                if (!File.Exists(itemDataPath))
                {
                    Console.WriteLine($"items.json을 찾을 수 없습니다: {itemDataPath}");
                    return GetDefaultContainer();
                }

                string json = File.ReadAllText(itemDataPath);
                var container = JsonConvert.DeserializeObject<ItemDataContainer>(json);

                if(container == null || container.items == null)
                {
                    Console.WriteLine("아이템 데이터 파싱 실패");
                    return GetDefaultContainer();
                }

                Console.WriteLine($"아이템 로드 완료: {container.items.Count}개");
                return container;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"아이템 로드 오류: {ex.Message}");
                return GetDefaultContainer();
            }
        }

        private static ItemDataContainer GetDefaultContainer()
        {
            return new ItemDataContainer
            {
                items = new List<ItemDataBase>(),
                potions = new List<PotionData>(),
                rarityInfo = new Dictionary<string, RarityInfo>()
            };
        }

        public static Items GetItemById(int itemId)
        {
            var itemData = cachedItems.items.FirstOrDefault(i => i.id ==itemId);
            if(itemData != null)
            {
                return CreateItemFromData(itemData);
            }

            var potionData = cachedItems.potions.FirstOrDefault(p => p.id == itemId);
            if(potionData != null)
            {
                return CreatePotionFromData(potionData);
            }

            return null;
        }

        public static Items GetItemByName(string name)
        {
            var itemData = cachedItems.items.FirstOrDefault(i => i.name == name);
            if (itemData != null)
            {
                return CreateItemFromData(itemData);
            }

            var potionData = cachedItems.potions.FirstOrDefault(p => p.name == name);
            if (potionData != null)
            {
                return CreatePotionFromData(potionData);
            }

            return null;
        }

        public static List<Items> GetShopItems()
        {
            List<Items> shopItems = new List<Items>();

            // 무기/방어구 중 상점 판매 아이템
            foreach (var item in cachedItems.items.Where(i =>
                i.obtainMethods != null &&
                i.obtainMethods.Contains("shop") &&
                (i.type == "무기" || i.type == "방어구")))  // ⭐ 재료 제외
            {
                shopItems.Add(CreateItemFromData(item));
            }

            // 포션
            foreach (var potion in cachedItems.potions.Where(p =>
                p.obtainMethods != null &&
                p.obtainMethods.Contains("shop")))
            {
                shopItems.Add(CreatePotionFromData(potion));
            }

            return shopItems;
        }

        public static List<Items> GetDungeonItems()
        {
            List<Items> dungeonItems = new List<Items>();

            foreach (var item in cachedItems.items.Where(i =>
                i.obtainMethods != null &&
                i.obtainMethods.Contains("dungeon") &&
                (i.type == "무기" || i.type == "방어구")))
            {
                dungeonItems.Add(CreateItemFromData(item));
            }

            foreach (var potion in cachedItems.potions.Where(p =>
                p.obtainMethods != null &&
                p.obtainMethods.Contains("dungeon")))
            {
                dungeonItems.Add(CreatePotionFromData(potion));
            }

            return dungeonItems;
        }

        public static List<Items> GetItemsByRarity(string rarity)
        {
            List<Items> items = new List<Items>();

            foreach (var item in cachedItems.items.Where(i =>
                i.rarity == rarity &&
                (i.type == "무기" || i.type == "방어구")))
            {
                items.Add(CreateItemFromData(item));
            }

            foreach (var potion in cachedItems.potions.Where(p => p.rarity == rarity))
            {
                items.Add(CreatePotionFromData(potion));
            }

            return items;
        }

        private static Items CreateItemFromData(ItemDataBase data)
        {
            switch (data.type)
            {
                case "무기":
                    return new Weapon(
                        name: data.name,
                        attackPower: data.attackPower,
                        defensePower: data.defensePower,
                        price: data.price,
                        itemHp: data.itemHp,
                        itemMp: data.itemMp)
                    {
                        ItemId = data.id,
                        EquipableJobs = data.equipJob,
                        Rarity = data.rarity
                    };

                case "방어구":
                    return new Armor(
                        name: data.name,
                        attackPower: data.attackPower,
                        defensePower: data.defensePower,
                        price: data.price,
                        itemHp: data.itemHp,
                        itemMp: data.itemMp)
                    {
                        ItemId = data.id,
                        EquipableJobs = data.equipJob,
                        Rarity = data.rarity
                    };

                //재료는 무시 (사용 안 함)
                default:
                    return null;
            }
        }

        private static Potion CreatePotionFromData(PotionData data)
        {
            return new Potion(
                name: data.name,
                healPower: data.healPower,
                price: data.price,
                potionCount: 1)
            {
                ItemId = data.id,
                Rarity = data.rarity
            };
        }

        public static Dictionary<string, RarityInfo> GetRarityInfo()
        {
            return cachedItems.rarityInfo;
        }
    }
}