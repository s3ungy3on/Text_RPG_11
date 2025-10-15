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
            _gameManager = manager;
            _battle = new Battle(manager);
        }

        public void DungeonBattle()
        {
            while (_battle.BattleState == Battle.BattleResult.InProgress)
            {
                // 1. 배틀 시작
                Console.WriteLine("\nBattle!!\n");
                _battle.PlayerInitialHP = _gameManager.Player.HP;
                
                // 2. 몬스터 스폰
                _battle.EnemySpawn();
                EnemyInfo();
                Console.WriteLine();
                
                // 3. 내 정보 출력
                Console.WriteLine("[내 정보]");
                Console.WriteLine($"Lv.{_gameManager.Player.Level} Chad ({_gameManager.Player.Job})\n\nHP {_gameManager.Player.HP} / {_gameManager.Player.MaxHP}\nMP {_gameManager.Player.MP} / {_gameManager.Player.MaxMP}\n");
                
                // 4. 행동 입력
                Console.WriteLine("1. 공격\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                int.TryParse(Console.ReadLine(), out int result);

                if (result == 1)
                    PlayerTurn();
                else
                    Console.WriteLine("잘못된 입력입니다.");
            }
        }

        // 몬스터 정보
        public void EnemyInfo()
        {
            for (int i = 0; i < _battle.Enemies.Count; i++)
            {
                if(_battle.Enemies[i].isDead)
                    // 1. 몬스터 체력이 0일 시 Dead로 처리
                    Console.WriteLine($"[{i + 1}] Lv. {_battle.Enemies[i].Level} {_battle.Enemies[i].Name}  Dead");
                // 회색 처리 필요
                else
                    // 2. 몬스터 체력이 0 이상일 시 HP 출력
                    Console.WriteLine($"[{i + 1}] Lv. {_battle.Enemies[i].Level} {_battle.Enemies[i].Name}  HP {_battle.Enemies[i].HP}");
            }
        }

        // 공격 / 스킬 사용 선택
        public void PlayerTurn()
        {
            // 1. 턴 시작 전 배틀 종료 조건 확인
            if (BattleEndCheck()) return;
            
            while (true)
            {
                Console.WriteLine("\nBattle!!\n");   
                EnemyInfo();
                Console.WriteLine();
                
                Console.WriteLine("[내 정보]");
                Console.WriteLine($"Lv.{_gameManager.Player.Level} Chad ({_gameManager.Player.Job})\n\nHP {_gameManager.Player.HP} / {_gameManager.Player.MaxHP}\nMP {_gameManager.Player.MP} / {_gameManager.Player.MaxMP}\n");
            
                // 2. 플레이어 행동 선택
                Console.WriteLine("1. 공격");
                Console.WriteLine("2. 스킬\n");
            
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                bool isWorked = int.TryParse(Console.ReadLine(), out int result);

                // 3. 입력에 따라 공격 / 스킬 메서드 진행
                switch (result)
                {
                    case 1:
                        PlayerTurnAttack();
                        break;
                    case 2:
                        PlayerTurnSkillSelect();
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }
        }
        
        // 공격 선택 > 공격할 몬스터 선택
        public void PlayerTurnAttack()
        {
            while (true)
            {
                Console.WriteLine("\nBattle!!\n");   
                EnemyInfo();
                Console.WriteLine();
            
                Console.WriteLine("[내 정보]");
                Console.WriteLine($"Lv.{_gameManager.Player.Level} Chad ({_gameManager.Player.Job})\nHP {_gameManager.Player.HP} / {_gameManager.Player.MaxHP}\n");
                
                // 1. 공격할 몬스터 입력
                Console.WriteLine("몬스터 숫자. 공격");
                Console.WriteLine("0. 취소\n");
                
                Console.WriteLine("대상을 선택해주세요.");
                Console.Write(">> ");
            
                bool isWorked = int.TryParse(Console.ReadLine(), out _monsterNum);

                // 2. 몬스터 공격
                if(_monsterNum > 0 && _monsterNum <= _battle.Enemies.Count)
                {
                    _battle.Attack(_monsterNum - 1);
                    PlayerTurnEnd();
                }
                else if (isWorked || _monsterNum == 0)
                {
                    // 나가기
                }
                else
                    Console.WriteLine("잘못된 입력입니다.");
            }
        }
        
        // 스킬 선택
        public void PlayerTurnSkillSelect()
        {
            
        }
        
        // 스킬 사용할 몬스터 선택
        public void PlayerTurnSkill()
        {
            
        }
        
        // 플레이어 턴 이후 결과 출력
        public void PlayerTurnEnd()
        {
            while (true)
            {
                EnemyInfo();
                Console.WriteLine("\nBattle!!\n");
            
                Console.WriteLine($"{_gameManager.Player.Name}의 공격!");
                Console.WriteLine($"Lv.{_battle.Enemies[_monsterNum - 1].Level} {_battle.Enemies[_monsterNum - 1].Name}을(를) 맞췄습니다. [데미지 : {_battle.AtkRand}]\n");
                
                // 1. 플레이어 공격으로 몬스터 사망 시 Dead 처리
                if (_battle.Enemies[_monsterNum - 1].isDead)
                {
                    Console.WriteLine($"Lv. {_battle.Enemies[_monsterNum - 1].Level} {_battle.Enemies[_monsterNum - 1].Name}");
                    Console.WriteLine($"HP {_battle.EnemyHP} -> Dead\n");
                }
                
                // 2. 몬스터 턴 진행
                Console.WriteLine($"0. 다음\n");
            
                Console.WriteLine("원하시는 행동을 입력해주세요.\n");
                Console.Write(">> ");
                
                bool isWorked = int.TryParse(Console.ReadLine(), out int result);
                
                if (isWorked || result== 0)
                {
                    EnemyTurn();
                }
                else
                    Console.WriteLine("잘못된 입력입니다.");
            }
        }

        // 에너미 턴 진행
        public void EnemyTurn()
        {
            // 1. 턴 시작 전 배틀 종료 조건 확인
            if (BattleEndCheck()) return;
            
            while (true)
            {
                EnemyInfo();
                Console.WriteLine("\nBattle!!\n");
                // 2. 공격 전, 플레이어의 체력을 저장
                _battle.PlayerHP = _gameManager.Player.HP;
                
                // 3. 살아있는 모든 몬스터가 플레이어를 공격
                for (_battle.Index = 0; _battle.Index < _battle.Enemies.Count; _battle.Index++)
                {
                    _battle.EnemyTurn();
                    
                    if (_battle.Enemies[_battle.Index].isDead)
                    {
                        // 죽은 몬스터의 공격은 출력 X
                    }
                    else
                    {
                        Console.WriteLine($"Lv. {_battle.Enemies[_battle.Index].Level} {_battle.Enemies[_battle.Index].Name}의 공격!");
                        Console.WriteLine($"{_gameManager.Player.Name} 을(를) 맞췄습니다.    [데미지 : {_battle.Enemies[_battle.Index].Attack}]\n");
                    }
                }
                
                // 4. 결과
                Console.WriteLine($"Lv.{_gameManager.Player.Level} Chad ({_gameManager.Player.Job})");
                Console.WriteLine($"HP {_battle.PlayerHP} -> {_gameManager.Player.HP}\n");
                
                // 5. 플레이어 턴 진행
                Console.WriteLine($"0. 다음\n");
            
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
            
                bool isWorked = int.TryParse(Console.ReadLine(), out int result);

                if (isWorked || result== 0)
                {
                    PlayerTurn();
                }
                else
                    Console.WriteLine("잘못된 입력입니다.");
            }
        }
        
        // 플레이어 승리 / 패배 확인 함수
        public bool BattleEndCheck()
        {
            // 1. 배틀 종료 조건 확인
            _battle.EndCheck();
            
            if (_battle.BattleState == Battle.BattleResult.Victory)
            {
                // 2. 승리 조건 충족 시 Victory 메서드 실행
                Victory();
                return true;
            }
            else if (_battle.BattleState == Battle.BattleResult.Defeat)
            {
                // 3. 패배 조건 충족 시 Defeat 메서드 실행
                Defeat();
                return true;
            }
            
            return false;
        }

        // 플레이어 승리 시 출력
        public void Victory()
        {
            while (true)
            {
                _battle.ClearReward();
                Console.WriteLine("Battle!! - Result\n");
                Console.WriteLine("Victory\n");
                Console.WriteLine($"던전에서 몬스터 {_battle.Enemies.Count}마리를 잡았습니다.\n");
            
                Console.WriteLine("[캐릭터 정보]");
                Console.WriteLine($"Lv.{_gameManager.Player.Level} Chad ({_gameManager.Player.Job})");
                Console.WriteLine($"HP {_battle.PlayerInitialHP} -> {_gameManager.Player.HP}\n");
                
                // 1. 획득 아이템 출력
                Console.WriteLine("[획득 아이템]");
                Console.WriteLine($"{_battle.RewardGold} Gold\n");
            
                Console.WriteLine($"0. 다음\n");
            
                Console.Write(">> ");
            
                bool isWorked = int.TryParse(Console.ReadLine(), out int result);

                if (isWorked || result== 0)
                {
                    // 돌아가기
                }
                else
                    Console.WriteLine("잘못된 입력입니다.");
            }
        }

        // 플레이어 패배 시 출력
        public void Defeat()
        {
            while (true)
            {
                Console.WriteLine("Battle!! - Result\n");
                Console.WriteLine("You Lose\n");
            
                Console.WriteLine("[캐릭터 정보]");
                Console.WriteLine($"Lv.{_gameManager.Player.Level} Chad ({_gameManager.Player.Job})");
                Console.WriteLine($"HP {_battle.PlayerInitialHP} -> {_gameManager.Player.HP}\n");
            
                Console.WriteLine($"0. 다음\n");
            
                Console.Write(">> ");
            
                bool isWorked = int.TryParse(Console.ReadLine(), out int result);

                if (isWorked || result== 0)
                {
                    // 돌아가기
                }
                else
                    Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }
}