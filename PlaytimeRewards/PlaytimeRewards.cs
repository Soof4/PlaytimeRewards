using IL.Microsoft.Xna.Framework;
using System.ComponentModel;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;

namespace PlaytimeRewards {
    [ApiVersion(2,1)]
    public class PlaytimeRewards : TerrariaPlugin {
        public override string Name => "PlaytimeRewards";
        public override Version Version => new Version(1, 1, 2);
        public override string Author => "Soofa";
        public override string Description => "Gives players rewards based on how much time they've played on the server.";

        public static string path = Path.Combine(TShock.SavePath + "/PlaytimeRewardsConfig.json");
        public static Config Config = new Config();
        public static DateTime lastTime = DateTime.UtcNow;
        public static bool wasHardmode = Main.hardMode;
        public PlaytimeRewards(Main game) : base(game) {
        }
        public override void Initialize() {
            ServerApi.Hooks.WorldStartHardMode.Register(this, OnWorldStartHardMode);
            ServerApi.Hooks.ServerLeave.Register(this, OnServerLeave);
            ServerApi.Hooks.ServerJoin.Register(this, OnServerJoin);
            GeneralHooks.ReloadEvent += OnReload;

            Commands.ChatCommands.Add(new Command("pr.getreward", GetRewardCmd, "getreward", "gr") {
                AllowServer = false,
                HelpText = "Get a reward for your playtime."
            });
            Commands.ChatCommands.Add(new Command("pr.playtime", PlayTimeCmd, "playtime", "pt") {
                AllowServer = false,
                HelpText = "Shows how much playtime you have."
            });

            if (File.Exists(path)) {
                Config = Config.Read();
            }
            else {
                Config.Write();
            }
        }
        private void OnServerJoin(JoinEventArgs args) {
            UpdateTime(Main.player[args.Who].name);
        }
        private void OnServerLeave(LeaveEventArgs args) {
            UpdateTime();
        }
        public void UpdateTime(string dontUpdatePlayerName="") {
            for (int i = 0; i < Main.maxPlayers; i++) {
                if (Main.player[i] == null) {
                    continue;
                }
                bool isFound = false;
                foreach (var kvp in Config.PlayerList) {
                    if (Main.player[i].name.Equals(kvp.Key)) {
                        isFound = true;
                        if (Main.player[i].name != dontUpdatePlayerName) {
                            Config.PlayerList[kvp.Key] += (int)(DateTime.UtcNow - lastTime).TotalMinutes;
                        }
                        break;
                    }
                }
                if (!isFound && Main.player[i].name != "") {
                    Config.PlayerList.Add(Main.player[i].name, 0);
                }
            }

            if ((DateTime.UtcNow - lastTime).TotalMinutes >= 1) {
                lastTime = DateTime.UtcNow;
            }

            Config.Write();
        }

        private void OnWorldStartHardMode(HandledEventArgs args) {
            UpdateTime();
            if (wasHardmode) {
                return;
            }
            foreach (var kvp in Config.PlayerList) {
                Config.PlayerList[kvp.Key] = (int)(kvp.Value*Config.SwitchToHMMultiplier);
            }
            Config.Write();
            TSPlayer.All.SendInfoMessage($"All players' playtime has been reduced by %{100*(1-Config.SwitchToHMMultiplier)}.");
            wasHardmode = true;
        }
        private void PlayTimeCmd(CommandArgs args) {
            UpdateTime();
            if (Config.PlayerList.ContainsKey(args.Player.Name)) {
                args.Player.SendInfoMessage($"You have {Config.PlayerList[args.Player.Name]} mins unused playtime.");
            }
            else {
                args.Player.SendErrorMessage("You don't have any playtime.");
            }
        }
        private void GetRewardCmd(CommandArgs args) {
            UpdateTime();
            TSPlayer Player = args.Player;
            Random rand = new Random();
            int itemIndex;

            if(!Config.PlayerList.ContainsKey(Player.Name)) {
                Player.SendErrorMessage("You need to play more to get rewards.");
                return;
            }
            else if (Config.PlayerList[Player.Name] < Config.TimeInMins) {
                Player.SendErrorMessage($"You have only {Config.PlayerList[Player.Name]} mins playtime. You need to have at least {Config.TimeInMins} mins.");
                return;
            }

            if(Main.hardMode) {
                itemIndex = rand.Next(0, Config.RewardsHM.Length);
                Player.GiveItem(Config.RewardsHM[itemIndex], 1);
            }
            else {
                itemIndex = rand.Next(0, Config.RewardsPreHM.Length);
                Player.GiveItem(Config.RewardsPreHM[itemIndex], 1);
            }
            Config.PlayerList[Player.Name] -= Config.TimeInMins;
            Config.Write();
            Player.SendSuccessMessage("You have given your reward.");
        }
        private void OnReload(ReloadEventArgs e) {
            if (File.Exists(path)) {
                Config = Config.Read();
            }
            else {
                Config.Write();
            }
            e.Player.SendSuccessMessage("PlaytimeRewards plugin has been reloaded.");
        }
        protected override void Dispose(bool disposing) {
            if(disposing) {
                ServerApi.Hooks.WorldStartHardMode.Deregister(this, OnWorldStartHardMode);
                ServerApi.Hooks.ServerLeave.Deregister(this, OnServerLeave);
                ServerApi.Hooks.ServerJoin.Deregister(this, OnServerJoin);
                GeneralHooks.ReloadEvent -= OnReload;
            }
            base.Dispose(disposing);
        }
    }
}