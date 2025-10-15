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
            // 배틀 시작
            _gameManager = manager;
            _battle = new Battle(manager);
            Console.WriteLine("Battle!!\n\n");
            _battle.MonsterSpawn();
            Console.WriteLine("[내 정보]");
            // 추후 Player 클래스 수정 후 currentHP or HpMax로 추가 예정
            Console.WriteLine($"Lv.{_gameManager.Player.Level} Chad ({_gameManager.Player.Job}) \n\n HP {_gameManager.Player.HP}");
        }

        // 공격 / 스킬 사용 선택
        public void PlayerTurn()
        {
            Console.WriteLine("Battle!!\n\n");   
            _battle.MonsterInfo();
            
            Console.WriteLine("[내 정보]");
            // 추후 Player 클래스 수정 후 currentHP or HpMax로 추가 예정
            Console.WriteLine($"Lv.{_gameManager.Player.Level} Chad ({_gameManager.Player.Job})  HP {_gameManager.Player.HP}\n\n");

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
            else if(monsterNum > 0 && monsterNum <= _battle.enemies.Count)
            {
                _battle.Attack(monsterNum - 1);
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

        public void EnemyTurnToPlayerTurn()
        {
            // 에너미 턴 이후 플레이어 턴 진행
        }
    }
}