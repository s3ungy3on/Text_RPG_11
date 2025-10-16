using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_RPG_11;


namespace Text_RPG_11
{
    internal class Battle
    {
        // 담은 몬스터에 관련된 변수명은 enemy로 통일, 담기 전 몬스터에 관련된 변수명은 monster로 통일
        
        public int Stage; // 던전 층
        
        public int PlayerInitialHP; // 플레이어 던전 진입 시 체력
        public int PlayerHP; // 플레이어 던전 진입 이후 턴 별 체력(에너미에게 공격 당하기 전)
        public int EnemyHP; // 에너미 턴 별 체력(플레이어에게 공격 당하기 전)
        
        public int Index; // Index는 몬스터가 자기자신을 지칭할 때 사용
        
        public int AtkRandInput; // 공격력 별 오차 범위
        public int AtkRand; // 오차 범위를 더한 공격력
        
        public int RewardExp; // 총 보상 계산
        public int RewardGold; // 총 보상 계산
        
        public BattleResult BattleState { get; private set; }
        
        public List<Monster> Monsters = new List<Monster>() // 모든 몬스터 객체를 담을 리스트 생성(임시: 차후 Monster 클래스로 옮기거나, 더 좋은 방법을 찾아볼 예정)
        {
            Monster.Minion(),
            Monster.Voidgrub(),
            Monster.CanonMinion(),
            Monster.SuperMinion(),
            Monster.Gromp(),
            Monster.Raptors(),
            Monster.Krugs(),
            Monster.BlueSentinel(),
            Monster.RedBrambleback(),
            Monster.ScuttleCrab()
        };
        public List<Monster> Enemies = new List<Monster>();
        
        public List<Skill> Skills = new List<Skill>() // 모든 스킬 담을 리스트 생성(임시: 차후 Skill 클래스로 옮기거나, 더 좋은 방법을 찾아볼 예정)
        {
            Text_RPG_11.Skill.Ashe_EnchantedCrystalArrow(),
            Text_RPG_11.Skill.Ashe_FrostShot(),
            Text_RPG_11.Skill.Ashe_Hawkshot(),
            Text_RPG_11.Skill.Ashe_Volley(),
            Text_RPG_11.Skill.Garen_Courage(),
            Text_RPG_11.Skill.Garen_DemacianJustice(),
            Text_RPG_11.Skill.Garen_Judgment(),
            Text_RPG_11.Skill.Garen_Perseverance(),
            Text_RPG_11.Skill.Lux_FinalSpark(),
            Text_RPG_11.Skill.Lux_LightBinding(),
            Text_RPG_11.Skill.Lux_LucentSingularity(),
            Text_RPG_11.Skill.Lux_PrismaticBarrier()
        };
        public List<Skill> PlayerSkills = new List<Skill>(); // 모든 스킬 중 플레이어의 직업, 레벨에 맞는 스킬을 담을 리스트 생성
        
        public List<(Skill skill, int remainingCooldown)> CooldownList = new List<(Skill, int)>(); // 쿨타임이 남은 스킬 저장
        public List<(Skill skill, int remainingDuration, int buffvalue)> BuffdurationList = new List<(Skill, int, int)>(); // 지속 시간이 남은 스킬 저장
        public List<(Skill skill, int remainingDuration, int debuffvalue)> DebuffdurationList = new List<(Skill, int, int)>(); // 지속 시간이 남은 스킬 저장

        
        private GameManager _gameManager;

        private Skill skillForAttack;
        
        public enum BattleResult // 현재 배틀 상태
        {
            InProgress, // 배틀 진행 중
            Victory, // 플레이어 승리
            Defeat // 플레이어 패배
        }
        
        public Battle(GameManager manager)
        {
            _gameManager = manager;
        }
        
        // 배틀 시작 후 몬스터 랜덤 등장
        public void EnemySpawn()
        {
            BattleState = BattleResult.InProgress; // 기본 배틀 상태는 InProgress
            
            // 1. 몬스터 수 생성
            Random random = new Random();
            int spawnNum = random.Next(1, 5);
            
            // 2. 등장할 몬스터 랜덤 선택
            for (int i = 0; i < spawnNum; i++)
            {
                Enemies.Add(Monsters[random.Next(0, Monsters.Count)]);
            }
        }
        
        // 플레이어 공격(평타)
        public void Attack(int enemyIndex)
        {
            Random rand = new Random();
            
            int criticalPercent = rand.Next(1, 100);
            // 공격 오차 범위
            AtkRandInput = (int)Math.Round(_gameManager.Player.MaxAttack * 0.1);
            // 공격 값
            AtkRand = rand.Next(_gameManager.Player.MaxAttack - AtkRandInput, _gameManager.Player.MaxAttack + AtkRandInput);
            
            // 치명타
            // 차후 Player CriticalChance에 맞춰 수정 예정
            if (criticalPercent <= 10)
            {
                AtkRand = (int)Math.Round(AtkRand * 1.6);
                EnemyHP = Enemies[enemyIndex].HP;
                Enemies[enemyIndex].HP -= ((int)Math.Round(AtkRand * 1.6) - Enemies[enemyIndex].Defense);
            }
            // 그 외
            else
            {
                EnemyHP = Enemies[enemyIndex].HP;
                Enemies[enemyIndex].HP -= (AtkRand - Enemies[enemyIndex].Defense);
            }
            
            // 에너미가 죽었다면 HP를 0으로(마이너스가 되지 않도록)
            if (Enemies[enemyIndex].HP <= 0)
            {
                Enemies[enemyIndex].HP = 0;
            }
        }

        // 플레이어가 사용 가능한 Skill만 불러온다
        public void Skill(int skillIndex)
        {
            // Player가 사용할 수 있는 스킬만 PlayerSkills에 입력
            PlayerSkills.AddRange(
                Skills.Where(skill =>
                    skill.RequiredJob == _gameManager.Player.Job &&
                    skill.RequiredLevel == _gameManager.Player.Level
                )
            );
            
            // 플레이어가 선택한 스킬을 저장
            skillForAttack = PlayerSkills[skillIndex];
            // 만약 스킬의 hit가 랜덤 타겟이라면 몬스터 선택 X, 매개변수는 null
        }
        
        // 스킬을 사용해 에너미 공격
        public void SkillUse(int enemyIndex)
        {
            // 스킬을 사용한다 
            
            // 사용할 스킬은 쿨타임에 할당
            skillForAttack.CurrentCooldown = skillForAttack.Cooldown;
            CooldownList.Add((skillForAttack, skillForAttack.Cooldown));
            // 만약 플레이어가 선택한 스킬의 currentCooldown != 0이라면 사용 불가
            // 플레이어가 선택한 스킬의 currentCooldown == 0이라면 사용 가능
            
            // 만약 사용한 스킬에 Duration이 있는 경우 해당 내용을 함께 List에 저장
            // enemyIndex == null일 경우 랜덤 타겟 진행

            switch (skillForAttack.Type)
            {
                case SkillType.Damage:
                    // 스킬 타입이 공격일 시 powerMultiplier을 사용자 공격력에 적용
                    // Duration이 없다고 가정
                    int finalDamage = (int)Math.Round(AtkRand * skillForAttack.Effects.DamageMultiplier);
                    
                    List<Monster> skillHitsEnemies = new List<Monster>();
                    
                    // hits가 양수일 시 랜덤 에너미 타격
                    if (skillForAttack.Hits > 0)
                    {
                        // 랜덤 에너미 선택
                        skillHitsEnemies.AddRange(Enemies.OrderBy(enemy => Guid.NewGuid()).Take(skillForAttack.Hits));
                        // 선택된 에너미 공격
                        foreach (Monster hitsEnemies in skillHitsEnemies)
                        {
                            hitsEnemies.HP -= finalDamage;
                        }
                    }
                    // 그 외 일시 입력받은 enemy를 타격
                    else
                    {
                        Enemies[enemyIndex].HP -= finalDamage;
                    }
                    break;
                
                case SkillType.Heal:
                    // 스킬 타입이 힐일 시 powerMultiplier 만큼 플레이어 체력에 추가
                    // 힐량이 플레이어 최대 체력을 넘을 시 HP = MaxHP
                    // Duration이 없다고 가정
                    if(_gameManager.Player.HP + (int)Math.Round(skillForAttack.PowerMultiplier) > _gameManager.Player.MaxHP)
                        _gameManager.Player.HP = _gameManager.Player.MaxHP;
                    else    
                        _gameManager.Player.HP += (int)Math.Round(skillForAttack.PowerMultiplier);
                    break;

                case SkillType.Buff:
                    // 본인 타겟
                    // powerMultiplier에 따라 effect 실행
                    if (skillForAttack.Effects.AttackBonus > 0)
                    {
                        // Attack일 경우 Player.Atk에 추가
                        _gameManager.Player.Attack += (int)Math.Round(skillForAttack.Effects.AttackBonus);
                        
                        // 지속 시간 추가
                        if (skillForAttack.Effects.Duration > 0)
                        {
                            skillForAttack.LeftDuration = skillForAttack.Effects.Duration;
                            BuffdurationList.Add((skillForAttack, skillForAttack.LeftDuration, (int)Math.Round(skillForAttack.Effects.AttackBonus)));
                        }
                    }
                    break;
                
                case SkillType.Debuff:
                    // 상대 타겟
                    // powerMultiplier에 따라 effect 실행
                    if (skillForAttack.Effects.AttackMinus > 0)
                    {
                        // Attack일 경우 에너미 공격 마이너스
                        Enemies[enemyIndex].Attack -= skillForAttack.Effects.AttackMinus;
                        
                        // 지속 시간 추가
                        if (skillForAttack.Effects.Duration > 0)
                        {
                            skillForAttack.LeftDuration = skillForAttack.Effects.Duration;
                            DebuffdurationList.Add((skillForAttack, skillForAttack.LeftDuration, (int)Math.Round(skillForAttack.Effects.AttackMinus)));
                        }
                    }
                    else if (skillForAttack.Effects.DefenseMinus > 0)
                    {
                        // Defense추가 일 경우 에너미 방어 마이너스
                        Enemies[enemyIndex].Defense -= skillForAttack.Effects.DefenseMinus;
                        
                        // 지속 시간 추가
                        if (skillForAttack.Effects.Duration > 0)
                        {
                            skillForAttack.LeftDuration = skillForAttack.Effects.Duration;
                            DebuffdurationList.Add((skillForAttack, skillForAttack.LeftDuration,(int)Math.Round(skillForAttack.Effects.DefenseMinus)));
                        }
                    }
                    break;
            }
        }
        
        // 지속시간 끝나면
        public void DurationEnd()
        {
            
        }

        // Enemy가 사용자 공격
        public void EnemyTurn()
        {
            Random rand = new Random();
            int missPercent = rand.Next(1, 101);

            // 10퍼센트 확률로 공격 미스
            // 차후 Player dodgeChance에 맞춰 수정 예정
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
        
        // 승리 조건 체크
        public BattleResult EndCheck()
        {
            if (Enemies.All(m => m.isDead) && _gameManager.Player.HP > 0)
            {
                // 모든 에너미가 죽은 채로 플레이어의 체력이 0이상일 시 승리
                BattleState = BattleResult.Victory;
            }
            else if (_gameManager.Player.HP <= 0)
            {
                // 플레이어의 체력이 0이하일 시 승리
                BattleState = BattleResult.Defeat;
            }

            return BattleState;
        }
        
        // 승리 시 보상 지급
        public void ClearReward()
        {
            foreach (Monster monster in Enemies)
            {
                RewardExp += monster.RewardExp;
                RewardGold += monster.RewardGold;
            }
            
            _gameManager.Player.Gold += RewardGold;
            _gameManager.Player.Exp += RewardExp;
        }

        /*public void ClearRewardItem()
        {
            List<Items> GetItems = new List<Items>(); // 모든 아이템을 하나씩 담는 리스트
            List<Items> RewardItems = new List<Items>(); // 보상으로 출력할 아이템을 담는 리스트
            
            Random rand = new Random();

            for (int i = 0; i < Enemies.Count; i++)
            {
                int itemGetPercent = Math.Max((100 - i) * 20, 10); // 몬스터 순서에 따른 아이템 획득 확률
                int itemDropPercent = rand.Next(1, 101); // 아이템 드랍 획률
                int itemTierPercent = rand.Next(1, 101); // 아이템 티어
                int itemTranscendencePercent = rand.Next(1, 10001);
                
                // int stageGroup = (int)Math.Ceiling(Stage / 10.0); // 구간별 스테이지
                
                int stagePercent = Stage switch // 구간별 기본템 드랍 확률
                {
                    <= 10 => 90,
                    <= 20 => 80,
                    <= 30 => 70,
                    <= 40 => 60,
                    _ => 50
                };
                
                // int i = 0
                // 
                
                // 아이템 드랍
                if (itemDropPercent <= itemGetPercent) 
                {
                    // 던전 층에 따라 보상 아이템의 티어가 달라짐
                    switch (Stage)
                    {
                        // 임의 작성: 퍼센트에 따라 RewardItems에 랜덤(기본 or 레어) 아이템 추가
                        case <= 10:
                            RewardItems.Add(itemTierPercent <= stagePercent 
                                ? GetItems
                                    .Where(item => item.Tier == ItemTier.Common)
                                    .OrderBy(_ => rand.Next()).First()
                                : GetItems
                                    .Where(item => item.Tier == ItemTier.Rare)
                                    .OrderBy(_ => rand.Next()).First());
                            break;
                        
                        // 임의 작성: 50층 이상일 시 초월 무기 등장 
                        default:
                            Items selectedItem;
                            if (itemTierPercent <= stagePercent)
                                selectedItem = GetItems.Where(item => item.Tier == ItemTier.Common).OrderBy(_ => rand.Next()).First();
                            else if (itemTranscendencePercent > 9995)
                                selectedItem = GetItems.Where(item => item.Tier == ItemTier.Transcendence).OrderBy(_ => rand.Next()).First();
                            else
                                selectedItem = GetItems.Where(item => item.Tier != ItemTier.Rare).OrderBy(_ => rand.Next()).First();

                            RewardItems.Add(selectedItem);
                            break;
                    }
                }
            }

        }*/
        
        // exp 충족 시 레벨 업
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
