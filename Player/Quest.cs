using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    internal class Quest
    {
        public string QuestName { get; }                      //이름
        public string Description { get; }                 //퀘스트 설명
        public bool IsCompleted { get; private set; }         //달성 조건
        public string Requirement { get; }                       // 퀘스트 달성 조건

        public int RewardGold { get; set; }                        //보상 골드
        public int RewardExp { get; set; }                        //보상 경험치
        public int RewardPotion { get; set; }                       //보상 포션
        public string RewardItem { get; set; }                     //보상 아이템

        public Quest(string questName, string description, string requirement, int rewardGold, int rewardExp, int rewardPotion, string rewardItem)
        {
            QuestName = questName;
            Description = description;
            Requirement = requirement;
            RewardGold = rewardGold;
            RewardExp = rewardExp;
            RewardPotion = rewardPotion;
            RewardItem = rewardItem;   // 직업별 아이템 보상
            IsCompleted = false;
        }

        public void Complete(Player player, List<Job> jobs)
        {
            if (IsCompleted) return;

            IsCompleted = true;

            // 직업별 보상 적용
            var job = jobs.FirstOrDefault(j => j.name == player.Job);
            if (job != null)
            {
                player.Gold += RewardGold;       // 기본 골드 보상
                player.Exp += RewardExp;       // 경험치 보상
                player.Potions += RewardPotion > 0 ? RewardPotion : 0;        // 포션 보상

                if (!string.IsNullOrEmpty(RewardItem))
                {
                    player.AddItem(RewardItem);
                }
            }
        }

        public bool CheckCompletionCondition(Player player, List<Job> jobs)
        {
            if (player.Level >= 5)
            {
                Complete(player, jobs);  
                return true;
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
        public static Quest CreateQuestForPlayer(Player player, List<Job> jobs)
        {
            string rewardItem = player.Job switch
            {
                "전사" => "롱소드",
                "마법사" => "증폭의 고서",
                "도적" => "단검",
                "궁수" => "롱소드",
                _ => "기본 아이템"
            };

            return new Quest(
                questName: "마을을 위협하는 미니언 처치",
                description: "마을 근처 미니언 5마리 처치",
                requirement: "Kill 5 monsters",
                rewardGold: 500,
                rewardExp: 30,
                rewardPotion: 2,
                rewardItem: rewardItem
            );
        }
    }
}
