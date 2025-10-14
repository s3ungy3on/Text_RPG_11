using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Text_RPG_11;

namespace Text_RPG_11
{
    internal class GameManager
    {
        private Player player;
        private Inventory inventory;
        private Shop shop;
        //private Dungeon dungeon;
        //private SaveManager saveManager;
        private Skill skill;
        //private Quest quest;
        //private Monster monster;
        private UIManager uiManager;
        //private Battle battle;

        public Player Player => player;
        public Items[] GameItems { get; private set; }

        public GameManager()
        {
            GameItems = ItemDatabase.CreateItemsData();
            //player = new Player($"{player.Name}", 1, $"{player.Job}", 10, 5, 100, 1500);
            inventory = new Inventory(this);
            shop = new Shop(this);
            uiManager = new UIManager(this);
        }

        public void GameStart()
        {
            player = new Player($"", 1, $"", 10, 5, 100, 1500);
            uiManager.Intro();
            uiManager.MainScreen();
        }
    }
}
