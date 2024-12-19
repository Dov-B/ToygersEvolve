
public class PlayerData
{
    public string playerId;
    public int level;
    public int coins;

    //Donnée qu'on veut save
    public PlayerData(string id, int lvl, int coins)
    {
        this.playerId = id;
        this.level = lvl;
        this.coins = coins;
    }
}
