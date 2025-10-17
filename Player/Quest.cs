using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Text_RPG_11
{
    internal enum QuestType
    {
        KillMonster,
        EquipItem
    }
    internal class Quest
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
            IsCompleted = false;                                                            //퀘스트 기본적으로 미완료
            Type = type;
            IsCompleted = false;
        }

        public void Complete(Player player, List<Job> jobs)
        {
            if (IsCompleted) return;                                            //퀘스트 완료된 퀘스트면 처리X

            IsCompleted = true;                                                     //퀘스트 완료 상태로 변경


            if (!string.IsNullOrEmpty(RewardItem))                                     //아이템 보상 적용
            {
                player.AddItem(RewardItem);                                             // 플레이어 인벤토리에 추가
            }

            player.Gold += RewardGold;
            player.Exp += RewardExp;
            player.Potions += RewardPotion;
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

            string rewardItem1 = player.Job switch                                  // 플레이어 직업에 따른 아이템 보상 결정
            {
                "전사" => "롱소드",
                "마법사" => "증폭의 고서",
                "도적" => "단검",
                "궁수" => "롱소드",
                _ => "기본 아이템"
            };

            // 새로운 퀘스트 객체 생성 후 반환
            quests.Add(new Quest(
                questName: "마을을 위협하는 미니언 처치",
                description: "마을 근처 미니언 5마리 처치",
                requirement: "미니언 5 처치",
                rewardGold: 500,
                rewardExp: 30,
                rewardPotion: 2,
                rewardItem: rewardItem1,
                type: QuestType.KillMonster
            ));

            string rewardItem2 = player.Job switch
            {
                "전사" => "강철 갑옷",
                "마법사" => "마법 로브",
                "도적" => "그림자 자켓",
                "궁수" => "사냥꾼의 망토",
                _ => "기본 방어구"
            };

            quests.Add(new Quest(
                questName: "전투 준비 태세",
                description: "장비 착용하기.",
                requirement: "장비 착용 완료",
                rewardGold: 300,
                rewardExp: 30,
                rewardPotion: 1,
                rewardItem: rewardItem2,
                type: QuestType.EquipItem
            ));

            return quests;
        }


    }

    internal class QuestManager
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
}
