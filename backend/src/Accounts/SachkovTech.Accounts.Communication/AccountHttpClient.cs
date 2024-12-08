using System.Net.Http.Json;
using CSharpFunctionalExtensions;
using SachkovTech.Accounts.Contracts.Responses;

namespace SachkovTech.Accounts.Communication;

public class AccountHttpClient(HttpClient httpClient)
{
    public async Task<Result<ConfirmationLinkResponse, string>> GetConfirmationLink(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post,"confirmation-link/" + userId.ToString());
        
        var response = await httpClient.SendAsync(request, cancellationToken);

        var payload = await response.Content.ReadFromJsonAsync<ResponseWrapper>(cancellationToken)  
                      ?? throw new Exception("ConfirmationLinkResponse can't be null");;
        
        if (!response.IsSuccessStatusCode)
        {
            return payload.Errors.ToString()!;
        }

        var result = new ConfirmationLinkResponse(
            payload.Result.Email,
            payload.Result.ConfirmationLink);
        
        return result;
    }
    
    private class ResponseWrapper
    {
        public ConfirmationLinkResponse Result { get; set; }
        public object Errors { get; set; }
        public DateTime TimeGenerated { get; set; }
    }
}