using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

public static class AwsV4Signer
{
    public static string SignRequest(string provider, string accessKey, string secretKey, string region, string service, string httpMethod, string canonicalUri, string canonicalQueryString, string payloadHash, DateTime now)
    {
        var date = now.ToString("yyyyMMdd");
        var time = now.ToString("yyyyMMddTHHmmssZ");
        var scope = $"{date}/{region}/{service}/aws4_request";

        var canonicalHeaders = $"host:{service}.{region}.{provider}\nx-amz-date:{time}\n";
        var signedHeaders = "host;x-amz-date";

        var canonicalRequest = $"{httpMethod}\n{canonicalUri}\n{canonicalQueryString}\n{canonicalHeaders}\n{signedHeaders}\n{payloadHash}";

        var stringToSign = $"AWS4-HMAC-SHA256\n{time}\n{scope}\n{ToHexString(SHA256Hash(canonicalRequest))}";

        var signingKey = GetSignatureKey(secretKey, date, region, service);
        var signature = ToHexString(HmacSHA256(stringToSign, signingKey));

        return $"Credential={accessKey}/{scope}, SignedHeaders={signedHeaders}, Signature={signature}";
    }

    private static byte[] GetSignatureKey(string key, string dateStamp, string regionName, string serviceName)
    {
        var kSecret = Encoding.UTF8.GetBytes("AWS4" + key);
        var kDate = HmacSHA256(dateStamp, kSecret);
        var kRegion = HmacSHA256(regionName, kDate);
        var kService = HmacSHA256(serviceName, kRegion);
        return HmacSHA256("aws4_request", kService);
    }

    private static byte[] HmacSHA256(string data, byte[] key)
    {
        using (var hmac = new HMACSHA256(key))
        {
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        }
    }

    public static byte[] SHA256Hash(string data)
    {
        using (var sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
        }
    }

    public static string ToHexString(byte[] data)
    {
        return BitConverter.ToString(data).Replace("-", "").ToLowerInvariant();
    }
}