namespace YeOldePub.WPF
{
    public class PintGlass: IYeOldePubObject
    {
        public bool IsClean { get; set; }
        public bool HasBeer { get; set; }

        public PintGlass()
        {
            IsClean = true;
            HasBeer = false;
        }
    }
}