namespace GarageBuddy.Data.Common.Models
{
    public interface IEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}
