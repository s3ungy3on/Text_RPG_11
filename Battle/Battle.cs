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
        // 담은 몬스터에 관련된 변수명은 enemy로 통일, 담기 전 몬스터에 관련된 변수명은 monster로 통일
        public int Stage { get; set; } // 던전 층
        
        public int PlayerInitialHP; // 플레이어 던전 진입 시 체력
        public int PlayerHP; // 플레이어 던전 진입 이후 턴 별 체력(에너미에게 공격 당하기 전)
        public int EnemyHP; // 에너미 턴 별 체력(플레이어에게 공격 당하기 전)
        
        public int Index; // Index는 몬스터가 자기자신을 지칭할 때 사용
        
        public int AtkRandInput;
        public int AtkRand;
        
        public int RewardExp; // 총 보상 계산
        public int RewardGold; // 총 보상 계산
        
        // 배틀 시작 후 몬스터 랜덤 등장
                
        // 0. 몬스터 담을 리스트 생성(이후에 추가)
        public List<Monster> Monsters = new List<Monster>()
        {
            Monster.Minion(),
            Monster.Voidling(),
            Monster.CanonMinion()
        };
        public List<Monster> Enemies = new List<Monster>();
        
        private GameManager _gameManager;
        
        public enum BattleResult
        {
            InProgress,
            Victory,
            Defeat
        }

        public BattleResult BattleState { get; private set; } = BattleResult.InProgress;
        
        public Battle(GameManager manager)
        {
            _gameManager = manager;
        }
        
        public void EnemySpawn()
        {
            // 1. 몬스터 수 생성
            Random random = new Random();
            int spawnNum = random.Next(1, 5);
            
            // 2. 등장할 몬스터 랜덤 선택
            for (int i = 0; i < spawnNum; i++)
            {
                Enemies.Add(Monsters[random.Next(0, Monsters.Count)]);
            }
        }
        
        public BattleResult EndCheck()
        {
            if (Enemies.All(m => m.isDead) && _gameManager.Player.HP > 0)
            {
                BattleState = BattleResult.Victory;
                return BattleState;
            }
            else if (_gameManager.Player.HP <= 0)
            {
                BattleState = BattleResult.Defeat;
                return BattleState;
            }

            return BattleState;
        }
        
        public void Attack(int enemyIndex)
        {
            Random rand = new Random();
            
            int criticalPercent = rand.Next(1, 100);
            // 공격 오차 범위
            AtkRandInput = (int)Math.Round(_gameManager.Player.MaxAttack * 0.1);
            // 공격 값
            AtkRand = rand.Next(_gameManager.Player.MaxAttack - AtkRandInput, _gameManager.Player.MaxAttack + AtkRandInput);

            // 치명타
            if (criticalPercent <= 10)
            {
                AtkRand = (int)Math.Round(AtkRand * 1.6);
                EnemyHP = Enemies[enemyIndex].HP;
                Enemies[enemyIndex].HP -= (int)Math.Round(AtkRand * 1.6);
            }
            // 그 외
            else
            {
                EnemyHP = Enemies[enemyIndex].HP;
                Enemies[enemyIndex].HP -= AtkRand;
            }
            
            // 에너미가 죽었다면 HP를 0으로(마이너스가 되지 않도록)
            if (Enemies[enemyIndex].HP <= 0)
            {
                Enemies[enemyIndex].HP = 0;
            }
        }

        public void UserSkill()
        {
            // 스킬을 사용해 에너미 공격
        }

        // Enemy가 사용자 공격
        public void EnemyTurn()
        {
            Random rand = new Random();
            int missPercent = rand.Next(1, 101);

            // 10퍼센트 확률로 공격 미스
            if (missPercent <= 10)
            {
                // 무사히 지나갔다! 이런 걸 출력해줘야 할 것 같은데 고민
            }
            else
            {
                if (Enemies[Index].HP > 0)
                {
                    // 플레이어 체력 > 깎인 체력
                    _gameManager.Player.HP -= Enemies[Index].Attack;
                }
                else
                {
                    // 죽은 미니언은 공격하지 않음
                    // 회색처리
                }
            }
        }
        
        public void ClearReward()
        {
            // 체력이 0이 된 몬스터의 reward를 화면에 띄움
            // 해당 reward를 플레이어에게 할당(reward 메서드 사용)
            
            foreach (Monster monster in Enemies)
            {
                RewardExp += monster.RewardExp;
                RewardGold += monster.RewardGold;
            }
            
            _gameManager.Player.Gold += RewardGold;
            _gameManager.Player.Exp += RewardExp;
            Stage++;
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