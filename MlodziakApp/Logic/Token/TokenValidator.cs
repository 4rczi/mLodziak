using Microsoft.IdentityModel.Tokens;
using MlodziakApp.ApiRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Logic.Token
{
    public class TokenValidator : ITokenValidator
    {
        private readonly IAccessTokenRequests _accessTokenRequests;

        public TokenValidator(IAccessTokenRequests accessTokenRequests)
        {
            _accessTokenRequests = accessTokenRequests;
        }

        public async Task<(bool isSuccess, HttpStatusCode statusCode)> ValidateAccessTokenAsync(string accessToken, string userId)
        {
            var validationResult = await _accessTokenRequests.ValidateAccessTokenAsync(accessToken);
            return validationResult;          
        }

        public bool ValidateRefreshToken(string refreshToken)
        {
            if (refreshToken != null)
            {
                return true;
            }

            return false;
        }
    }
}
