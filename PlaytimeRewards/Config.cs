using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaytimeRewards {
    public class Config {
        public int TimeInMins = 60;
        public double SwitchToHMMultiplier = 0.3;
        public int[] RewardsPreHM = { 2334, 2335, 2336, 3203, 3204, 3205, 3206, 3207, 3208, 4405, 4407, 4877, 5002 };
        public int[] RewardsHM = { 3979, 3980, 3981, 3982, 3983, 3984, 3985, 3986, 3987, 4406, 4408, 4878, 5003 };
        public Dictionary<string, int> PlayerList = new();
        public void Write() {
            File.WriteAllText(PlaytimeRewards.path, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
        public static Config Read() {
            if (!File.Exists(PlaytimeRewards.path)) {
                return new Config();
            }
            return JsonConvert.DeserializeObject<Config>(File.ReadAllText(PlaytimeRewards.path));
        }
    }
}
