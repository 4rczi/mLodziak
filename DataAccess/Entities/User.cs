namespace DataAccess.Entities
{
    public class User : BaseEntity
    {
        public string? Id { get; set; }
        public ICollection<UserHistory>? UserHistory { get; set; }
    }
}
