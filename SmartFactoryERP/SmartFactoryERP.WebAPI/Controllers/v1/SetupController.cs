using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartFactoryERP.Domain.Common;
using SmartFactoryERP.Domain.Entities.Identity;
using System.Threading.Tasks;

namespace SmartFactoryERP.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SetupController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // هنا بنحقن الأدوات اللي محتاجينها عشان نكلم الداتابيز مباشرة
        public SetupController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("initialize-admin")]
        public async Task<IActionResult> InitializeAdmin()
        {
            // 1. التأكد من وجود كل الرولز في النظام
            // لو الرول مش موجودة، بنكريتها
            foreach (var roleName in Roles.AllRoles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. إنشاء المستخدم الـ Super Admin
            var adminEmail = "superadmin@smartfactory.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Super Factory Admin",
                    EmailConfirmed = true,
                    EmployeeId = null
                };

                // باسوورد قوية عشان تعدي الـ Validation
                var result = await _userManager.CreateAsync(adminUser, "P@ssword123!");

                if (result.Succeeded)
                {
                    // 3. إعطاء الصلاحيات الكاملة
                    await _userManager.AddToRoleAsync(adminUser, Roles.SuperAdmin);
                    await _userManager.AddToRoleAsync(adminUser, Roles.Admin);

                    return Ok(new { Message = "Done! Super Admin Created.", Email = adminEmail, Password = "P@ssword123!" });
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }

            return BadRequest("Admin user already exists!");
        }
    }
}