using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{

    internal enum SkillType { Damage, Heal, Buff, Debuff }

    internal class Skill
    {
        public string Name { get; set; }
        public int UseMP { get; set; }
        public SkillType Type { get; set; }
        // 타입별 해석:
        // - Damage: 공격 배수(×값)
        // - Heal  : 고정 회복량(+값)
        // - Buff  : 공격력 +값(전투 동안 TempAttack에 누적)
        // - Debuff: 방어력 -값(전투 동안 TempDefense에 누적)

        public float PowerMultiplier { get; set; } = 1.0f; // 공격/치명타/회복 배수 (예: 2.0f -> 공격력*2)
        public bool IsCritical { get; set; } // Damage 한정: 확정 치명 스위치

        public int Hits { get; set; } = 1; 


        // 스킬 설명
        public string Description { get; set; }

        public Skill(string name, int useMP, SkillType type, float powerMultiplier, int hits, string desc = "",bool isCritical = false)
        {
            Name = name;
            UseMP = useMP;
            Type = type;
            PowerMultiplier = powerMultiplier;
            Hits = hits;
            Description = desc;
            IsCritical = isCritical;
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
