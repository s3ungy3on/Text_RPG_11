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
        public string Name { get; }                                            //플레이어 이름       
        public int Level { get; private set; }                                              //플레이어 레벨

        public int Attack { get; private set; }                                             //기본 공격력
        public float Defense { get; private set; }                                            //기본 방어력
        public int HP { get; private set; }                                                 //플레이어 현재 체력
        public int MP { get; private set; }                                                //플레이어 현재 마나


        public int Gold { get; private set; }                                               //소지 골드
        public int Exp { get; private set; }                                                //경험치
        public int DefaultHP { get; private set; }                                          // 기본 체력
        public int DefaultMP { get; private set; }                                          //기본 마나


        //현재 던전 스테이지
        private int currentStage = 1;
        public int CurrentStage => currentStage;
        public void ClearDungeon() => currentStage++;                                       //던전 클리어시 스테이지 상승


        private int itemHP = 0, itemMP = 0, itemAttack = 0;                                 //장착 아이템을 얻는 능력치
        private float itemDefense = 0;

        public int MaxHP => DefaultHP + itemHP;                                             //최대 체력
        public int MaxMP => DefaultMP + itemMP;                                             //최대 마나
        public int MaxAttack => Attack + itemAttack;                                        //최종 공격력
        public float MaxDefense => Defense + itemDefense;                                     //최종 방어력

        //인벤토리
        private readonly Inventory _inventory;                                      
        public Inventory Inventory => _inventory;


        //장착 아이템 관리
        private readonly List<Items> _equippedItems = new List<Items>();
        public IReadOnlyList<Items> EquippedItems => _equippedItems.AsReadOnly();
        public bool HasEquippedItem => _equippedItems.Count > 0;


        public Player(string name, int level, int baseAttack, int baseDefense, int defaultHP, int defaultMP, int gold, int exp = 0, Inventory inventory)
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

            _inventory = inventory;
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
        public void AddItem(Items item)                                         //아이템 추가
        {
            if (item != null)
            Inventory.Items.Add(item);
        }

        // 아이템 장착
        public void EquipItem(Items item)
        {
            if (item == null || !Inventory.Items.Contains(item) || item.IsEquipped) return;          // 이미 장착 중이면 X

            _equippedItems.Add(item);
            item.IsEquipped = true;
            StatUpdate();
        }

        // 아이템 해제
        public void UnequipItem(Items item)
        {
            if (item == null || !_equippedItems.Contains(item)) return;

            _equippedItems.Remove(item);
            item.IsEquipped = false;
            StatUpdate();
        }

        // 포션 사용
        public void UsePotion()
        {
            var potion = Inventory.Items.OfType<Potion>().FirstOrDefault(p => p.PotionCount > 0);

            if (potion == null) return; // 포션 없으면 종료

            potion.PotionCount--;
            HP += potion.HealPower;
            if (HP > MaxHP) HP = MaxHP;

            // 포션 개수가 0이면 인벤토리에서 제거
            if (potion.PotionCount <= 0)
                Inventory.Items.Remove(potion);
        }

        // 장착 아이템 기반으로 스탯 계산
        public void StatUpdate()
        {
            itemHP = itemMP = itemAttack = 0;
            itemDefense = 0;

            foreach (var item in _equippedItems.Where(i => i.IsEquipped))
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