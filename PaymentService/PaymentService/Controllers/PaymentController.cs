using APIHandler.Models;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        [HttpPost(Name = "PaymentAPI")]
        public ActionResult<APIResult<object>> PAymentAPI([FromBody] object requestData)
        {
            var result = new APIResult<object>
            {
                value = requestData,
                Message = "Payment done successfully",
                IsSuccess = true,
                HttpStatus = 200
            };

            return Ok(result);
        }
    }
}
