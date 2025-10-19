using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Text_RPG_11
{
    public class GameManager
    {
        public Player player { get; set; }
        public Inventory inventory;
        public Shop shop;
        private Dungeon dungeon;
        public Skill skill;
        public Quest quest;
        public Monster monster;
        public UIManager uiManager;
        private Battle battle;
        public RewardManager rewardManager;
        public QuestManager questManager;

        public Player Player => player;

        public GameManager()
        {
            inventory = new Inventory(this);
            shop = new Shop(this);
            dungeon = new Dungeon(this);
            skill = new Skill(this);
            questManager = new QuestManager(this);
            monster = new Monster(this);
            uiManager = new UIManager(this);
            battle = new Battle(this);

        }

        public void GameStart()
        {
            ItemDatabase.GetShopItems();
            uiManager.Intro();
            GameMain();
        }

        public void CreatdPlayer(string name, Job job) //플레이어 생성, ui매니저에서 호출
        {
            player = new Player(name, job, inventory);
        }

        public void GameMain()
        {
            uiManager.MainScreen();

            int choice = Messages.ReadInput(0, 5);

            switch (choice)
            {
                case 0:
                    Environment.Exit(0); //종료
                    break;
                case 1://상태보기
                    break;
                case 2://인벤토리
                    uiManager.ShowInventoryDisplay(inventory);
                    break;
                case 3://탐험하기
                    break;
                case 4:
                    uiManager.ShowShopDisplay();
                    break;
                case 5://휴식하기
                    break;
            }
        }
    }
}
