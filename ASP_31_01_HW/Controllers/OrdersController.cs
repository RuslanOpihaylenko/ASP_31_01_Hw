using ASP_31_01_HW.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP_31_01_HW.Controllers
{
    /// <summary>
    /// API controller for orders 
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController:ControllerBase
    {
        private readonly OrdersStore _orderStore;

        public OrdersController(OrdersStore orderStore)
        {
            _orderStore = orderStore;
        }
        /// <summary>
        /// Method for GetOrders
        /// </summary>
        /// <returns>return orders</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ICollection<Order> GetOrders()
        {
            return _orderStore.GetAll();
        }
        /// <summary>
        /// Method for CreateOrder
        /// </summary>
        /// <param name="dto">Name of order</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <summary>
        ///     Method for GetOrderById
        /// </summary>
        /// <param name="id"> order idification</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id:int:min(1)}")]
        public IActionResult GetOrderById(int id)
        {
            var order = _orderStore.GetById(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }
        /// <summary>
        /// Method for UpdateOrder
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <summary>
        /// Method for DeleteOrder
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id:int:min(1)}")]
        public IActionResult DeleteOrder(int id)
        {
            bool deleted = _orderStore.Delete(id);

            if (!deleted)
                return NotFound();

            return NoContent(); // 204
        }

        // GET /api/orders/search?number=...&minTotal=...
        /// <summary>
        /// Method for Search
        /// </summary>
        /// <param name="number"></param>
        /// <param name="minTotal"></param>
        /// <returns></returns>
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
