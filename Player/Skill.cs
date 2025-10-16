using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{

    internal enum SkillType
    {
        Attack,       //기존 Damage에서 Attack으로 변경하였습니다.
        Heal,
        Buff,
        Debuff,
        AttackDebuff, // 공격 + 디버프
        AttackBuff    // 공격 + 버프 
    }

    internal sealed class SkillEffects
    {
        // Buff(+)
        public int AttackBonus { get; set; } = 0;
        public int DefenseBonus { get; set; } = 0;
        // Debuff(양수로 표기. 절댓값만큼 해당수치(공격/방어)를 감소시킵니다. ex) AttackMinus = 1 -> 몬스터 공격력 1 감소)
        public int AttackMinus { get; set; } = 0;
        public int DefenseMinus { get; set; } = 0;

        public int Duration { get; set; } = 0;             // 0이면 비지속, >0이면 N턴 지속

        public List<string> AdditionalEffects { get; set; } = new List<string>(); // UI/로깅용 태그

        // 전투부 합성값: +면 버프, -면 디버프
        public int EffectiveAttackDelta => AttackBonus - AttackMinus;
        public int EffectiveDefenseDelta => DefenseBonus - DefenseMinus;
    }

    internal class Skill
    {
        public int Id { get; private set; }                     //스킬 고유 번호
        public string Name { get; private set; }                //스킬 이름
        public string Description { get; private set; }         //스킬에 대한 설명
        public string RequiredJob { get; private set; }         // "전사" / "마법사" / "궁수" … 해당 스킬 사용하기 위해 요구되는 직업
        public int RequiredLevel { get; private set; }          // 해당스킬 사용하기 위해 요구되는 레벨
        public int ManaCost { get; private set; }               //마나 소비량
        public int Cooldown { get; private set; }               //쿨타임 우리게임은 턴제이므로 턴으로 표기, 턴마다 1씩 감소
        public SkillEffects Effects { get; private set; }       
        
        
        public SkillType Type { get; private set; }
        public float PowerMultiplier { get; private set; }  // Damage/Heal 계수(Heal은 정수 캐스팅)
        public int Hits { get; private set; }             // 다타격 횟수(0/1 = 단일)
        public bool GuaranteedCritical { get; private set; }
        public string Desc { get; private set; }
        
        public int CurrentCooldown { get; set; } = 0;       // 런타임 쿨다운(턴)
        public int LeftDuration { get; set; } = 0;      // 남은 지속(턴)

        public IEnumerable<string> GetEffectTags()
        {
            if (GuaranteedCritical) yield return "guaranteed-crit";
            if (Effects.AttackBonus > 0) yield return $"attack+{Effects.AttackBonus}";
            if (Effects.DefenseBonus > 0) yield return $"defense+{Effects.DefenseBonus}";
            if (Effects.AttackMinus > 0) yield return $"attack-{Effects.AttackMinus}";
            if (Effects.DefenseMinus > 0) yield return $"defense-{Effects.DefenseMinus}";
            if (Effects.Duration > 0) yield return $"duration:{Effects.Duration}";
        }

        private Skill(
            int id, string name, string description,
            string requiredJob, int requiredLevel, 
            int manaCost, int cooldown,
            SkillType type, float powerMultiplier, int hits, string desc,
            bool guaranteedCritical = false,
            int attackBonus = 0, int defenseBonus = 0,
            int attackMinus = 0, int defenseMinus = 0,
            int duration = 0)
        {
            Id = id;
            Name = name;
            Description = description;
            RequiredJob = requiredJob;
            RequiredLevel = requiredLevel;
            ManaCost = manaCost;
            Cooldown = cooldown;

            Type = type;
            PowerMultiplier = powerMultiplier;
            Hits = hits;
            Desc = desc;
            GuaranteedCritical = guaranteedCritical;

            // Effects 생성을 기존 스킬별 팩토리메서드에서 스킬 생성자 내부로 옮겼습니다
            Effects = new SkillEffects
            {
                AttackBonus = attackBonus,
                DefenseBonus = defenseBonus,
                AttackMinus = attackMinus,
                DefenseMinus = defenseMinus,
                Duration = duration
            };
        }

        //====================이하 가렌(전사) 스킬==================

        // 심판: 무작위 3타 ×1.15
        public static Skill Garen_Judgment() =>
            new Skill(
                id: 101,
                name: "Judgment",
                description: "회전 공격으로 적을 연속 타격합니다.",
                requiredJob: "전사",
                requiredLevel: 3,
                manaCost: 14,
                cooldown: 4,
                type: SkillType.Attack,
                powerMultiplier: 1.15f,
                hits: 3,
                desc: "무작위 적을 3회 타격(각 ×1.15)."
            );
        
        // 용기: 방어 +4 (3턴)
        public static Skill Garen_Courage() =>
            new Skill(
                id: 102,
                name: "Courage",
                description: "굳건한 의지로 몸을 단단히 합니다.",
                requiredJob: "전사",
                requiredLevel: 3,
                manaCost: 10,
                cooldown: 5,
                type: SkillType.Buff,
                powerMultiplier: 0,                     // 버프/디버프는 SkillEffects 의 Attack/DefenseBonus 로 처리합니다
                hits: 0,
                desc: "자신의 방어력 +4 (3턴).",
                defenseBonus: 4, duration: 3            /* 기존 effect: new SkillEffects()를 삭제하고(스킬 생성자에서 만드는것으로 수정)
                                                           팩토리 메서드 안에 직접 값을 기재하는 것으로 바꾸었습니다. */
            );

        // 인내: HP +25
        public static Skill Garen_Perseverance() =>
            new Skill(
                id: 103,
                name: "Perseverance",
                description: "잠시 호흡을 고르며 체력을 회복합니다.",
                requiredJob: "전사",
                requiredLevel: 2,
                manaCost: 10,
                cooldown: 4,
                type: SkillType.Heal,
                powerMultiplier: 25f,
                hits: 0,
                desc: "자신의 체력을 25 회복."
            );

        // 데마시아의 정의: 확정 치명 ×1.9
        public static Skill Garen_DemacianJustice() =>
            new Skill(
                id: 104,
                name: "Demacian Justice",
                description: "데마시아의 이름으로 적을 처단합니다.",
                requiredJob: "전사",
                requiredLevel: 6,
                manaCost: 22,
                cooldown: 6,
                type: SkillType.Attack,
                powerMultiplier: 1.9f,
                hits: 1,
                desc: "공격력×1.9, 반드시 치명타로 적용.",
                guaranteedCritical: true
            );

        //==============================이하 럭스(마법사) 스킬============================

        // 빛의 속박: ×1.4 + 방깎 -2 (3턴)
        public static Skill Lux_LightBinding() =>
            new Skill(
                id: 201,
                name: "Light Binding",
                description: "빛줄기로 적을 속박합니다.",
                requiredJob: "마법사",
                requiredLevel: 3,
                manaCost: 12,
                cooldown: 3,
                type: SkillType.AttackDebuff,
                powerMultiplier: 1.4f,
                hits: 1,
                desc: "×1.4, 추가로 대상 방어 -2 (3턴).",
                defenseMinus: 2, duration: 3
            );

        // 광휘의 특이점: 무작위 3타 ×1.2
        public static Skill Lux_LucentSingularity() =>
            new Skill(
                id: 202,
                name: "Lucent Singularity",
                description: "광휘의 구체가 폭발합니다.",
                requiredJob: "마법사",
                requiredLevel: 5,
                manaCost: 18,
                cooldown: 5,
                type: SkillType.Attack,
                powerMultiplier: 1.2f,
                hits: 3,
                desc: "무작위 적에게 3회 타격(각 ×1.2)."
            );

        // 프리즘 보호막: HP +30
        public static Skill Lux_PrismaticBarrier() =>
            new Skill(
                id: 203,
                name: "Prismatic Barrier",
                description: "빛의 보호막으로 치유합니다.",
                requiredJob: "마법사",
                requiredLevel: 2,
                manaCost: 10,
                cooldown: 4,
                type: SkillType.Heal,
                powerMultiplier: 30f,
                hits: 0,
                desc: "자신의 체력을 30 회복."
            );

        // 최후의 섬광: 단일 ×2.2
        public static Skill Lux_FinalSpark() =>
            new Skill(
                id: 204,
                name: "Final Spark",
                description: "최후의 섬광으로 적을 태웁니다.",
                requiredJob: "마법사",
                requiredLevel: 8,
                manaCost: 24,
                cooldown: 6,
                type: SkillType.Attack,
                powerMultiplier: 2.2f,
                hits: 1,
                desc: "공격력×2.2의 강력한 일격."
            );

        //======================이하 애쉬(궁수) 스킬=======================
        // 일제사격: 무작위 4타 ×1.1
        public static Skill Ashe_Volley() =>
            new Skill(
                id: 301,
                name: "Volley",
                description: "화살비를 퍼붓습니다.",
                requiredJob: "궁수",
                requiredLevel: 2,
                manaCost: 12,
                cooldown: 3,
                type: SkillType.Attack,
                powerMultiplier: 1.1f,
                hits: 4,
                desc: "무작위 적에게 4회 타격(×1.1)."
            );

        // 매의 시야: 공격 +3 (3턴)
        public static Skill Ashe_Hawkshot() =>
            new Skill(
                id: 302,
                name: "Hawkshot",
                description: "집중력을 높여 공격력을 끌어올립니다.",
                requiredJob: "궁수",
                requiredLevel: 3,
                manaCost: 8,
                cooldown: 5,
                type: SkillType.Buff,
                powerMultiplier: 0f,        // 버프/디버프는 SkillEffects 의 Attack/DefenseBonus 로 처리합니다
                hits: 0,
                desc: "자신의 공격력 +3 (3턴).",
                attackBonus: 3, duration: 3
            );

        // 수정화살: 확정 치명 ×1.7 + 방깎 -3 (3턴)
        public static Skill Ashe_EnchantedCrystalArrow() =>
            new Skill(
                id: 303,
                name: "Enchanted Crystal Arrow",
                description: "거대한 얼음 화살을 발사합니다.",
                requiredJob: "궁수",
                requiredLevel: 6,
                manaCost: 20,
                cooldown: 6,
                type: SkillType.AttackDebuff,
                powerMultiplier: 1.7f,
                hits: 1,
                desc: "×1.7, 확정 치명타. 추가로 방어 -3 (3턴).",
                attackBonus: 3, duration: 3,
                guaranteedCritical: true
            );

        // 냉기 화살: 방어 -3 (3턴)
        public static Skill Ashe_FrostShot() =>
            new Skill(
                id: 304,
                name: "Frost Shot",
                description: "냉기 화살로 적의 약점을 노출시킵니다.",
                requiredJob: "궁수",
                requiredLevel: 4,
                manaCost: 10,
                cooldown: 4,
                type: SkillType.Debuff,
                powerMultiplier: 0f,                //버프/디버프는 SkillEffects 의 Attack/DefenseBonus 로 처리합니다
                hits: 0,
                desc: "대상 방어 -3 (3턴).",
                defenseMinus: 3, duration: 3
            );

        //=====================이하 제드(도적) 스킬=====================


        // 면도 표창: 단일 ×1.5 (2회) — 멀티히트 수리검
        public static Skill Zed_RazorShuriken() =>
            new Skill(
                id: 401,
                name: "Razor Shuriken",
                description: "면도날처럼 예리한 표창을 던집니다.",
                requiredJob: "도적",
                requiredLevel: 1,
                manaCost: 10,
                cooldown: 2,
                type: SkillType.Attack,
                powerMultiplier: 1.5f,
                hits: 2,
                desc: "단일 대상에 2회 타격(각 ×1.5)."
            );

        // 그림자 절개: 무작위 3타 ×1.2 — 광역 느낌
        public static Skill Zed_ShadowSlash() =>
            new Skill(
                id: 402,
                name: "Shadow Slash",
                description: "그림자의 궤적이 적을 베어넘깁니다.",
                requiredJob: "도적",
                requiredLevel: 3,
                manaCost: 14,
                cooldown: 3,
                type: SkillType.Attack,
                powerMultiplier: 1.2f,
                hits: 3,
                desc: "무작위 적 3회 타격(각 ×1.2)."
            );

        // 살아있는 그림자: 공격 +4 (3턴) — 자기 버프
        public static Skill Zed_LivingShadow() =>
            new Skill(
                id: 403,
                name: "Living Shadow",
                description: "그림자와 일체가 되어 감각을 일깨웁니다.",
                requiredJob: "도적",
                requiredLevel: 4,
                manaCost: 10,
                cooldown: 5,
                type: SkillType.Buff,
                powerMultiplier: 0f,            // 버프/디버프는 SkillEffects 의 Attack/DefenseBonus 로 처리합니다 
                hits: 0,
                desc: "자신의 공격력 +4 (3턴).",
                attackBonus: 4, duration: 3
            );

        // 죽음의 표식: 확정 치명 ×1.8 + 방깎 -4 (3턴)
        public static Skill Zed_DeathMark() =>
            new Skill(
                id: 404,
                name: "Death Mark",
                description: "표식을 남기고 그늘 속에서 마무리합니다.",
                requiredJob: "도적",
                requiredLevel: 6,
                manaCost: 22,
                cooldown: 6,
                type: SkillType.AttackDebuff,
                powerMultiplier: 1.8f,
                hits: 1,
                desc: "×1.8, 반드시 치명타. 추가로 방어 -4 (3턴).",
                defenseMinus: 4, duration: 3,
                guaranteedCritical: true
            );
    }
}
