using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    internal sealed class DropItem                          //몬스터가 사망시 드랍하는 아이템을 식별하는 Id와 드롭확률입니다
                                                            //sealed->상속금지, 다른 클래스에서의 상속을 막아두었습니다.
    {
        public int ItemId { get; set; }                     //드랍하는 아이템의 식별자
        public int DropChance { get; set; }                 // %로 표시할 예정이라 int타입으로 기재했습니다. ex) Dropchance = 90 => 드랍확률 90%
        public DropItem(int itemId, int dropChance)         //DropItem의 생성자입니다.
        { ItemId = itemId; DropChance = dropChance; }       //생성자: 필수값 두 개를 받아 필드 초기화 
    }

    internal sealed class Rewards                           //몬스터 사망시 경험치와 골드 획득량입니다.
    {
        public int Exp { get; set; }                        // 처치 시 주는 경험치
        public int Gold { get; set; }                       // 처치 시 주는 골드
        public List<DropItem> DropItems { get; set; } = new List<DropItem>();    // 드랍 아이템 목록(기본값: 빈 리스트)
                                                                                // null 회피를 위해 초기화 사용
        public Rewards(int exp, int gold) { Exp = exp; Gold = gold; }   // 생성자: 경험치/골드  
    }

    internal class Monster
    {
        public int Id { get; set; }                             //몬스터 식별자(식별ID)
        public string Name { get; set; }                        //이름
        public string Description { get; set; }                //설명
        public int Level { get; set; }                          //레벨

        public int MaxHP { get; set; }                          //최대체력
        public int HP { get; set; }                             //체력

        public int Attack { get; set; }                         //기본 공격력
        public int Defense { get; set; }                        //기본 방어력

        public int DodgeChance { get; set; }            // 회피 확률(%)
        public int CriticalChance { get; set; }         // 치명타 확률(%)

        // 전투 중 가감치(누적) — 버프/디버프는 여기에만 더하고 빼기
        public int TempAttack { get; set; }                     // 공격 보정(버프+, 디버프-)
        public int TempDefense { get; set; }                    // 방어 보정(버프+, 디버프-)

        // 보상/드랍/스폰 정보                                    // Rewards 객체와 스폰 위치 목록
        public Rewards Rewards { get; set; }                // 보상 묶음(경험치/골드/드랍)
        public IReadOnlyList<string> SpawnLocations => _spawnLocations;         // 읽기 전용 뷰 제공
        private readonly List<string> _spawnLocations = new List<string>();     // 내부 보관 리스트


        // 이하 람다식 프로퍼티로 Rewards가 null일 때 0을 반환해서 NullReferenceException을 방지합니다.
        /* if(Exp == 0)
             {
                get {
                      if (Rewards == null) return 0;
                      else return Rewards.Exp;
             } 와 동치입니다. Reward가 0이 아니면, Rewards.Exp를 반환합니다.
        */            
        public int RewardExp => Rewards?.Exp ?? 0;
        // 람다식 프로퍼티로 마찬가지로 Reward가 null일 때 0을 반환해서 NullReferenceException을 방지합니다.(0아니면 Rewards.Gold 반환)
        public int RewardGold => Rewards?.Gold ?? 0;

        /* 
          최종 전투 수치 
          TempAttack, TempDefense에는 디버프와 버프로 인한 수치가 누적됩니다. (예: 공격감소 2 -> temp = -2)
          이는 각각 Attack, Defense에 더해져서 최종적으로 AttackTotal, DefenseTotal에 저장됩니다. 
          (예: 원래공격력 + 템프공격력 = 최종공격력 -> 5 + (-2) = 3)
          음수가 나오는 것을 방지하기 위하여 Math.Max함수를 사용했습니다.괄호안에서 더 큰 값이 반환됩니다.
        */
        public int AttackTotal => Math.Max(0, Attack + TempAttack);     
        public int DefenseTotal => Math.Max(0, Defense + TempDefense);

        //몬스터 생성자 new Monster(...)로 호출
        public Monster(
           int id,
           string name,
           string description,
           int level,
           int maxHP,
           int attack,
           int defense,
           int dodgeChance,
           int criticalChance,
           Rewards rewards,
           IEnumerable<string> spawnLocations)
        {
            Id = id;                                               // 식별자 설정
            Name = name;                                           // 이름 설정
            Description = description;                             // 설명 설정
            Level = level;                                         // 레벨 설정

            MaxHP = maxHP;                                         // 최대 HP
            HP = maxHP;                                            // 시작 체력은 최대치
            Attack = attack;                                       // 공격력
            Defense = defense;                                     // 방어력

            DodgeChance = Math.Clamp(dodgeChance, 0, 100);         // 0~100 범위로 클램프(변수,최솟값,최댓값)
                                                                   // 확률이 0보다 작거나 100보다 큰 경우를 방지합니다(퍼센트 기준 예시1: -1% 예시2: 105%)
            CriticalChance = Math.Clamp(criticalChance, 0, 100);   // 0~100 범위로 클램프

            Rewards = rewards ?? new Rewards(0, 0);                // null 방지: 기본 보상 0,0
            if (spawnLocations != null) _spawnLocations.AddRange(spawnLocations); // 스폰 위치 복사
                                                                                  // AddRange(더할것들)로 Add()와 달리 복수의 대상을 추가가능

            TempAttack = 0;                                        // 전투 시작 시 보정 0
            TempDefense = 0;
        }

        //몬스터 피격시 데미지만큼 체력이 감소
        public void TakeDamage(int dmg)
        {
            int taken = Math.Max(0, dmg);       //0과 dmg중 큰것을 반환합니다 ->피해량이 음수인 경우 방지
            HP = Math.Max(0, HP - dmg);         //0과 HP-dmg중 큰것을 반환합니다 ->Hp가 음수인 경우 방지
        }

        //전투 종료시 Temp의 값 리셋
        public void ResetTemps()
        {
            TempAttack = 0;
            TempDefense = 0;
        }

        public bool isDead => HP <= 0; //몬스터 사망 여부


        //이하 몬스터 생성 메서드
        public static Monster Minion()=>
            new Monster(name:"미니언", level:2, maxHP:15, attack:5, defense:0,rewardExp:2, rewardGold:3);
        
        public static Monster Voidling()=>
            new Monster(name:"공허충",level:3, maxHP:10, attack:9,defense:1,rewardExp:3,rewardGold:4);

        public static Monster CanonMinion()=>
            new Monster(name:"대포미니언",level:5, maxHP:25, attack:8,defense:5,rewardExp:5,rewardGold:6);

        public static Monster SuperMinion() =>
            new Monster(name: "슈퍼 미니언", level: 7, maxHP: 45, attack: 12, defense: 7, rewardExp: 8, rewardGold: 10);

        //정글 몬스터
        public static Monster Gromp() =>
            new Monster(name: "그롬프 (Gromp)", level: 4, maxHP: 26, attack: 9, defense: 3, rewardExp: 5, rewardGold: 7);

        public static Monster Raptors() =>
            new Monster(name: "칼날부리 (Raptors)", level: 4, maxHP: 22, attack: 10, defense: 2, rewardExp: 5, rewardGold: 7);

        public static Monster Krugs() =>
            new Monster(name: "돌거북 (Krugs)", level: 4, maxHP: 30, attack: 8, defense: 4, rewardExp: 5, rewardGold: 7);

        public static Monster BlueSentinel() =>
            new Monster(name: "블루 센티널", level: 6, maxHP: 40, attack: 9, defense: 6, rewardExp: 7, rewardGold: 9);

        public static Monster RedBrambleback() =>
            new Monster(name: "레드 브램블백", level: 6, maxHP: 40, attack: 11, defense: 4, rewardExp: 7, rewardGold: 9);

        public static Monster ScuttleCrab() =>
            new Monster(name: "바위게 (Scuttle Crab)", level: 5, maxHP: 24, attack: 6, defense: 8, rewardExp: 6, rewardGold: 8);
    }
}
