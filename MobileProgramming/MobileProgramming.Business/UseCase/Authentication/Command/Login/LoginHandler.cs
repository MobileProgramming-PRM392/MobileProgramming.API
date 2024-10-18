using AutoMapper;
using Infrastructure.ExternalServices.Authentication;
using MediatR;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Data.Interfaces;
using System.Text;
using MobileProgramming.Business.Models.ResponseMessage;
using MobileProgramming.Business.Models.DTO.User.ResponseDto;
using System.Net;

namespace MobileProgramming.Business.UseCase.Authentication.Command.Login;

public class LoginHandler : IRequestHandler<LoginCommand, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;

    public LoginHandler(IUserRepository userRepository, IJwtProvider jwtProvider, IMapper mapper)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existUser = await _userRepository.GetUserByUsernameAsync(request.Login.username);
            if (existUser == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.Unauthorized,
                    Message = MessageCommon.Unauthorized,
                    Data = null
                };
            }
            string password = request.Login.password;
            byte[] inputPasswordBytes = Encoding.UTF8.GetBytes(password);
            string encodedInputPassword = Convert.ToBase64String(inputPasswordBytes);
            if (encodedInputPassword != existUser.PasswordHash)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.Unauthorized,
                    Message = MessageCommon.Unauthorized,
                    Data = null
                };
            }
            UserInfoResponseDto user = _mapper.Map<UserInfoResponseDto>(existUser);
            user.AccessToken = await _jwtProvider.GenerateAccessToken(user.Email);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.CreateSuccesfully,
                Data = user
            };
        }
        catch (Exception ex)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.Unauthorized,
                Message = MessageCommon.Unauthorized,
                Data = ex.Message
            };
        }
    }
}
