namespace NS2.FeatureToggler
{
    public class State<T>
    {
        public State(bool isEnabled)
        {
            Enabled = isEnabled;
        }
        public bool Enabled { get; }
        public bool Disabled => !Enabled;
    }
}
