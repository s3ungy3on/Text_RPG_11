using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    public static class ItemDatabase
    {
        private static string itemDataPath = "Data/items.json";
        private static ItemDataContainer cachedItems;  // ← ItemData. 제거

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

                if (container == null || container.items == null)
                {
                    Console.WriteLine("아이템 데이터 파싱 실패");
                    return GetDefaultContainer();
                }

                Console.WriteLine($"아이템 로드 완료: {container.items.Count}개");
                return container;
            }
            catch (Exception ex)
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
            ItemDataBase itemData = null;
            foreach (var item in cachedItems.items)
            {
                if (item.id == itemId)
                {
                    itemData = item;
                    break;
                }
            }

            if (itemData != null)
            {
                return CreateItemFromData(itemData);
            }

            PotionData potionData = null;
            foreach (var potion in cachedItems.potions)
            {
                if (potion.id == itemId)
                {
                    potionData = potion;
                    break;
                }
            }

            if (potionData != null)
            {
                return CreatePotionFromData(potionData);
            }

            return null;
        }

        public static Items GetItemByName(string name)
        {
            ItemDataBase itemData = null;
            foreach (var item in cachedItems.items)
            {
                if (item.name == name)
                {
                    itemData = item;
                    break;
                }
            }

            if (itemData != null)
            {
                return CreateItemFromData(itemData);
            }

            PotionData potionData = null;
            foreach (var potion in cachedItems.potions)
            {
                if (potion.name == name)
                {
                    potionData = potion;
                    break;
                }
            }

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
            foreach (var item in cachedItems.items)
            {
                if (item.obtainMethods != null &&
                   item.obtainMethods.Contains("shop") &&
                   (item.type == "무기" || item.type == "방어구"))
                {
                    shopItems.Add(CreateItemFromData(item));
                }
            }

            // 포션
            foreach (var potion in cachedItems.potions)
            {
                if (potion.obtainMethods != null &&
                   potion.obtainMethods.Contains("shop"))
                {
                    shopItems.Add(CreatePotionFromData(potion));
                }
            }

            return shopItems;
        }

        public static List<Items> GetDungeonItems()
        {
            List<Items> dungeonItems = new List<Items>();

            foreach (var item in cachedItems.items)
            {
                if (item.obtainMethods != null &&
                   item.obtainMethods.Contains("dungeon") &&
                   (item.type == "무기" || item.type == "방어구"))
                {
                    dungeonItems.Add(CreateItemFromData(item));
                }
            }

            foreach (var potion in cachedItems.potions)
            {
                if (potion.obtainMethods != null &&
                   potion.obtainMethods.Contains("dungeon"))
                {
                    dungeonItems.Add(CreatePotionFromData(potion));
                }
            }

            return dungeonItems;
        }

        public static List<Items> GetItemsByRarity(string rarity)
        {
            List<Items> items = new List<Items>();

            foreach (var item in cachedItems.items)
            {
                if (item.rarity == rarity &&
                   (item.type == "무기" || item.type == "방어구"))
                {
                    items.Add(CreateItemFromData(item));
                }
            }

            foreach (var potion in cachedItems.potions)
            {
                if (potion.rarity == rarity)
                {
                    items.Add(CreatePotionFromData(potion));
                }
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