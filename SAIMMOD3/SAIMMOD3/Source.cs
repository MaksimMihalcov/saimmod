namespace SAIMMOD3
{
    internal class Source
    {
        private int tactsToBid;
        public int TactsToBid => tactsToBid;
        public Source()
        {
            tactsToBid = 2;
        }

        public bool Tact()
        {
            if (tactsToBid == 1)
            {
                tactsToBid = 2;
                return true;
            }
            tactsToBid--;
            return false;
        }
    }
}
