using System;
using System.Collections.Generic;
using System.Net;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;
using APIUtility.Constants;
using APIUtility.Models;
using JWT.Exceptions;

namespace APIUtility.Helpers
{
    public class DBManagerUtility
    {
        // Mark: This method creates and encode Json Web Token
        public static string encodeJWT(Dictionary<string, object> payload)
        {

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var token = encoder.Encode(payload, StringConstants.Key.JWTSecretKey);
            Console.WriteLine(token);
            return token;
        }

        // Mark: This method decode and returns Json Web Token
        public static object decodeJWT(string token)
        {
            try
            {
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                var json = decoder.Decode(token, StringConstants.Key.JWTSecretKey, verify: true);
                JWTPayload payload = JsonConvert.DeserializeObject<JWTPayload>(Convert.ToString(json));
                return payload;
            }
            catch (TokenExpiredException)
            {
                ErrorResponse error = new ErrorResponse(StringConstants.Message.TokenExpired, HttpStatusCode.BadRequest);
                return error;
            }
            catch (SignatureVerificationException)
            {
                ErrorResponse error = new ErrorResponse(StringConstants.Message.TokenInvalidSignature, HttpStatusCode.BadRequest);
                return error;
            }
        }
    }
}
