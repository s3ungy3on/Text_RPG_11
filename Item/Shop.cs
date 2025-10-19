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
            if (shopItem == null) //shopItem 값이 null 이라면 false 리턴(되돌아감)
            {
                return false;
            }

            if (shopItem.Price > gameManager.Player.Gold) //shopItem의 가격이 플레이어가 보유중인 골드보다 크다면 false 리턴(되돌아감)
            {
                Console.WriteLine("보유중인 골드가 부족합니다.");
                return false;
            }

            if(shopItem is Potion potion) //shopItem이 포션이라면,
            {
                gameManager.inventory.AddItem(potion); //인벤토리에 해당 아이템 추가
                gameManager.Player.Gold -= potion.Price; //플레이어 골드에서 아이템 가격만큼 빼기
                potion.IsPurchased = true; //아이템 구매 상태로 변경
                Console.WriteLine($"{shopItem.Name}을 구매했습니다.");
                return true; //true 값을 리턴해서 해당 메소드 종료

            }

            if (shopItem.IsPurchased) //아이템이 이미 구매한 상태라면, false 리턴(되돌아감)
            {
                Console.WriteLine("이미 구매한 아이템입니다.");
                return false;
            }

            Items purchasedItem = ItemDatabase.GetItemById(shopItem.ItemId); //상점에 있는 아이템은 원본 데이터임, 데이터 변경을 방지하기 위해 인벤토리에 넣을 새로운 아이템 객체(클론)를 Id로 불러옴

            if (purchasedItem == null)
            {
                Console.WriteLine("아이템을 찾을 수 없습니다.");
                return false;
            }

            gameManager.inventory.AddItem(purchasedItem); //인벤토리에 아이템 추가
            gameManager.Player.Gold -= shopItem.Price; //플레이어 골드에서 아이템 가격만큼 빼기
            shopItem.IsPurchased = true; //아이템 구매 상태로 변경

            Console.WriteLine($"{shopItem.Name}을 구매했습니다.");
            return true; //true값 리턴해서 메소드 종료
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
