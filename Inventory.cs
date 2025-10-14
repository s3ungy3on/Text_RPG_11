using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    internal class Inventory
    {
        private GameManager gameManager;

        public Inventory(GameManager manager)
        {
            gameManager = manager;
        }

        public void ShowInventoryDisplay()
        {
            Console.Clear();
            Messages.TextTitleHlight("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n\n[아이템 목록]");

            for (int i = 0; i < gameManager.GameItems.Length; i++)
            {
                Items items = gameManager.GameItems[i];

                if (items.IsPurchased == true)
                {
                    Console.Write("- ");
                    Messages.TextMagentaHlight($"{i + 1}");
                    Messages.Equipped(items.IsEquipped);
                    Console.WriteLine($"{items.Name}\t | {items.ItemStats()}\t | {items.Desc}");
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
                    //메인메뉴
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

                if (items.IsPurchased == true)
                {
                    Console.Write("- ");
                    Messages.TextMagentaHlight($"{i + 1}");
                    Messages.Equipped(items.IsEquipped);
                    Console.WriteLine($"{items.Name}\t | {items.ItemStats()}\t | {items.Desc}");
                }
            }

            Console.WriteLine();
            Messages.TextMagentaHlight("0");
            Console.Write(". 나가기\n\n원하시는 행동을 입력해주세요.\n>> ");

            int intNumber = Messages.ReadInput(0, gameManager.GameItems.Length);

            switch (intNumber)
            {
                case 0:
                    //메인메뉴로
                    break;

                default:
                    int itemIndex = intNumber - 1;
                    Items selectItem = gameManager.GameItems[itemIndex];

                    if (!selectItem.IsEquipped)
                    {
                        foreach(var items in gameManager.GameItems)
                        {
                            items.IsEquipped = false;
                        }

                        selectItem.IsEquipped = true;
                    }
                    else
                    {
                        selectItem.IsEquipped = false;
                    }

                    ItemEquipped();
                    break;

            }
        }


    }
}
