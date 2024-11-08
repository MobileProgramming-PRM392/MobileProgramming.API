﻿using AutoMapper;
using Infrastructure.ExternalServices.Authentication;
using MediatR;
using MobileProgramming.Business.Models.DTO.User.ResponseDto;
using MobileProgramming.Business.Models.Enum;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.Models.ResponseMessage;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Authentication.Command.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterHandler(IUserRepository userRepository, IJwtProvider jwtProvider, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var emailcheck = await _userRepository.GetUserByEmailAsync(request.User.Email!);
            if (emailcheck != null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageCommon.EmailAlreadyExist,
                    Data = null
                };
            }
                var userExist = await _userRepository.GetUserByUsernameAsync(request.User.Username);
            if (userExist == null && emailcheck == null)
            {
                User newUser = _mapper.Map<User>(request.User);
                newUser.Role = UserRole.User.ToString();

                string password = request.User.Password;
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                string encodedPassword = Convert.ToBase64String(passwordBytes);
                newUser.PasswordHash = encodedPassword;
                await _userRepository.Add(newUser);
                if (await _unitOfWork.SaveChangesAsync() > 0)
                {
                    UserInfoResponseDto user = _mapper.Map<UserInfoResponseDto>(newUser);
                    user.AccessToken = await _jwtProvider.GenerateAccessToken(user.Username);
                    return new APIResponse
                    {
                        StatusResponse = HttpStatusCode.OK,
                        Message = MessageCommon.CreateSuccesfully,
                        Data = user
                    };
                }
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageCommon.CreateFailed,
                    Data = null
                };
            }
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.UsernameInvalid,
                Data = null
            };
        }
        catch (Exception ex)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageCommon.CreateFailed,
                Data = ex.Message
            };
        }
    }
}
