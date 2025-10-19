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
        public bool BuyItem(Items shopItem)
        {
            if (shopItem == null)
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
            if (playerItem == null)
            {
                return false;
            }

            if (playerItem.IsEquipped)
            {
                Console.WriteLine("장착 중인 아이템은 판매할 수 없습니다.");
                return false;
            }

            gameManager.inventory.RemoveItem(playerItem);
            gameManager.Player.Gold += playerItem.Price;

            foreach (var shopItem in shopInventory)
            {
                if (shopItem.ItemId == playerItem.ItemId)
                {
                    shopItem.IsPurchased = false;
                    break;
                }
            }

            Console.WriteLine($"{playerItem.Name}을 판매했습니다.");
            return true;
        }
        #endregion
    }
}
