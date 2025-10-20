using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    public static class JobDatabase
    {
        private static string jobDataPath = "Data/jobs.json";
        private static JobDataContainer cachedJobs;

        static JobDatabase()
        {
            cachedJobs = LoadJobs();
        }

        private static JobDataContainer LoadJobs()
        {
            try
            {
                if (!File.Exists(jobDataPath))
                {
                    Console.WriteLine($"jobs.json을 찾을 수 없습니다: {jobDataPath}");
                    return GetDefaultContainer();
                }

                string json = File.ReadAllText(jobDataPath);
                var container = JsonConvert.DeserializeObject<JobDataContainer>(json);

                if (container == null || container.jobs == null)
                {
                    Console.WriteLine("직업 데이터 파싱 실패");
                    return GetDefaultContainer();
                }

                Console.WriteLine($"직업 로드 완료: {container.jobs.Count}개");
                return container;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"직업 로드 오류: {ex.Message}");
                return GetDefaultContainer();
            }
        }

        private static JobDataContainer GetDefaultContainer()
        {
            return new JobDataContainer
            {
                jobs = new List<JobInfo>()
            };
        }

        public static List<Job> GetAllJobs()
        {
            List<Job> jobs = new List<Job>();

            foreach (var jobInfo in cachedJobs.jobs)
            {
                jobs.Add(CreateJobFromData(jobInfo));
            }

            return jobs;
        }

        public static Job GetJobByName(string name)
        {
            JobInfo jobInfo = null;

            foreach (var job in cachedJobs.jobs)
            {
                if (job.name == name)
                {
                    jobInfo = job;
                    break;
                }
            }

            if (jobInfo != null)
            {
                return CreateJobFromData(jobInfo);
            }

            return null;
        }

        private static Job CreateJobFromData(JobInfo data)
        {
            return new Job(
                name: data.name,
                displayName: data.displayName,
                description: data.description,
                hp: data.baseStats.hp,
                mp: data.baseStats.mp,
                attack: data.baseStats.attack,
                defense: data.baseStats.defense,
                criticalChance: data.baseStats.criticalChance,
                dodgeChance: data.baseStats.dodgeChance,
                gold: data.baseStats.gold
            );
        }
    }
}