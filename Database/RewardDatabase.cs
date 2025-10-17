//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Text_RPG_11.ItemData;
//using static Text_RPG_11.RewardData;

//namespace Text_RPG_11
//{
//    public static class RewardDatabase
//    {
//        private static string rewardDataPath = "Data/rewards.json";
//        private static RewardDataContainer cachedReward;

//        private static Random random = new Random();

//        static RewardDatabase()
//        {
//            cachedReward = LoadRewards();
//        }


//        public static RewardDataContainer LoadRewards()
//        {
//            try
//            {
//                string json = File.ReadAllText(rewardDataPath);
//                return JsonConvert.DeserializeObject<RewardDataContainer>(json);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"보상 데이터 로드 오류: {ex.Message}");
//                return GetDefaultContainer();
//            }

//        }

//        private static RewardDataContainer GetDefaultContainer()
//        {
//            return new RewardDataContainer
//            {
//                dungeonRewards = new List<DungeonRewardData>(),
//                questRewards = new List<QuestRewardData>()
//            };
//        }

//        #region 던전 클리어 보상 (스테이지)
//        public static List<Items> GetDungeonDropItems(int stage)
//        {
//            List<Items> dungeonRewards = new List<Items>();

//            var stageData = cachedReward.dungeonRewards.FirstOrDefault(dr => stage >= dr.stageRange[0] && stage <= dr.stageRange[1]);

//            if (stageData != null)
//            {
//                Console.WriteLine($"스테이지 {stage}에 대한 보상 테이블이 없습니다.");
//                return dungeonRewards;
//            }

//            var seletedGroup = SelectRewardGroup(stageData.rewardGorups);

//            if (seletedGroup == null)
//            {
//                return dungeonRewards;
//            }

//            var seletedItem = SeletItemFromGroup(selectedGroup);

//            if (seletedItem != null)
//            {
//                dungeonRewards.Add(selectedItem);
//            }

//            return dungeonRewards;
//        }

//        private static RewardGroup SelectRewardGroup(List<RewardGroup> groups)
//        {
//            if (groups == null || groups.Count == 0)
//            {
//                return null;
//            }

//            int totalChance = groups.Sum(g => g.groupChance);

//            int roll = random.Next(1, totalChance + 1);

//            int cumulative = 0;
//            foreach (var group in groups)
//            {
//                cumulative += group.groupChance;
//                if (roll <= cumulative)
//                {
//                    return group;
//                }
//            }

//            return null;
//        }

//        private static Items SelectItemFromGroup(RewardGroup group)
//        {
//            if (group.items == null || group.items.Count == 0)
//                return null;

//            // 1. 총 확률 계산
//            int totalChance = group.items.Sum(i => i.dropChance);

//            // 2. 랜덤 값 생성
//            int roll = random.Next(1, totalChance + 1);

//            // 3. 누적 확률로 아이템 선택
//            int cumulative = 0;
//            foreach (var reward in group.items)
//            {
//                cumulative += reward.dropChance;
//                if (roll <= cumulative)
//                {
//                    var item = ItemDatabase.GetItemById(reward.itemId);
//                    if (item != null)
//                    {
//                        return item;
//                    }
//                }
//            }

//            return null;
//        }
//        #endregion

//        #region 퀘스트 클리어 보상
//        public static QuestClearResult GetQuestClearReward(int questId)
//        {
//            var questReward = GetQuestReward(questId);

//            if (questReward == null)
//            {
//                Console.WriteLine($"경고: 퀘스트 ID {questId}를 찾을 수 없습니다.");
//                return null;
//            }

//            var result = new QuestClearResult
//            {
//                ExpReward = questReward.expReward,
//                GoldReward = questReward.goldReward,
//                Items = new List<Items>()
//            };

//            // 랜덤 보상 계산
//            if (questReward.randomRewards != null)
//            {
//                foreach (var randomReward in questReward.randomRewards)
//                {
//                    int roll = random.Next(1, 101);

//                    if (roll <= randomReward.dropChance)
//                    {
//                        var item = ItemDatabase.GetItemById(randomReward.itemId);
//                        if (item != null)
//                        {
//                            // amount만큼 추가
//                            for (int i = 0; i < randomReward.amount; i++)
//                            {
//                                result.Items.Add(item);
//                            }
//                        }
//                    }
//                }
//            }

//            return result;
//        }

//        #endregion
//    }
//}