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
        
        public Dungeon(GameManager manager)
        {
            while (true)
            {
                // 배틀 시작
                _gameManager = manager;
                _battle = new Battle(manager);
                Console.WriteLine("Battle!!\n\n");
                _battle.MonsterSpawn();

                Console.WriteLine("[내 정보]");
                Console.WriteLine($"Lv.{_gameManager.Player.Level} Chad ({_gameManager.Player.Job}) \n\n HP {_gameManager.Player.HP} / {_gameManager.Player.MaxHP}\n\n");

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

        // 공격 / 스킬 사용 선택
        public void PlayerTurn()
        {
            Console.WriteLine("Battle!!\n\n");   
            _battle.MonsterInfo();
            
            Console.WriteLine("[내 정보]");
            Console.WriteLine($"Lv.{_gameManager.Player.Level} Chad ({_gameManager.Player.Job}) \n\n HP {_gameManager.Player.HP} / {_gameManager.Player.MaxHP}\n\n");
            bool isWorked = int.TryParse(Console.ReadLine(), out int result);

            switch (result)
            {
                case 1:
                    PlayerAttack();
                    break;
                case 2:
                    PlayerSkill();
                    break;
            }
        }
        
        // 공격
        public void PlayerAttack()
        {
            Console.WriteLine("Battle!!\n\n");   
            _battle.MonsterInfo();
            
            Console.WriteLine("[내 정보]");
            // 추후 Player 클래스 수정 후 currentHP or HpMax로 추가 예정
            Console.WriteLine($"Lv.{_gameManager.Player.Level} Chad ({_gameManager.Player.Job})  HP {_gameManager.Player.HP}\n\n");
            
            // 플레이어 턴 = 공격 입력
            // 만약 1을 입력했다면 공격
            // 만약 2를 입력했다면 스킬을 보여주고 > 스킬 공격
            
            Console.WriteLine("몬스터 숫자. 공격\n\n");
            Console.WriteLine("0. 취소\n\n");

            Console.WriteLine("대상을 선택해주세요.");
            Console.Write(">>");
            
            bool isWorked = int.TryParse(Console.ReadLine(), out int monsterNum);

            if (isWorked || monsterNum == 0)
            {
                // 나가기
            }
            else if(monsterNum > 0 && monsterNum <= _battle.Enemies.Count)
            {
                _battle.Attack(monsterNum - 1);
                PlayerTurnEnd();
            }
        }
        
        // 스킬 선택
        public void PlayerSkillSelect()
        {
            
        }
        
        // 스킬 공격
        public void PlayerSkill()
        {
            
        }

        public void PlayerTurnEnd()
        {
            
        }

        public void EnemyTurnToPlayerTurn()
        {
            _battle.MonsterInfo();
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