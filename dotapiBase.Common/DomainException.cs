﻿namespace dotapiBase.Common
{
    public class DomainException : Exception
    {
        public DomainException(string message, string? code = null)
            : base(message)
        {
            Code = code;
        }

        public string? Code { get; }
    }
}
