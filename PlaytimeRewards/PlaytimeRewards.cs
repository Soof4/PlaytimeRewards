using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;

namespace PlaytimeRewards {
    [ApiVersion(2,1)]
    public class PlaytimeRewards : TerrariaPlugin {
        public override string Name => "PlaytimeRewards";
        public override Version Version => new Version(1, 1, 0);
        public override string Author => "Soofa";
        public override string Description => "Gives players rewards based on how much time they've played on the server.";

        public static string path = Path.Combine(TShock.SavePath + "/PlaytimeRewardsConfig.json");
        public static Config Config = new Config();

        public static int ticks = 0;
        public PlaytimeRewards(Main game) : base(game) {
        }
        public override void Initialize() {
            ServerApi.Hooks.GameUpdate.Register(this, OnGameUpdate);
            GeneralHooks.ReloadEvent += OnReload;
            Commands.ChatCommands.Add(new Command("pr.getreward", GetRewardCmd, "getreward", "gr"));
            Commands.ChatCommands.Add(new Command("pr.playtime", PlayTimeCmd, "playtime", "pt"));

            if (File.Exists(path)) {
                Config = Config.Read();
            }
            else {
                Config.Write();
            }
        }

        private void PlayTimeCmd(CommandArgs args) {
            if (Config.PlayerList.ContainsKey(args.Player.Name)) {
                args.Player.SendInfoMessage($"You have {Config.PlayerList[args.Player.Name]} mins unused playtime.");
            }
            else {
                args.Player.SendErrorMessage("You don't have any playtime.");
            }
        }

        private void GetRewardCmd(CommandArgs args) {
            if (args.Player.RealPlayer == false) {
                return;
            }

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
        private void OnGameUpdate(EventArgs args) {
            if (++ticks % 3600 * Config.TimeInMins == 0) {
                for (int i = 0; i < Main.maxPlayers; i++) {
                    bool isFound = false;
                    foreach (var kvp in Config.PlayerList) {
                        if (Main.player[i].name.Equals(kvp.Key)) {
                            Config.PlayerList[kvp.Key]++;
                            isFound = true;
                        }
                    }
                    if(!isFound && Main.player[i].name != "") {
                        Config.PlayerList.Add(Main.player[i].name, 1);
                    }
                }
                ticks = 0;
                Config.Write();
            }
        }
    }
}