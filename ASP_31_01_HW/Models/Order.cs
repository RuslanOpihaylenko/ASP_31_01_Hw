using Microsoft.AspNetCore.Http.HttpResults;

namespace ASP_31_01_HW.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public decimal Total { get; set; } 
        public DateTime CreatedAt { get; set; }
        public Order() { }
        public Order(int id, string number, decimal total, DateTime createdat) 
        {
            Id = id;
            Number = number;
            Total = total;
            CreatedAt = createdat;
        }
        public override string ToString()
        {
            return $"{Number} {Total} {CreatedAt}";
        }
    }
}
