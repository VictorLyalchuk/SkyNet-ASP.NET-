using SkyNet.Core;
using SkyNet.Infrastructure;
using SkyNet.Infrastructure.Initizalizers;

var builder = WebApplication.CreateBuilder(args);

//Create connection string
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
//Database context
builder.Services.AddDbContext(connection);

//Add Infrasturcture servisec
builder.Services.AddInfrastuctureService();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add core services
builder.Services.AddCoreServices();

//Add Mapping 
builder.Services.AddMapping();

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
app.UseAuthorization();

app.UseStatusCodePagesWithRedirects("/Error/{0}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
await UsersAndRolesInitializer.SeedUserAndRole(app);

app.Run();
