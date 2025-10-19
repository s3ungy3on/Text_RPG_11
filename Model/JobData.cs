using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    public class JobDataContainer
    {
        public List<JobInfo> jobs { get; set; } //JobInfo 리스트
    }

    public class JobInfo //직업 정보
    {
        public string name { get; set; } = ""; //직업 이름
        public string displayName { get; set; } //직업 선택 시 화면에 챔피언 이름으로
        public string description { get; set; } = ""; //직업 설명
        public BaseStats baseStats { get; set; } = new BaseStats(); //직업 기본 능력치
    }

    public class BaseStats //직업 기본 능력치
    {
        public int hp { get; set; } //기본 체력
        public int mp { get; set; } //기본 마나
        public int attack { get; set; } //기본 공격력
        public int defense { get; set; } //기본 방어력
        public int criticalChance { get; set; } // 치명타 확률
        public int dodgeChance { get; set; } // 회피 확률
        public int gold { get; set; } //시작골드
    }
}
