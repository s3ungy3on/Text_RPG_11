using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Text_RPG_11
{
    public enum QuestType
    {
        KillMonster,
        EquipItem
    }
    public class Quest
    {
        public string QuestName { get; }                      //퀘스트 이름
        public string Description { get; }                   //퀘스트 설명
        public bool IsCompleted { get; private set; }         //퀘스트 완료 여부
        public string Requirement { get; }                       // 퀘스트 달성 조건

        public int RewardGold { get; set; }                        //보상 골드
        public int RewardExp { get; set; }                        //보상 경험치
        public int RewardPotion { get; set; }                       //보상 포션 수량
        public string RewardItem { get; set; }                     //보상 아이템 이름

        public QuestType Type { get; }

        public Quest(string questName, string description, string requirement, int rewardGold, int rewardExp, int rewardPotion, string rewardItem, QuestType type)
        {
            QuestName = questName;                                                          //퀘스트 이름 저장
            Description = description;                                                     //퀘스트 내용 저장
            Requirement = requirement;                                                      //퀘스트 조건 저장
            RewardGold = rewardGold;                                                        //골드 보상 저장
            RewardExp = rewardExp;                                                          //경험치 보상 저장
            RewardPotion = rewardPotion;                                                    //포션 보상 저장
            RewardItem = rewardItem;                                                        // 직업별 아이템 보상 저장
            Type = type;
            IsCompleted = false;                                                            //퀘스트 기본적으로 미완료
        }

        public void Complete(Player player, List<Job> jobs)
        {
            if (IsCompleted) return;                                            //퀘스트 완료된 퀘스트면 처리X

            IsCompleted = true;                                                     //퀘스트 완료 상태로 변경

            player.Gold += RewardGold;
            player.Exp += RewardExp;
            player.Potions += RewardPotion;

            if (!string.IsNullOrEmpty(RewardItem))                                     //아이템 보상 적용
            {
                player.AddItem(RewardItem);                                             // 플레이어 인벤토리에 추가
            }
        }

        public bool CheckCompletionCondition(Player player, List<Job> jobs)
        {
            if (IsCompleted) return false;

            switch (Type)
            {
                case QuestType.KillMonster:
                    if (player.Level >= 5)
                    {
                        Complete(player, jobs);
                        return true;
                    }
                    break;

                case QuestType.EquipItem:
                    if (player.HasEquippedItem)
                    {
                        Complete(player, jobs);
                        return true;
                    }
                    break;
            }

            return false;
        }

        public void ShowQuestInfo(Player player)
        {
            Console.WriteLine($"퀘스트 이름: {QuestName}");
            Console.WriteLine($"퀘스트 설명: {Description}");
            Console.WriteLine($"퀘스트 요구 사항: {Requirement}");
            Console.WriteLine($"보상 - 경험치: {RewardExp}, 골드: {RewardGold}, 포션: {RewardPotion}, 아이템: {RewardItem}");
            Console.WriteLine($"플레이어 직업: {player.Job}");
            Console.WriteLine($"퀘스트 완료 여부: {IsCompleted}");
        }

        // 플레이어 직업에 맞춘 퀘스트 생성
        public static List<Quest> CreateQuestForPlayer(Player player, List<Job> jobs)
        {
            List<Quest> quests = new();

            // quests.json 파일 읽기
            string json = File.ReadAllText("quests.json");

            // JSON → C# 객체로 변환
            QuestData? questData = JsonSerializer.Deserialize<QuestData>(json);

            if (questData == null || questData.questRewards == null)
                return quests;

            // JSON의 각 퀘스트를 Quest 객체로 변환
            foreach (var q in questData.questRewards)
            {
                // 직업별 보상 아이템 선택
                string rewardItem = q.rewardItems.ContainsKey(player.Job)
                    ? q.rewardItems[player.Job]
                    : "기본 아이템";

                // Quest 객체 생성
                quests.Add(new Quest(
                questName: q.questName,
                description: "", 
                requirement: "",
                rewardGold: q.goldReward,
                rewardExp: q.expReward,
                rewardPotion: q.potionReward,
                rewardItem: rewardItem,
                type: Enum.TryParse(q.questType, out QuestType questType) ? questType : QuestType.KillMonster
 ));
            }

            return quests;
        }

    }

    public class QuestManager
    {
        private List<Quest> quests = new List<Quest>();   // 전체 퀘스트 목록
        private int currentQuestIndex = 0;                // 현재 진행 중인 퀘스트 인덱스

        public Quest? CurrentQuest
        {
            get
            {
                if (currentQuestIndex >= 0 && currentQuestIndex < quests.Count)
                    return quests[currentQuestIndex];
                return null;
            }
        }

        // 플레이어 직업에 맞는 퀘스트 초기화
        public void Initialize(Player player, List<Job> jobs)
        {
            quests = Quest.CreateQuestForPlayer(player, jobs);
            currentQuestIndex = 0;
        }

        // 퀘스트 진행 상태 갱신 (조건 충족 시 자동 완료)
        public void UpdateQuestProgress(Player player, List<Job> jobs)
        {
            if (CurrentQuest == null) return;

            bool isCompleted = CurrentQuest.CheckCompletionCondition(player, jobs);

            if (isCompleted)
                MoveToNextQuest();
        }

        // 다음 퀘스트로 이동
        private void MoveToNextQuest()
        {
            currentQuestIndex++;

            if (currentQuestIndex >= quests.Count)
                currentQuestIndex = quests.Count; // 모든 퀘스트 완료 시 멈춤
        }

        // 현재 퀘스트 반환
        public Quest? GetCurrentQuest()
        {
            return CurrentQuest;
        }

        // 전체 퀘스트 반환
        public List<Quest> GetAllQuests()
        {
            return quests;
        }

        // 완료된 퀘스트 목록 반환
        public List<Quest> GetCompletedQuests()
        {
            return quests.FindAll(q => q.IsCompleted);
        }

        // 진행 중인 퀘스트 목록 반환
        public List<Quest> GetActiveQuests()
        {
            return quests.FindAll(q => !q.IsCompleted);
        }
    }

    public class QuestRewardData
    {
        public int questId { get; set; }
        public string questName { get; set; }
        public string questType { get; set; }
        public int expReward { get; set; }
        public int goldReward { get; set; }
        public int potionReward { get; set; }
        public Dictionary<string, string> rewardItems { get; set; }

    }

    public class QuestData
    {
        public List<QuestRewardData> questRewards { get; set; }
    }
}
