using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG
{
    internal class Dungeon
    {
        public int stage = 1;
        public Dungeon()
        {
            //
        }

        public void LevelUp()
        {
            // 전투 클리어 후 사냥한 몬스터만큼 경험치 증가
            // 몬스터를 순회하면서 체력이 0이 된 몬스터의 
        }

        public void MonsterSpawn()
        {
            // 배틀 시작 후 몬스터 랜덤 등장
            
            // 0. 몬스터 담을 리스트 생성(이후에 추가)
            List<Monster> monsters = new List<Monster>();
            List<Monster> enemies = new List<Monster>();
            
            // 1. 몬스터 수 생성
            Random random = new Random();
            int spawnNum = random.Next(1, 5);
            
            // 2. 등장할 몬스터 랜덤 선택
            for (int i = 0; i < spawnNum - 1; i++)
            {
                enemies.Add(monsters[random.Next(0, monsters.Count)]);
            }
        }
        
        public void UserPotion()
        {
            // 유저 포션 사용 기능
            
            // 만약 유저의 포션 개수가 0보다 크다면
            // 힐파워만큼 체력을 채우고
            // 개수 1개 차감
            // 만약 유저의 포션 개수가 0보다 작다면
            // 포션이 부족합니다
        }
    }
}