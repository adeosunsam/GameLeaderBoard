using System.Net.Http.Headers;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;

namespace Infrastructure.Utility
{
    public class Result<T>
    {

        public string ResponseMessage { get; set; }
        public string ResponseCode { get; set; }
        public T? Data { get; set; }

        /*public Result(string responseCode, string responseMessage, T data)
        {
            Data = data;
            ResponseCode = responseCode;
            ResponseMessage = responseMessage;
        }*/
        public Result()
        {
        }
        public static Result<T> Fail(string responseMessage, string responseCode)
        {
            return new Result<T> { ResponseMessage = responseMessage, ResponseCode = responseCode };
        }
        public static Result<T> Success(string responseMessage, string responseCode = "200", T? data = default)
        {
            return new Result<T> { ResponseMessage = responseMessage, Data = data, ResponseCode = responseCode };
        }

    }
}


/*[HttpGet]
[Route("~/auth/google")]
public async Task<IActionResult> GoogleAuth()
{
    try
    {

        string scopeURL1 = "https://accounts.google.com/o/oauth2/auth?redirect_uri={0}&prompt={1}&response_type={2}&client_id={3}&scope={4}&access_type={5}";
        var redirectURL = "https://localhost:7153/auth/callback";
        string prompt = "consent";
        string response_type = "code";
        string clientID = "416931340373-ji6tnk93rf5lmrllsr7c7eov78ng0j9g.apps.googleusercontent.com";
        string scope = "https://www.googleapis.com/auth/userinfo.email";
        string access_type = "offline";
        string redirect_uri_encode = urlEncodeForGoogle(redirectURL);
        var mainURL = string.Format(scopeURL1, redirect_uri_encode, prompt, response_type, clientID, scope, access_type);

        return Ok(mainURL);
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}


private static string urlEncodeForGoogle(string url)
{
    string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-.~";
    StringBuilder result = new StringBuilder();
    foreach (char symbol in url)
    {
        if (unreservedChars.IndexOf(symbol) != -1)
        {
            result.Append(symbol);
        }
        else
        {
            result.Append("%" + ((int)symbol).ToString("X2"));
        }
    }

    return result.ToString();
}

[HttpGet]
[Route("~/auth/callback")]
public async Task<IActionResult> Callback()
{
    string code = HttpContext.Request.Query["code"];

    var clientId = "416931340373-ji6tnk93rf5lmrllsr7c7eov78ng0j9g.apps.googleusercontent.com";
    string clientSecret = "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";
    var redirectURL = "https://localhost:7153/auth/callback";
    var tokenEndpoint = "https://accounts.google.com/o/oauth2/token";
    var content = new StringContent($"code={code}&redirect_uri={Uri.EscapeDataString(redirectURL)}&client_id={clientId}&client_secret={clientSecret}&grant_type=authorization_code", Encoding.UTF8, "application/x-www-form-urlencoded");

    var response = await _httpClient.PostAsync(tokenEndpoint, content);
    var responseContent = await response.Content.ReadAsStringAsync();

    if (response.IsSuccessStatusCode)
    {
        var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleTokenResponse>(responseContent);

        var userEmail = await GetEmail(tokenResponse.access_token);

        return Ok(userEmail);
    }
    else
    {
        // Handle the error case when authentication fails
        return BadRequest($"Failed to authenticate: {responseContent}");
    }
}
private async Task<string> GetEmail(string accessToken)
{
    string json = string.Empty;
    string url = $"https://www.googleapis.com/oauth2/v2/userinfo?fields=email";

    using (var client = new HttpClient())
    {
        // set the access token at the request header
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        HttpResponseMessage response = await client.GetAsync(url);
        json = await response.Content.ReadAsStringAsync();
    }
    return json;
}


public class GoogleTokenResponse
{
    public string access_token
    {
        get;
        set;
    }

    public long expires_in
    {
        get;
        set;
    }

    public string refresh_token
    {
        get;
        set;
    }

    public string scope
    {
        get;
        set;
    }

    public string token_type
    {
        get;
        set;
    }
}*/

