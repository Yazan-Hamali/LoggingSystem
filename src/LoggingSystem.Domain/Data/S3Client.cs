using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

public class S3Client:DomainService
{
    private HttpClient _httpClient;
    private readonly string provider; //amazonaws.com
    private readonly string accessKey;
    private readonly string secretKey;
    private readonly string region;
    private readonly string bucketName;

    public S3Client(IConfiguration conf) 
    {
        _httpClient = new HttpClient();
        provider = conf["BucketConf:provider"];
        accessKey = conf["BucketConf:accessKey"];
        secretKey = conf["BucketConf:secretKey"];
        bucketName = conf["BucketConf:bucketName"];
        region = conf["BucketConf:region"];
    }

    public async Task UploadObjectAsync(string objectKey,string content)
    {
        var now = DateTime.UtcNow;
        var payloadHash = AwsV4Signer.ToHexString(AwsV4Signer.SHA256Hash(content));
        var canonicalUri = $"/{objectKey}";
        var canonicalQueryString = "";

        var authorization = AwsV4Signer.SignRequest(provider,accessKey, secretKey, region, "s3", "PUT", canonicalUri, canonicalQueryString, payloadHash, now);

        var request = new HttpRequestMessage(HttpMethod.Put, $"https://{bucketName}.s3.{region}.{provider}/{objectKey}")
        {
            Content = new StringContent(content, Encoding.UTF8, "application/octet-stream")
        };

        request.Headers.Add("x-amz-date", now.ToString("yyyyMMddTHHmmssZ"));
        request.Headers.Add("x-amz-content-sha256", payloadHash);
        request.Headers.Authorization = new AuthenticationHeaderValue("AWS4-HMAC-SHA256", authorization.ToString());
        try
        {
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
        catch(Exception e) { }
        
    }

    public async Task<string> GetObjectAsync(string objectKey)
    {
        var now = DateTime.UtcNow;
        var payloadHash = AwsV4Signer.ToHexString(AwsV4Signer.SHA256Hash(""));
        var canonicalUri = $"/{objectKey}";
        var canonicalQueryString = "";

        var authorization = AwsV4Signer.SignRequest(provider,accessKey, secretKey, region, "s3", "GET", canonicalUri, canonicalQueryString, payloadHash, now);

        var request = new HttpRequestMessage(HttpMethod.Get, $"https://{bucketName}.s3.{region}.{provider}/{objectKey}");
        request.Headers.Add("x-amz-date", now.ToString("yyyyMMddTHHmmssZ"));
        request.Headers.Add("x-amz-content-sha256", payloadHash);
        request.Headers.Authorization = new AuthenticationHeaderValue("AWS4-HMAC-SHA256", authorization.ToString());
        
        try
        {
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            return "[]";
            throw;
        }
        
        
    }
}