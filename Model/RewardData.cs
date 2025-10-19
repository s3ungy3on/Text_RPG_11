using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    // JSON 전체 구조
    public class RewardDataContainer
    {
        public List<DungeonRewardData> dungeonRewards { get; set; }
        public List<QuestRewardData> questRewards { get; set; }
    }

    // 던전 보상 데이터
    public class DungeonRewardData
    {
        public List<int> stageRange { get; set; }
        public List<RewardGroup> rewardGroups { get; set; }  // ← 수정
    }

    // 보상 그룹 (아이템/포션/꽝)
    public class RewardGroup
    {
        public string groupName { get; set; }      // "아이템", "포션", "꽝"
        public int groupChance { get; set; }       // 그룹 선택 확률
        public List<DungeonRewardItem> items { get; set; }  // 그룹 내 아이템들 (null 가능)
    }

    // 던전 보상 아이템
    public class DungeonRewardItem
    {
        public int itemId { get; set; }
        public float dropChance { get; set; }
    }

    // 퀘스트 보상 데이터
    public class QuestRewardData
    {
        public int questId { get; set; }
        public string questName { get; set; }
        public string description { get; set; }
        public string requirement { get; set; }
        public int expReward { get; set; }
        public int goldReward { get; set; }
        public int potionReward { get; set; }
        public Dictionary<string, string> itemRewards { get; set; }  // 직업별 아이템
        public string type { get; set; }  // "KillMonster", "EquipItem" 등
    }
}