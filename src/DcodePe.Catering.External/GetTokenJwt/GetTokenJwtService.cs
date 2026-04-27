namespace DcodePe.Catering.External.GetTokenJwt
{
    public class GetTokenJwtService: IGetTokenJwtService
    {
        private readonly IConfiguration _configuration;
        public GetTokenJwtService(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public string Execute(string id)
        {
           var tokenHandler = new JwtSecurityTokenHandler();
            var key = _configuration["Jwt:SecretKeyJwt"] ?? string.Empty;
            var singiKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(singiKey, SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:IssuerJwt"],
                Audience = _configuration["Jwt:AudienceJwt"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
