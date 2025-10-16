using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
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

        public Quest(string questName, string description, string requirement, int rewardGold, int rewardExp, int rewardPotion, string rewardItem)
        {
            QuestName = questName;                                                          //퀘스트 이름 저장
            Description = description;                                                     //퀘스트 내용 저장
            Requirement = requirement;                                                      //퀘스트 조건 저장
            RewardGold = rewardGold;                                                        //골드 보상 저장
            RewardExp = rewardExp;                                                          //경험치 보상 저장
            RewardPotion = rewardPotion;                                                    //포션 보상 저장
            RewardItem = rewardItem;                                                        // 직업별 아이템 보상 저장
            IsCompleted = false;                                                            //퀘스트 기본적으로 미완료
        }

        public void Complete(Player player, List<Job> jobs)
        {
            if (IsCompleted) return;                                            //퀘스트 완료된 퀘스트면 처리X

            IsCompleted = true;                                                     //퀘스트 완료 상태로 변경


            if (!string.IsNullOrEmpty(RewardItem))                                     //아이템 보상 적용
            {
                player.AddItem(RewardItem);                                             // 플레이어 인벤토리에 추가
            }
        }

        public bool CheckCompletionCondition(Player player, List<Job> jobs)                 //퀘스트 완료 조건 확인
        {
            if (player.Level >= 5)                                                  // 예시: 플레이어 레벨이 5 이상이면 완료
            {
                Complete(player, jobs);                                             // 완료 처리 및 보상 지급
                return true;                                                        // 완료됨을 반환
            }

            return false;                                                           // 완료되지 않음
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
            string rewardItem = player.Job switch                                  // 플레이어 직업에 따른 아이템 보상 결정
            {
                "전사" => "롱소드",
                "마법사" => "증폭의 고서",
                "도적" => "단검",
                "궁수" => "롱소드",
                _ => "기본 아이템"
            };

            // 새로운 퀘스트 객체 생성 후 반환
            return new Quest(
                questName: "마을을 위협하는 미니언 처치",
                description: "마을 근처 미니언 5마리 처치",
                requirement: "미니언 5 처치",
                rewardGold: 500,
                rewardExp: 30,
                rewardPotion: 2,
                rewardItem: rewardItem
            );
        }

    }
}
