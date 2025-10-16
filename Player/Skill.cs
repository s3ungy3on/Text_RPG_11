using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{

    internal enum SkillType { Damage, Heal, Buff, Debuff }

    internal sealed class SkillEffects
    {
        public float DamageMultiplier { get; set; } = 0f;   // 0이면 미사용, >0이면 배수로 사용
        public int DefenseBonus { get; set; } = 0;         // 0이면 미사용, >0이면 방어 +X
        public int Duration { get; set; } = 0;             // 0이면 비지속, >0이면 N턴 지속
        public List<string> AdditionalEffects { get; set; } = new List<string>(); // UI/로깅용 태그
    }

    internal class Skill
    {
        public int Id { get; set; }                     //스킬 고유 번호
        public string Name { get; set; }                //스킬 이름
        public string Description { get; set; }         //스킬에 대한 설명
        public string RequiredJob { get; set; }         // "전사" / "마법사" / "궁수" … 해당 스킬 사용하기 위해 요구되는 직업
        public int RequiredLevel { get; set; }          // 해당스킬 사용하기 위해 요구되는 레벨
        public string TypeText { get; set; }            // "attack" / "buff" / "heal" / "debuff"
        public int ManaCost { get; set; }               //마나 소비량
        public int Cooldown { get; set; }               //쿨타임 우리게임은 턴제이므로 턴으로 표기, 턴마다 1씩 감소
        public SkillEffects Effects { get; set; }       
        
        
        public SkillType Type { get; private set; }
        public float PowerMultiplier { get; private set; }  // Damage/Heal 계수(Heal은 정수 캐스팅)
        public int Hits { get; private set; }             // 다타격 횟수(0/1 = 단일)
        public bool GuaranteedCritical { get; private set; }
        public string Desc { get; private set; }
        public int CurrentCooldown { get; set; } = 0;       // 런타임 쿨다운(턴)

        private Skill(
            int id, string name, string description,
            string requiredJob, int requiredLevel,
            string typeText, int manaCost, int cooldown, SkillEffects effects,
            SkillType type, float powerMultiplier, int hits, string desc, bool guaranteedCritical = false)
        {
            Id = id;
            Name = name;
            Description = description;
            RequiredJob = requiredJob;
            RequiredLevel = requiredLevel;
            TypeText = typeText;
            ManaCost = manaCost;
            Cooldown = cooldown;
            Effects = effects ?? new SkillEffects();        // effects가 널이면 => new SkillEffects 실행해서 널 방지

            Type = type;
            PowerMultiplier = powerMultiplier;
            Hits = hits;
            Desc = desc;
            GuaranteedCritical = guaranteedCritical;
        }

        //이하 가렌(전사) 스킬

        public static Skill Garen_DemacianJustice() =>
            new Skill("Demacian Justice", 22, SkillType.Damage, 1.9f, 1,
                "적을 처단하는 일격(×1.9, 반드시 치명타).", isCritical: true);

        public static Skill Garen_Courage() =>
            new Skill("Courage", 10, SkillType.Buff, 4f, 0,
                "자신의 방어를 +4 올립니다(전투 동안)."); // Battle에서 player.TempDefense += 4로 해석

        public static Skill Garen_Perseverance() =>
            new Skill("Perseverance", 10, SkillType.Heal, 25f, 0,
                "잠시 호흡을 고르며 체력을 25 회복.");

        //이하 럭스 스킬
        public static Skill Lux_LightBinding() =>
            new Skill("Light Binding", 12, SkillType.Damage, 1.4f, 1,
                "빛줄기로 적을 속박(×1.4). 맞은 적은 방어 -2."); // Battle에서 target.TempDefense -= 2 추가

        public static Skill Lux_LucentSingularity() =>
            new Skill("Lucent Singularity", 18, SkillType.Damage, 1.2f, 3,
                "광휘 구체가 폭발하며 무작위 적 3회 타격(×1.2).");

        public static Skill Lux_FinalSpark() =>
            new Skill("Final Spark", 24, SkillType.Damage, 2.2f, 1,
                "광선으로 선상 목표를 관통(×2.2, 치명타 확률 적용).");

        public static Skill Lux_PrismaticBarrier() =>
            new Skill("Prismatic Barrier", 10, SkillType.Heal, 30f, 0,
                "지팡이를 휘둘러 자신을 치유(+30).");
        
        //이하 애쉬(궁수) 스킬
        public static Skill Ashe_Volley() =>
            new Skill("Volley", 12, SkillType.Damage, 1.1f, 4,
                "화살비로 무작위 적 4회 타격(×1.1).");

        public static Skill Ashe_Hawkshot() =>
            new Skill("Hawkshot", 8, SkillType.Buff, 3f, 0,
                "집중력을 높여 공격력 +3(전투 동안)."); // Battle에서 player.TempAttack += 3

        public static Skill Ashe_EnchantedCrystalArrow() =>
            new Skill("Enchanted Crystal Arrow", 20, SkillType.Damage, 1.7f, 1,
                "거대한 얼음 화살(×1.7, 반드시 치명타·추가로 방어 -3 적용).", isCritical: true); // ※ target.TempDefense -= 3

        public static Skill Ashe_FrostShot() =>
            new Skill("Frost Shot", 10, SkillType.Debuff, 3f, 0,
                "냉기 화살로 적의 방어 -3(전투 동안)."); // Battle에서 target.TempDefense -= 3
    }
}
