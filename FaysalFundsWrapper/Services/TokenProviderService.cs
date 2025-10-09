using FaysalFundsWrapper.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public sealed class TokenProviderService(IConfiguration configuration)
{
    public string Create(JWTPayload model)
    {
        var secretKey = configuration["Jwt:Secret"];
        if (string.IsNullOrEmpty(secretKey))
            throw new InvalidOperationException("JWT Secret is missing from configuration.");

        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            //new Claim(JwtRegisteredClaimNames.Sub," model.UserId"),
            new Claim(ClaimTypes.NameIdentifier, model.UserId.ToString()),  
            new Claim(ClaimTypes.Name, model.Name),
            new Claim(ClaimTypes.Email, model.Email),
            new Claim(ClaimTypes.MobilePhone, model.PhoneNo),
            new Claim(CustomClaimTypes.CNIC,model.CNIC)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            //Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
            Expires = DateTime.UtcNow.AddHours(configuration.GetValue<int>("Jwt:ExpirationInHours")),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"]
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var secretKey = configuration["Jwt:Secret"];
        if (string.IsNullOrEmpty(secretKey))
            throw new ApiException("JWT Secret is missing from configuration.");

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateLifetime = false // Ignore expiration for refresh
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        var jwtToken = securityToken as JwtSecurityToken;

        if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            //throw new ApiException(ApiErrorCodes.BadRequest,"Invalid token");
            return null;
        return principal;
    }

    public string CreateOtpToken(string email)
    {
        var secretKey = configuration["Jwt:Secret"];
        if (string.IsNullOrEmpty(secretKey))
            throw new ApiException("JWT Secret is missing from configuration.");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim(ClaimTypes.Email, email)
    };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(3), // OTP tokens expire quickly
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"]
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    //public void ValidateOtpToken(string token)
    //{
    //    var secretKey = configuration["Jwt:Secret"];
    //    if (string.IsNullOrEmpty(secretKey))
    //        throw new ApiException("JWT Secret is missing from configuration.");

    //    var tokenValidationParameters = new TokenValidationParameters
    //    {
    //        ValidateAudience = false,
    //        ValidateIssuer = true,
    //        ValidIssuer = configuration["Jwt:Issuer"],
    //        ValidateIssuerSigningKey = true,
    //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
    //        ValidateLifetime = true,
    //        ClockSkew = TimeSpan.Zero // No time buffer for expiration
    //    };

    //    var tokenHandler = new JwtSecurityTokenHandler();
    //    try
    //    {
    //        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
    //        var jwtToken = validatedToken as JwtSecurityToken;

    //        if (jwtToken == null ||
    //            !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
    //        {
    //            throw new ApiException("Invalid OTP token.");
    //        }
    //    }
    //    catch (SecurityTokenExpiredException)
    //    {
    //        throw new ApiException("OTP token has expired.");
    //    }
    //    catch (SecurityTokenException)
    //    {
    //        throw new ApiException("Invalid OTP token.");
    //    }
    //    catch (Exception)
    //    {
    //        throw new ApiException("An error occurred while validating the OTP token.");
    //    }
    //}

    public string ValidateOtpToken(string token)
    {
        var secretKey = configuration["Jwt:Secret"];
        if (string.IsNullOrEmpty(secretKey))
            throw new ApiException("JWT Secret is missing from configuration.");

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            var jwtToken = validatedToken as JwtSecurityToken;

            if (jwtToken == null ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ApiException("Invalid OTP token.");
            }

            var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (emailClaim == null || string.IsNullOrEmpty(emailClaim.Value))
                throw new ApiException("Email claim is missing in OTP token.");

            return emailClaim.Value;
        }
        catch (SecurityTokenExpiredException)
        {
            throw new ApiException("OTP token has expired.");
        }
        catch (SecurityTokenException)
        {
            throw new ApiException("Invalid OTP token.");
        }
        catch (Exception)
        {
            throw new ApiException("An error occurred while validating the OTP token.");
        }
    }

}