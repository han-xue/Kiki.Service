using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Kiki.CourierService.Shared.Features.Cost;
using MediatR;

namespace Kiki.CourierService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CostController : ControllerBase
    {

        private readonly ISender _mediator;

        public CostController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public Task<Cost.Result> Post([FromBody] Kiki.CourierService.Shared.Features.Cost.Cost.Query query) => _mediator.Send(query);
    }
}
