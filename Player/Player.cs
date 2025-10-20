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
        public class Player
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

        //현재 던전 스테이지
        private int currentStage = 1;
        public int CurrentStage
        {
            get => currentStage;
            private set
            {
                currentStage = Math.Max(1, value); // 최소 1 이상 보장
            }
        }

        public void ClearDungeon()                                          //던전 클리어시  스테이지상승
        {
            CurrentStage++;
        }


        private int itemHP = 0;                                                             //장착 아이템으로 얻는 능력치
        private int itemMP = 0;
        private int itemAttack = 0;
        private float itemDefense = 0;

        public int MaxHP => DefaultHP + itemHP;                                             //최대 체력
        public int MaxMP => DefaultMP + itemMP;                                             //최대 마나
        public int MaxAttack => Attack + itemAttack;                                        //최종 공격력
        public float MaxDefense => Defense + itemDefense;                                     //최종 방어력


        private Inventory inventory; // 기존 코드에서 누락되어 있던 private 필드
        public Inventory Inventory => inventory ??= new Inventory();

        //장착 아이템 관리
        private readonly List<Items> equippedItems = new List<Items>();
        public IReadOnlyList<Items> EquippedItems => equippedItems.AsReadOnly();
        public bool HasEquippedItem => equippedItems.Count > 0;


        public Player(string name, int level, string job, int baseAttack, int baseDefense, int defaultHP, int defaultMP, int gold, int exp = 0)
        {
            Name = name;
            Level = Math.Max(1, level);
            Attack = baseAttack;
            Defense = baseDefense;
            DefaultHP = defaultHP;                                                  //최대체력 기준값 저장
            DefaultMP = defaultMP;                                                  //최대 마나 기준값 저장
            HP = DefaultHP;                                                         //현재 체력 초기화
            MP = DefaultMP;                                                         //현재 마나 초기화
            Gold = gold;
            Exp = exp;
        }


        public void GainExp(int amount)
        {
            if (amount <= 0) return;

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
            Inventory.Items.Add(item);
        }

        // 아이템 장착
        public void EquipItem(Items item)
        {
            if (item == null) return;
            if (!Inventory.Items.Contains(item)) return;                            // 인벤토리에 없는 아이템이면 X
            if (item.IsEquipped) return;                                        // 이미 장착 중이면 X

            equippedItems.Add(item);
            item.IsEquipped = true;
            StatUpdate();
        }

        // 아이템 해제
        public void UnequipItem(Items item)
        {
            if (item == null) return;
            if (!equippedItems.Contains(item)) return;

            equippedItems.Remove(item);
            item.IsEquipped = false;
            StatUpdate();
        }

        // 포션 사용
        public void UsePotion()
        {
            var potion = Inventory.Items.OfType<Potion>().FirstOrDefault();
            if (potion == null)
            {
                // 인벤토리에 포션 없음
                return;
            }


            // 포션 소비
            Inventory.Items.Remove(potion);


            // 치유량은 포션의 HealAmount 프로퍼티(가정)를 사용, 없다면 기본 50
            int heal = (potion?.HealAmount) ?? 50;
            HP += heal;
            if (HP > MaxHP) HP = MaxHP;
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