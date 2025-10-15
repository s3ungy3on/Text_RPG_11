using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Text_RPG_11;

namespace Text_RPG_11
{
    internal class UIManager
    {
        public string act; // 행동 번호 입력
        public string name; // 플레이어 이름
        public string job; // 플레이어 직업

        public string logo = " .|'''.|                             .              '||''|.                                                      \r\n" +
            " ||..  '  ... ...   ....   ... ..  .||.   ....       ||   ||  ... ...  .. ...     ... .   ....    ...   .. ...   \r\n" +
            "  ''|||.   ||'  || '' .||   ||' ''  ||   '' .||      ||    ||  ||  ||   ||  ||   || ||  .|...|| .|  '|.  ||  ||  \r\n" +
            ".     '||  ||    | .|' ||   ||      ||   .|' ||      ||    ||  ||  ||   ||  ||    |''   ||      ||   ||  ||  ||  \r\n" +
            "|'....|'   ||...'  '|..'|' .||.     '|.' '|..'|'    .||...|'   '|..'|. .||. ||.  '||||.  '|...'  '|..|' .||. ||. \r\n" +
            "           ||                                                                   .|....'                          \r\n" +
            "          ''''                                                                                                   ";

        private GameManager gameManager;

        public UIManager(GameManager manager)
        {
            gameManager = manager;
        }

        public void Intro() // 시작시 초기 설정 및 스토리 화면
        {
            foreach (char ch in logo)
            {
                Console.Write(ch);
                Thread.Sleep(4);
            }
            Thread.Sleep(1000);

            Console.WriteLine("스파르타 마을에 오신 용사님 환영합니다." +
                "\n용사님의 이름은 무엇인가요.\n");
            Console.Write(">>");
            name = Console.ReadLine(); // 이름 입력 
            Console.Clear();
            Console.WriteLine($"{name}이름이 맞으십니까?\n\n1. 맞습니다\n2. 아닙니다\n\n");
            act = Console.ReadLine();
            gameManager.Player.Name = name; // 게임매니저에다가 이름 넣어주기
            Console.Clear();
            while (true)
            {
                if (act == "1")
                {
                    break;
                }
                else if (act == "2")
                {
                    Console.Clear();
                    Console.WriteLine("용사님의 이름을 다시 알려주십시오\n\n");
                    name = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("용사란 놈이 이거 하나 제대로 못하나\n\n");
                    Console.WriteLine($"이름이 {name} 맞냐고 아니냐고\n\n1. 맞습니다\n2. 아닙니다.\n\n");
                    act = Console.ReadLine();
                    continue;
                }
                Console.WriteLine($"{name}이름이 맞으십니까?\n\n1. 맞습니다\n2. 아닙니다\n\n");
                act = Console.ReadLine();
            }
            Console.WriteLine($"{name} 용사님 과연 이름부터가 휘황찬란하시군요\n헌데 용사님의 직업은 무엇인지요\n\n" +
                $"1. 전사, 2. 마법사, 3. 궁수");
            Console.Write(">>");
            act = Console.ReadLine();
            while (true)
            {
                if (act == "1")
                {
                    Console.Clear();
                    job = "전사";
                    break;
                }
                else if (act == "2")
                {
                    Console.Clear();
                    job = "마법사";
                    break;
                }
                else if (act == "3")
                {
                    Console.Clear();
                    job = "궁수";
                    break;
                }
                else
                {
                    Console.Clear(); Console.WriteLine("마 니 용사 맞나?\n혹시 폐급 용사가?\n단디 해라이\n\n1. 전사, 2. 마법사, 3. 궁수");
                    Console.Write(">>");
                    act = Console.ReadLine();
                }
            }
            Console.WriteLine($"{job}시라니 정말 대단한 직업이군요.");
            gameManager.Player.Job = job;
        }

        public void MainScreen()
        {
            Console.Clear();
            Console.WriteLine("이 곳에서 던전으로 들어가기 전 활동을 할 수 있습니다\n\n1. 상태보기\n2. 인벤토리" +
                "\n3. 탐험하기\n4. 상점\n\n원하시는 행동을 입력해주세요");
            Console.Write(">>");
            act = Console.ReadLine();
        }

        public void Rest()
        {
            Console.Clear();
            Console.WriteLine("지친 피로를 충분히 풀고 있습니다\n용사의 체력과 마나가 모두 찹니다.");
            // 현재체력과 마나를 모두 최대치랑 똑같이 맞추기
        }
    }
}


//while (true)
//{
//    if (act == "0")
//    {
//        Console.Clear();
//        Console.WriteLine("이 곳에서 던전으로 들어가기 전 활동을 할 수 있습니다\n\n1. 상태보기\n2. 인벤토리" +
//            "\n3. 탐험하기\n4. 상점\n\n원하시는 행동을 입력해주세요");
//        Console.Write(">>");
//        act = Console.ReadLine();
//    }
//    else if (act == "1")
//    {
//        Console.Clear();
//        playerMake.TotalPlayerStat();
//        inventory.UseItems();
//        Console.WriteLine("\n\n0. 나가기\n\n원하시는 행동을 입력해주세요");
//        Console.Write(">>");
//        act = Console.ReadLine();
//        if (act != "0")
//        {
//            while (true)
//            {
//                Console.Clear();
//                playerMake.TotalPlayerStat();
//                inventory.UseItems();
//                Console.WriteLine("\n\n0. 나가기\n\n잘못된 입력 입니다. 다시 입력하십시오.");
//                Console.Write(">>");
//                act = Console.ReadLine();
//                if (act == "0") { break; }
//            }
//        }
//    }
//    else if (act == "2")
//{
//    act = "0";
//    while (true)
//    {
//        if (act == "0")
//        {
//            inventory.InventoryInfo();
//            Console.WriteLine("\n1. 장착 관리\n0. 나가기\n\n원하시는 행동을 입력해주세요");
//            Console.Write(">>");
//            act = Console.ReadLine();
//            if (act == "0") break;
//            continue;
//        }
//        else if (act == "1")
//        {
//            while (true)
//            {
//                inventory.InventoryItemUse();
//                Console.Write("\n0. 나가기\n\n원하시는 행동을 입력해주세요.\n>>");
//                act = Console.ReadLine();
//                if (act == "0") { break; }
//                else if (1 <= int.Parse(act) && int.Parse(act) <= inventory.myItems.Count)
//                {
//                    inventory.TotalStatItemUse(int.Parse(act), playerMake);
//                    continue;
//                }
//                else { continue; }
//            }
//        }
//        else
//        {
//            while (true)
//            {
//                Console.Clear();
//                inventory.InventoryInfo();
//                Console.WriteLine("\n1. 장착 관리\n0. 나가기\n\n잘못된 입력입니다\n다시 입력하십시오\n");
//                act = Console.ReadLine();
//                if (act == "0" || act == "1") { break; }
//            }
//            if (act == "1") { continue; }
//            break;
//        }
//    }
//}
//else if (act == "3")
//{
//    Random random = new Random();
//    Monster monster = new Monster();
//    Adventure adventure = new Adventure();
//    int i = random.Next(0, 10);
//    if (i < 5) monster.Goblin();
//    else if (i >= 5 && i <= 7) monster.Oak();
//    else if (i > 7) monster.Ghost();
//    adventure.AdventureStat(monster, playerMake);
//    act = "0";
//}
//else if (act == "4")
//{
//    while (true)
//    {
//        Console.Clear();
//        shop.DisplayItems(playerMake);
//        Console.WriteLine("\n\n0. 나가기\n\n구매 할 아이템 번호 또는 행동을 입력하시오");
//        act = Console.ReadLine();
//        if (act == "0") { break; }
//        else { shop.ItemBuy(playerMake, act, inventory); }
//    }
//}
//else
//{
//    while (true)
//    {
//        Console.Clear();
//        Console.WriteLine("이 곳에서 던전으로 들어가기 전 활동을 할 수 있습니다\n\n1. 상태보기\n2. 인벤토리" +
//            "\n3. 탐험하기\n4. 상점\n\n");
//        Console.WriteLine("잘못된 입력입니다\n다시 입력하십시오\n");
//        Console.Write(">>");
//        act = Console.ReadLine();
//        if (act == "1" || act == "2" || act == "3") { break; }
//    }
//}

//if (playerMake.alive == false) break;

//// 레벨업 시 상점에 아이템 추가하는 로직
//if (playerMake.level == 2 && playerMake.levelUp == true)
//{
//    playerMake.levelUp = false;
//    shop.AddItem(new Item("롱 소드", 12, "공격력 +", 8, "철로 만들어진 긴 검으로 타격감이 좋습니다", false));
//    Console.ReadLine();
//    shop.AddItem(new Item("정령의 형상", 40, "마나 +", 20, "정령을 본 따 만들어진 물건으로 마나순환이 빨라지는 기분이 듭니다", false));
//    Console.ReadLine();
//    shop.AddItem(new Item("에이스 방패", 17, "방어력 +", 6, "에이스 대장장이가 만들었다고 전해지는 방패입니다.", false));
//    Console.ReadLine();
//    shop.AddItem(new Item("롯데팬의 분노", 999, "공격력 +", 999, "5%확률을 뚫고 가을을 가지 못 한 사직의 분노입니다.", false));
//    Console.ReadLine();
//}
//}
