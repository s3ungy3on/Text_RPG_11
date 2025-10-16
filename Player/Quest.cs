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
       public string Desctipyion { get; }                 //퀘스트 설명
       public bool IsCompleted { get; private set; }         //달성 조건
       public int RewardGold { get; }                        //보상 골드
       public int RewardExp { get; }                        //보상 경험치
       public string Requirement { get; }                 //

        public Quest(string questName, string description, int rewardGold, int rewardExp)
        {
            QuestName = questName;
            Description = description;
            Requirement = requirement;
            RewardGold = rewardGold;
            RewardExp = rewardExp;
            IsCompleted = false; // 기본은 미완료
        }

        public void Complete()
        {
            IsCompleted = true;
        }

        public bool CheckCompletionCondition(Player player)
        {
            if (player.Level >= 5)
            {
                Complete();
                return true;
            }

            return false;
        }
    }

    internal static class QuestData
    {
        public static List<Quest> GetDefaultQuests()
        {
            return new List<Quest>
        {
            new Quest(
                "마을을 위협하는 미니언 처치",
                "마을 주변에 나타난 미니언을 처치하자.",
                "미니언 1마리 처치",
                100,
                50
            ),
            new Quest(
                "장비를 장착해보자",
                "획득한 장비를 직접 착용해보자.",
                "아이템 1개 장착",
                150,
                70
            ),
            new Quest(
                "더욱 더 강해지기!",
                "수련을 통해 강해지자.",
                "플레이어 레벨 5 달성",
                200,
                100
            )
        };

        }
