using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    public class Job
    {
        public string Name { get; }
        public string DisplayName { get; }
        public string Description { get; }
        public int BaseHp { get; }
        public int BaseMp { get; }
        public int BaseAttack { get; }
        public int BaseDefense { get; }
        public int BaseCriticalChance { get; }
        public int BaseDodgeChance { get; }
        public int BaseGold { get; }

        public Job(string name, string displayName, string description, int hp, int mp, int attack, int defense, int criticalChance, int dodgeChance, int gold)
        {
            Name = name;
            DisplayName = displayName;
            Description = description;
            BaseHp = hp;
            BaseMp = mp;
            BaseAttack = attack;
            BaseDefense = defense;
            BaseCriticalChance = criticalChance;
            BaseDodgeChance = dodgeChance;
            BaseGold = gold;
        }

        // 직업 정보 출력
        public string GetJobInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"직업: {DisplayName} ({Name})");
            sb.AppendLine($"설명: {Description}");
            sb.AppendLine($"\n[기본 스탯]");
            sb.AppendLine($"체력: {BaseHp}");
            sb.AppendLine($"마나: {BaseMp}");
            sb.AppendLine($"공격력: {BaseAttack}");
            sb.AppendLine($"방어력: {BaseDefense}");
            sb.AppendLine($"치명타 확률: {BaseCriticalChance}%");
            sb.AppendLine($"회피 확률: {BaseDodgeChance}%");
            sb.AppendLine($"시작 골드: {BaseGold}");
            return sb.ToString();
        }

        // 간단한 직업 요약 정보
        public string GetJobSummary()
        {
            return $"{DisplayName} ({Name}) - {Description}";
        }
    }
}