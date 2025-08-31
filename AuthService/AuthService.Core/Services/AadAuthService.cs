using System.Security.Claims;
using AuthService.API.DTO;
using AuthService.Core.DTO;
using AuthService.Core.Entities;
using AuthService.Core.Interfaces;

namespace AuthService.Core.Services;

public class AadAuthService(ITokenService tokenService, IUserRepository userRepository) : IAadAuthService
{
    /// <summary>
    /// Exchanges AAD token for internal JWT and syncs user to DB
    /// </summary>
    public async Task<ExchangeResponse?> ExchangeTokenAsync(ClaimsPrincipal aadPrincipal)
    {
        var sub = aadPrincipal.FindFirst("oid")?.Value
                  ?? aadPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? "unknown";

        var email = aadPrincipal.FindFirst("upn")?.Value
                    ?? aadPrincipal.FindFirst("unique_name")?.Value
                    ?? aadPrincipal.FindFirst(ClaimTypes.Email)?.Value;

        var name = aadPrincipal.FindFirst("name")?.Value;

        var roles = aadPrincipal.FindAll("roles").Select(c => c.Value)
            .Concat(aadPrincipal.FindAll(ClaimTypes.Role).Select(c => c.Value))
            .Distinct()
            .ToList();

        if (!roles.Any())
            return null;
        
        // Sync user to DB
         userRepository.GetOrCreateAadUserAsync(sub, email, roles);

        var claims = new List<Claim>
        {
            new Claim("id", sub),
            new Claim("email", email),
            new Claim(type:"name",name)
        };

        claims.AddRange(roles.Select(r => new Claim("role", r)));

        var internalToken = tokenService.CreateInternalJwt(sub, claims);

        return new ExchangeResponse(
            internalToken,
            DateTime.UtcNow.AddMinutes(tokenService.GetExpiryMinutes()),
            sub,
            email,
            roles
        );
    }

    // ================== UNUSED / COMMENTED CODE FOR REFERENCE ==================
    /*
    private readonly IConfiguration _config;
    private readonly IConfigurationManager<OpenIdConnectConfiguration> _configurationManager;
    private readonly string _issuerBase;
    private readonly string _audience;

    public AadAuthService(IConfiguration config)
    {
        _config = config;
        var azure = _config.GetSection("AzureAd");
        // var instance = azure.GetValue<string>("Instance")?.TrimEnd('/') ??
        //                throw new ArgumentException("AzureAd:Instance missing");
        // var tenant = azure.GetValue<string>("TenantId") ?? throw new ArgumentException("AzureAd:TenantId missing");
        var instance = "https://login.microsoftonline.com";
        var tenant = "564a5d09-beec-49b6-9496-b481f3c331bd";
        _audience = "api://87f32d9d-633c-4e64-a860-6d8e43cdc83e";
        // _audience = azure.GetValue<string>("Audience") ?? throw new ArgumentException("AzureAd:Audience missing");

        _issuerBase = $"{instance}/{tenant}/v2.0";

        // OpenID Connect metadata endpoint
        var metadataAddress = $"{_issuerBase}/.well-known/openid-configuration";

        _configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            metadataAddress,
            new OpenIdConnectConfigurationRetriever());
    }

    public async Task<AadUser> ValidateAccessTokenAsync(string aadAccessToken, CancellationToken cancellation = default)
    {
        if (string.IsNullOrWhiteSpace(aadAccessToken))
            throw new ArgumentException("AAD token is empty", nameof(aadAccessToken));

        if (aadAccessToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            aadAccessToken = aadAccessToken.Substring("Bearer ".Length).Trim();

        var oidcConfig = await _configurationManager.GetConfigurationAsync(cancellation).ConfigureAwait(false);

        var validationParameters = new TokenValidationParameters
        {
            ValidIssuer = _issuerBase,
            ValidateIssuer = true,
            ValidAudience = _audience,
            ValidateAudience = true,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKeys = oidcConfig.SigningKeys,
        };

        var handler = new JwtSecurityTokenHandler();

        try
        {
            var principal = handler.ValidateToken(aadAccessToken, validationParameters, out var validatedToken);
            return BuildClaimsFromPrincipal(principal);
        }
        catch (SecurityTokenExpiredException ex)
        {
            throw new SecurityTokenValidationException("AAD token expired", ex);
        }
        catch (SecurityTokenException ex)
        {
            throw new SecurityTokenValidationException("AAD token validation failed", ex);
        }
    }
    */
}