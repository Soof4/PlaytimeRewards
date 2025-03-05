using IL.Microsoft.Xna.Framework;
using Microsoft.Data.Sqlite;
using System.ComponentModel;
using System.Data;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;


namespace PlaytimeRewards;

[ApiVersion(2, 1)]
public class PlaytimeRewards : TerrariaPlugin
{
    public override string Name => "PlaytimeRewards";
    public override Version Version => new Version(1, 3, 0);
    public override string Author => "Soofa";
    public override string Description => "Gives players rewards based on how much time they've played on the server.";

    private bool isHM;
    public static string path = Path.Combine(TShock.SavePath + "/PlaytimeRewardsConfig.json");
    public static Config Config = new Config();
    public static DateTime lastTime = DateTime.UtcNow;
    private IDbConnection db = null!;
    public static DatabaseManager dbManager = null!;
    public static Dictionary<string, int> onlinePlayers = new();
    public PlaytimeRewards(Main game) : base(game) { }
    public override void Initialize()
    {
        db = new SqliteConnection("Data Source=" + Path.Combine(TShock.SavePath, "PlaytimeRewards.sqlite"));
        dbManager = new DatabaseManager(db);

        ServerApi.Hooks.WorldStartHardMode.Register(this, OnWorldStartHardMode);
        ServerApi.Hooks.ServerJoin.Register(this, OnServerJoin);
        ServerApi.Hooks.ServerLeave.Register(this, OnServerLeave);
        GeneralHooks.ReloadEvent += OnReload;
        ServerApi.Hooks.GamePostInitialize.Register(this, OnGamePostInitialize);

        Commands.ChatCommands.Add(new Command("pr.getreward", GetRewardCmd, "getreward", "gr")
        {
            AllowServer = false,
            HelpText = "Get a reward for your playtime. Usage: \"/getreward <amount>\" or \"/getreward all\""
        });

        Commands.ChatCommands.Add(new Command("pr.playtime", PlayTimeCmd, "playtime", "pt")
        {
            AllowServer = false,
            HelpText = "Shows how much playtime you have."
        });

        if (File.Exists(path))
        {
            Config = Config.Read();
        }
        else
        {
            Config.Write();
        }
    }


    #region Hooks
    private void OnGamePostInitialize(EventArgs args)
    {
        isHM = Main.hardMode;
    }

    private void OnServerJoin(JoinEventArgs args)
    {
        UpdateTime();
        try
        {
            onlinePlayers.Add(Main.player[args.Who].name, dbManager.GetPlayerTime(Main.player[args.Who].name));
        }
        catch (NullReferenceException)
        {
            dbManager.InsertPlayer(Main.player[args.Who].name);
            onlinePlayers.Add(Main.player[args.Who].name, 0);
        }
    }

    private void OnServerLeave(LeaveEventArgs args)
    {
        UpdateTime();
        onlinePlayers.Remove(Main.player[args.Who].name);
    }

    private void OnWorldStartHardMode(HandledEventArgs args)
    {
        if (isHM)
        {
            return;
        }
        UpdateTime();
        foreach (var kvp in onlinePlayers)
        {
            onlinePlayers[kvp.Key] = (int)(kvp.Value * Config.SwitchToHMMultiplier);
            dbManager.SavePlayer(kvp.Key, kvp.Value);
        }
        TSPlayer.All.SendInfoMessage($"Everyone's playtime has been reduced by {100 * (1 - Config.SwitchToHMMultiplier)}%.");
        isHM = true;
    }

    private void OnReload(ReloadEventArgs e)
    {
        if (File.Exists(path))
        {
            Config = Config.Read();
        }
        else
        {
            Config.Write();
        }
        e.Player.SendSuccessMessage("PlaytimeRewards plugin has been reloaded.");
    }
    #endregion

    #region Commands
    private void PlayTimeCmd(CommandArgs args)
    {
        UpdateTime();
        args.Player.SendInfoMessage($"You have {onlinePlayers[args.Player.Name]} mins unused playtime.");
    }
    private void GetRewardCmd(CommandArgs args)
    {
        UpdateTime();
        TSPlayer Player = args.Player;
        int num = 1;

        if (args.Parameters.Count > 0)
        {
            if (args.Parameters[0].Equals("all"))
            {
                if (Config.TimeInMins <= 0)
                {
                    num = 1;
                }
                else
                {
                    num = onlinePlayers[Player.Name] / Config.TimeInMins;
                }
            }
            else
            {
                int.TryParse(args.Parameters[0], out num);
            }

            if (num < 1)
            {
                args.Player.SendErrorMessage("Amount can't be lower than one.");
                return;
            }
        }
        Random rand = new Random();

        if (onlinePlayers[Player.Name] < Config.TimeInMins * num)
        {
            Player.SendErrorMessage($"You have only {onlinePlayers[Player.Name]} mins playtime. You need to have at least {Config.TimeInMins * num} mins.");
            return;
        }

        Reward reward;

        if (Main.hardMode)
        {
            var hmRewards = Config.Rewards.Where(r => r.IsHardmode);

            while (num > 0)
            {
                reward = rand.Choice(hmRewards);
                Player.GiveItem(reward.ItemID, reward.IsAmountRandom ? rand.Next(1, reward.Amount + 1) : reward.Amount);
                num--;
                onlinePlayers[Player.Name] -= Config.TimeInMins;
                dbManager.SavePlayer(Player.Name, onlinePlayers[Player.Name]);
            }
        }
        else
        {
            var preHmRewards = Config.Rewards.Where(r => r.IsHardmode);

            while (num > 0)
            {
                reward = rand.Choice(preHmRewards);
                Player.GiveItem(reward.ItemID, reward.IsAmountRandom ? rand.Next(1, reward.Amount + 1) : reward.Amount);
                num--;
                onlinePlayers[Player.Name] -= Config.TimeInMins;
                dbManager.SavePlayer(Player.Name, onlinePlayers[Player.Name]);
            }
        }

        dbManager.SavePlayer(Player.Name, onlinePlayers[Player.Name]);
        Player.SendSuccessMessage("You were given your reward(s).");
    }
    #endregion


    public void UpdateTime()
    {
        if ((DateTime.UtcNow - lastTime).TotalMinutes < 1)
        {
            return;
        }
        foreach (var plr in onlinePlayers)
        {
            onlinePlayers[plr.Key] += (int)(DateTime.UtcNow - lastTime).TotalMinutes;
            dbManager.SavePlayer(plr.Key, onlinePlayers[plr.Key]);
        }
        lastTime = DateTime.UtcNow;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ServerApi.Hooks.WorldStartHardMode.Deregister(this, OnWorldStartHardMode);
            ServerApi.Hooks.ServerLeave.Deregister(this, OnServerLeave);
            ServerApi.Hooks.ServerJoin.Deregister(this, OnServerJoin);
            ServerApi.Hooks.GameWorldConnect.Deregister(this, OnGamePostInitialize);
            GeneralHooks.ReloadEvent -= OnReload;
        }
        base.Dispose(disposing);
    }
}
