using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    internal class Monster
    {
        public string Name { get; set; }
        public int Level { get; set; }

        public int MaxHP { get; set; }
        public int HP { get; set; }

        public int Attack { get; set; }
        public int Defense { get; set; }

        public int RewardExp { get; set; }
        public int RewardGold { get; set; }
    }
}
