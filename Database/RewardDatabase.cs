using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Text_RPG_11.ItemData;
using static Text_RPG_11.RewardData;

namespace Text_RPG_11
{
    public static class RewardDatabase
    {
        private static string rewardDataPath = "Data/rewards.json";
        private static RewardDataContainer cachedRewards;

        static RewardDatabase()
        {
            cachedRewards = LoadRewards();
        }

        private static RewardDataContainer LoadRewards()
        {
            try
            {
                if (!File.Exists(rewardDataPath))
                {
                    Console.WriteLine($"rewards.json을 찾을 수 없습니다: {rewardDataPath}");
                    return GetDefaultContainer();
                }

                string json = File.ReadAllText(rewardDataPath);
                var container = JsonConvert.DeserializeObject<RewardDataContainer>(json);

                if (container == null || container.dungeonRewards == null)
                {
                    Console.WriteLine("보상 데이터 파싱 실패");
                    return GetDefaultContainer();
                }

                Console.WriteLine($"보상 데이터 로드 완료: 던전 {container.dungeonRewards.Count}개 구간");
                return container;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"보상 데이터 로드 오류: {ex.Message}");
                return GetDefaultContainer();
            }
        }

        private static RewardDataContainer GetDefaultContainer()
        {
            return new RewardDataContainer
            {
                dungeonRewards = new List<DungeonRewardData>(),
                questRewards = new List<QuestRewardData>()
            };
        }

        // 특정 스테이지의 던전 보상 데이터 가져오기
        public static DungeonRewardData GetDungeonRewardByStage(int stage)
        {
            foreach (var rewardData in cachedRewards.dungeonRewards)
            {
                if (rewardData.stageRange != null &&
                    rewardData.stageRange.Count >= 2 &&
                    stage >= rewardData.stageRange[0] &&
                    stage <= rewardData.stageRange[1])
                {
                    return rewardData;
                }
            }

            Console.WriteLine($"스테이지 {stage}에 해당하는 보상 데이터가 없습니다.");
            return null;
        }

        // 특정 퀘스트 ID로 퀘스트 보상 데이터 가져오기
        public static QuestRewardData GetQuestRewardById(int questId)
        {
            foreach (var questData in cachedRewards.questRewards)
            {
                if (questData.questId == questId)
                {
                    return questData;
                }
            }

            Console.WriteLine($"퀘스트 ID {questId}에 해당하는 보상 데이터가 없습니다.");
            return null;
        }

        // 모든 던전 보상 데이터 가져오기 (테스트용)
        public static List<DungeonRewardData> GetAllDungeonRewards()
        {
            return cachedRewards.dungeonRewards;
        }

        // 모든 퀘스트 보상 데이터 가져오기 (테스트용)
        public static List<QuestRewardData> GetAllQuestRewards()
        {
            return cachedRewards.questRewards;
        }
    }
}