using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_RPG_11;

namespace Text_RPG
{
    internal class Dungeon
    {
        private GameManager _gameManager;
        private Battle battle;
        
        public Dungeon(GameManager manager)
        {
            // 배틀 시작
            _gameManager = manager;
            battle = new Battle(manager);
            Console.WriteLine("Battle!!");
            battle.MonsterSpawn();
            Console.WriteLine("[내 정보]");
            // 추후 Player 클래스 수정 후 currentHP or HpMax로 추가 예정
            Console.WriteLine($"Lv.{_gameManager.Player.Level} Chad ({_gameManager.Player.Job}) \n\n HP {_gameManager.Player.HP}");
        }
        
        public void PlayerTurn()
        {
            // 플레이어 턴 = 공격 입력
            // 만약 1을 입력했다면 공격
            // 만약 2를 입력했다면 스킬을 보여주고 > 스킬 공격
        }

        public void EnemyTurnToPlayerTurn()
        {
            // 에너미 턴 이후 플레이어 턴 진행
        }
    }
}