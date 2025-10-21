using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_RPG_11;

namespace Text_RPG_11
{
    public class Dungeon
    {
        // 평타 관련 메서드는 이름에 Attack을 포함
        // 스킬 관련 메서드는 이름에 Skill을 포함
        // 둘 다 사용되지 않은 메서드는 공동 사용
        
        private GameManager _gameManager;
        public Battle _battle;

        private int _monsterNum = 1;
        private int _skillNum;
        
        private const int HpBarLength = 24;           // HP바 길이
        private const int MpBarLength = 24;           // MP바 길이
        
        public Dungeon(GameManager manager)
        {
            _gameManager = manager;
            _battle = new Battle(manager);
        }

        public void DungeonBattle()
        {
           // 던전 상태 초기화
            _battle.BattleState = Battle.BattleResult.InProgress;
            
            Console.WriteLine($"[던전 {_battle.Stage}층]");
            
            while (_battle.BattleState == Battle.BattleResult.InProgress)
            {
                // 1. 배틀 시작
                Console.WriteLine("\nBattle!!\n");
                _battle.PlayerInitialHP = _gameManager.Player.HP;
                
                // 2. 몬스터 스폰
                _battle.EnemySpawn();
                EnemyInfo();
                Console.WriteLine();
                
                // 3. 플레이어가 사용 가능한 스킬 로드
                _battle.Skill();
                
                // 4. 내 정보 출력
                PlayerInfo();
                
                // 5. 행동 입력
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
                if(_battle.Enemies[i].IsDead)
                    // 1. 몬스터 체력이 0일 시 Dead로 처리
                    Console.WriteLine($"[{i + 1}] Lv. {_battle.Enemies[i].Level} {_battle.Enemies[i].Name}  Dead");
                // 회색 처리 필요
                else
                    // 2. 몬스터 체력이 0 이상일 시 HP 출력
                    Console.WriteLine($"[{i + 1}] Lv. {_battle.Enemies[i].Level} {_battle.Enemies[i].Name}  HP {_battle.Enemies[i].HP}");
            }
        }
        
        // 사용 가능한 스킬 정보
        public void SkillInfo()
        {
            for (int i = 0; i < _battle.PlayerSkills.Count; i++)
            {
                // 쿨타임 남은 스킬은 사용 불가
                if (_battle.PlayerSkills[i].CurrentCooldown != 0)
                    Console.WriteLine($"{i + 1}. {_battle.PlayerSkills[i].Name} - 쿨타임 {_battle.PlayerSkills[i].CurrentCooldown}턴: 사용 불가");
                else
                    Console.WriteLine($"{i + 1}. {_battle.PlayerSkills[i].Name} - MP {_battle.PlayerSkills[i].ManaCost}");
                
                Console.WriteLine($"{_battle.PlayerSkills[i].Description}");
                Console.WriteLine($"{_battle.PlayerSkills[i].Desc}");
                Console.WriteLine();
            }
            
            if (_battle.PlayerSkills.Count == 0)
                Console.WriteLine("사용할 수 있는 스킬이 없습니다.");
        }

        public void PlayerInfo()
        {
            Console.WriteLine("[내 정보]");
            Console.WriteLine($"Lv.{_gameManager.Player.Level} {_gameManager.Player.Job.Name}\n\nHP {_gameManager.Player.HP} / {_gameManager.Player.MaxHP}\nMP {_gameManager.Player.MP} / {_gameManager.Player.MaxMP}\n");
            PrintStatusBar("체력", _gameManager.Player.HP, _gameManager.Player.MaxHP, ConsoleColor.Green, ShowHPBarInline);
            PrintStatusBar("마나", _gameManager.Player.MP, _gameManager.Player.MaxMP, ConsoleColor.Blue, ShowMPBarInline);
            Console.WriteLine();
        }

        // 공격 / 스킬 사용 선택
        public void PlayerTurn()
        {
            Console.Clear();
            
            // 1. 턴 시작 전 배틀 종료 조건 확인
            if (BattleEndCheck()) return;
            
            // 2. 사용한 스킬 쿨타임 / 지속 시간 체크
            _battle.CooldownEnd();
            _battle.DurationEnd();
            
            while (true)
            {
                Console.WriteLine("\nBattle!!\n");   
                EnemyInfo();
                Console.WriteLine();
                
                PlayerInfo();
                
                // 3. 플레이어 행동 선택
                Console.WriteLine("1. 공격");
                Console.WriteLine("2. 스킬\n");
            
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                bool isWorked = int.TryParse(Console.ReadLine(), out int result);

                // 4. 입력에 따라 공격 / 스킬 메서드 진행
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
                Console.Clear();
                Console.WriteLine("\nBattle!!\n");   
                EnemyInfo();
                Console.WriteLine();
            
                PlayerInfo();
                
                // 1. 공격할 몬스터 입력
                Console.WriteLine("몬스터 숫자. 공격");
                Console.WriteLine("0. 취소\n");
                
                Console.WriteLine("대상을 선택해주세요.");
                Console.Write(">> ");
            
                bool isWorked = int.TryParse(Console.ReadLine(), out _monsterNum);

                // 2. 몬스터 공격
                if(_monsterNum > 0 && _monsterNum <= _battle.Enemies.Count)
                {
                    if (_battle.Enemies[_monsterNum - 1].IsDead)
                    {
                        Console.WriteLine("죽은 몬스터는 공격할 수 없습니다.");
                    }
                    else
                    {
                        _battle.Attack(_monsterNum - 1);
                        AttackTurnEnd();
                    }
                }
                else if (isWorked || _monsterNum == 0)
                {
                    // 공격 or 스킬 재선택
                    PlayerTurn();
                }
                else
                    Console.WriteLine("잘못된 입력입니다.");
            }
        }
        
        // 스킬 선택
        public void PlayerTurnSkillSelect()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nBattle!!\n");   
                EnemyInfo();
                Console.WriteLine();
            
                PlayerInfo();
                
                // 1. 사용하고 싶은 스킬 확인
                
                SkillInfo();
                Console.WriteLine("\n0. 취소\n");
                
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
            
                bool isWorked = int.TryParse(Console.ReadLine(), out _skillNum);

                // 2. 스킬 선택
                if (_battle.PlayerSkills.Count > 0)
                {
                    if(_skillNum > 0 && _skillNum <= _battle.PlayerSkills.Count)
                    {
                        // 선택한 스킬에 쿨타임이 남아 있다면 사용 불가능
                        if (_battle.PlayerSkills[_skillNum - 1].CurrentCooldown != 0)
                        {
                            Console.WriteLine($"쿨타임 {_battle.PlayerSkills[_skillNum - 1].CurrentCooldown}턴: 사용 불가");
                        }
                        else
                        {
                            // Hits(다중 타격)가 0 이상이거나, 스킬 타입이 힐 또는 버프거나, 스킬 타입이 공격+버프이고 다중 타격이 0인 경우 몬스터 선택 없이 진행
                            if (_battle.PlayerSkills[_skillNum - 1].Hits > 1 || _battle.PlayerSkills[_skillNum - 1].Type == SkillType.Heal || _battle.PlayerSkills[_skillNum - 1].Type == SkillType.Buff || (_battle.PlayerSkills[_skillNum - 1].Hits > 1 && _battle.PlayerSkills[_skillNum - 1].Type == SkillType.AttackBuff))
                            {
                                _monsterNum = 1;
                            
                                _battle.SkillSelect(_skillNum - 1);
                                _battle.SkillUse(-1);
                            
                                SkillTurnEnd();
                            }
                            else
                            {
                                _battle.SkillSelect(_skillNum - 1);
                                PlayerTurnSkill();
                            }
                        }
                    }
                    else if (isWorked && _skillNum == 0)
                    {
                        // 공격 or 스킬 재선택
                        PlayerTurn();
                    }
                    else
                        Console.WriteLine("잘못된 입력입니다.");
                }
                else
                {
                    if (isWorked && _skillNum == 0)
                    {
                        // 공격 or 스킬 재선택
                        PlayerTurn();
                    }
                    else
                        Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }
        
        // 스킬 사용할 몬스터 선택
        public void PlayerTurnSkill()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nBattle!!\n");   
                EnemyInfo();
                Console.WriteLine();
            
                PlayerInfo();
                
                // 1. 공격할 몬스터 입력
                Console.WriteLine("몬스터 숫자. 공격");
                Console.WriteLine("0. 취소\n");
                
                Console.WriteLine("대상을 선택해주세요.");
                Console.Write(">> ");
            
                bool isWorked = int.TryParse(Console.ReadLine(), out _monsterNum);

                
                // 2. 몬스터 공격
                if(_monsterNum > 0 && _monsterNum <= _battle.Enemies.Count)
                {
                    if (_battle.Enemies[_monsterNum - 1].IsDead)
                    {
                        Console.WriteLine("죽은 몬스터는 공격할 수 없습니다.");
                    }
                    else
                    {
                        _battle.SkillUse(_monsterNum - 1);
                        SkillTurnEnd();
                    }
                }
                else if (isWorked || _monsterNum == 0)
                {
                    // 스킬 재선택
                    PlayerTurnSkillSelect();
                }
                else
                    Console.WriteLine("잘못된 입력입니다.");
            }
        }
        
        // 공격 이후 플레이어 턴 결과 출력
        public void AttackTurnEnd()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nBattle!!\n");
            
                Console.WriteLine($"{_gameManager.Player.Name}의 공격!");
                Console.WriteLine($"Lv.{_battle.Enemies[_monsterNum - 1].Level} {_battle.Enemies[_monsterNum - 1].Name}을(를) 맞췄습니다. [데미지 : {_battle.AtkRand}]\n");
                
                // 1. 플레이어 공격으로 몬스터 사망 시 Dead 처리
                if (_battle.Enemies[_monsterNum - 1].IsDead)
                {
                    Console.WriteLine($"Lv. {_battle.Enemies[_monsterNum - 1].Level} {_battle.Enemies[_monsterNum - 1].Name}");
                    Console.WriteLine($"HP {_battle.EnemyHP} -> Dead\n");
                }
                
                // 2. 몬스터 턴 진행
                Console.WriteLine($"0. 다음\n");
                
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
        
        // 스킬 사용 이후 플레이어 턴 결과 출력
        public void SkillTurnEnd()
        {
            switch (_battle.PlayerSkills[_skillNum - 1].Type)
            {
                case SkillType.Attack:
                    while (true)
                    {
                        // 일반 타격 스크립트
                        if (_battle.PlayerSkills[_skillNum - 1].Hits <= 1)
                        {
                            while (true)
                            {
                                Console.Clear();
                                EnemyInfo();
                                Console.WriteLine("\nBattle!!\n");
            
                                Console.WriteLine($"{_gameManager.Player.Name}의 공격!");
                                Console.WriteLine($"Lv.{_battle.Enemies[_monsterNum - 1].Level} {_battle.Enemies[_monsterNum - 1].Name}을(를) 맞췄습니다. [데미지 : {_battle.SkillAtk}]\n");
                                
                                // 1. 플레이어 공격으로 몬스터 사망 시 Dead 처리
                                if (_battle.Enemies[_monsterNum - 1].IsDead)
                                {
                                    Console.WriteLine($"Lv. {_battle.Enemies[_monsterNum - 1].Level} {_battle.Enemies[_monsterNum - 1].Name}");
                                    Console.WriteLine($"HP {_battle.EnemyHP} -> Dead\n");
                                }
                
                                // 2. 몬스터 턴 진행
                                Console.WriteLine($"0. 다음\n");
                
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
                        // 다중 타격 스크립트
                        else
                        {
                            while (true)
                            {
                                Console.Clear();
                                EnemyInfo();
                                Console.WriteLine("\nBattle!!\n");
                                
                                for (int i = 0; i < _battle.HitsEnemies.Count; i++)
                                {
                                    Console.WriteLine($"{_gameManager.Player.Name}의 공격!");
                                    Console.WriteLine($"Lv.{_battle.HitsEnemies[i].EnemyHits.Level} {_battle.HitsEnemies[i].EnemyHits.Name}을(를) 맞췄습니다. [데미지 : {_battle.HitsEnemies[i].Damage}]\n");
                                    
                                    // 1. 플레이어 공격으로 몬스터 사망 시 Dead 처리
                                    if (_battle.HitsEnemies[i].EnemyHits.IsDead)
                                    {
                                        Console.WriteLine($"Lv. {_battle.HitsEnemies[i].EnemyHits.Level} {_battle.HitsEnemies[i].EnemyHits.Name}");
                                        Console.WriteLine($"HP {_battle.HitsEnemies[i].EnemyHpHit} -> Dead\n");
                                    }
                                }
                                
                                // 2. 몬스터 턴 진행
                                Console.WriteLine($"0. 다음\n");
                
                                Console.Write(">> ");
                
                                bool isWorked = int.TryParse(Console.ReadLine(), out int result);
                
                                if (isWorked || result== 0)
                                {
                                    // 다음 턴 시작 전 다중 타격 리스트 클리어
                                    _battle.SkillHitsEnemies.Clear();
                                    _battle.HitsEnemies.Clear();
                                    
                                    EnemyTurn();
                                }
                                else
                                    Console.WriteLine("잘못된 입력입니다.");
                            }
                        }
                    }

                case SkillType.Heal:
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("\nBattle!!\n");
                        
                        // 1. 플레이어 회복량 출력
                        Console.WriteLine($"{_battle.PlayerSkills[_skillNum - 1].PowerMultiplier}만큼 회복했습니다.");
                
                        // 2. 몬스터 턴 진행
                        Console.WriteLine($"0. 다음\n");
                
                        Console.Write(">> ");
                
                        bool isWorked = int.TryParse(Console.ReadLine(), out int result);
                
                        if (isWorked || result== 0)
                        {
                            EnemyTurn();
                        }
                        else
                            Console.WriteLine("잘못된 입력입니다.");
                    }

                case SkillType.Buff:
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("\nBattle!!\n");
                        
                        // 1. 플레이어 버프량 출력
                        if (Math.Abs(_battle.PlayerSkills[_skillNum - 1].Effects.EffectiveAttackDelta) > 0)
                        {
                            Console.WriteLine($"{_gameManager.Player.Name}의 공격력이 {_battle.PlayerSkills[_skillNum - 1].Effects.EffectiveAttackDelta}만큼 증가했습니다.");
                            Console.WriteLine($"[현재 공격력: {_gameManager.Player.MaxAttack}]");
                        }

                        if (Math.Abs(_battle.PlayerSkills[_skillNum - 1].Effects.EffectiveDefenseDelta) > 0)
                        {
                            Console.WriteLine($"{_gameManager.Player.Name}의 방어력이 {_battle.PlayerSkills[_skillNum - 1].Effects.EffectiveDefenseDelta}만큼 증가했습니다.");
                            Console.WriteLine($"[현재 방어력: {_gameManager.Player.MaxDefense}]");
                        }

                        // 2. 몬스터 턴 진행
                        Console.WriteLine($"0. 다음\n");

                        Console.Write(">> ");

                        bool isWorked = int.TryParse(Console.ReadLine(), out int result);

                        if (isWorked || result == 0)
                        {
                            EnemyTurn();
                        }
                        else
                            Console.WriteLine("잘못된 입력입니다.");
                    }

                case SkillType.Debuff:
                    while (true)
                    {
                        Console.Clear();
                        EnemyInfo();
                        
                        Console.WriteLine("\nBattle!!\n");
                        
                        // 1. 에너미 디버프량 출력
                        if (Math.Abs(_battle.PlayerSkills[_skillNum - 1].Effects.EffectiveAttackDelta) > 0)
                        {
                            Console.WriteLine($"{_gameManager.Player.Name}의 공격!");
                            Console.WriteLine($"Lv.{_battle.Enemies[_monsterNum - 1].Level} {_battle.Enemies[_monsterNum - 1].Name}을(를) 맞췄습니다. [공격력 디버프 : {_battle.PlayerSkills[_skillNum - 1].Effects.EffectiveAttackDelta}]\n");
                        }

                        if (Math.Abs(_battle.PlayerSkills[_skillNum - 1].Effects.EffectiveDefenseDelta) > 0)
                        {
                            Console.WriteLine($"{_gameManager.Player.Name}의 공격!");
                            Console.WriteLine($"Lv.{_battle.Enemies[_monsterNum - 1].Level} {_battle.Enemies[_monsterNum - 1].Name}을(를) 맞췄습니다. [방어력 디버프 : {_battle.PlayerSkills[_skillNum - 1].Effects.EffectiveDefenseDelta}]\n");
                        }
                        
                        // 2. 몬스터 턴 진행
                        Console.WriteLine($"0. 다음\n");

                        Console.Write(">> ");

                        bool isWorked = int.TryParse(Console.ReadLine(), out int result);

                        if (isWorked || result == 0)
                        {
                            EnemyTurn();
                        }
                        else
                            Console.WriteLine("잘못된 입력입니다.");
                    }

                case SkillType.AttackBuff:
                    // 미완
                    break;

                case SkillType.AttackDebuff:
                    // 미완
                    break;
            }
        }

        // 에너미 턴 진행
        public void EnemyTurn()
        {
            // 1. 턴 시작 전 배틀 종료 조건 확인
            if (BattleEndCheck()) return;
            
            while (true)
            {
                Console.Clear();
                EnemyInfo();
                Console.WriteLine("\nBattle!!\n");
                // 2. 공격 전, 플레이어의 체력을 저장
                _battle.PlayerHP = _gameManager.Player.HP;
                
                // 3. 살아있는 모든 몬스터가 플레이어를 공격
                for (_battle.Index = 0; _battle.Index < _battle.Enemies.Count; _battle.Index++)
                {
                    _battle.EnemyTurn();
                    
                    if (_battle.Enemies[_battle.Index].IsDead)
                    {
                        // 죽은 몬스터의 공격은 출력 X
                    }
                    else
                    {
                        if (_battle.Enemies[_battle.Index].Attack - (int)Math.Round(_gameManager.Player.MaxDefense) > 0)
                        {
                            Console.WriteLine($"Lv. {_battle.Enemies[_battle.Index].Level} {_battle.Enemies[_battle.Index].Name}의 공격!");
                            Console.WriteLine($"{_gameManager.Player.Name} 을(를) 맞췄습니다.    [데미지 : {_battle.Enemies[_battle.Index].Attack - _gameManager.Player.MaxDefense}]\n");
                        }
                        else
                        {
                            Console.WriteLine($"Lv. {_battle.Enemies[_battle.Index].Level} {_battle.Enemies[_battle.Index].Name}의 공격!");
                            Console.WriteLine($"{_gameManager.Player.Name} 을(를) 맞췄습니다.    [데미지 : 1]\n");
                        }
                    }
                }
                
                // 4. 결과
                Console.WriteLine($"Lv.{_gameManager.Player.Level} {_gameManager.Player.Job.Name}");
                Console.WriteLine($"HP {_battle.PlayerHP} -> {_gameManager.Player.HP}\n");
                
                // 5. 플레이어 턴 진행
                Console.WriteLine($"0. 다음\n");
                
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
            _battle.ClearReward();
            _battle.Stage++;
            _battle.ClearRewardItem();
            
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Battle!! - Result\n");
                Console.WriteLine("Victory\n");
                Console.WriteLine($"던전에서 몬스터 {_battle.Enemies.Count}마리를 잡았습니다.\n");
            
                Console.WriteLine("[캐릭터 정보]");
                Console.WriteLine($"Lv.{_gameManager.Player.Level} {_gameManager.Player.Job.Name}");
                Console.WriteLine($"HP {_battle.PlayerInitialHP} -> {_gameManager.Player.HP}\n");
                
                Console.WriteLine("[획득 아이템]");
                // 1. 획득 골드 출력
                Console.WriteLine($"{_battle.RewardGold} Gold\n");
                
                // 2. 획득 아이템 출력
                foreach (var rewardItem in _battle.RewardItems)
                {
                    // var item = _gameManager.GameItems.FirstOrDefault(i => i.ItemId == rewardItem.itemId);
                    // 추후 수정 예정
                    var item = ItemDatabase.GetItemById(rewardItem.itemId);
                    
                    if (item != null) 
                        Console.WriteLine($"{item.Name}\n");
                }
                
                // 배틀 종료
                Console.WriteLine($"0. 다음\n");
            
                Console.Write(">> ");
            
                bool isWorked = int.TryParse(Console.ReadLine(), out int result);

                if (isWorked || result== 0)
                {
                    _gameManager.GameMain(); // 돌아가기
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
                Console.Clear();
                Console.WriteLine("Battle!! - Result\n");
                Console.WriteLine("You Lose\n");
            
                Console.WriteLine("[캐릭터 정보]");
                Console.WriteLine($"Lv.{_gameManager.Player.Level} {_gameManager.Player.Job.Name}");
                Console.WriteLine($"HP {_battle.PlayerInitialHP} -> {_gameManager.Player.HP}\n");
            
                Console.WriteLine($"0. 다음\n");
            
                Console.Write(">> ");
            
                bool isWorked = int.TryParse(Console.ReadLine(), out int result);

                if (isWorked || result== 0)
                {
                    _gameManager.GameMain(); // 돌아가기
                }
                else
                    Console.WriteLine("잘못된 입력입니다.");
            }
        }
        
        private void PrintColoredLine(string text, ConsoleColor color) // 색상 있는 텍스트 출력
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private void WriteLineDivider(char c = '-', int repeat = 40) // 선 구분선 출력
        {
            Console.WriteLine(new string(c, repeat));
        }
        
        #region HP/MP 바 출력
        private void ShowHPBar(int current, int max)
        {
            ShowHPBarInline(current, max);
            Console.Write($"  {current}/{max}");
        }

        private void ShowHPBarInline(int current, int max)
        {
            ShowBarInline(current, max, HpBarLength, GetHPColor);
        }

        private void ShowMPBar(int current, int max)
        {
            ShowMPBarInline(current, max);
            Console.Write($"  {current}/{max}");
        }

        private void ShowMPBarInline(int current, int max)
        {
            ShowBarInline(current, max, MpBarLength, _ => ConsoleColor.Blue);
        }

        private void ShowBarInline(int current, int max, int barLength, Func<double, ConsoleColor> colorSelector)
        {
            double ratio = max <= 0 ? 0.0 : (double)current / max;
            ratio = Math.Clamp(ratio, 0.0, 1.0);
            int filled = (int)Math.Round(ratio * barLength);
            int empty = barLength - filled;

            Console.Write("[");
            Console.ForegroundColor = colorSelector(ratio);
            Console.Write(new string('█', filled));
            Console.ResetColor();
            Console.Write(new string('░', empty));
            Console.Write("]");
        }

        private ConsoleColor GetHPColor(double ratio)
        {
            if (ratio > 0.4) return ConsoleColor.Green;
            if (ratio > 0.2) return ConsoleColor.Yellow;
            return ConsoleColor.Red;
        }
        
        private void PrintStatusBar(string label, int current, int max, ConsoleColor color, Action<int, int> barAction)
        {
            Console.ForegroundColor = color;
            Console.Write($"{label} : ");
            barAction(current, max);
            Console.ResetColor();
            Console.WriteLine($"  {current}/{max}");
        }
        #endregion
    }
}