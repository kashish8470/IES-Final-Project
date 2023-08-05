using assignment5.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

internal class Program
{
    
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("ApplicationDbConext");

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("usersconnection")));

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

        // Add the authorization policy for Manager
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ManagerPolicy", policy =>
                policy.RequireRole("Manager"));
        });

        var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication(); ;

            app.UseAuthorization();

            app.MapRazorPages();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "Admin", "Manager", "User" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string email = "admin@admin.com";
            string password = "Arabella@12";

            if(await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser();
                user.UserName = email;
                user.Email = email;

                await userManager.CreateAsync(user, password);

                await userManager.AddToRoleAsync(user, "Admin");
            }

            string managerEmail = "manager@manager.com";
            string managerPassword = "Manager@2023";

            if (await userManager.FindByEmailAsync(managerEmail) == null)
            {
                var managerUser = new IdentityUser
                {
                    UserName = managerEmail,
                    Email = managerEmail
                };

                await userManager.CreateAsync(managerUser, managerPassword);
                await userManager.AddToRoleAsync(managerUser, "Manager");
            }
        }

        

        app.Run();
        }
    }
