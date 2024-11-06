using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.Models.ResponseMessage
{
    public static class MessageCommon
    {
        public const string GetSuccesfully = "GET_SUCCESSFULLY";
        public const string GetFailed = "GET_FAILED";
        public const string SavingSuccesfully = "SAVING_SUCCESSFULLY";
        public const string SavingFailed = "SAVING_FAILED";
        public const string DeleteSuccessfully = "DELETE_SUCCESFULLY";
        public const string DeleteFailed = "DELETE_EVENT_FAIL";
        public const string UpdateSuccesfully = "UPDATE_SUCCESSFULLY";
        public const string UpdateFailed = "UPDATE_FAILED";
        public const string CreateSuccesfully = "CREATED_SUCCESSFULLY";
        public const string CreateFailed = "CREATED_FAILED";

        public const string NotFound = "NOT_FOUND";
        public const string Complete = "COMPLETE";

        public const string ServerError = "SOMETHINGS WENT WRONG";
        public const string SessionTimeout = "SESSION_TIMEOUT";

        public const string ReturnListHasValue = "LIST_HAS_VALUE";
        public const string lockAcquired = "LOCK_ACQUIRED";

        public const string Unauthorized = "UNAUTHORIZED";
        public const string Blocked = "USER_BLOCKED";
        public const string InvalidToken = "INVALID_TOKEN";
        public const string LogInFailed = "INVALID_USERNAME_OR_PASSWORD";
        public const string EmailAlreadyExist = "EMAIL_ALREADY_EXIST";
        public const string UsernameInvalid = "INVALID_USERNAME";
    }
}
