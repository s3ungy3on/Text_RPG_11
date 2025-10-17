using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    public class RewardData
    {
        public class RewardDataContainer
        {
            public List<DungeonRewardData> dungeonRewards {  get; set; }
            public List<QuestRewardData> questRewards { get; set; }
        }

        public class DungeonRewardData
        {
            public List<int> stageRange { get; set; }
            public List<DungeonRewardItem> rewardItems { get; set; }
        }

        public class DungeonRewardItem
        {
            public int itemId {  get; set; }
            public int dropChance {  get; set; }
        }

        public class QuestRewardData
        {
            public int questId { get; set;}
            public string questName {  get; set; }
            public int expReward {  get; set; }
            public int goldReward {  get; set; }
            public List<QuestRewardItem> randomRewards { get; set; }
        }

        public class QuestRewardItem
        {
            public int itemId {  get; set; }
            public int amount {  get; set; }
            public int dropChance {  get; set; }
        }
    }
}
