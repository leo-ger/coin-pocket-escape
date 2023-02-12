using System.ComponentModel;

namespace coin_pocket_escape
{
    public class Config
    {
        [Description("Toggle forced coin spawns in the beginning of the Round")]
        public bool ForcedCoins { get; set; } = true;
        
        [Description("Changes the number of coins that will spawn at the beginning of the match")]
        public int ForcedCoinsNumber { get; set; } = 10;
       
        [Description("Set the waiting time after the coin is flipped in ms")]
        public int WaitingTime { get; set; } = 2000;

        [Description("Toggle coin success randomization")]
        public bool CoinRandom { get; set; } = true;
    }
}