using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Text_RPG_11.ItemData;

namespace Text_RPG_11
{
    public static class ItemDatabase
    {
        private static string itemDataPath = "Data/items.json";
        private static ItemDataContainer cachedItems;

        public static ItemDataContainer LoadItems()
        {
            if(cachedItems!= null) return cachedItems;

            try
            {
                string json = File.ReadAllText(itemDataPath);
                cachedItems = JsonConvert.DeserializeObject<ItemDataContainer>(json);
                return cachedItems;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"아이템 데이터 로드 오류:{ex.Message}");
                return GetDefaultItems();
            }
        }

        public static List<Items> GetShopItems() //상점 아이템
        {
            var data = LoadItems();
            List<Items> shopItems = new List<Items>();

            foreach (var item in data.items.Where(w => w.obtainMethods.Contains("shop")))
            {
                shopItems.Add(new Items());
            }


        }

        public static Items GetMonsterDropItems(int monsterId) //몬스터 드롭 아이템
        {
            var data = LoadItems();

        }

        public static List<Items> GetDungeonDropItems(int dungeonId) //던전 드롭 아이템
        {
            var data = LoadItems();
        }

        public static Items GetItemId(int itemId)
        {
            var data = LoadItems();
        }
    }
}
