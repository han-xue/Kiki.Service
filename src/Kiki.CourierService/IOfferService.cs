using System.Collections.Generic;
using System.Threading.Tasks;
using Kiki.CourierService.Shared.Features.Cost;
using Refit;

namespace Kiki.CourierService
{
    public interface ICostService
        {
            [Post("/cost")]
            //[Get("/cost")]
            Task<Cost.Result> Cost([Body] Cost.Query query); 
            //Task<Cost.Result> Cost(Cost.Query query); 
        }
}
