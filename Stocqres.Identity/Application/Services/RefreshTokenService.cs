﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Stocqres.Core.Authentication;
using Stocqres.Core.Exceptions;
using Stocqres.Identity.Domain;
using Stocqres.Identity.Repositories;

namespace Stocqres.Identity.Application.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordHasher<Domain.User> _passwordHasher;
        private readonly IJwtHandler _jwtHandler;

        public RefreshTokenService(IUserRepository userRepository, 
            IRefreshTokenRepository refreshTokenRepository, 
            IPasswordHasher<User> passwordHasher, IJwtHandler jwtHandler)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _passwordHasher = passwordHasher;
            _jwtHandler = jwtHandler;
        }
        public async Task CreateAsync(Guid userId)
        {
            var user = await _userRepository.GetUserAsync(userId);
            if (user == null)
            {
                throw new StocqresException("UserCodes does not exist.");
            }

            await _refreshTokenRepository.CreateAsync(new RefreshToken(user, _passwordHasher));
        }

        public async Task<JsonWebToken> CreateAccessTokenAsync(string token)
        {
            var refreshToken = await _refreshTokenRepository.FindAsync(x=>x.Token == token);
            if (refreshToken == null)
            {
                throw new StocqresException("Refresh token was not found");
            }

            if (refreshToken.Revoked)
            {
                throw new StocqresException($"Refresh token: {refreshToken.Id} was revoked.");
            }

            var user = await _userRepository.FindAsync(x => x.Id == refreshToken.UserId);
            if (user == null)
            {
                throw new StocqresException("UserCodes does not exist.");
            }

            var jwt = _jwtHandler.CreateToken(user.Id.ToString());

            jwt.RefreshToken = refreshToken.Token;

            return jwt;
        }
        public async Task RevokeAsync(string token, Guid userId)
        {
            var refreshToken = await _refreshTokenRepository.FindAsync(x=>x.Token == token);
            if (refreshToken == null || refreshToken.UserId != userId)
            {
                throw new StocqresException("Refresh token was not found.");
            }
            refreshToken.Revoke();
            _refreshTokenRepository.Update(refreshToken);
        }
    }
}
