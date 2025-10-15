using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_RPG_11;


namespace Text_RPG_11
{
    internal class Battle
    {
        public int Stage { get; }
        public int PlayerHP;
        public int Index;
        
        // 배틀 시작 후 몬스터 랜덤 등장
                
        // 0. 몬스터 담을 리스트 생성(이후에 추가)
        public List<Monster> Monsters = new List<Monster>();
        public List<Monster> Enemies = new List<Monster>();
        
        private GameManager _gameManager;
        
        public Battle(GameManager manager)
        {
            _gameManager = manager;
        }

        public void MonsterSpawn()
        {
            // 1. 몬스터 수 생성
            Random random = new Random();
            int spawnNum = random.Next(1, 5);
            
            // 2. 등장할 몬스터 랜덤 선택
            for (int i = 0; i < spawnNum - 1; i++)
            {
                Enemies.Add(Monsters[random.Next(0, Monsters.Count)]);
            }
                
            // 3. 랜덤 선택된 몬스터 출력
            Console.WriteLine("Battle!!");
                
            foreach (var enemy in Enemies)
            {
                Console.WriteLine($"Lv. {enemy.Level} {enemy.Name}  HP {enemy.HP}");
            }
        }

        public void MonsterInfo()
        {
            for(int i = 0; i < Enemies.Count - 1; i++)
                Console.WriteLine($"[{i + 1}] Lv. {Enemies[i].Level} {Enemies[i].Name}  HP {Enemies[i].HP}");
        }
        
        public void WinCheck()
        {
            if (Enemies.All(m => m.HP == 0) && _gameManager.Player.HP > 0)
            {
                Console.WriteLine("승리");
            }
            else if (_gameManager.Player.HP <= 0)
            {
                Console.WriteLine("패배");
            }
            
        }
        
        public void Attack(int enemyIndex)
        {
            Random rand = new Random();
            
            int criticalPercent = rand.Next(1, 100);
            // 공격 오차 범위
            int atkRandInput = (int)Math.Round(_gameManager.Player.MaxAttack * 0.1);
            int atkRand = rand.Next((int)Math.Round(_gameManager.Player.MaxAttack * (1.0 - atkRandInput)), (int)Math.Round(_gameManager.Player.MaxAttack * (1.0 + atkRandInput)));

            // 치명타
            if (criticalPercent <= 10)
            {
                Enemies[enemyIndex].HP -= (int)Math.Round(atkRand * 1.6);
            }
            // 그 외
            else
            {
                Enemies[enemyIndex].HP -= atkRand;
            }
        }

        public void UserSkill()
        {
            // 스킬을 사용해 에너미 공격
        }

        public void EnemyTurn()
        {
            // Enemy가 사용자 공격
            // 10퍼센트 확률로 적중하지 않을 수 있음
            Index = 0;
                
            if (Enemies[Index].HP > 0)
            {
                // 플레이어 체력 > 깎인 체력
                PlayerHP = _gameManager.Player.HP;
                _gameManager.Player.HP -= Enemies[Index].Attack;
                Index++;
            }
            else
            {
                // 죽은 미니언은 공격하지 않음
                // 회색처리
            }
            
            if(Index == Enemies.Count - 1)
                Index = 0;
        }
        
        public void RewardShow()
        {
            // 체력이 0이 된 몬스터의 reward를 화면에 띄움
            // 해당 reward를 플레이어에게 할당(reward 메서드 사용)
        }
        
        public void LevelUp()
        {
            // 전투 클리어 후 사냥한 몬스터만큼 경험치 증가
            // 몬스터를 순회하면서 체력이 0이 된 몬스터가 제공하는 exp만큼 플레이어의 경험치를 상승
        }
        
        // public void UserPotion()
        // {
        //     // 유저 포션 사용 기능
        //
        //     // 프로퍼티 접근 체인 수정 필요
        //     if (_gameManager.inventory.Potion.Count > 0)
        //     {
        //         GameManager.Player.Hp += GameManager.Player.Inventory.Potion.HealPower;
        //         GameManager.Player.Inventory.Potion.Count--;
        //         Console.WriteLine("회복을 완료했습니다.");
        //     }
        //     else
        //     {
        //         Console.WriteLine("포션이 부족합니다.");
        //     }
        // }
    }

}