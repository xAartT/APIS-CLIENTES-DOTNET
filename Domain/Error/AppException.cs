using System;

namespace App.Domain.Error;

public class AppException(string message, string errorCode = "error", int statusCode = 500) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
    public string ErrorCode { get; } = errorCode;
}