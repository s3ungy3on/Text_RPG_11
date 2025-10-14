namespace Text_RPG_11;

internal class Battle
{
    public int stage = 1;
    
    // 배틀 시작 후 몬스터 랜덤 등장
            
    // 0. 몬스터 담을 리스트 생성(이후에 추가)
    public List<Monster> monsters = new List<Monster>();
    public List<Monster> enemies = new List<Monster>();
    
    private GameManager _gameManager;
    
    public Battle(GameManager manager)
    {
        _gameManager = manager;
    }

    public void MonsterSpawn()
    {
        // 1. 몬스터 수 생성
        Random random = new Random();
        int spawnNum = random.Next(1, 5);
        
        // 2. 등장할 몬스터 랜덤 선택
        for (int i = 0; i < spawnNum - 1; i++)
        {
            enemies.Add(monsters[random.Next(0, monsters.Count)]);
        }
            
        // 3. 랜덤 선택된 몬스터 출력
        Console.WriteLine("Battle!!");
            
        foreach (var enemy in enemies)
        {
            Console.WriteLine($"Lv. {enemy.Level} {enemy.Name}  HP {enemy.Hp}");
        }
    }
    
    public void WinCheck()
    {
        if (enemies.All(m => m.Hp == 0) && _gameManager.Player.HP > 0)
        {
            Console.WriteLine("승리");
        }
        else if (_gameManager.Player.HP <= 0)
        {
            Console.WriteLine("패배");
        }
        
    }
    
    public void Attack()
    {
        // 사용자가 에너미를 공격
        // 10퍼센트 확률로 160% 딜
    }

    public void UserSkill()
    {
        // 스킬을 사용해 에너미 공격
    }

    public void EnemyTurn()
    {
        // Enemy가 사용자 공격
        // 10퍼센트 확률로 적중하지 않을 수 있음
    }
    
    public void RewardShow()
    {
        // 체력이 0이 된 몬스터의 reward를 화면에 띄움
        // 해당 reward를 플레이어에게 할당(reward 메서드 사용)
    }
    
    public void LevelUp()
    {
        // 전투 클리어 후 사냥한 몬스터만큼 경험치 증가
        // 몬스터를 순회하면서 체력이 0이 된 몬스터가 제공하는 exp만큼 플레이어의 경험치를 상승
    }
    
    // public void UserPotion()
    // {
    //     // 유저 포션 사용 기능
    //
    //     // 프로퍼티 접근 체인 수정 필요
    //     if (_gameManager.inventory.Potion.Count > 0)
    //     {
    //         GameManager.Player.Hp += GameManager.Player.Inventory.Potion.HealPower;
    //         GameManager.Player.Inventory.Potion.Count--;
    //         Console.WriteLine("회복을 완료했습니다.");
    //     }
    //     else
    //     {
    //         Console.WriteLine("포션이 부족합니다.");
    //     }
    // }
}

namespace Text_RPG_11
{
    internal class GameManager
    {
        
    }
}