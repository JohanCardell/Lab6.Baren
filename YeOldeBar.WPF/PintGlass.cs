namespace YeOldePub.WPF
{
    public class PintGlass
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