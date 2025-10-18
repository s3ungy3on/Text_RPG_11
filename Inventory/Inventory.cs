using System;
using System.Collections.Generic;
using System.Linq;

namespace Text_RPG_11
{
    public class Inventory
    {
        private GameManager gameManager;
        public List<Items> Items { get; private set; }

        public Inventory(GameManager manager)
        {
            gameManager = manager;
            Items = new List<Items>();
        }

        #region 아이템 추가
        public void AddItem(Items item)
        {
            if(item == null) //입력한 값이 null 이라면 return
            {
                return;
            }

            if(item is Potion newPotion) //입력한 값이 포션인지 체크
            {
                Potion existing = null; //기존 포션을 넣을 변수. 기본값은 null

                foreach (var i in Items) //Items 리스트 순회, i는 리스트의 각 항목
                {
                    if(i is Potion p && p.ItemId == newPotion.ItemId) //i가 포션일경우 p에 저장, p의 id와 입력한 값의 id가 같다면
                    {
                        existing = p; //existing에 p를 저장
                        break; //찾았으니 순회 중단
                    }
                }

                if(existing != null) //existing 이 null 이 아니라면
                {
                    existing.PotionCount += newPotion.PotionCount; //기존 포션 개수에 새 포션의 수량을 더한다.
                    return; //합쳤으니 메소드 종료
                }
                else
                {
                    newPotion.IsPurchased = true; //existing이 null이라면 새 포션을 구매 상태로 변경
                }
            }

            item.IsPurchased = true;
            Items.Add(item); //포션이 아니라면 리스트에 새 아이템 추가
        }
        #endregion

        #region 아이템 제거
        public bool RemoveItem(Items item, int quantity = 1)
        {
            if(item == null)
            {
                return false;
            }

            if (item.IsEquipped)
            {
                return false;
            }

            if(item is Potion p)
            {
                if (p.PotionCount > quantity)
                {
                    p.PotionCount -= quantity;
                    return true;
                }
                else
                {
                    return Items.Remove(p);
                }
            }

            return Items.Remove(item); //리스트에서 해당 아이템 제거
        }
        #endregion

        #region 타입 별 필터링
        public List<Weapon> GetWeapons()
        {
            List<Weapon> weapons = new List<Weapon>();

            foreach (var item in Items)
            {
                if (item is Weapon weapon)
                {
                    weapons.Add(weapon);
                }
            }

            return weapons;
        }

        public List<Armor> GetArmors()
        {
            List<Armor> armors = new List<Armor>();

            foreach (var item in Items)
            {
                if( item is Armor armor)
                {
                    armors.Add(armor);
                }
            }

            return armors;
        }

        public List<Potion> GetPotions()
        {
            List<Potion> potions = new List<Potion>();

            foreach (var item in Items)
            {
                if(item is Potion potion)
                {
                    potions.Add(potion);
                }
            }

            return potions;
        }
        #endregion

        #region 아이템 정렬
        public void SortItems()
        {
            Items = Items.OrderBy(i => i.ItemType())
                .ThenBy(i => i.Name)
                .ToList();
        }
        #endregion
    }
}