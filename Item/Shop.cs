using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    public class Shop
    {
        private GameManager gameManager;
        private List<Items> shopInventory;

        public Shop(GameManager manager)
        {
            gameManager = manager;
            LoadShopInventory();
        }

        #region 상점 아이템 로드
        private void LoadShopInventory()
        {
            shopInventory = ItemDatabase.GetShopItems();
        }
        #endregion

        #region 다른 곳에서 상점 아이템 목록 부르기(UI에서 호출)
        public List<Items> GetShopInventory()
        {
            return shopInventory;
        }
        #endregion

        #region 아이템 구매
        public bool ButItem(Items shopItem)
        {
            if(shopItem == null)
            {
                return false;
            }

            if (shopItem.Price > gameManager.Player.Gold)
            {
                Console.WriteLine("보유중인 골드가 부족합니다.");
                return false;
            }

            if (shopItem.IsPurchased)
            {
                Console.WriteLine("이미 구매한 아이템입니다.");
                return false;
            }

            Items purchasedItem = ItemDatabase.GetItemById(shopItem.ItemId);

            if (purchasedItem == null)
            {
                Console.WriteLine("아이템을 찾을 수 없습니다.");
                return false;
            }

            gameManager.inventory.AddItem(purchasedItem);
            gameManager.Player.Gold -= shopItem.Price;
            shopItem.IsPurchased = true;

            Console.WriteLine($"{shopItem.Name}을 구매했습니다.");
            return true;
        }
        #endregion

        #region 아이템 판매
        public bool SellItem(Items playerItem)
        {
            if(playerItem == null)
            {
                return false;
            }

            if(playerItem.IsEquipped)
            {
                Console.WriteLine("장착 중인 아이템은 판매할 수 없습니다.");
                return false;
            }

            gameManager.inventory.RemoveItem(playerItem);
            gameManager.Player.Gold += playerItem.Price;

            foreach(var shopItem in shopInventory)
            {
                if(shopItem.ItemId == playerItem.ItemId)
                {
                    shopItem.IsPurchased = false;
                    break;
                }
            }

            Console.WriteLine($"{playerItem.Name}을 판매했습니다.");
            return true;
        }
        #endregion

        //public void ShowShopDisplay()
        //{
        //    Console.Clear();
        //    Messages.TextTitleHlight("상점");
        //    Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n\n[보유 골드]");
        //    Messages.TextMagentaHlight($"{gameManager.Player.Gold}");
        //    Console.WriteLine(" G\n\n[아이템 목록]");

        //    for (int i = 0; i < gameManager.GameItems.Length; i++)
        //    {
        //        Items items = gameManager.GameItems[i];

        //        Console.Write("- ");
        //        Messages.TextMagentaHlight($"{i + 1} ");
        //        Messages.Equipped(items.IsEquipped);
        //        Console.WriteLine($" {items.Name}\t | {items.ItemStats()}\t | {items.Desc}");
        //    }

        //    Console.WriteLine();
        //    Messages.TextMagentaHlight("1");
        //    Console.WriteLine(". 아이템 구매");
        //    Messages.TextMagentaHlight("2");
        //    Console.WriteLine(". 아이템 판매");
        //    Messages.TextMagentaHlight("0");
        //    Console.Write(". 나가기\n\n원하시는 행동을 입력해주세요.\n>> ");

        //    int intNumber = Messages.ReadInput(0, 2);

        //    switch (intNumber)
        //    {
        //        case 0:
        //            gameManager.GameMain();
        //            break;

        //        case 1:
        //            ItemBuy();
        //            break;

        //        case 2:
        //            ItemSell();
        //            break;
        //    }
        //}

        //public void ItemBuy()
        //{
        //    Console.Clear();
        //    Messages.TextTitleHlight("상점 - 아이템 구매");
        //    Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n\n[보유 골드]");
        //    Messages.TextMagentaHlight($"{gameManager.Player.Gold}");
        //    Console.WriteLine(" G\n\n[아이템 목록]");

        //    for (int i = 0; i < gameManager.GameItems.Length; i++)
        //    {
        //        Items items = gameManager.GameItems[i];

        //        Console.Write("- ");
        //        Messages.TextMagentaHlight($"{i + 1} ");
        //        Messages.Equipped(items.IsEquipped);
        //        if (items is Potion potion)
        //        {
        //            Console.WriteLine($" {items.Name} {(potion.PotionCount > 0 ? "x" + potion.PotionCount : "")}\t | {items.ItemStats()}\t | {items.Desc} | {items.Price}");
        //        }
        //        else
        //        {
        //            Console.WriteLine($" {items.Name}\t | {items.ItemStats()}\t | {items.Desc} | {(items.IsPurchased ? "구매 완료" : items.Price)}");
        //        }
        //    }

        //    Console.WriteLine();
        //    Messages.TextMagentaHlight("0");
        //    Console.Write(". 나가기\n\n원하시는 행동을 입력해주세요.\n>> ");

        //    int intNumber = Messages.ReadInput(0, gameManager.GameItems.Length);

        //    switch (intNumber)
        //    {
        //        case 0:
        //            ShowShopDisplay();
        //            break;

        //        default:

        //            int itemIndex = intNumber - 1;
        //            Items seletItem = gameManager.GameItems[itemIndex];

        //            if (gameManager.Player.Gold >= seletItem.Price)
        //            {
        //                if (seletItem is Potion potion)
        //                {
        //                    potion.PotionCount++;
        //                    potion.IsPurchased = true;
        //                    gameManager.Player.Gold -= seletItem.Price;
        //                }
        //                else if (!seletItem.IsPurchased)
        //                {
        //                    seletItem.IsPurchased = true;
        //                    gameManager.Player.Gold -= seletItem.Price;
        //                }
        //            }
        //            else if (gameManager.Player.Gold < seletItem.Price)
        //            {
        //                Console.WriteLine("소지금이 부족합니다.");
        //            }

        //            ItemBuy();
        //            break;
        //    }
        //}

        //public void ItemSell()
        //{
        //    Console.Clear();
        //    Messages.TextTitleHlight("상점 - 아이템 판매");
        //    Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n\n[보유 골드]");
        //    Messages.TextMagentaHlight($"{gameManager.Player.Gold}");
        //    Console.WriteLine(" G\n\n[아이템 목록]");

        //    for (int i = 0; i < gameManager.GameItems.Length; i++)
        //    {
        //        Items items = gameManager.GameItems[i];

        //        if (items.IsPurchased == true)
        //        {
        //            Console.Write("- ");
        //            Messages.TextMagentaHlight($"{i + 1} ");
        //            Messages.Equipped(items.IsEquipped);
        //            if (items is Potion potion)
        //            {
        //                Console.WriteLine($" {items.Name} {(potion.PotionCount > 0 ? "x" + potion.PotionCount : "")}\t | {items.ItemStats()}\t | {items.Desc} | {items.Price}");
        //            }
        //            else
        //            {
        //                Console.WriteLine($" {items.Name}\t | {items.ItemStats()}\t | {items.Desc} | {items.Price}");
        //            }
        //        }
        //    }

        //    Console.WriteLine();
        //    Messages.TextMagentaHlight("0");
        //    Console.Write(". 나가기\n\n원하시는 행동을 입력해주세요.\n>> ");

        //    int intNumber = Messages.ReadInput(0, gameManager.GameItems.Length);

        //    switch (intNumber)
        //    {
        //        case 0:
        //            ShowShopDisplay();
        //            break;

        //        default:
        //            int itemIndex = intNumber - 1;
        //            Items seletItem = gameManager.GameItems[itemIndex];

        //            if (seletItem is Potion potion)
        //            {
        //                if (potion.PotionCount > 0)
        //                {
        //                    potion.PotionCount--;
        //                    gameManager.Player.Gold += seletItem.Price;
        //                }

        //                if (potion.PotionCount == 0)
        //                {
        //                    potion.IsPurchased = false;
        //                }
        //            }
        //            else if (seletItem.IsPurchased)
        //            {
        //                seletItem.IsPurchased = false;
        //                gameManager.Player.Gold += seletItem.Price;
        //                seletItem.IsEquipped = false;
        //            }

        //            ItemSell();
        //            break;
        //    }

        //}
    }
