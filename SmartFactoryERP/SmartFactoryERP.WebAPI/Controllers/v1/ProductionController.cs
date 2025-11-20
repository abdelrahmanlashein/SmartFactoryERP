using Microsoft.AspNetCore.Http;
using SmartFactoryERP.Application.Features.Production.Commands.CreateBOM;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SmartFactoryERP.WebAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductionController : BaseApiController
    {
        // POST api/v1/production/bom
        [HttpPost("bom")]
        public async Task<IActionResult> CreateBillOfMaterial([FromBody] CreateBillOfMaterialCommand command)
        {
            var bomId = await Mediator.Send(command);
            return Ok(bomId);
        }
    }
    #region test 
    /*
     * How to Test (Postman)
To test this, you need to set up your inventory data first.

Scenario: You want to define that 1 Table (Product) requires 4 Legs (Component).

Prerequisite (Inventory):

Create a Material "Table Leg" (Raw Material) -> Assume ID: 1

Create a Material "Dining Table" (Finished Good) -> Assume ID: 2

Test Request:

Method: POST

URL: https://localhost:7093/api/v1/production/bom

Body (JSON):

JSON

{
  "productId": 2,
  "componentId": 1,
  "quantity": 4
}
Expected Result: 200 OK with the new ID.

Once you confirm this works, we can move to the main event: Creating a Production Order to actually start manufacturing!
     */
    #endregion

}
