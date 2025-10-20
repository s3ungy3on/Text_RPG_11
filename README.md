# 🧙‍♂️ Text RPG 11

![게임 실행 장면](./play.gif)

---

## 🎮 프로젝트 소개

**Text_RPG_11**은 콘솔 환경에서 동작하는 **C# 텍스트 기반 RPG 게임**입니다.  
플레이어는 챔피언을 선택하고, 탐험을 통해 몬스터를 처치하며  
경험치와 골드를 획득해 성장해 나갑니다.  

- 무기, 방어구, 포션 시스템  
- 상점 구매/판매  
- 인벤토리 장착 및 해제  
- 직업 선택 및 레벨업  
- 전투, 탐험, 퀘스트 등 다양한 상호작용

---

## ⚙️ 실행 방법

1️. **Visual Studio 또는 dotnet CLI 환경이 설치되어 있어야 합니다.**

git clone https://github.com/s3ungy3on/Text_RPG_11.git
cd Text_RPG_11
dotnet run
2️. 콘솔이 실행되면 다음 단계로 게임이 시작됩니다.

이름 입력 → 챔피언 선택 → 메인 화면 진입

---

## 🧩 주요 기능

🧑‍🎤 플레이어(Player)
직업(Job) 선택에 따라 기본 스탯 결정

레벨업 시 능력치 상승 및 체력/마나 회복

무기·방어구 장착으로 능력치 강화

포션 사용 시 HP 회복

⚔️ 전투(Battle)
랜덤 몬스터 등장

일반 공격 / 스킬 공격 / 포션 사용 가능

HP, MP 실시간 표시 및 전투 로그 지원

승리 시 경험치 및 골드 획득

🏪 상점(Shop)
아이템 구매 및 판매 가능

구매 시 플레이어 골드 차감

판매 시 100% 가격으로 환급

희귀도에 따른 색상 구분 (일반~초월)

희귀도	색상
common	⚪ White
rare	🔵 Blue
epic	🟣 Magenta
legend	🟡 Yellow
myth	🩵 Cyan
transcended	🔴 Red

🎒 인벤토리(Inventory)
보유 아이템 목록 확인

장착/해제 표시 [E] 자동 출력

아이템 등급별 색상 출력

---

## 🗂️ 프로젝트 구조
Text_RPG_11/
│
├── Program.cs            # 게임 실행 진입점
├── GameManager.cs        # 전반적인 게임 흐름 관리
├── UiManager.cs          # UI 및 콘솔 출력 담당
├── Player.cs             # 플레이어 상태 및 능력치 관리
├── Inventory.cs          # 인벤토리 및 장비/포션 관리
├── Shop.cs               # 상점 로직
├── Items.cs              # 아이템, 무기, 방어구, 포션 정의
├── Monster.cs            # 몬스터 데이터
├── Battle.cs             # 전투 시스템
├── Skill.cs              # 스킬 시스템
├── Quest.cs              # 퀘스트 시스템
│
├── Database/
│   ├── ItemDatabase.cs
│   ├── JobDatabase.cs
│   ├── RewardDatabase.cs
│   ├── items.json
│   ├── jobs.json
│   └── rewards.json
│
└── play.gif              # 🎬 실행 영상 (README 표시용)
🎬 실행 영상
아래 GIF는 실제 게임 실행 장면입니다 👇
(콘솔 환경에서 자동으로 텍스트가 출력됩니다.)
![게임 실행 장면](./play.gif)


💻 개발 환경
언어: C#

플랫폼: .NET 8.0

IDE: Visual Studio 2022

운영체제: Windows 10 이상

🧠 개발자 팁
콘솔 전체화면은 Console.SetWindowSize(240, 68); 로 설정 가능 (1920*1080 해상도 기준)

JSON 데이터(items.json, jobs.json, rewards.json)는 게임 내 데이터베이스로 사용

인벤토리, 상점, 전투 등의 UI 모두 UIManager.cs에서 관리

📜 라이선스
이 프로젝트는 자유롭게 수정 및 사용이 가능합니다.
단, 상업적 이용 시 원저작자 표기를 권장합니다.

👥 팀 정보
이름	역할
🧙 김경찬(팀장) / UI 및 시나리오
👩‍💻‍ 이연우 / Dungeon, Battle
🧑‍💻 조현일 / Monster, Skill
🧑‍💻 백성현 / Quest, Player
👩‍💻‍ 임승연 / Item, Inventory, Shop

🚀 넣을려 했으나 능력 부족으로 실패한 부분
아이템 조합하기

던전 난이도 및 보상 조정

저장/로드 기능 구현

퀘스트 다양화