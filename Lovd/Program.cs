using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Lovd.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Lovd.Models;
using _3psp;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("LovdContextConnection") ?? throw new InvalidOperationException("Connection string 'LovdContextConnection' not found.");
builder.Services.AddDbContext<LoveContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<IdentityUser>
    (options => options.SignIn.RequireConfirmedAccount = true
    ).AddRoles<IdentityRole>().AddRoleManager<RoleManager<IdentityRole>>().AddEntityFrameworkStores<LoveContext>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
    hubOptions.KeepAliveInterval = System.TimeSpan.FromMinutes(5);
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); 
app.MapRazorPages();
app.UseAuthorization();
app.Use((context, next) =>
{
    Thread.CurrentPrincipal = context.User;
    return next(context);
});
app.MapControllerRoute (
     name: " ",
     pattern: "{controller=Article}/{action=DifferentsArticles}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<CommentsHub>("/chat");
});
app.Run();
