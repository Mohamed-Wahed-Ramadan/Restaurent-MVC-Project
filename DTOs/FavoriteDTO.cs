namespace DTOs
{
    public class FavoriteDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int MenuProductId { get; set; }
        public DateTime CreatedAt { get; set; }
        public MenuProductDTO MenuProduct { get; set; }
    }

    public class FavoriteCreateDTO
    {
        public string UserId { get; set; }
        public int MenuProductId { get; set; }
    }
}