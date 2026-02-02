using ASP_31_01_HW.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP_31_01_HW.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController:ControllerBase
    {
        private readonly OrdersStore _orderStore;

        public OrdersController(OrdersStore orderStore)
        {
            _orderStore = orderStore;
        }
        [HttpGet]
        public ICollection<Order> GetOrders()
        {
            return _orderStore.GetAll();
        }
        [HttpPost]
        public IActionResult CreateOrder([FromBody] OrderCreateUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Number))
                return BadRequest("Field 'Number' is required.");

            if (dto.Total == null || dto.Total <= 0)
                return BadRequest("Field 'Total' must be greater than 0.");

            var order = _orderStore.Create(dto.Number, dto.Total.Value);

            return CreatedAtAction(
                nameof(GetOrderById),
                new { id = order.Id },
                order
            );
        }
        [HttpGet]
        [Route("{id:int:min(1)}")]
        public IActionResult GetOrderById(int id)
        {
            var order = _orderStore.GetById(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }
        [HttpPut]
        [Route("{id:int:min(1)}")]
        public IActionResult UpdateOrder(int id, [FromBody] OrderCreateUpdateDto dto)
        {
            var order = _orderStore.GetById(id);
            if (order == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(dto.Number))
                return BadRequest("Field 'Number' is required.");

            if (dto.Total == null || dto.Total <= 0)
                return BadRequest("Field 'Total' must be greater than 0.");

            order.Number = dto.Number;
            order.Total = dto.Total.Value;

            return Ok(order);
        }
        [HttpDelete]
        [Route("{id:int:min(1)}")]
        public IActionResult DeleteOrder(int id)
        {
            bool deleted = _orderStore.Delete(id);

            if (!deleted)
                return NotFound();

            return NoContent(); // 204
        }

        // GET /api/orders/search?number=...&minTotal=...
        [HttpGet]
        [Route("search")]
        public IActionResult Search(
            [FromQuery] string? number,
            [FromQuery] decimal? minTotal)
        {
            if (string.IsNullOrWhiteSpace(number))
                return BadRequest("Query parameter 'number' is required.");

            var result = _orderStore.Search(number, minTotal);

            return Ok(result);
        }
    }
}
