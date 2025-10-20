using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    public class RewardManager
    {
        private GameManager gameManager;
        private Random random;

        public RewardManager(GameManager manager)
        {
            gameManager = manager;
            random = new Random();
        }

        #region 던전 보상
        //public Items GetDungeonReward(int stage)
        //{

        //}

        //private RewardGroup SelectRewardGroup(List<RewardGroup> groups) //보상 그룹 뽑기
        //{

        //}

        //private DungeonRewardItem SelectRewardItem(List<DungeonRewardItem> items) //그룹 내 아이템 뽑기
        //{

        //}
        //#endregion

        #region 퀘스트 보상
        public void QuestReward(int questId)
        {

        }
        #endregion
    }
}
#endregion