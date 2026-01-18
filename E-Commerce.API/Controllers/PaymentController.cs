using AutoMapper;
using E_Commerce.Application.DTOs.Basket;
using E_Commerce.Application.DTOs.Response;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Models.Basket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.V2.Core;

namespace E_Commerce.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentServices _paymentServices;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentServices paymentServices,IMapper mapper)
        {
            _paymentServices = paymentServices;
            _mapper = mapper;
        }
        [HttpPost("{basketId}")]
        public async Task<ActionResult<DataResponse<CustomerBasketDto>>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var customerBasket = await _paymentServices.CreateOrUpdatePaymentIntent(basketId);
            if (customerBasket == null) return BadRequest(new BaseResponse(false,400,"There Is A Error"));
            var MappedBasket = _mapper.Map<CustomerBasket, CustomerBasketDto>(customerBasket);
            return Ok(new DataResponse<CustomerBasketDto>(true,200,"PaymentIntent Does Successfully",MappedBasket));
        }
        [HttpPost("webhook")]
        public async Task<IActionResult> Index()
        {
            const string endpointSecret = "Wwhsec_624fba8891f7b0107a0df319a029215905f999cdec52e6884458c8ff8e18b343";

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                try
                {
                    var stripeEvent = EventUtility.ParseEvent(json);
                    var signatureHeader = Request.Headers["Stripe-Signature"];
                    stripeEvent = EventUtility.ConstructEvent(json,
                            signatureHeader, endpointSecret);
                
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                // If on SDK version < 46, use class Events instead of EventTypes
                    if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                    {
                        Console.WriteLine("A successful payment for {0} was made.", paymentIntent.Amount);
                        await _paymentServices.UpdateOrderPaymentSucceededOrFailed(paymentIntent.Id, true);
                    }
                    else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
                    {
                        await _paymentServices.UpdateOrderPaymentSucceededOrFailed(paymentIntent.Id, true);
                    }
                    else
                    {
                        Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                    } 
                        return Ok();
                }
                catch (StripeException e)
                {
                    Console.WriteLine("Error: {0}", e.Message);
                    return BadRequest();
                }
                catch (Exception e)
                {
                    return StatusCode(500);
                }
        }
    }
}
