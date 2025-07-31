using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Services.Interfaces;
using Domain.Entities.Identity;
using Domain.Helper;
using Domain.Repositories;
using Domain.UnitOfWork;

namespace Application.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<User> _userRepository;
        private readonly ITokenService _tokenService;

        public UserService(IUnitOfWork unitOfWork, IGenericRepository<User> userRepository, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetAsync(x => x.Email == request.Email);

            if (existingUser != null)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Error = "User already exists",
                    ErrorCode = "S02"
                };
            }

            var salt = PasswordHelper.GetSecureSalt();
            var passwordHash = PasswordHelper.HashUsingPbkdf2(request.Password, salt);

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = Convert.ToBase64String(salt),
                FirstName = request.FirstName,
                LastName = request.LastName,
                IsActive = true,
                Avatar = request.Avatar ?? "default-avatar-icon.jpg",
                CreatedDateTime = request.CreatedDateTime,
                LastActivityTime = request.LastActivityTime,
                PhoneNumber = request.PhoneNumber,
            };

            await _userRepository.AddAsync(user, string.Empty);

            var userRoles = request.RoleIds.Select(roleId => new UserRole
            {
                UserId = user.Id,
                RoleId = roleId
            }).ToList();

            var saveResponse = await _unitOfWork.CompleteAsync();

            if (saveResponse >= 0)
            {
                return new RegisterResponse { Success = true, Email = user.Email };
            }

            return new RegisterResponse
            {
                Success = false,
                Error = "Error creating new user",
                ErrorCode = "S05"
            };
        }

        public async Task<TokenResponse> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _userRepository.GetAsync(user => user.IsActive == true && user.UserName == loginRequest.UserName);

            if (user == null)
            {
                return new TokenResponse
                {
                    Success = false,
                    Error = "User does not exist!",
                    ErrorCode = "L02"
                };
            }

            var passwordHash = PasswordHelper.HashUsingPbkdf2(loginRequest.Password, Convert.FromBase64String(user.PasswordSalt));

            if (user.PasswordHash != passwordHash)
            {
                return new TokenResponse
                {
                    Success = false,
                    Error = "Invalid password!",
                    ErrorCode = "L03"
                };
            }

            var token = await Task.Run(() => _tokenService.GenerateTokensAsync(user.Id));


            user.LastActivityTime = DateTime.Now;
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                return new TokenResponse { Success = false };
            }

            await _userRepository.UpdateAsync(user, null);

            var saveResponse = await _unitOfWork.CompleteAsync();
            if (saveResponse <= 0)
            {
                return new TokenResponse { Success = false };
            }

            return new TokenResponse
            {
                Success = true,
                AccessToken = token.Item1,
                RefreshToken = token.Item2,
                UserId = user.Id,
                UserName = user.FullName,
                //Role = dbContext.UserRoles.Where(x => x.UserId == user.Id).Select(x => x.Role.Name).FirstOrDefault(),
                ExpirationTime = ((DateTimeOffset)user.LastActivityTime.Value.AddDays(loginRequest.RememberMe ? 60 : 1)).ToUnixTimeSeconds()
            };
        }
    }
}
