using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Extensions;


    public static class StringExtensions
    {
        public static string IfNullOrEmpty(this string str, string defaultVal) => string.IsNullOrEmpty(str) ? defaultVal : str;
        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

        public static T Deserialize<T>(this string src) where T:class
        {
            return JsonSerializer.Deserialize<T>(src, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        public static T Deserialize<T>(this string src,JsonSerializerOptions options) where T:class
        {
            return JsonSerializer.Deserialize<T>(src, options);
        }
        public static T DeserializeErrorResponse<T>(this string src) where T:class
        {
            var options=new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
            return JsonSerializer.Deserialize<T>(src,options);
        }
        public static string Base64Encode(this string plainText) {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }       
        public static string Base64Decode(this string base64EncodedData) {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        // method to convert string to int32
        public static int ToInt(this string source)=>
             int.TryParse(source.Trim(), out var result) ? result : 0;
 
        public static string ToShortEnvName(this string env)
        {
            return env switch
            {
                "Development" => "dev",
                "Staging" => "stg",
                "Production" => "prod",
                _ => env
            };
        }
    }
    