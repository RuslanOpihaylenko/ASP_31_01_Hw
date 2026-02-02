using ASP_31_01_HW.Models;

namespace ASP_31_01_HW.Controllers
{
    public class OrdersStore
    {
        private List<Order> _orders = new()
        {
            new Order(1, "First", 20.99m,DateTime.UtcNow),
            new Order(2, "Second", 25.99m,DateTime.UtcNow),
            new Order(3, "Third", 45.99m,DateTime.UtcNow),
            new Order(4, "Fourth", 15.99m,DateTime.UtcNow)
        };
        public List<Order> GetAll() => _orders;
        public Order? GetById(int id) =>
            _orders.FirstOrDefault(o => o.Id == id);
        public Order Create(string number, decimal total)
        {
            int newId = _orders.Any() ? _orders.Max(o => o.Id) + 1 : 1;

            var order = new Order
            {
                Id = newId,
                Number = number,
                Total = total,
                CreatedAt = DateTime.UtcNow
            };

            _orders.Add(order);
            return order;
        }
        public bool Delete(int id)
        {
            var order = GetById(id);
            if (order == null)
                return false;

            _orders.Remove(order);
            return true;
        }

        public IEnumerable<Order> Search(string number, decimal? minTotal)
        {
            var query = _orders
                .Where(o => o.Number.Contains(number, StringComparison.OrdinalIgnoreCase));

            if (minTotal.HasValue)
                query = query.Where(o => o.Total >= minTotal.Value);

            return query;
        }
    }
}
