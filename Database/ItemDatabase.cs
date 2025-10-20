using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    /// <summary>
    /// 아이템 데이터베이스: items.json 파일을 로드하고 관리하는 정적 클래스
    /// - 게임 시작 시 한 번만 JSON 파일을 읽어 캐싱
    /// - 아이템 ID나 이름으로 아이템 객체 생성
    /// - 상점, 던전 등 특정 용도의 아이템 목록 제공
    /// </summary>
    public static class ItemDatabase
    {
        #region 필드

        /// <summary>
        /// items.json 파일의 경로
        /// </summary>
        private static string itemDataPath = "Data/items.json";

        /// <summary>
        /// JSON에서 로드한 아이템 데이터를 캐싱하는 컨테이너
        /// 게임 실행 중 한 번만 로드되고 재사용됨
        /// </summary>
        private static ItemDataContainer cachedItems;

        /// <summary>
        /// 아이템 원본 데이터를 ID별로 보관하는 딕셔너리 (현재 미사용)
        /// 추후 원본 데이터 참조가 필요할 경우를 대비한 예약 필드
        /// </summary>
        private static Dictionary<int, ItemDataBase> itemOriginals = new Dictionary<int, ItemDataBase>();

        /// <summary>
        /// 포션 원본 데이터를 ID별로 보관하는 딕셔너리 (현재 미사용)
        /// 추후 원본 데이터 참조가 필요할 경우를 대비한 예약 필드
        /// </summary>
        private static Dictionary<int, PotionData> potionOriginals = new Dictionary<int, PotionData>();

        #endregion

        #region 생성자

        /// <summary>
        /// 정적 생성자: ItemDatabase 클래스가 처음 사용될 때 자동으로 호출됨
        /// items.json 파일을 로드하여 cachedItems에 저장
        /// </summary>
        static ItemDatabase()
        {
            cachedItems = LoadItems();
        }

        #endregion

        #region JSON 로드 및 초기화

        /// <summary>
        /// items.json 파일을 읽어서 ItemDataContainer 객체로 변환
        /// </summary>
        /// <returns>로드된 아이템 데이터 컨테이너 (실패 시 빈 컨테이너)</returns>
        private static ItemDataContainer LoadItems()
        {
            try
            {
                // 1. 파일 존재 여부 확인
                if (!File.Exists(itemDataPath))
                {
                    Console.WriteLine($"items.json을 찾을 수 없습니다: {itemDataPath}");
                    return GetDefaultContainer();
                }

                // 2. JSON 파일 읽기
                string json = File.ReadAllText(itemDataPath);

                // 3. JSON 문자열을 C# 객체로 역직렬화 (Newtonsoft.Json 사용)
                var container = JsonConvert.DeserializeObject<ItemDataContainer>(json);

                // 4. 데이터 유효성 검증
                if (container == null || container.items == null)
                {
                    Console.WriteLine("아이템 데이터 파싱 실패");
                    return GetDefaultContainer();
                }

                // 5. 로드 성공 메시지 출력
                Console.WriteLine($"아이템 로드 완료: {container.items.Count}개");
                return container;
            }
            catch (Exception ex)
            {
                // 예외 발생 시 오류 메시지 출력 후 빈 컨테이너 반환
                Console.WriteLine($"아이템 로드 오류: {ex.Message}");
                return GetDefaultContainer();
            }
        }

        /// <summary>
        /// 빈 ItemDataContainer 생성
        /// JSON 로드 실패 시 기본값으로 사용하여 게임 중단 방지
        /// </summary>
        /// <returns>빈 리스트와 딕셔너리로 초기화된 컨테이너</returns>
        private static ItemDataContainer GetDefaultContainer()
        {
            return new ItemDataContainer
            {
                items = new List<ItemDataBase>(),           // 빈 아이템 리스트
                potions = new List<PotionData>(),           // 빈 포션 리스트
                rarityInfo = new Dictionary<string, RarityInfo>()  // 빈 등급 정보
            };
        }

        #endregion

        #region 아이템 조회 메소드

        /// <summary>
        /// 아이템 ID로 아이템 객체 생성
        /// - 무기/방어구/포션 모두 검색 가능
        /// - 매번 새로운 인스턴스를 생성하여 반환
        /// </summary>
        /// <param name="itemId">검색할 아이템의 고유 ID</param>
        /// <returns>생성된 아이템 객체 (없으면 null)</returns>
        public static Items GetItemById(int itemId)
        {
            // 1. 일반 아이템(무기/방어구)에서 검색
            ItemDataBase itemData = null;
            foreach (var item in cachedItems.items)
            {
                if (item.id == itemId)
                {
                    itemData = item;
                    break;  // 찾았으면 반복 중단 (성능 최적화)
                }
            }

            // 일반 아이템을 찾았으면 객체 생성 후 반환
            if (itemData != null)
            {
                return CreateItemFromData(itemData);
            }

            // 2. 포션에서 검색
            PotionData potionData = null;
            foreach (var potion in cachedItems.potions)
            {
                if (potion.id == itemId)
                {
                    potionData = potion;
                    break;
                }
            }

            // 포션을 찾았으면 객체 생성 후 반환
            if (potionData != null)
            {
                return CreatePotionFromData(potionData);
            }

            // 3. 아무것도 못 찾으면 null 반환
            return null;
        }

        /// <summary>
        /// 아이템 이름으로 아이템 객체 생성
        /// - 주로 퀘스트 보상 등에서 이름으로 아이템을 지급할 때 사용
        /// - 대소문자를 구분하며, 정확히 일치하는 이름만 검색됨
        /// </summary>
        /// <param name="name">검색할 아이템의 이름</param>
        /// <returns>생성된 아이템 객체 (없으면 null)</returns>
        public static Items GetItemByName(string name)
        {
            // 1. 일반 아이템(무기/방어구)에서 이름으로 검색
            ItemDataBase itemData = null;
            foreach (var item in cachedItems.items)
            {
                if (item.name == name)
                {
                    itemData = item;
                    break;
                }
            }

            if (itemData != null)
            {
                return CreateItemFromData(itemData);
            }

            // 2. 포션에서 이름으로 검색
            PotionData potionData = null;
            foreach (var potion in cachedItems.potions)
            {
                if (potion.name == name)
                {
                    potionData = potion;
                    break;
                }
            }

            if (potionData != null)
            {
                return CreatePotionFromData(potionData);
            }

            return null;
        }

        /// <summary>
        /// 상점에서 판매할 아이템 목록 생성
        /// - obtainMethods에 "shop"이 포함된 아이템만 반환
        /// - 무기, 방어구, 포션 모두 포함
        /// - 재료 아이템은 제외
        /// </summary>
        /// <returns>상점 판매 아이템 리스트</returns>
        public static List<Items> GetShopItems()
        {
            List<Items> shopItems = new List<Items>();

            // 1. 무기/방어구 중 상점 판매 아이템 추가
            foreach (var item in cachedItems.items)
            {
                // 조건 1: obtainMethods가 null이 아님
                // 조건 2: obtainMethods에 "shop" 포함
                // 조건 3: 타입이 "무기" 또는 "방어구"
                if (item.obtainMethods != null &&
                   item.obtainMethods.Contains("shop") &&
                   (item.type == "무기" || item.type == "방어구"))
                {
                    shopItems.Add(CreateItemFromData(item));
                }
            }

            // 2. 포션 중 상점 판매 아이템 추가
            foreach (var potion in cachedItems.potions)
            {
                if (potion.obtainMethods != null &&
                   potion.obtainMethods.Contains("shop"))
                {
                    shopItems.Add(CreatePotionFromData(potion));
                }
            }

            return shopItems;
        }

        /// <summary>
        /// 던전에서 드랍될 수 있는 아이템 목록 생성
        /// - obtainMethods에 "dungeon"이 포함된 아이템만 반환
        /// - 실제 드랍 확률은 RewardManager에서 처리
        /// </summary>
        /// <returns>던전 드랍 가능 아이템 리스트</returns>
        public static List<Items> GetDungeonItems()
        {
            List<Items> dungeonItems = new List<Items>();

            // 1. 무기/방어구 중 던전 드랍 아이템
            foreach (var item in cachedItems.items)
            {
                if (item.obtainMethods != null &&
                   item.obtainMethods.Contains("dungeon") &&
                   (item.type == "무기" || item.type == "방어구"))
                {
                    dungeonItems.Add(CreateItemFromData(item));
                }
            }

            // 2. 포션 중 던전 드랍 아이템
            foreach (var potion in cachedItems.potions)
            {
                if (potion.obtainMethods != null &&
                   potion.obtainMethods.Contains("dungeon"))
                {
                    dungeonItems.Add(CreatePotionFromData(potion));
                }
            }

            return dungeonItems;
        }

        /// <summary>
        /// 특정 등급의 아이템 목록 생성
        /// - UI에서 등급별 필터링할 때 사용
        /// </summary>
        /// <param name="rarity">검색할 등급 (예: "common", "rare", "epic" 등)</param>
        /// <returns>해당 등급의 아이템 리스트</returns>
        public static List<Items> GetItemsByRarity(string rarity)
        {
            List<Items> items = new List<Items>();

            // 1. 무기/방어구에서 해당 등급 검색
            foreach (var item in cachedItems.items)
            {
                if (item.rarity == rarity &&
                   (item.type == "무기" || item.type == "방어구"))
                {
                    items.Add(CreateItemFromData(item));
                }
            }

            // 2. 포션에서 해당 등급 검색
            foreach (var potion in cachedItems.potions)
            {
                if (potion.rarity == rarity)
                {
                    items.Add(CreatePotionFromData(potion));
                }
            }

            return items;
        }

        #endregion

        #region 아이템 객체 생성

        /// <summary>
        /// ItemDataBase(JSON 데이터)를 실제 게임에서 사용하는 Items 객체로 변환
        /// - 타입에 따라 Weapon 또는 Armor 객체 생성
        /// - 재료 아이템은 무시 (현재 미사용)
        /// </summary>
        /// <param name="data">JSON에서 로드된 아이템 데이터</param>
        /// <returns>생성된 Weapon 또는 Armor 객체 (재료면 null)</returns>
        private static Items CreateItemFromData(ItemDataBase data)
        {
            switch (data.type)
            {
                case "무기":
                    // Weapon 객체 생성
                    return new Weapon(
                        name: data.name,                    // 아이템 이름
                        attackPower: data.attackPower,      // 공격력
                        defensePower: data.defensePower,    // 방어력
                        price: data.price,                  // 가격
                        itemHp: data.itemHp,                // 체력 보너스
                        itemMp: data.itemMp)                // 마나 보너스
                    {
                        // 객체 초기화 후 추가 속성 설정
                        ItemId = data.id,                   // 아이템 ID
                        EquipableJobs = data.equipJob,      // 장착 가능 직업 목록
                        Rarity = data.rarity                // 아이템 등급
                    };

                case "방어구":
                    // Armor 객체 생성 (구조는 Weapon과 동일)
                    return new Armor(
                        name: data.name,
                        attackPower: data.attackPower,
                        defensePower: data.defensePower,
                        price: data.price,
                        itemHp: data.itemHp,
                        itemMp: data.itemMp)
                    {
                        ItemId = data.id,
                        EquipableJobs = data.equipJob,
                        Rarity = data.rarity
                    };

                // 재료 아이템은 현재 게임에서 사용하지 않으므로 null 반환
                default:
                    return null;
            }
        }

        /// <summary>
        /// PotionData(JSON 데이터)를 Potion 객체로 변환
        /// - 항상 개수 1로 생성되며, 인벤토리에 추가될 때 스택됨
        /// </summary>
        /// <param name="data">JSON에서 로드된 포션 데이터</param>
        /// <returns>생성된 Potion 객체</returns>
        private static Potion CreatePotionFromData(PotionData data)
        {
            return new Potion(
                name: data.name,                // 포션 이름
                healPower: data.healPower,      // 회복량
                price: data.price,              // 가격
                potionCount: 1)                 // 기본 개수 1 (인벤토리에서 자동으로 합쳐짐)
            {
                ItemId = data.id,               // 포션 ID
                Rarity = data.rarity            // 포션 등급
            };
        }

        #endregion

        #region 기타 데이터 조회

        /// <summary>
        /// 아이템 등급별 정보 반환
        /// - UI에서 등급별 색상, 표시명 등을 가져올 때 사용
        /// - "common", "rare", "epic" 등의 키로 접근
        /// </summary>
        /// <returns>등급 정보 딕셔너리 (등급명 → RarityInfo)</returns>
        public static Dictionary<string, RarityInfo> GetRarityInfo()
        {
            return cachedItems.rarityInfo;
        }

        #endregion
    }
}