using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Text_RPG_11.RewardData;

namespace Text_RPG_11
{
    public static class RewardDatabase
    {
        private static string RewardDataPath = "Data/reward.json";
        private static RewardDataContainer cachedreward;
    }
}
