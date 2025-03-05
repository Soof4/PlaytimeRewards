# PlaytimeRewards

A TShock plugin that gives players rewards based on how much time they've played on the server.

If you want to read this in another language: [Spanish](https://github.com/Soof4/PlaytimeRewards/blob/main/README_SPANISH.md)

## Permissions and Commands

| Permissions     | Commands         | Syntax                                                                                | Example Usage                          |
| --------------- | ---------------- | ------------------------------------------------------------------------------------- | -------------------------------------- |
| pr.getreward    | getreward, gr    | gr \<all \| number>                                                                   | /gr all                                |
| pr.playtime     | playtime, pt     | pt                                                                                    | /pt                                    |
| pr.addreward    | addreward, ar    | ar \<item ID or name> \<amount> \<is amount random> \<is pre hardmode> \<is hardmode> | /ar "nebula sofa" 420 false false true |
| pr.removereward | removereward, rr | rr \<item ID \| number>                                                               | /rr "nebula sofa"                      |

## Configuration

When plugin runs for the first time it'll create a file named "PlaytimeRewardsConfig.json".
| Keys | Descriptions |
|------|--------------|
| TimeInMins | How many minutes needed for to claim a reward|
| SwitchToHMMultiplier | When switched to hardmode, players' unused playtime will <br>be multiplied by this number. (If this equals to 0, playtimes will reset.)|
| Rewards | A list of rewards.|
