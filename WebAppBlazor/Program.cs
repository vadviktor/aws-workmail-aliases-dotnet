using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Route53Domains;
using Amazon.Runtime;
using Amazon.WorkMail;
using WebAppBlazor.Components;
using WebAppBlazor.Settings;

var builder = WebApplication.CreateBuilder(args);

var awsCredentials = new AwsCredentialsSettings();
builder.Configuration.Bind("AWS", awsCredentials);

var awsRoute53Settings = new AwsRoute53Settings();
builder.Configuration.Bind("AWS:Route53", awsRoute53Settings);

var awsWorkmailSettings = new AwsWorkmailSettings();
builder.Configuration.Bind("AWS:WorkMail", awsWorkmailSettings);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());

builder.Services.AddAWSService<IAmazonRoute53Domains>(new AWSOptions()
{
    Credentials = new BasicAWSCredentials(awsCredentials.AccessKey, awsCredentials.SecretKey),
    Region = RegionEndpoint.GetBySystemName(awsRoute53Settings.Region)
});

builder.Services.AddAWSService<IAmazonWorkMail>(new AWSOptions()
{
    Credentials = new BasicAWSCredentials(awsCredentials.AccessKey, awsCredentials.SecretKey),
    Region = RegionEndpoint.GetBySystemName(awsWorkmailSettings.Region)
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