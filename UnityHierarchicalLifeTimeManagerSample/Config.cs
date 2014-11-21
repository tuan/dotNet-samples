namespace UnityHierarchicalLifeTimeManagerSample
{
    public class Config : IConfig
    {
        public ISnapshot Snapshot
        {
            get;
            set;
        }

        public Config(ISnapshot snapshot)
        {
            Snapshot = snapshot;
        }
        
    }
}
