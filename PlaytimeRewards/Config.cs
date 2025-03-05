using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaytimeRewards
{
    public class Config
    {
        public int TimeInMins = 60;
        public double SwitchToHMMultiplier = 0.3;
        public List<Reward> Rewards = new List<Reward>() {
            new Reward(2334, 1, false, true, true)
        };

        public void Write()
        {
            File.WriteAllText(PlaytimeRewards.path, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public static Config Read()
        {
            if (!File.Exists(PlaytimeRewards.path))
            {
                return new Config();
            }

            return JsonConvert.DeserializeObject<Config>(File.ReadAllText(PlaytimeRewards.path)) ?? new Config();
        }
    }
}
