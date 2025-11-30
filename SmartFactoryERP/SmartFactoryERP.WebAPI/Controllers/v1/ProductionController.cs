using Microsoft.AspNetCore.Http;
using SmartFactoryERP.Application.Features.Production.Commands.CreateBOM;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartFactoryERP.Application.Features.Production.Commands.CompleteProduction;
using SmartFactoryERP.Application.Features.Production.Commands.StartProduction;
using SmartFactoryERP.Application.Features.Production.Commands.CreateProductionOrder;

namespace SmartFactoryERP.WebAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductionController : BaseApiController
    {
        // POST api/v1/production/bom
        [HttpPost("bom")]
        public async Task<IActionResult> CreateBillOfMaterial([FromBody] CreateBillOfMaterialCommand command) //tested
        {
            var bomId = await Mediator.Send(command);
            return Ok(bomId);
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

    // POST api/v1/production/orders
    [HttpPost("orders")]
        public async Task<IActionResult> CreateProductionOrder([FromBody] CreateProductionOrderCommand command) // tested but need to retest after BOM
        {
            var orderId = await Mediator.Send(command);
            return Ok(orderId);
        }
        #region Test
        /* 

      🧪 How to Test (Postman)
Rebuild & Run the project.

Scenario: Plan to produce 10 Tables (Product ID: 2).

Request:

Method: POST

URL: https://localhost:7093/api/v1/production/orders

Body (JSON):

JSON

{
  "productId": 2,
  "quantity": 10,
  "startDate": "2025-12-01T09:00:00",
  "notes": "Urgent order for Client X"
}
Expected Result: 200 OK with the new Order ID.

🔮 What's Next?
Right now, the order status is Planned. It's just a piece of paper.

The real magic happens when we Start Production. This is where the logic gets complex because we need to:

Check the BOM (Recipe).

Check if we have enough Raw Materials (Legs/Wood) in Inventory.

Deduct the Raw Materials from Inventory (Work In Progress).
         */
        #endregion

        // POST api/v1/production/orders/{id}/start
        [HttpPost("orders/{id}/start")]
        public async Task<IActionResult> StartProduction(int id) //500
        {
            await Mediator.Send(new StartProductionCommand { Id = id });
            return NoContent();
        }
        // POST api/v1/production/orders/{id}/complete
        [HttpPost("orders/{id}/complete")]
        public async Task<IActionResult> CompleteProduction(int id)
        {
            await Mediator.Send(new CompleteProductionCommand { Id = id });
            return NoContent();
        }
        #region Test
        /* 
         🧪 Full Manufacturing Lifecycle Test (Postman)This is the ultimate test to prove your SmartFactory works.Scenario: Manufacturing 10 Chairs1. Setup (Inventory Check):Check Wood (Raw Material, ID 1): Stock should be 60 (remember we deducted 40 in the previous step).Check Chair (Finished Product, ID 2): Stock should be 0 (or whatever your starting amount was).2. Action (Complete Production):Method: POSTURL: https://localhost:7093/api/v1/production/orders/{id}/completeResult: 204 No Content.3. Verification (The Magic Moment):Check Chair Stock (GET /inventory/materials/2).Expectation: Stock should now be 10.If this works, CONGRATULATIONS! 🎓You have built a system where:Purchasing buys Wood $\rightarrow$ Wood Stock Goes Up.Production uses Wood $\rightarrow$ Wood Stock Goes Down.Production finishes Chairs $\rightarrow$ Chair Stock Goes Up.Sales sells Chairs $\rightarrow$ Chair Stock Goes Down.This is a fully integrated ERP loop.Are you ready to wrap this up, or would you like to add a specific report (Analytics) to see all this data in one dashboard?
         */
        #endregion
    }
}
