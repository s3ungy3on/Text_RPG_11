using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Text_RPG_11
{
    internal class Player
    {
        public string Name { get; set; }                                            //플레이어 이름       
        public int Level { get; set; }                                              //플레이어 레벨

        public string Job { get; set; }                                             //직업 이름

        public int Attack { get; set; }                                             //기본 공격력
        public float Defense { get; set; }                                            //기본 방어력
        public int HP { get; set; }                                                 //플레이어 현재 체력
        public int MP { get; set; }                                                //플레이어 현재 마나


        public int Gold { get; set; }                                               //소지 골드
        public int Exp { get; set; }                                                //경험치
        public int DefaultHP { get; set; }                                          // 기본 체력
        public int DefaultMP { get; set; }                                          //기본 마나
        public int Potions { get; set; } = 0;                                       //소지 포션 수량



        private int itemHP = 0;                                                             //장착 아이템으로 얻는 능력치
        private int itemMP = 0;
        private int itemAttack = 0;
        private float itemDefense = 0;

        public int MaxHP => DefaultHP + itemHP;                                             //최대 체력
        public int MaxMP => DefaultMP + itemMP;                                             //최대 마나
        public int MaxAttack => Attack + itemAttack;                                        //최종 공격력
        public float MaxDefense => Defense + itemDefense;                                     //최종 방어력


        private List<Items> inventory = new();                                           //인벤토리
        public IReadOnlyList<Items> Inventory => inventory.AsReadOnly();

        private List<Items> equippedItems = new();                                           //장착 아이템
        public IReadOnlyList<Items> EquippedItems => equippedItems.AsReadOnly();

        public bool HasEquippedItem => equippedItems.Count > 0;



        public Player(string name, int level, string job, int attack, int defense, int defaultHP, int defaultMP, int gold, int exp = 0)
        {
            Name = name;
            Level = level;
            Job = job;
            Attack = attack;
            Defense = defense;
            DefaultHP = defaultHP;                                                  //최대체력 기준값 저장
            DefaultMP = defaultMP;                                                  //최대 마나 기준값 저장
            HP = DefaultHP;                                                         //현재 체력 초기화
            MP = DefaultMP;                                                         //현재 마나 초기화
            Gold = gold;
            Exp = exp;
        }

        public void GainExp(int amount)
        {
            Exp += amount; // 경험치 추가
            int maxExp = Level * 20 + Level;

            // 경험치가 충분하면 레벨업 반복 가능
            while (Exp >= maxExp)
            {
                Exp -= maxExp;
                LevelUp();
                maxExp = Level * 20 + Level;                    // 레벨업 후 새로운 요구 경험치 갱신
            }
        }

        // 실제 레벨업 처리
        private void LevelUp()
        {
            Level++;
            DefaultHP += 10;                                                        //레벨업시 기본 체력 증가
            DefaultMP += 5;                                                         //레벨업시 기본 마나 증가
            Attack += 1;                                                            //레벨업시 기본 공격력 증가
            Defense += 0.5f;                                                        //레벨업시 기본 방어력 증가

            HP = MaxHP;                                                         //레벨업시 체력회복
            MP = MaxMP;                                                         //레벨업시 마나회복
        }
        public void AddItem(Items item)
        {
            if (item != null)
                inventory.Add(item);
        }

        // 아이템 장착
        public void EquipItem(Items item)
        {
            if (item == null || !inventory.Contains(item)) return;

            if (!equippedItems.Contains(item))
            {
                equippedItems.Add(item);
                item.IsEquipped = true;
                StatUpdate();
            }
        }

        // 아이템 해제
        public void UnequipItem(Items item)
        {
            if (item == null || !equippedItems.Contains(item)) return;

            equippedItems.Remove(item);
            item.IsEquipped = false;
            StatUpdate();
        }

        // 포션 사용
        public void UsePotion()
        {
            if (Potions > 0)
            {
                Potions--;
                HP += 50;
                if (HP > MaxHP) HP = MaxHP;
            }
        }

        // 장착 아이템 기반으로 스탯 계산
        public void StatUpdate()
        {
            itemHP = itemMP = itemAttack = 0;
            itemDefense = 0;

            foreach (var item in equippedItems)
            {
                if (item is Weapon w)
                {
                    itemAttack += w.AttackPower;
                    itemHP += w.ItemHp;
                    itemMP += w.ItemMp;
                }
                else if (item is Armor a)
                {
                    itemDefense += a.DefensePower;
                    itemHP += a.ItemHp;
                    itemMP += a.ItemMp;
                }
            }

            if (HP > MaxHP) HP = MaxHP;
            if (MP > MaxMP) MP = MaxMP;
        }
    } 
    
}

    internal class JobData
    {
        public List<Job> jobs { get; set; } = new List<Job>();                                      //직업 리스트
    }

    internal class Job                                                                                //직업 정보
    {
        public string name { get; set; } = "";                                                      //직업 이름
        public string description { get; set; } = "";                                               //직업 설명
        public BaseStats baseStats { get; set; } = new BaseStats();                             //직업 기본 능력치
    }

    internal class BaseStats                                      //직업 기본 능력치
    {
        public int hp { get; set; } = 0;                    //기본 체력
        public int mp { get; set; } = 0;                    //기본 마나
        public int attack { get; set; } = 0;                //기본 공격력
        public int defense { get; set; } = 0;               //기본 방어력
        public int criticalChance { get; set; } = 0;  // 치명타 확률
        public int dodgeChance { get; set; } = 0;     // 회피 확률
        public int gold { get; set; } = 0;              //시작골드
    }