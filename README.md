# PlaytimeRewards
A TShock plugin that gives players rewards based on how much time they've played on the server.

## Permissions and Commands
|Permissions  | Commands    |
|-------------|-------------|
|pr.getreward |getreward, gr|
|pr.playtime  |playtime, pt |
|pr.updatept  |updatept||

## Configuration
When plugin runs for the first time it'll create a file named "PlaytimeRewardsConfig.json".
|Keys                |Descriptions |
|--------------------|-------------|
|TimeInMins          |This is how many minutes needed for a reward|
|SwitchToHMMultiplier|When switched tohardmode every players' unused playtime will <br>be multiplied by this number. (If this equals to 0, playtimes will reset.)|
|RewardsPreHM        |An array of item IDs for pre-hardmode rewards.|
|RewardsHM        |An array of item IDs for hardmode rewards.|
