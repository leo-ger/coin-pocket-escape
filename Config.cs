using System.ComponentModel;

namespace coin_pocket_escape
{
    public class Config
    {
        [Description("Set the waiting time after the coin is flipped in ms")]
        public int WaitingTime { get; set; } = 2500;
    }
}