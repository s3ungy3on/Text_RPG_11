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
        public bool RemoveItem(Items item, int quantity = 1) //포션의 경우 개수가 여러개기에, 매개변수로 개수 = 1 선언
        {
            if(item == null) //아이템이 null 이라면 false 반환해서 메서드 종료
            {
                return false;
            }

            if(item is Potion p) //아이템이 포션이라면 p로 저장
            {
                if (p.PotionCount > quantity) //포션의 카운트가 1개 이상이라면
                {
                    p.PotionCount -= quantity; //포션 카운트 - quantity(1개)
                    return true; //true 반환해서 메서드 종료
                }
                else
                {
                    return Items.Remove(p); //포션의 개수가 0개라면 리스트에서 포션을 지우고 메서드 종료
                }
            }

            return Items.Remove(item); //리스트에서 해당 아이템 제거
        }
        #endregion

        #region 타입 별 필터링
        public List<Weapon> GetWeapons() //무기 타입만 반환하는 메서드
        {
            List<Weapon> weapons = new List<Weapon>(); //반환할 무기를 저장할 리스트 생성

            foreach (var item in Items) //인벤토리 내 모든 아이템 순회
            {
                if (item is Weapon weapon) //아이템이 Weapon 타입이라면 weapon으로 캐스팅
                {
                    weapons.Add(weapon); //해당 아이템 weapon리스트에 추가
                }
            }

            return weapons; //리스트 반환
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
            Items = Items.OrderBy(i => i.ItemType()) //아이템 타입 기준으로 정렬
                .ThenBy(i => i.Name) //같은 타입 내에선 이름순으로 정렬
                .ToList(); //정렬 한 걸 리스트로 다시 저장
        }
        #endregion
    }
}