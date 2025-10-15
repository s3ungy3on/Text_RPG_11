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
    }
}
