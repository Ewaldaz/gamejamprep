namespace Assets.Scripts.Core
{
    public class MapTile
    {
        public int x { get; set; }
        public int z { get; set; }
        public bool center { get; set; }
        public bool occupied { get; set; }
        public bool hasTerrain { get; set; }
    }
}
