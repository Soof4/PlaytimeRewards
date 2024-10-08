# PlaytimeRewards
A TShock plugin that gives players rewards based on how much time they've played on the server.

If you want to read this in another language: [Spanish](https://github.com/Soof4/PlaytimeRewards/blob/main/README_SPANISH.md)

## Permissions and Commands
|Permissions  | Commands    |
|-------------|-------------|
|pr.getreward |getreward, gr|
|pr.playtime  |playtime, pt |
|pr.updateplaytime |updateplaytime||

Note: If you're updating from v1.1.x to latest version use the command ``/updateplaytime`` once, so the database is updated.

## Configuration
When plugin runs for the first time it'll create a file named "PlaytimeRewardsConfig.json".
|Keys                |Descriptions |
|--------------------|-------------|
|TimeInMins          |This is how many minutes needed for a reward|
|SwitchToHMMultiplier|When switched to hardmode every players' unused playtime will <br>be multiplied by this number. (If this equals to 0, playtimes will reset.)|
|RewardsPreHM        |An array of item IDs for pre-hardmode rewards.|
|RewardsHM        |An array of item IDs for hardmode rewards.|
