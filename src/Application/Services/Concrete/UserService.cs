using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Services.Interfaces;
using Domain.Entities.Tables;
using Domain.Helper;
using Domain.UnitOfWork;

namespace Application.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _unitOfWork.UserRepository.GetAsync(x => x.Email == request.Email);

            if (existingUser != null)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Error = "User already exists",
                    ErrorCode = "S02"
                };
            }

            var passwordHash = PasswordHelper.HashPassword(request.Password);

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = passwordHash,
                FirstName = request.FirstName,
                LastName = request.LastName,
                IsActive = true,
                Avatar = request.Avatar ?? "default-avatar-icon.jpg",
                CreatedDateTime = request.CreatedDateTime,
                LastActivityTime = request.LastActivityTime,
                PhoneNumber = request.PhoneNumber,
            };

            await _unitOfWork.UserRepository.AddAsync(user, string.Empty);

            var userRoles = request.RoleIds.Select(roleId => new UserRole
            {
                UserId = user.Id,
                RoleId = roleId
            }).ToList();

            //dbContext.UserRoles.AddRange(userRoles);
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
    }
}
