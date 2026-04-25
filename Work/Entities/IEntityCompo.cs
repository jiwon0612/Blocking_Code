namespace Entities
{
    public interface IEntityCompo
    {
        void Initialize(Entity entity);
    }
    
    public interface IAfterInit
    {
        void AfterInit();
    }
}