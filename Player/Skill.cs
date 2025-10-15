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

        // 공격/치명타/회복 배수 (예: 2.0f -> 공격력*2)
        public float PowerMultiplier { get; set; } = 1.0f;

        //시전 횟수 (예: 더블 스트라이크 =2)
        public int Hits { get; set; } = 1;

        // 스킬 설명
        public string Description { get; set; }

        public Skill(string name, int useMP, SkillType type, float powerMultiplier, int hits, string desc = "")
        {
            Name = name;
            UseMP = useMP;
            Type = type;
            PowerMultiplier = powerMultiplier;
            Hits = hits;
            Description = desc;
        }

        //팩토리: 요구사항 스킬
        public static Skill AlphaStrike() =>
            new Skill(
                name: "알파 스트라이크",
                useMP: 10,
                type: SkillType.Damage,
                powerMultiplier: 2.0f,
                hits: 1,
                desc: "공격력 *2 로 하나의 적을 공격합니다.");

        public static Skill DoubleStrike() =>
            new Skill(
                name: "더블 스트라이크",
                useMP: 15,
                type: SkillType.Damage,
                powerMultiplier: 1.5f,
                hits: 2,
                desc: "공격력 *1.5 로 두 명의 적을 랜덤으로 공격합니다.");

        public static Skill SwordDance() =>
            new Skill(
                name: "칼춤",
                useMP: 10,
                type: SkillType.Buff,
                powerMultiplier: 5f,
                hits: 0,
                desc: "격렬한 칼춤을 춰서 기세를 높입니다.");

        public static Skill Recover() =>
            new Skill(
                name: "HP회복",
                useMP: 12,
                type: SkillType.Heal,
                powerMultiplier: 30f,
                hits: 0,
                desc: "세포를 재생시켜 30만큼의 HP를 회복합니다.");

        public static Skill Reer() =>
            new Skill(
                name: "째려보기",
                useMP: 10,
                type: SkillType.Debuff,
                powerMultiplier: 3f,
                hits: 0,
                desc: "매섭게 째려보아 상대의 방어를 3만큼 낮춥니다.");


    }
}
