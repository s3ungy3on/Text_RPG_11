using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_RPG_11;

namespace Text_RPG_11
{
    internal class Dungeon
    {
        private GameManager _gameManager;
        private Battle _battle;

        private int _monsterNum;
        
        public Dungeon(GameManager manager)
        {
            while (true)
            {
                // 배틀 시작
                _gameManager = manager;
                _battle = new Battle(manager);
                Console.WriteLine("Battle!!\n\n");
                _battle.EnemySpawn();

                Console.WriteLine("[내 정보]");
                Console.WriteLine($"Lv.{_gameManager.Player.Level} Chad ({_gameManager.Player.Job}) \n\n HP {_gameManager.Player.HP} / {_gameManager.Player.MaxHP}\n\nMP {_gameManager.Player.MP} / {_gameManager.Player.MaxMP}\n\n");

                Console.WriteLine("1. 공격");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                int.TryParse(Console.ReadLine(), out int playerInput);

                if (playerInput == 1)
                    PlayerTurn();
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }

        public void EnemyInfo()
        {
            foreach (var enemy in _battle.Enemies)
            {
                Console.WriteLine($"Lv. {enemy.Level} {enemy.Name}  HP {enemy.HP}");
            }
        }

        // 공격 / 스킬 사용 선택
        public void PlayerTurn()
        {
            Console.WriteLine("Battle!!\n\n");   
            EnemyInfo();
            
            Console.WriteLine("[내 정보]");
            Console.WriteLine($"Lv.{_gameManager.Player.Level} Chad ({_gameManager.Player.Job}) \n\n HP {_gameManager.Player.HP} / {_gameManager.Player.MaxHP}\n\nMP {_gameManager.Player.MP} / {_gameManager.Player.MaxMP}\n\n");
            bool isWorked = int.TryParse(Console.ReadLine(), out int result);

            switch (result)
            {
                case 1:
                    PlayerTurnAttack();
                    break;
                case 2:
                    PlayerTurnSkillSelect();
                    break;
            }
        }
        
        // 공격
        public void PlayerTurnAttack()
        {
            Console.WriteLine("Battle!!\n\n");   
            _battle.EnemyInfo();
            
            Console.WriteLine("[내 정보]");
            Console.WriteLine($"Lv.{_gameManager.Player.Level} Chad ({_gameManager.Player.Job}) \n\n HP {_gameManager.Player.HP} / {_gameManager.Player.MaxHP}\n\n");
            
            Console.WriteLine("몬스터 숫자. 공격\n\n");
            Console.WriteLine("0. 취소\n\n");

            Console.WriteLine("대상을 선택해주세요.");
            Console.Write(">>");
            
            bool isWorked = int.TryParse(Console.ReadLine(), out _monsterNum);

            if (isWorked || _monsterNum == 0)
            {
                // 나가기
            }
            else if(_monsterNum > 0 && _monsterNum <= _battle.Enemies.Count)
            {
                _battle.Attack(_monsterNum - 1);
                PlayerTurnEnd();
            }
        }
        
        // 스킬 선택
        public void PlayerTurnSkillSelect()
        {
            
        }
        
        // 스킬 사용
        public void PlayerTurnSkill()
        {
            
        }

        public void PlayerTurnEnd()
        {
            _battle.EnemyInfo();
            Console.WriteLine("Battle!!\n\n");
            
            Console.WriteLine($"{_gameManager.Player.Name}의 공격!");
            Console.WriteLine($"Lv.{_battle.Enemies[_monsterNum - 1].Level} {_battle.Enemies[_monsterNum - 1].Name}을(를) 맞췄습니다. [데미지 : {_battle.AtkRand}\n\n");
            
            Console.WriteLine($"Lv. {_battle.Enemies[_monsterNum - 1].Level} {_battle.Enemies[_monsterNum - 1].Name}");
            Console.WriteLine($"HP {_battle.EnemyHP} -> {_battle.Enemies[_monsterNum - 1].HP}\n\n");
            
            Console.WriteLine($"0. 다음\n\n");
            
            Console.WriteLine("대상을 선택해주세요.");
            Console.Write(">>");
            
            bool isWorked = int.TryParse(Console.ReadLine(), out int result);

            if (isWorked || result== 0)
            {
                EnemyTurnToPlayerTurn();
            }
        }

        public void EnemyTurnToPlayerTurn()
        {
            _battle.EnemyInfo();
            Console.WriteLine("Battle!!\n\n");
            Console.WriteLine($"Lv. {_battle.Enemies[_battle.Index].Level} {_battle.Enemies[_battle.Index].Name}의 공격!");
            Console.WriteLine($"{_gameManager.Player.Job} 을(를) 맞췄습니다.    [데미지 : {_battle.Enemies[_battle.Index].Attack}\n\n");
            
            Console.WriteLine($"Lv.{_gameManager.Player.Level} Chad ({_gameManager.Player.Job})");
            Console.WriteLine($"HP {_battle.PlayerHP} -> {_gameManager.Player.HP}\n\n");
            
            Console.WriteLine($"0. 다음\n\n");
            
            Console.WriteLine("대상을 선택해주세요.");
            Console.Write(">>");
            
            bool isWorked = int.TryParse(Console.ReadLine(), out int result);

            if (isWorked || result== 0)
            {
                PlayerTurn();
            }
        }
    }
}