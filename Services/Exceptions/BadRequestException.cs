using System;


namespace ApiClientes.Services.Exceptions
{
    public class BadRequestException : Exception 
    {
        public BadRequestException
         (string message) : base(message) { 
          
        }
    }
}
