using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFactoryERP.Application.Features.Inventory.Commands.AdjustStock;
using SmartFactoryERP.Application.Features.Inventory.Commands.CreateMaterial;
using SmartFactoryERP.Application.Features.Inventory.Commands.DeleteMaterial;
using SmartFactoryERP.Application.Features.Inventory.Commands.UpdateMaterial;
using SmartFactoryERP.Application.Features.Inventory.Queries.GetMaterialById;
using SmartFactoryERP.Application.Features.Inventory.Queries.GetMaterialsList;
using SmartFactoryERP.Application.Features.Inventory.Queries.GetStockTransactions;

namespace SmartFactoryERP.WebAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InventoryController : BaseApiController
    {
        [HttpPost("materials")]
        public async Task<IActionResult> CreateMaterial([FromBody] CreateMaterialCommand command) //tested
        {
            // 2. إرسال الأمر إلى MediatR
            // MediatR سيجد الـ Handler الصحيح (CreateMaterialCommandHandler)
            // وينفذه.
            var materialId = await Mediator.Send (command);

            // 3. إرجاع الرد
            // ( الأفضل إرجاع CreatedAtRoute للحصول على 201 Created)
            return Ok(materialId);
        }

        // --- الـ Endpoint الجديد ---
        [HttpGet("materials")]
        public async Task<IActionResult> GetMaterialsList() //tested
        {
            // 1. إنشاء الـ Query
            var query = new GetMaterialsListQuery();

            // 2. إرساله لـ MediatR
            var materialsList = await Mediator.Send(query);

            // 3. إرجاع النتيجة
            return Ok(materialsList);
        }

        // --- الـ Endpoint الجديد ---
        [HttpGet("materials/{id}")] // لاحظ إضافة {id}
        public async Task<IActionResult> GetMaterialById(int id) //tested
        {
            // 1. إنشاء الـ Query ووضع الـ Id فيه
            var query = new GetMaterialByIdQuery { Id = id };

            // 2. إرساله لـ MediatR
            var materialDto = await Mediator.Send(query);

            // 3. إرجاع النتيجة
            return Ok(materialDto);
        }
        // --- الـ Endpoint الجديد ---
        [HttpPut("materials/{id}")] // HttpPUT للتعديل
        public async Task<IActionResult> UpdateMaterial(int id, [FromBody] UpdateMaterialCommand command) //tested
        {
            // 1. التأكد أن الـ Id في الـ URL هو نفسه الـ Id في الـ Body (اختياري لكن مهم)
            if (id != command.Id)
            {
                return BadRequest("ID mismatch in route and body.");
            }

            // 2. إرسال الأمر لـ MediatR
            await Mediator.Send(command);

            // 3. إرجاع رد (204 No Content هو الرد الأفضل للتعديل الناجح)
            return NoContent();
        }
        // --- الـ Endpoint الجديد ---
        [HttpDelete("materials/{id}")] // HttpDELETE للحذف
        public async Task<IActionResult> DeleteMaterial(int id) //tested
        {
            // 1. إنشاء الأمر
            var command = new DeleteMaterialCommand { Id = id };

            // 2. إرسال الأمر لـ MediatR
            await Mediator.Send(command);

            // 3. إرجاع رد (204 No Content هو الرد الأفضل للحذف الناجح)
            return NoContent();
        }
        //[HttpPost("inventory/adjust-stock")]
        [HttpPost("adjust-stock")]// HttpPOST لعملية تؤثر على الرصيد
        public async Task<IActionResult> AdjustStock([FromBody] AdjustStockCommand command) //404
        {
            // 1. إرسال الأمر لـ MediatR
            await Mediator.Send(command); 

            // 2. إرجاع رد
            return Ok(); // أو NoContent()
        }
        // --- الـ Endpoint الجديد ---
        // هذا يعني "جلب الحركات التابعة للمادة رقم {materialId}"
        [HttpGet("materials/{materialId}/transactions")]
        public async Task<IActionResult> GetStockTransactions(int materialId) // depends on adjust stock and goods receipt 
        {
            // 1. إنشاء الـ Query
            var query = new GetStockTransactionsQuery { MaterialId = materialId };

            // 2. إرساله لـ MediatR
            var transactionsList = await Mediator.Send(query);

            // 3. إرجاع النتيجة
            return Ok(transactionsList);
        }
    }
}
