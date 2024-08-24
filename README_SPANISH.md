# PlaytimeRewards
Un Plugin de Tshock que permite a los jugadores reclamar unas recompensas por tiempo. (Traducido por [FrankV22](https://github.com/itsFrankV22))

## Permisos y comandos
|   Permisos  | Comandos    |
|-------------|-------------|
|pr.getreward |getreward, gr|
|pr.playtime  |playtime, pt |
|pr.updateplaytime |updateplaytime||

Nota: Si está actualizando desde v1.1.x a la última versión, use el comando ``/updateplaytime`` Para actualizar la base de datos.

## Configuración 
Cuando el complemento se ejecuta por primera vez, creará un archivo llamado "PlaytimeRewardsConfig.json".
|Keys                |Descripciones |
|--------------------|--------------|
|TimeInMins          |Los minutos que nececita acumular el jugador para reclamar.|
|SwitchToHMMultiplier|Cuando se cambia al modo difícil, el tiempo de juego no utilizado de cada jugador <br>be será multiplicado por este número. (Si es es igual a 0, los tiempos  se restablecerán.)|
|RewardsPreHM        |Recompensas que se obtendran antes del HardMode.|
|RewardsHM        |Recompensas que se obtendran despues del HardMode.|
