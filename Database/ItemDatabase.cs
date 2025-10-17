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
            InitDatabase();
        }

        private static void InitDatabase()
        {
            cachedItems = LoadItems();

            foreach (var item in cachedItems.items)
            {
                itemOriginals[item.id] = item;
            }

            foreach (var potion in cachedItems.potions)
            {
                potionOriginals[potion.id] = potion;
            }
        }

        public static ItemDataContainer LoadItems()
        {
            try
            {
                string json = File.ReadAllText(itemDataPath);
                return JsonConvert.DeserializeObject<ItemDataContainer>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"아이템 데이터 로드 오류:{ex.Message}");
                return GetDefaultContainer();
            }
        }

        private static ItemDataContainer GetDefaultContainer()
        {
            return new ItemDataContainer
            {
                craftingConfig = new CraftingConfig { alwaysSuccess = true },
                items = new List<ItemDataBase>(),
                potions = new List<PotionData>(),
                rarityInfo = new Dictionary<string, RarityInfo>()
            };
        }

        #region 아이템 반환 메서드 (복제본)
        public static Items GetItemById(int itemId)
        {
            if (itemOriginals.TryGetValue(itemId, out var itemData))
            {
                return CreateItemFromData(itemData);
            }

            if (potionOriginals.TryGetValue(itemId, out var potionData))
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

        public static Weapon GetWeapon(string name)
        {
            var item = GetItemByName(name);
            return item as Weapon;
        }

        public static Armor GetArmor(string name)
        {
            var item = GetItemByName(name);
            return item as Armor;
        }

        public static Potion GetPotion(string name)
        {
            var item = GetItemByName(name);
            return item as Potion;
        }

        public static Material GetMaterial(string name)
        {
            var item = GetItemByName(name);
            return item as Material;
        }
        #endregion

        #region 획득 경로별 아이템 목록
        public static List<Items> GetShopItems() //상점 아이템
        {
            List<Items> shopItems = new List<Items>();

            foreach (var item in cachedItems.items.Where(i => i.obtainMethods.Contains("shop")))
            {
                shopItems.Add(CreateItemFromData(item));
            }

            foreach(var potion in cachedItems.potions.Where(p => p.obtainMethods.Contains("shop")))
            {
                shopItems.Add(CreatePotionFromData(potion));
            }

            return shopItems;
        }

        public static List<Items> GetCraftableItems() //합성 아이템
        {
            List<Items> craftableItems = new List<Items>();

            foreach(var item in cachedItems.items.Where(i => i.crafting))
            {
                craftableItems.Add(CreateItemFromData(item));
            }

            return craftableItems;
        }

        public static List<Items> GetItemsByRarity(string rarity) //등급 별 아이템
        {
            List<Items> items = new List<Items>();

            foreach (var item in cachedItems.items.Where(i=>i.rarity == rarity))
            {
                items.Add(CreateItemFromData(item));
            }

            foreach(var potion in cachedItems.potions.Where(p=>p.rarity == rarity))
            {
                items.Add(CreatePotionFromData(potion));
            }

            return items;
        }
        #endregion

        #region 합성 관련

        public static bool CanCraft(int itemId, Dictionary<int, int> ownedItems, int playerGold)
        {
            if (!itemOriginals.TryGetValue(itemId, out var itemData))
            {
                return false;
            }

            if (!itemData.crafting || itemData.craftingRecipe == null)
            {
                return false;
            }

            var rarityInfo = GetRarityInfo(itemData.rarity);
            if (playerGold < rarityInfo.craftingGoldCost)
            {
                return false;
            }

            foreach (var requird in itemData.craftingRecipe.requiredItems)
            {
                if (!ownedItems.ContainsKey(requird.itemId) || ownedItems[requird.itemId] < requird.quantity)
                {
                    return false;
                }
            }

            return true;
        }

        public static CraftingRecipe GetCraftingRecipe(int itemId)
        {
            if(itemOriginals.TryGetValue(itemId, out var itemData))
            {
                return itemData.craftingRecipe;
            }

            return null;
        }
        #endregion

        #region 등급 정보
        public static RarityInfo GetRarityInfo(string rarity)
        {
            if (cachedItems.rarityInfo.ContainsKey(rarity))
            {
                return cachedItems.rarityInfo[rarity];
            }

            return null;
        }

        #endregion

        #region 데이터 변환 메서드
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
                        EquipableJobs = data.equipJob
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
                        EquipableJobs = data.equipJob
                    };

                case "재료":
                    return new Material(
                        name: data.name,
                        attackPower: data.attackPower,
                        defensePower: data.defensePower,
                        price: data.price,
                        itemHp: data.itemHp,
                        itemMp: data.itemMp)
                    {
                        ItemId = data.id
                    };

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
                ItemId = data.id
            };
        }

        #endregion
    }
}