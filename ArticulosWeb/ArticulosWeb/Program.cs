using ArticulosWeb.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<ArticulosApi>(client =>
{
    client.BaseAddress = new Uri("http://192.168.100.22:5200");
});


var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Articulos}/{action=Index}/{id?}");

app.Run();
