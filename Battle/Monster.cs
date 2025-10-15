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


        //이하 몬스터 팩토리 메서드
        public static Monster Minion() =>
            new Monster(
                id: 1,
                name: "미니언",
                description: "소환사의 협곡을 누비는 기본 병력.",
                level: 2,
                maxHP: 15, attack: 5, defense: 0,
                dodgeChance: 5, criticalChance: 0,
                rewards: new Rewards(2, 3)
                {
                    DropItems = new List<DropItem> { new DropItem(201, 30) } // 아이템 201을 30% 확률로 드랍
                },
                spawnLocations: new[] { "미드 라인", "탑 라인", "바텀 라인" }  // 스폰 위치 문자열 배열
            );

        public static Monster Voidgrub() =>
            new Monster(
                id: 12,
                name: "공허충 (Voidgrub)",
                description: "공허에 물든 벌레형 몬스터. 군집으로 출몰해 성가시다.",
                level: 3,
                maxHP: 18,attack: 9,defense: 1,
                dodgeChance: 10, criticalChance: 5,
                rewards: new Rewards(4, 6)
                {
                    DropItems = new List<DropItem> { new DropItem(itemId: 201, dropChance: 40) } // 예: 재료 201을 40% 확률로 드랍
                },
                spawnLocations: new[] { "미드 라인", "탑 라인", "바텀 라인" }  // 스폰 위치 문자열 배열
            );

        public static Monster CanonMinion() =>
            new Monster(
                id: 4,
                name: "대포 미니언",
                description: "라인을 압박하는 포격 병력.",
                level: 5,
                maxHP: 28, attack: 8, defense: 5,
                dodgeChance: 3, criticalChance: 0,
                rewards: new Rewards(5, 6)
                {
                    DropItems = new List<DropItem> { new DropItem(201, 50) }
                },
                spawnLocations: new[] { "각 라인 포격 지점" }
            );
        public static Monster SuperMinion() =>
            new Monster(
                id: 5,
                name: "슈퍼 미니언",
                description: "억제기 파괴 후 출현하는 강력한 병력.",
                level: 7,
                maxHP: 45, attack: 12, defense: 7,
                dodgeChance: 0, criticalChance: 5,
                rewards: new Rewards(8, 10)
                {
                    DropItems = new List<DropItem> { new DropItem(202, 20) }
                },
                spawnLocations: new[] { "각 라인 전진 축" }
            );
        public static Monster Gromp() =>
            new Monster(
                id: 6,
                name: "그롬프 (Gromp)",
                description: "수풀 속에 사는 커다란 두꺼비.",
                level: 4,
                maxHP: 26, attack: 9, defense: 3,
                dodgeChance: 5, criticalChance: 0,
                rewards: new Rewards(5, 7),
                spawnLocations: new[] { "블루 정글" }
            );

        public static Monster Raptors() =>
            new Monster(
                id: 7,
                name: "칼날부리 (Raptors)",
                description: "무리 지어 다니는 새형 몬스터.",
                level: 4,
                maxHP: 22, attack: 10, defense: 2,
                dodgeChance: 10, criticalChance: 0,
                rewards: new Rewards(5, 7)
                {
                    DropItems = new List<DropItem> { new DropItem(201, 50) }
                },
                spawnLocations: new[] { "레드 정글" }
            );

        public static Monster Krugs() =>
            new Monster(
                id: 8,
                name: "돌거북 (Krugs)",
                description: "단단한 바위 거북.",
                level: 4,
                maxHP: 30, attack: 8, defense: 4,
                dodgeChance: 0, criticalChance: 0,
                rewards: new Rewards(5, 7),
                spawnLocations: new[] { "레드 정글" }
            );

        public static Monster BlueSentinel() =>
            new Monster(
                id: 9,
                name: "블루 센티널",
                description: "푸른 정령의 수호자.",
                level: 6,
                maxHP: 40, attack: 9, defense: 6,
                dodgeChance: 0, criticalChance: 0,
                rewards: new Rewards(7, 9)
                {
                    DropItems = new List<DropItem> { new DropItem(301, 100) } // 100% 드랍 예시
                },
                spawnLocations: new[] { "블루 정글 수호석" }
            );
        public static Monster RedBrambleback() =>
            new Monster(
                id: 10,
                name: "레드 브램블백",
                description: "붉은 정령의 수호자.",
                level: 6,
                maxHP: 40, attack: 11, defense: 4,
                dodgeChance: 0, criticalChance: 0,
                rewards: new Rewards(7, 9)
                {
                    DropItems = new List<DropItem> { new DropItem(302, 100) }
                },
                spawnLocations: new[] { "레드 정글 수호석" }
            );
        public static Monster ScuttleCrab() =>
            new Monster(
                id: 11,
                name: "바위게 (Scuttle Crab)",
                description: "강가를 순찰하는 중립 몬스터.",
                level: 5,
                maxHP: 24, attack: 6, defense: 8,
                dodgeChance: 20, criticalChance: 0,
                rewards: new Rewards(6, 8),
                spawnLocations: new[] { "강가, 협곡 중앙" }
            );
    }
}
