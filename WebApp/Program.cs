using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Route53Domains;
using Amazon.Runtime;
using Amazon.WorkMail;
using WebApp.Components;

var builder = WebApplication.CreateBuilder(args);

var akey = builder.Configuration.GetValue<string>("AWS:AccessKey");
var asecret = builder.Configuration.GetValue<string>("AWS:SecretKey");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonRoute53Domains>();
builder.Services.AddAWSService<IAmazonWorkMail>(new AWSOptions()
{
    Credentials = new BasicAWSCredentials(akey, asecret),
    Region = RegionEndpoint.EUWest1
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();