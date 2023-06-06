namespace WebApplication2
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Detail> Details { get; set; }
    }
}