# PlaytimeRewards

Un plugin de TShock que otorga recompensas a los jugadores según el tiempo que han jugado en el servidor.

Si quieres leer esto en otro idioma: [Inglés](https://github.com/Soof4/PlaytimeRewards/blob/main/README.md)

## Permisos y Comandos

| Permisos        | Comandos         | Sintaxis                                                                                                 | Ejemplo de uso                            |
| --------------- | ---------------- | -------------------------------------------------------------------------------------------------------- | ----------------------------------------- |
| pr.getreward    | getreward, gr    | gr \<all \| número>                                                                                      | /gr all                                   |
| pr.playtime     | playtime, pt     | pt                                                                                                       | /pt                                       |
| pr.addreward    | addreward, ar    | ar \<ID o nombre del objeto> \<cantidad> \<¿cantidad aleatoria?> \<¿antes de hardmode?> \<¿en hardmode?> | /ar "sofá de nébula" 420 false false true |
| pr.removereward | removereward, rr | rr \<ID del objeto \| número>                                                                            | /rr "sofá de nébula"                      |

## Configuración

Cuando el plugin se ejecuta por primera vez, creará un archivo llamado "PlaytimeRewardsConfig.json".

| Claves               | Descripciones                                                                                                                                                              |
| -------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| TimeInMins           | Minutos necesarios para reclamar una recompensa.                                                                                                                           |
| SwitchToHMMultiplier | Cuando se cambia a hardmode, el tiempo de juego no utilizado de los jugadores se <br>multiplicará por este número. (Si este valor es 0, el tiempo de juego se reiniciará). |
| Rewards              | Una lista de recompensas.                                                                                                                                                  |
