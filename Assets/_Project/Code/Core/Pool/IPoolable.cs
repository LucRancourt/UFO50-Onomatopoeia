namespace _Project.Code.Core.Pool
{
    public interface IPoolable
    {
        void OnCreateForPool();
        void OnSpawnFromPool();
        void OnReturnToPool();
    }
}