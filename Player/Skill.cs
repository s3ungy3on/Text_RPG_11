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
    }
}
