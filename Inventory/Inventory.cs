using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    public class Inventory
    {
        private GameManager gameManager;
        public Items[] EquipSlots { get; private set; }
        public const int MAX_EQUIP_SLOTS = 7;
        public List<Items> Bag { get; private set; }
        public enum SlotType
        {
            Item1 = 0,
            Item2 = 1,
            Item3 = 2,
            Item4 = 3,
            Item5 = 4,
            Item6 = 5,
            Potion = 6
        }

        public Inventory(GameManager manager)
        {
            gameManager = manager;
            EquipSlots = new Items[MAX_EQUIP_SLOTS];
            Bag = new List<Items>();
        }

        #region 아이템 추가 기능
        public void AddItem(Items item) //EquipSlots에 아이템 추가
        {
            if (item is Potion newPotion) //포션이면 수량 합치기
            {
                int potionSlot = (int)SlotType.Potion;

                if (EquipSlots[potionSlot] is Potion existingPotion && existingPotion.Name == newPotion.Name)
                {
                    existingPotion.PotionCount += newPotion.PotionCount;
                    return;
                }
                else if (EquipSlots[potionSlot] == null)
                {
                    EquipSlots[potionSlot] = newPotion;
                    newPotion.IsPurchased = true;
                }

                return;
            }

            for (int i = (int)SlotType.Item1; i <= (int)SlotType.Item6; i++) //비어있는 슬롯에 아이템 넣기
            {
                if (EquipSlots[i] == null)
                {
                    EquipSlots[i] = item;
                    item.IsPurchased = true;
                    return;
                }
            }
        }
        #endregion

        //public Items GetItemById(int itemId) //id로 아이템 검색하기
        //{
        //    foreach (var item in EquipSlots) //장착 슬롯에서 검색
        //    {
        //        if(item != null && item.Get)
        //    }

        //    return Bag.FirstOrDefault(i => i.)
        //}
        //}

        //public int GetItemQuantity(int itemId) //아이템 수량 확인
        //{

        //}

        //public void RemoveItem(int itemId) //아이템 제거
        //{

        //}

        //public bool IsFull() //아이템 꽉찼는지
        //{

        //}

        //public int? GetEmptySlotIndex() //비어있는 칸 찾기
        //{

        //}

        //public void SortItems() //정렬기능
        //{

        //}































        public void ShowInventoryDisplay()
        {
            Console.Clear();
            Messages.TextTitleHlight("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n\n[아이템 목록]");

            for (int i = 0; i < gameManager.GameItems.Length; i++)
            {
                Items items = gameManager.GameItems[i];

                if (items.IsPurchased == true) //보유중인 아이템만 출력
                {
                    Console.Write("- ");
                    Messages.TextMagentaHlight($"{i + 1} ");
                    Messages.Equipped(items.IsEquipped);

                    if(items is Potion potion && potion.PotionCount > 0) //포션을 0개 이상 보유중이면 개수 출력
                    {
                        Console.WriteLine($"{items.Name} x{potion.PotionCount}\t | {items.ItemStats()}\t | ");
                    }
                    else
                    {
                        Console.WriteLine($" {items.Name}\t | {items.ItemStats()}\t |");
                    }
                }
            }

            Console.WriteLine();
            Messages.TextMagentaHlight("1");
            Console.WriteLine(". 장착 관리");
            Messages.TextMagentaHlight("2");
            Console.WriteLine(". 아이템 정렬");
            Messages.TextMagentaHlight("0");
            Console.WriteLine(". 나가기\n\n원하시는 행동을 입력해주세요.\n>> ");

            int intNumber = Messages.ReadInput(0, 2);

            switch (intNumber)
            {
                case 0:
                    gameManager.GameMain(); //메인으로
                    break;

                case 1:
                    ItemEquipped(); //장착관리
                    break;

                case 2:
                    //아이템 정렬
                    break;  
            }
        }

        public void ItemEquipped()
        {
            Console.Clear();
            Messages.TextTitleHlight("인벤토리 - 장착 관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n\n[아이템 목록]");

            for (int i = 0; i < gameManager.GameItems.Length; i++)
            {
                Items items = gameManager.GameItems[i];

                if (items.IsPurchased == true && items.ItemType() != "물약") //물약 타입 제외하고 출력
                {
                    Console.Write("- ");
                    Messages.TextMagentaHlight($"{i + 1} ");
                    Messages.Equipped(items.IsEquipped);
                    Console.WriteLine($" {items.Name}\t | {items.ItemStats()}\t");
                }
            }

            Console.WriteLine();
            Messages.TextMagentaHlight("0");
            Console.Write(". 나가기\n\n원하시는 행동을 입력해주세요.\n>> ");

            int intNumber = Messages.ReadInput(0, gameManager.GameItems.Length);

            switch (intNumber)
            {
                case 0:
                    ShowInventoryDisplay(); //인벤토리로 돌아가기
                    return;

                default:
                    int itemIndex = intNumber - 1;
                    Items selectItem = gameManager.GameItems[itemIndex];

                    if (!selectItem.IsEquipped)
                    {
                        foreach(var items in gameManager.GameItems)
                        {
                            if(items.ItemType() == selectItem.ItemType())
                            {
                                items.IsEquipped = false; //같은 타입 장착 해제
                            }
                        }

                        selectItem.IsEquipped = true; //아이템 장착
                    }
                    else
                    {
                        selectItem.IsEquipped = false; //장착된 걸 선택하면 장착 해제
                    }

                    gameManager.Player.StatUpdate(gameManager);
                    ItemEquipped();
                    break;

            }
        }


    }
}
