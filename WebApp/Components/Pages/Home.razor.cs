using System.ComponentModel.DataAnnotations;
using Amazon.WorkMail.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WebApp.Settings;
using Amazon.Route53Domains;
using Amazon.WorkMail;
using Microsoft.AspNetCore.Components.Forms;

namespace WebApp.Components.Pages;

public partial class Home : ComponentBase
{
    private List<string>? _aliases;
    private IEnumerable<string>? _filteredAliases;
    private List<string>? _domains;
    private readonly CancellationTokenSource _cts = new();
    private readonly AwsWorkmailSettings _workmailSettings = new();
    private readonly AwsRoute53Settings _route53Settings = new();
    private int _aliasLength = 16;
    private string _showDeleteChoicesFor = string.Empty;
    private string _newlyCreatedAlias = string.Empty;
    private EditContext? _searchEditContext;
    private readonly Helpers.FlashMessage _flashMessage = new();
    [SupplyParameterFromForm] public EmailAddress? AliasModel { get; set; }
    [SupplyParameterFromForm] public AliasFilter? SearchModel { get; set; }

    [Inject] public IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] public IConfiguration Configuration { get; set; } = null!;
    [Inject] public IAmazonWorkMail WorkMailClient { get; set; } = null!;
    [Inject] public IAmazonRoute53Domains Route53DomainsClient { get; set; } = null!;
    [Inject] public NavigationManager Navigation { get; set; } = null!;

    public class EmailAddress
    {
        [Required] public string? Domain { get; set; }

        [Required] public string? Mailbox { get; set; }
    }

    public class AliasFilter
    {
        [Required]
        [MinLength(3)]
        public string? Alias { get; set; }
    }

    protected override async Task OnInitializedAsync()
    {
        AliasModel ??= new EmailAddress();
        SearchModel ??= new AliasFilter();
        _searchEditContext = new EditContext(SearchModel);
        await GetAliasesAsync();

        var domainResponse = await Route53DomainsClient.ListDomainsAsync(_cts.Token);
        _domains = domainResponse.Domains.Select(d => d.DomainName).ToList();
    }

    private async Task GetAliasesAsync()
    {
        Configuration.Bind("AWS:WorkMail", _workmailSettings);
        Configuration.Bind("AWS:Route53", _route53Settings);

        var request = new ListAliasesRequest
        {
            EntityId = _workmailSettings.UserId,
            OrganizationId = _workmailSettings.OrgId
        };

        var aliasResponse = await WorkMailClient.ListAliasesAsync(request, _cts.Token);
        _aliases = aliasResponse.Aliases;
    }

    private async Task CreateAlias()
    {
        var newAlias = $"{AliasModel!.Mailbox}@{AliasModel!.Domain}";
        var request = new CreateAliasRequest
        {
            Alias = newAlias,
            EntityId = _workmailSettings.UserId,
            OrganizationId = _workmailSettings.OrgId
        };

        try
        {
            await WorkMailClient.CreateAliasAsync(request, _cts.Token);
            _newlyCreatedAlias = newAlias;
            _flashMessage.SuccessClickable = $"Alias <b>{newAlias}</b> has been created. Click to copy to clipboard";
            AliasModel = new EmailAddress();
            SearchModel = new AliasFilter();
            await GetAliasesAsync();
        }
        catch (Exception ex)
        {
            _flashMessage.Error = ex.Message;
        }
    }

    private void SearchAlias()
    {
        _filteredAliases = (_aliases ?? []).Where(a => a.Contains(SearchModel!.Alias!));
    }

    private void ClearSearcForm()
    {
        SearchModel!.Alias = string.Empty;
        _filteredAliases = null;
    }

    private async Task DeleteAlias(string alias)
    {
        var request = new DeleteAliasRequest
        {
            Alias = alias,
            EntityId = _workmailSettings.UserId,
            OrganizationId = _workmailSettings.OrgId
        };

        try
        {
            await WorkMailClient.DeleteAliasAsync(request, _cts.Token);
            _flashMessage.Info = $"Alias <b>{alias}</b> has been deleted";
            await GetAliasesAsync();
            // scroll to the top of the page
            Navigation.NavigateTo("#");
        }
        catch (Exception ex)
        {
            _flashMessage.Error = ex.Message;
        }
    }

    private async Task CopyAliasToClipboard(string text)
    {
        var result = await JsRuntime.InvokeAsync<IDictionary<string, string>>("copyToClipboard", _cts.Token, text);
        switch (result["type"])
        {
            case "error":
                _flashMessage.Error = result["message"];
                break;
            default:
                _flashMessage.Info = result["message"];
                break;
        }
    }

    private void GenerateRandomAlias()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        var stringChars = new char[_aliasLength];
        var random = new Random();

        for (var i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        AliasModel!.Mailbox = new string(stringChars);
    }
}