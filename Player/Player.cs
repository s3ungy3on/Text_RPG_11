using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Text_RPG_11
{
    internal class Player
    {
        public string Name { get; set; }                                         //플레이어이름
        public int Level { get; set; }                                          //플레이어 레벨
        public string Job { get; set; }                                         //플레이어 직업
        public int Attack { get; set; }                                         //플레이어 공격력
        public int Defense { get; set; }                                        //플레이어 방어력
        public int HP { get; set; }                                            //현재 플레이어 체력
        public int MP { get; set; }                                             //현재 플레이어마나
        public int Gold { get; set; }                                           //플레이어 골드
        public int Exp { get; set; }                                            //플레이어 경험치
        public int BaseHP { get; private set; }                                 //기본 체력
        public int BaseMP { get; private set; }                                 //기본 마나


        private int itemHP = 0;                                                 //장비로 추가된 체력
        private int itemMP = 0;                                                 //장비로 추가된 마나
        public int itemAttack = 0;                                              //장비로 추가된 공격력
        public int itemDefense = 0;                                             //장비로 추가된 방어력

        public int MaxHP { get { return BaseHP  + itemHP; } }                                //최대체력
        public int MaxMP { get { return BaseMP + itemMP; } }                                //최대 마나
        public int MaxAttack { get { return Attack + itemAttack; } }                   //최종 공격력
        public int MaxDefense { get { return Defense + itemDefense; } }                 //최종 방어력

        // 플레이어 기본 스탯
        public Player(string name, int level, string job, int attack, int defense, int hp, int gold, int exp = 0, int mp = 50)
        {
            Name = name;
            Level = level;
            Job = job;
            Attack = attack;
            Defense = defense;
            BaseHP = hp;
            HP = BaseHP;
            Gold = gold;
            Exp = exp;
            MP = mp;
        }

        public void StatUpdate(GameManager gameManager)                                 //장비 장착에 따른 스탯 추가
        {
            itemHP = 0;                                                                 //아이템 능력치 초기화
            itemMP = 0;                                                                 //아이템 능력치 초기화
            itemAttack = 0;                                                             //아이템 능력치 초기화
            itemDefense = 0;                                                            //아이템 능력치 초기화

            if (gameManager.GameItems == null)                                 // 게임 매니저에서 아이템 정보가 없으면 아무것도 하지 않음
            {
                return;
            }

            foreach (var item in gameManager.GameItems)                        // 게임 매니저의 아이템 목록을 하나씩 확인
            {
                if(item != null && item.IsEquipped)                          // item이 존재하고, 장착이 되어 있다면만 실행 
                {
                    if (item is Weapon weapon)                                  // 무기(Weapon)인 경우
                    {
                        itemAttack += weapon.AttackPower;                   // 무기 공격력 추가

                        if (weapon.ItemHp > 0)                              //무기 장착시 체력 증가
                            itemHP += weapon.ItemHp;

                        if (weapon.ItemMp > 0)                              //무기 장착시 마나 증가
                            itemMP += weapon.ItemMp;
                        

                    }
                    else if (item is Armor armor)                               // 방어구(Armor)인 경우
                    {
                        itemDefense += armor.DefensePower;                      // 방어구 방어력 추가

                        if (armor.ItemHp > 0)                                       //방어구 장착시 체력 증가
                        
                            itemHP += armor.ItemHp;

                        if (armor.ItemMp > 0)                                       //방어구 장착시 마나 증가
                            itemMP += armor.ItemMp;

                        
                    } 
                }
            }
        }
    }

    public class JobBaseStats                                                       //직업 기본 능력치
    {
        public int hp { get; set; }                                                 //체력
        public int mp { get; set; }                                                //마나
        public int attack { get; set; }                                             //공격력
        public int defense { get; set; }                                            //방어력
        public int criticalChance { get; set; }                                     //치명타 확률
        public int dodgeChance { get; set; }                                        //회피 확률
        public int gold { get; set; }                                               //시작 골드
    }

    public class Job
    {
        public string name { get; set; }                                        //직업 이름
        public string description { get; set; }                                 //직업 설명
        public JobBaseStats baseStats { get; set; }                             //해당 직업 기본 스탯
    }

    public class JobData
    {
        public List<Job> jobs { get; set; }                                     //직업 리스트
    }
}
