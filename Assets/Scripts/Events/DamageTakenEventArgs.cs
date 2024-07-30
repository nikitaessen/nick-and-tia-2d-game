namespace Events
{
    public class DamageTakenEventArgs
    {
        public int Insanity { get; private set; }
        
        public DamageTakenEventArgs(int insanity)
        {
            Insanity = insanity;
        }
    }
}