using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Services.Interfaces;
using Domain.Entities.Identity;
using Domain.Helper;
using Domain.Repositories;
using Domain.UnitOfWork;

namespace Application.Services.Concrete
{
    public class TokenService : ITokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;

        public TokenService(IGenericRepository<User> userRepository, IUnitOfWork unitOfWork, IGenericRepository<RefreshToken> refreshTokenRepository)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Tuple<string, string>> GenerateTokensAsync(int userId)
        {
            var accessToken = await TokenHelper.GenerateAccessToken(userId);
            var refreshToken = await TokenHelper.GenerateRefreshToken();

            //var userRecord = await _gDContext.Users.Include(o => o.RefreshTokens).FirstOrDefaultAsync(e => e.Id == userId);
            var userRecord = await _userRepository.GetAsync(x => x.Id == userId, x => x, x => x.RefreshTokens);

            if (userRecord == null)
            {
                return null;
            }

            var salt = PasswordHelper.GetSecureSalt();

            var refreshTokenHashed = PasswordHelper.HashUsingPbkdf2(refreshToken, salt);

            var refreshTokens = new List<RefreshToken>() { };
            if (userRecord.RefreshTokens != null && userRecord.RefreshTokens.Any())
            {
                await RemoveRefreshTokenAsync(userRecord);
            }
            refreshTokens.Add(new RefreshToken
            {
                ExpiryDate = DateTime.Now.AddDays(14),
                UserFId = userId,
                TokenHash = refreshTokenHashed,
                TokenSalt = Convert.ToBase64String(salt),
                TS = DateTime.Now,
                CreatedBy = userId.ToString(),
                CreatedAt = DateTime.Now,
            });

            await _refreshTokenRepository.AddAsync(refreshTokens, default);

            await _unitOfWork.CompleteAsync();

            var token = new Tuple<string, string>(accessToken, refreshToken);

            return token;
        }

        public async Task<bool> RemoveRefreshTokenAsync(User user)
        {
            //var userRecord = await _gDContext.Users.Include(o => o.RefreshTokens).FirstOrDefaultAsync(e => e.Id == user.Id);
            var userRecord = await _userRepository.GetAsync(x => x.Id == user.Id, x => x, x => x.RefreshTokens);

            if (userRecord == null)
            {
                return false;
            }

            if (userRecord.RefreshTokens != null && userRecord.RefreshTokens.Any())
            {
                var currentRefreshTokens = userRecord.RefreshTokens.ToList();
                //_gDContext.RefreshTokens.RemoveRange(currentRefreshTokens);
                await _refreshTokenRepository.DeleteAsync(currentRefreshTokens);
                await _unitOfWork.CompleteAsync();
                return true;
            }

            return false;
        }

        public async Task<ValidateRefreshTokenResponse> ValidateRefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
        {
            //var refreshToken = await _gDContext.RefreshTokens.FirstOrDefaultAsync(o => o.UserFId == refreshTokenRequest.UserId);
            var refreshToken = await _refreshTokenRepository.GetAsync(o => o.UserFId == refreshTokenRequest.UserId);

            var response = new ValidateRefreshTokenResponse();
            if (refreshToken == null)
            {
                response.Success = false;
                response.Error = "Invalid session or user is already logged out";
                response.ErrorCode = "invalid_grant";
                return response;
            }

            var refreshTokenToValidateHash = PasswordHelper.HashUsingPbkdf2(refreshTokenRequest.RefreshToken, Convert.FromBase64String(refreshToken.TokenSalt));

            if (refreshToken.TokenHash != refreshTokenToValidateHash)
            {
                response.Success = false;
                response.Error = "Invalid refresh token";
                response.ErrorCode = "invalid_grant";
                return response;
            }

            if (refreshToken.ExpiryDate < DateTime.Now)
            {
                response.Success = false;
                response.Error = "Refresh token has expired";
                response.ErrorCode = "invalid_grant";
                return response;
            }

            response.Success = true;
            response.UserId = refreshToken.UserFId;

            return response;
        }
    }
}
