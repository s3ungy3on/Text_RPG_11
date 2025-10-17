//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Text_RPG_11.ItemData;

//namespace Text_RPG_11
//{
//    public class Crafting
//    {
//        private GameManager gameManager;
//        private ItemDataContainer itemData;

//        public Crafting(GameManager manager)
//        {
//            gameManager = manager;
//            itemData = ItemDatabase.LoadItems();
//        }

//        // 합성 가능한 아이템 목록 표시
//        public void ShowCraftingMenu()
//        {
//            Console.Clear();
//            Messages.TextTitleHlight("아이템 합성");
//            Console.WriteLine("재료를 조합하여 새로운 아이템을 만들 수 있습니다.\n");
//            Console.WriteLine($"보유 골드: {gameManager.Player.Gold}G\n");

//            var craftableItems = itemData.items.Where(i => i.crafting).ToList();

//            Console.WriteLine("[합성 가능한 아이템]");
//            for (int i = 0; i < craftableItems.Count; i++)
//            {
//                var item = craftableItems[i];
//                var rarityInfo = ItemDatabase.GetRarityInfo(item.rarity);

//                Console.Write($"{i + 1}. ");
//                Console.ForegroundColor = GetRarityColor(item.rarity);
//                Console.Write($"[{rarityInfo.displayName}] {item.name}");
//                Console.ResetColor();

//                // 합성 비용 표시
//                int craftCost = rarityInfo.craftingGoldCost;
//                Console.Write($" (합성 비용: {craftCost}G)");

//                // 합성 가능 여부 표시
//                bool canCraft = CanCraft(item, out string reason);
//                if (canCraft)
//                {
//                    Console.ForegroundColor = ConsoleColor.Green;
//                    Console.WriteLine(" [합성 가능]");
//                }
//                else
//                {
//                    Console.ForegroundColor = ConsoleColor.Red;
//                    Console.WriteLine($" [불가: {reason}]");
//                }
//                Console.ResetColor();
//            }

//            Console.WriteLine("\n0. 나가기");
//            Console.Write("\n합성할 아이템 번호를 선택하세요\n>> ");

//            int input = Messages.ReadInput(0, craftableItems.Count);

//            if (input == 0)
//            {
//                gameManager.GameMain();
//                return;
//            }

//            CraftItem(craftableItems[input - 1]);
//        }

//        // 아이템 합성 가능 여부 확인
//        private bool CanCraft(ItemDataBase item, out string reason)
//        {
//            reason = "";

//            // 골드 확인
//            var rarityInfo = ItemDatabase.GetRarityInfo(item.rarity);
//            if (gameManager.Player.Gold < rarityInfo.craftingGoldCost)
//            {
//                reason = "골드 부족";
//                return false;
//            }

//            // 재료 확인
//            foreach (var required in item.craftingRecipe.requiredItems)
//            {
//                var requiredItemData = itemData.items.FirstOrDefault(i => i.id == required.itemId);
//                if (requiredItemData == null)
//                {
//                    reason = "재료 정보 오류";
//                    return false;
//                }

//                int ownedQuantity = GetMaterialQuantity(required.itemId);
//                if (ownedQuantity < required.quantity)
//                {
//                    reason = $"{requiredItemData.name} 부족 ({ownedQuantity}/{required.quantity})";
//                    return false;
//                }
//            }

//            return true;
//        }

//        // 재료 보유 수량 확인
//        private int GetMaterialQuantity(int itemId)
//        {
//            int total = 0;

//            foreach (var item in gameManager.inventory.BagItems)
//            {
//                // 아이템 ID 비교 로직 (실제로는 Items 클래스에 ID 필드 추가 필요)
//                if (item is Material material && material.Name == GetItemNameById(itemId))
//                {
//                    total += material.Quantity;
//                }
//            }

//            return total;
//        }

//        // ID로 아이템 이름 가져오기
//        private string GetItemNameById(int itemId)
//        {
//            var item = itemData.items.FirstOrDefault(i => i.id == itemId);
//            return item?.name ?? "";
//        }

//        // 아이템 합성 실행
//        private void CraftItem(ItemDataBase itemData)
//        {
//            Console.Clear();
//            Messages.TextTitleHlight($"{itemData.name} 합성");

//            // 필요 재료 표시
//            Console.WriteLine("\n[필요 재료]");
//            foreach (var required in itemData.craftingRecipe.requiredItems)
//            {
//                var requiredItem = this.itemData.items.FirstOrDefault(i => i.id == required.itemId);
//                int owned = GetMaterialQuantity(required.itemId);

//                Console.Write($"  - {requiredItem.name}: ");
//                if (owned >= required.quantity)
//                {
//                    Console.ForegroundColor = ConsoleColor.Green;
//                    Console.WriteLine($"{owned}/{required.quantity}");
//                }
//                else
//                {
//                    Console.ForegroundColor = ConsoleColor.Red;
//                    Console.WriteLine($"{owned}/{required.quantity}");
//                }
//                Console.ResetColor();
//            }

//            var rarityInfo = ItemDatabase.GetRarityInfo(itemData.rarity);
//            Console.WriteLine($"\n합성 비용: {rarityInfo.craftingGoldCost}G");
//            Console.WriteLine($"현재 골드: {gameManager.Player.Gold}G");

//            // 합성 가능 여부 최종 확인
//            if (!CanCraft(itemData, out string reason))
//            {
//                Console.WriteLine($"\n합성 불가: {reason}");
//                Console.WriteLine("\n아무 키나 눌러 돌아가기...");
//                Console.ReadKey();
//                ShowCraftingMenu();
//                return;
//            }

//            Console.WriteLine("\n합성하시겠습니까? (Y/N)");
//            Console.Write(">> ");
//            string confirm = Console.ReadLine()?.ToUpper();

//            if (confirm != "Y")
//            {
//                ShowCraftingMenu();
//                return;
//            }

//            // 합성 처리
//            ExecuteCrafting(itemData, rarityInfo.craftingGoldCost);
//        }

//        // 합성 실행
//        private void ExecuteCrafting(ItemDataBase itemData, int craftCost)
//        {
//            // 재료 소모
//            foreach (var required in itemData.craftingRecipe.requiredItems)
//            {
//                ConsumeMaterial(required.itemId, required.quantity);
//            }

//            // 골드 소모
//            gameManager.Player.Gold -= craftCost;

//            // 아이템 생성 및 지급
//            Items craftedItem = CreateItemFromData(itemData);
//            gameManager.inventory.AddItem(craftedItem);

//            Console.Clear();
//            Console.ForegroundColor = ConsoleColor.Yellow;
//            Console.WriteLine("★★★ 합성 성공! ★★★\n");
//            Console.ResetColor();

//            Console.ForegroundColor = GetRarityColor(itemData.rarity);
//            Console.WriteLine($"{itemData.name}을(를) 획득했습니다!");
//            Console.ResetColor();

//            Console.WriteLine($"\n남은 골드: {gameManager.Player.Gold}G");

//            Console.WriteLine("\n아무 키나 눌러 계속...");
//            Console.ReadKey();
//            ShowCraftingMenu();
//        }

//        // 재료 소모
//        private void ConsumeMaterial(int itemId, int quantity)
//        {
//            string itemName = GetItemNameById(itemId);
//            int remaining = quantity;

//            for (int i = gameManager.inventory.BagItems.Count - 1; i >= 0 && remaining > 0; i--)
//            {
//                var item = gameManager.inventory.BagItems[i];
//                if (item is Material material && material.Name == itemName)
//                {
//                    if (material.Quantity <= remaining)
//                    {
//                        remaining -= material.Quantity;
//                        gameManager.inventory.BagItems.RemoveAt(i);
//                    }
//                    else
//                    {
//                        material.Quantity -= remaining;
//                        remaining = 0;
//                    }
//                }
//            }
//        }

//        // ItemDataBase를 Items 객체로 변환
//        private Items CreateItemFromData(ItemDataBase data)
//        {
//            switch (data.type)
//            {
//                case "무기":
//                    return new Weapon(data.name, data.attackPower, data.defensePower, data.price, data.itemHp, data.itemMp);
//                case "방어구":
//                    return new Armor(data.name, data.attackPower, data.defensePower, data.price, data.itemHp, data.itemMp);
//                case "재료":
//                    return new Material(data.name, data.attackPower, data.defensePower, data.price, data.itemHp, data.itemMp);
//                default:
//                    return null;
//            }
//        }

//        // 등급별 색상
//        private ConsoleColor GetRarityColor(string rarity)
//        {
//            return rarity switch
//            {
//                "common" => ConsoleColor.White,
//                "rare" => ConsoleColor.Blue,
//                "epic" => ConsoleColor.Magenta,
//                "legend" => ConsoleColor.Yellow,
//                "myth" => ConsoleColor.Red,
//                "transcended" => ConsoleColor.Cyan,
//                _ => ConsoleColor.White
//            };
//        }
//    }
//}
