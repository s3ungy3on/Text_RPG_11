using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections.Generic;

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


        private List<string> inventory = new List<string>();   // 인벤토리
        public IReadOnlyList<string> Inventory => inventory.AsReadOnly(); // 외부에서 읽기 전용

        public void AddItem(string itemName)
        {
            if (!string.IsNullOrEmpty(itemName))
            {
                inventory.Add(itemName);
            }
        }



        public Player(string name, int level, string job, int attack, int defense, int defaultHP, int defaultMP, int gold, int exp = 0)
        {
            Name = name;
            Level = level;
            Job = job;
            Attack = attack;
            Defense = defense;
            HP = DefaultHP;                                                         //생성시 현재 체력 = 최대 체력
            MP = DefaultMP;                                                         //생성시 현재 마나 = 최대 마나
            Gold = gold;
            Exp = exp;
        }

        public void StatUpdate(GameManager gameManager)
        {
            itemHP = 0;                                                                         //아이템 스텟 초기화
            itemMP = 0;
            itemAttack = 0;
            itemDefense = 0;

            if (gameManager.GameItems == null)                                                   //장착 아이템 없으면 종료
            {
                return;
            }

            foreach (var item in gameManager.GameItems)
            {
                if (item != null && item.IsEquipped)                                             //아이템 장착 떄만 적용
                {
                    if (item is Weapon weapon)
                    {
                        itemAttack += weapon.AttackPower;                                   //무기 장착시 공격력 증가
                        itemHP += weapon.ItemHp;                                            //무기 장착시 체력 증가
                        itemMP += weapon.ItemMp;                                            //무기 장착시 마나 증가


                    }
                    else if (item is Armor armor)
                    {
                        itemDefense += armor.DefensePower;                                      //방어력 증가
                        itemHP += armor.ItemHp;                                                 //방어구 장착시 체력 증가
                        itemMP += armor.ItemMp;                                                 //방어구 장착시 마나 증가


                    }
                }
            }

            if (HP > MaxHP)                                              //HP/MP가 최대치보다 높으면 최대치로 세팅
            {
                HP = MaxHP;
            }

            if (MP > MaxMP)
            {
                MP = MaxMP;
            }

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

        public bool HasEquippedItem { get; set; } = false;

        // 게임매니저에서 장비 착용 상태 업데이트 후 HasEquippedItem 갱신 가능
        public void UpdateEquippedStatus(GameManager gameManager)
        {
            if (gameManager.GameItems == null)
            {
                HasEquippedItem = false;
                return;
            }

            HasEquippedItem = gameManager.GameItems.Exists(i => i != null && i.IsEquipped);
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
}
