using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.IO;
using System.IO.Enumeration;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using Text_RPG_11;


namespace Text_RPG_11
{
    public class Battle
    {
        // 담은 몬스터에 관련된 변수명은 enemy로 통일, 담기 전 몬스터에 관련된 변수명은 monster로 통일
        // 변경 사항 + 추가 기능에 따라 수정해야 할 사항은 "추가 수정 필요" 로 주석 달아둠
        
        public int Stage = 1; // 던전 층
        
        public int PlayerInitialHP; // 플레이어 던전 진입 시 체력
        public int PlayerHP; // 플레이어 던전 진입 이후 턴 별 체력(에너미에게 공격 당하기 전)
        public int EnemyHP; // 에너미 턴 별 체력(플레이어에게 공격 당하기 전)
        
        public int Index; // Index는 몬스터가 자기자신을 지칭할 때 사용
        
        public int AtkRandInput; // 공격력 별 오차 범위
        public int AtkRand; // 오차 범위를 더한 공격력

        public int FinalDamage; // 개체 타격 공격 스킬 사용 시 최종 데미지
        public int SkillAtk; // 다중 타격 공격 타입 스킬 사용 시 최종 데미지
        
        public int RewardExp; // 총 보상 계산
        public int RewardGold; // 총 보상 계산
        
        public BattleResult BattleState { get; private set; }
        
        public List<Monster> Monsters = new List<Monster>() // 모든 몬스터 객체를 담을 리스트 생성(임시: 차후 Monster 클래스로 옮기거나, 더 좋은 방법을 찾아볼 예정)
        {
            Monster.Minion(),
            Monster.Voidgrub(),
            Monster.CannonMinion(),
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
        
        public List<Cooldown> CooldownList = new List<Cooldown>(); // 쿨타임이 남은 스킬 저장
        public List<BuffDuration> BuffdurationList = new List<BuffDuration>(); 
        // 지속 시간이 남은 스킬 저장: 사용한 스킬 / 남은 지속 시간
        public List<DebuffDuration> DebuffdurationList = new List<DebuffDuration>(); 
        // 지속 시간이 남은 스킬 저장: 사용한 스킬 / 남은 지속 시간 / 대상

        public List<Monster> SkillHitsEnemies = new List<Monster>(); // 스킬로 공격 받은 몬스터 리스트
        public List<(Monster EnemyHits, int EnemyHpHit, int Damage)> HitsEnemies = new List<(Monster, int, int)>(); // 스킬로 공격 받은 몬스터 hp 리스트
        // public List<int> AtkRand_SkillHitsEnemies =  new List<int>(); // 스킬로 공격 받은 몬스터가 받은 데미지 리스트

        private int currentStage; // 현재 스테이지
        public DungeonRewardData stageItem; // stageItem을 받아옴
        
        public List<DungeonRewardItem> RewardItems = new List<DungeonRewardItem>(); // 보상용 아이템을 담는 리스트
        
        private GameManager _gameManager;
        private RewardDataContainer _rewardContainer = new RewardDataContainer();

        private Random rand = new Random();

        private Skill skillForAttack;
        
        // private JobData jobData;
        
        public enum BattleResult // 현재 배틀 상태
        {
            InProgress, // 배틀 진행 중
            Victory, // 플레이어 승리
            Defeat // 플레이어 패배
        }
        
        public Battle(GameManager manager)
        {
            _gameManager = manager;
            // currentStage = this.Stage;
        }
        
        public class Cooldown
        {
            public Skill Skill;
            public int RemainingCooldown;
            public bool IsFirstTurn = true;
        }

        public class BuffDuration
        {
            public Skill Skill;
            public int RemainingDuration;
        }
        
        public class DebuffDuration
        {
            public Skill Skill;
            public int RemainingDuration;
            public int EnemyIndex;
        }
        
        // 배틀 시작 후 몬스터 랜덤 등장
        public void EnemySpawn()
        {
            BattleState = BattleResult.InProgress; // 기본 배틀 상태는 InProgress

            Enemies.Clear(); // 몬스터 리스트 초기화

            // 25층 이하일 경우 몬스터 스폰 1~5마리
            // 25층 이상일 경우 몬스터 스폰 1~11마리
            int spawnNumMax = Stage switch
            {
                <= 25 => 6,
                _ => 11
            };
            
            int spawnNumMin = Stage switch
            {
                <= 25 => 1,
                _ => 5
            };
            
            // 1. 몬스터 수 생성
            int spawnNum = rand.Next(spawnNumMin, spawnNumMax);
            
            for (int i = 0; i < spawnNum; i++)
            {
                var monsterTypeName = Monsters[rand.Next(Monsters.Count)].Name;
                
                // 객체 생성
                Enemies.Add(
                    monsterTypeName switch
                    {
                        "미니언" => Monster.Minion(),
                        "공허충 (Voidgrub)" => Monster.Voidgrub(),
                        "대포 미니언" => Monster.CannonMinion(),
                        "슈퍼 미니언" => Monster.SuperMinion(),
                        "그롬프 (Gromp)" => Monster.Gromp(),
                        "칼날부리 (Raptors)" => Monster.Raptors(),
                        "돌거북 (Krugs)" => Monster.Krugs(),
                        "블루 센티널" => Monster.BlueSentinel(),
                        "레드 브램블백" => Monster.RedBrambleback(),
                        "바위게 (Scuttle Crab)" => Monster.ScuttleCrab(),
                        _ => Monster.Minion(),
                    }
                );
                
                // 에너미 레벨 설정
                Enemies[i].Level = rand.Next(1, Stage + 5);
            }
        }
        
        // 플레이어 공격(평타)
        public void Attack(int enemyIndex)
        {
            int criticalPercent = rand.Next(1, 100);
            // 공격 오차 범위
            AtkRandInput = (int)Math.Round(_gameManager.Player.MaxAttack * 0.1);
            // 공격 값
            AtkRand = rand.Next(_gameManager.Player.MaxAttack - AtkRandInput, _gameManager.Player.MaxAttack + AtkRandInput);
            EnemyHP = Enemies[enemyIndex].HP;
            
            // 치명타
            // 차후 Player CriticalChance에 맞춰 추후 수정 필요
            if (criticalPercent <= 10)
            {
                AtkRand = (int)Math.Round(AtkRand * 1.6);
                Enemies[enemyIndex].HP -= ((int)Math.Round(AtkRand * 1.6) - Enemies[enemyIndex].Defense);
            }
            // 그 외
            else
            {
                Enemies[enemyIndex].HP -= (AtkRand - Enemies[enemyIndex].Defense);
            }
            
            // 에너미가 죽었다면 HP를 0으로(마이너스가 되지 않도록)
            if (Enemies[enemyIndex].HP <= 0)
            {
                Enemies[enemyIndex].HP = 0;
            }
        }

        // 플레이어가 사용 가능한 Skill만 불러온다
        public void Skill()
        {
            // Player가 사용할 수 있는 스킬만 PlayerSkills에 입력
            PlayerSkills.AddRange(
                Skills.Where(skill =>
                    skill.RequiredJob == _gameManager.Player.Job.Name &&
                    skill.RequiredLevel <= _gameManager.Player.Level
                )
            );
        }

        public void SkillSelect(int skillIndex)
        {
            // 플레이어가 선택한 스킬을 저장
            skillForAttack = PlayerSkills[skillIndex];
            // 만약 스킬의 hit가 랜덤 타겟이라면 몬스터 선택 X, 매개변수는 null
        }
        
        // 스킬을 사용해 에너미 공격
        public void SkillUse(int enemyIndex)
        {
            // 스킬 사용 직전 초기화
            SkillHitsEnemies.Clear();
            HitsEnemies.Clear();
            
            AtkRandInput = (int)Math.Round(_gameManager.Player.MaxAttack * 0.1);
            // 공격 값
            AtkRand = rand.Next(_gameManager.Player.MaxAttack - AtkRandInput, _gameManager.Player.MaxAttack + AtkRandInput);
            
            // 사용할 스킬은 쿨타임에 할당
            skillForAttack.CurrentCooldown = skillForAttack.Cooldown;
            CooldownList.Add(new Cooldown { Skill = skillForAttack, RemainingCooldown = skillForAttack.Cooldown});
            // 만약 플레이어가 선택한 스킬의 currentCooldown != 0이라면 사용 불가
            // 플레이어가 선택한 스킬의 currentCooldown == 0이라면 사용 가능
            
            // 만약 사용한 스킬에 Duration이 있는 경우 해당 내용을 함께 List에 저장
            // enemyIndex == null일 경우 랜덤 타겟 진행

            switch (skillForAttack.Type)
            {
                case SkillType.Attack:
                    // 스킬 타입이 공격일 시 powerMultiplier을 사용자 공격력에 적용
                    // Duration이 없다고 가정
                    FinalDamage = (int)Math.Round(AtkRand * skillForAttack.PowerMultiplier);
                    
                    
                    // hits가 1 이상 일 시 랜덤 에너미 타격
                    // enemyIndex
                    if (skillForAttack.Hits > 1)
                    {
                        // 랜덤 에너미 선택
                        SkillHitsEnemies.AddRange(
                            Enemies
                                .Where(enemy => !enemy.IsDead)
                                .OrderBy(enemy => Guid.NewGuid())
                                .Take(skillForAttack.Hits)
                        );
                        // 선택된 에너미 공격
                        foreach (Monster hitsEnemies in SkillHitsEnemies)
                        {
                            SkillAtk = FinalDamage - hitsEnemies.Defense;
                            HitsEnemies.Add((hitsEnemies, hitsEnemies.HP, SkillAtk));
                            hitsEnemies.HP -= FinalDamage - hitsEnemies.Defense;
                        }
                    }
                    // 그 외 일시 입력받은 enemy를 타격
                    else
                    {
                        SkillAtk = FinalDamage - Enemies[enemyIndex].Defense;
                        EnemyHP = Enemies[enemyIndex].HP;
                        Enemies[enemyIndex].HP -= FinalDamage - Enemies[enemyIndex].Defense;
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

                // 임시 작성
                case SkillType.Buff:
                    // 타겟에 상관 없이 Attack / Defense만 구분
                    // 공격값이나 방어값이 없는 스킬의 초기화가 0으로 되어있는지 / 아닌지 모름 > 추후 수정 필요
                    // 중복되는 코드 리팩토링 필요
                    if (skillForAttack.Effects.EffectiveAttackDelta != 0)
                    {
                        _gameManager.Player.MaxAttack += skillForAttack.Effects.EffectiveAttackDelta;
                        
                        // 지속 시간이 있는 경우
                        if (skillForAttack.Effects.Duration > 0)
                        {
                            // 스킬의 남은 지속 시간을 기본 지속 시간과 동기화
                            skillForAttack.LeftDuration = skillForAttack.Effects.Duration;
                            // 지속이 남은 스킬 리스트에 입력
                            BuffdurationList.Add(new BuffDuration { Skill = skillForAttack, RemainingDuration = skillForAttack.LeftDuration});
                        }
                    }
                    
                    if (skillForAttack.Effects.EffectiveDefenseDelta != 0)
                    {
                        _gameManager.Player.MaxDefense += skillForAttack.Effects.EffectiveDefenseDelta;
                        
                        // 지속 시간이 있는 경우
                        if (skillForAttack.Effects.Duration > 0)
                        {
                            skillForAttack.LeftDuration = skillForAttack.Effects.Duration;
                            BuffdurationList.Add(new BuffDuration { Skill = skillForAttack, RemainingDuration = skillForAttack.LeftDuration});
                        }
                    }

                    break;
                
                case SkillType.Debuff:
                    if (skillForAttack.Effects.EffectiveAttackDelta != 0)
                    {
                        Enemies[enemyIndex].Attack += skillForAttack.Effects.EffectiveAttackDelta;
                        
                        // 지속 시간이 있는 경우
                        if (skillForAttack.Effects.Duration > 0)
                        {
                            skillForAttack.LeftDuration = skillForAttack.Effects.Duration;
                            DebuffdurationList.Add(new DebuffDuration { Skill = skillForAttack, RemainingDuration = skillForAttack.LeftDuration, EnemyIndex = enemyIndex });
                        }
                    }
                    
                    if (skillForAttack.Effects.EffectiveDefenseDelta != 0)
                    {
                        Enemies[enemyIndex].Defense += skillForAttack.Effects.EffectiveDefenseDelta;
                        
                        // 지속 시간이 있는 경우
                        if (skillForAttack.Effects.Duration > 0)
                        {
                            skillForAttack.LeftDuration = skillForAttack.Effects.Duration;
                            DebuffdurationList.Add(new DebuffDuration { Skill = skillForAttack, RemainingDuration = skillForAttack.LeftDuration, EnemyIndex = enemyIndex });
                        }
                    }
                    break;
                
                case SkillType.AttackBuff:
                    break;
                    
                
                case SkillType.AttackDebuff:
                    break;
            }
        }

        // 쿨다운 체크
        public void CooldownEnd()
        {
            for (int i = CooldownList.Count - 1; i >= 0; i--)
            {
                var skillInPlayer = PlayerSkills.FirstOrDefault(s => s.Name == CooldownList[i].Skill.Name);

                if (CooldownList[i].RemainingCooldown == 0)
                {
                    if (skillInPlayer != null)
                        skillInPlayer.CurrentCooldown = 0;
                    CooldownList.RemoveAt(i);
                }
                else
                {
                    // 첫 턴이 아닐 때만 감소, 첫 턴은 유지
                    if (!CooldownList[i].IsFirstTurn)
                    {
                        CooldownList[i].RemainingCooldown--;
                    }
                    else
                    {
                        CooldownList[i].IsFirstTurn = false; // 플래그 해제
                    }

                    // Skill 내 CurrentCooldown과 동기화
                    if (skillInPlayer != null)
                        skillInPlayer.CurrentCooldown = CooldownList[i].RemainingCooldown;
                }
            }
        }


        
        // 지속시간 체크
        public void DurationEnd()
        {
            // 버프
            for (int i = BuffdurationList.Count - 1; i >= 0; i--)
            {
                // 버프 지속 시간이 끝난 경우
                if (BuffdurationList[i].RemainingDuration == 0)
                {
                    // 공격 버프 체크
                    if (BuffdurationList[i].Skill.Effects.EffectiveAttackDelta != 0)
                    {
                        // 버프 받은 만큼 음수 처리
                        _gameManager.Player.MaxAttack -= BuffdurationList[i].Skill.Effects.EffectiveAttackDelta;
                    }
                    
                    // 방어 버프 체크
                    if (BuffdurationList[i].Skill.Effects.EffectiveDefenseDelta != 0)
                    {
                        // 버프 받은 만큼 음수 처리
                        _gameManager.Player.MaxDefense -= BuffdurationList[i].Skill.Effects.EffectiveDefenseDelta;
                    }
                    
                    // 체크 후 삭제
                    BuffdurationList.RemoveAt(i);
                }
                else
                {
                    BuffdurationList[i].RemainingDuration--;
                }
            }
            
            // 디버프
            for (int i = DebuffdurationList.Count - 1; i >= 0; i--)
            {
                // 디버프 지속 시간이 끝난 경우
                if (DebuffdurationList[i].RemainingDuration == 0)
                {
                    // 공격 디버프 체크
                    if (DebuffdurationList[i].Skill.Effects.EffectiveAttackDelta != 0)
                    {
                        // 디버프 받은 만큼 음수 처리
                        Enemies[DebuffdurationList[i].EnemyIndex].Attack -= DebuffdurationList[i].Skill.Effects.EffectiveAttackDelta;
                    }
                    
                    // 방어 디버프 체크
                    if (DebuffdurationList[i].Skill.Effects.EffectiveDefenseDelta != 0)
                    {
                        // 디버프 받은 만큼 음수 처리
                        Enemies[DebuffdurationList[i].EnemyIndex].Defense -= DebuffdurationList[i].Skill.Effects.EffectiveDefenseDelta;
                    }
                    
                    // 체크 후 삭제
                    DebuffdurationList.RemoveAt(i);
                }
                else
                {
                    DebuffdurationList[i].RemainingDuration--;
                }
            }
        }

        // Enemy가 사용자 공격
        public void EnemyTurn()
        {
            int missPercent = rand.Next(1, 101);
            
            // 10퍼센트 확률로 공격 미스
            // 차후 Player dodgeChance에 맞춰 추후 수정 필요
            if (missPercent <= 10)
            {
                // 무사히 지나갔다! 이런 걸 출력해줘야 할 것 같은데 고민
            }
            else
            {
                if (Enemies[Index].HP > 0)
                {
                    int damage = Math.Max(1, Enemies[Index].Attack - (int)Math.Round(_gameManager.Player.MaxDefense));
                    // 플레이어 체력 > 깎인 체력
                    _gameManager.Player.HP -= damage;
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
            if (Enemies.All(m => m.IsDead) && _gameManager.Player.HP > 0)
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
            // 디폴트 MP의 10퍼센트 만큼 회복
            _gameManager.Player.MP += (int)Math.Round(_gameManager.Player.DefaultMP * (10.0 / 100.0));
        }

        // 던전 클리어 보상 아이템 지급
        public void ClearRewardItem()
        {
            DungeonRewardData rewardData = RewardDatabase.GetDungeonRewardByStage(Stage);
    
            if (rewardData == null) return;
    
            // 1단계: 그룹 선택 (item / potion / 꽝)
            int itemGroupPercent = rand.Next(1, 101);
            int cumulativeGroupChance = 0;
            RewardGroup selectedGroup = null;
    
            foreach (var group in rewardData.rewardGroups)
            {
                cumulativeGroupChance += group.groupChance;
        
                if (itemGroupPercent <= cumulativeGroupChance)
                {
                    selectedGroup = group;
                    break; // 하나만 선택!
                }
            }
    
            // 꽝이거나 그룹이 없으면 종료
            if (selectedGroup == null || selectedGroup.items == null || selectedGroup.items.Count == 0)
            {
                return;
            }
    
            // 2단계: 선택된 그룹 내에서 아이템 선택
            int itemPercent = rand.Next(1, 101);
            int cumulativeItemChance = 0;
    
            foreach (var item in selectedGroup.items)
            {
                cumulativeItemChance += (int)(item.dropChance);
        
                if (itemPercent <= cumulativeItemChance)
                {
                    RewardItems.Add(item);
                    break; // 하나만 선택!
                }
            }
        }

        
        /*public void ClearRewardItem()
        {
            List<Items> GetItems = new List<Items>(); // 모든 아이템을 하나씩 담는 리스트
            List<Items> RewardItems = new List<Items>(); // 보상으로 출력할 아이템을 담는 리스트

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
    }
}