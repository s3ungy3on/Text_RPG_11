using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.IO;
using System.Text.Json;

namespace Text_RPG_11
{
    internal class GameManager
    {
        private Player player;
        private Inventory inventory;
        private Shop shop;
        private Dungeon dungeon;
        private SaveManager saveManager;
        private Skill skill;
        private Quest quest;
        private Monster monster;
        private UIManager uiManager;
        private Battle battle;

        public Player Player => player;
        public Items[] GameItems { get; private set; }

        public GameManager()
        {
            GameItems = ItemDatabase.CreateItemsData();
            inventory = new Inventory(this);
            shop = new Shop(this);
            dungeon = new Dungeon(this);
            saveManager = new SaveManager(this);
            // skill = new Skill(this);
            // quest = new Quest(this);
            // monster = new Monster(this);
            uiManager = new UIManager(this);
            battle = new Battle(this);

        }

        public void GameStart()
        {
            string jsonString = File.ReadAllText("jobs.json");

            JobData? jobData = JsonSerializer.Deserialize<JobData>(jsonString);

            Job? selectedJob = jobData.jobs.Find(j => j.name == "전사");

            player = new Player
            (
            name: "",
        level: 1,
        job: selectedJob.name,
        attack: selectedJob.baseStats.attack,
        defense: selectedJob.baseStats.defense,
        defaultHP: selectedJob.baseStats.hp,
        defaultMP: selectedJob.baseStats.mp,
        gold: selectedJob.baseStats.gold
            );

            uiManager.Intro();
            GameMain();
        }

        public void GameMain()
        {
            uiManager.MainScreen();
        }

        public void InventoryOpen()
        {
            inventory.ShowInventoryDisplay();
        }

        public void ShopOpen()
        {
            shop.ShowShopDisplay();
        }
    }
}
