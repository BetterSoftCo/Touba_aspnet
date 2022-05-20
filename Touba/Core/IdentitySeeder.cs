using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Touba.Core;

namespace Anbor.Core {
    public class AdminUser {
        public AdminUser(
            string u,
            string p,
            string f,
            string m) {
            UserName = u;
            Password = p;
            FullName = f;
            Mobile = m;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
    }

    public class IdentitySeeder {
        private readonly AppDbContext _ctx;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public static readonly List<AdminUser> AdminList = new() {
            new AdminUser("nimasoftco@gmail.com", "Test@123*", "برنامه نویس", "09125760915"),
            new AdminUser("nimasoftco@gmail.com", "Test@123*", "برنامه نویس", "09125760915"),
        };

        public static readonly List<IdentityRole> Roles = new() {
            new IdentityRole("SuperAdmin"),
            new IdentityRole("Admin"),
            new IdentityRole("Manager"),
            new IdentityRole("Support"),
        };

        private IServiceProvider _serviceProvider;

        public IdentitySeeder(
            IServiceProvider serviceProvider,
            AppDbContext ctx,
            UserManager<UserEntity> userManager,
            RoleManager<IdentityRole> roleManager) {
            _ctx = ctx;
            _userManager = userManager;
            _roleManager = roleManager;
            _serviceProvider = serviceProvider;
        }

        public async Task Seed() {
            RoleManager<IdentityRole>? RoleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            UserManager<UserEntity>? UserManager = _serviceProvider.GetRequiredService<UserManager<UserEntity>>();
            IdentityResult roleResult;

            foreach (IdentityRole? role in Roles) {
                try {
                    bool roleExist = RoleManager.RoleExistsAsync(role.Name).Result;
                    if (!roleExist) {
                        //create the roles and seed them to the database: Question 1  
                        roleResult = await RoleManager.CreateAsync(new IdentityRole(role.Name));
                    }
                }
                catch (Exception e) {
                    Debug.WriteLine(e.Message);
                }
            }

            _ctx.Database.EnsureCreated();

            //todo:اصلاح
            foreach (AdminUser? admin in AdminList) {
                if (!_ctx.Users.Any(x => x.UserName == admin.UserName)) {
                    UserEntity? user = new() {
                        Id = Guid.NewGuid().ToString(),
                        Email = admin.UserName,
                        UserName = admin.UserName,
                        FullName = admin.FullName,
                        Wallet = 100000000,
                        EmailConfirmed = true,
                        PhoneNumber = "",
                        Suspend = false
                    };

                    IdentityResult? result = await _userManager.CreateAsync(user, admin.Password);
                    if (result.Succeeded) {
                        user.EmailConfirmed = true;
                        await _userManager.UpdateAsync(user);
                        foreach (IdentityRole? r in Roles) {
                            try {
                                if (!UserManager.IsInRoleAsync(user, r.Name).Result) {
                                    await UserManager.AddToRoleAsync(user, r.Name);
                                }
                            }
                            catch { }
                        }
                    }
                }
                else {
                    UserEntity? user = await _userManager.FindByNameAsync(admin.UserName);
                    foreach (IdentityRole? r in Roles) {
                        try {
                            if (!UserManager.IsInRoleAsync(user, r.Name).Result) {
                                await UserManager.AddToRoleAsync(user, r.Name);
                            }
                        }
                        catch { }
                    }
                }
            }
        }
    }
}