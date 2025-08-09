using System;
using System.Collections.Generic;

namespace SyndicApp.Application.DTOs.Residences
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string[] Errors { get; set; } = Array.Empty<string>();
        public T? Data { get; set; }

        public static Result<T> Ok(T data) => new() { Success = true, Data = data };

        public static Result<T> Fail(List<string> errors) => new() { Success = false, Errors = errors.ToArray() };

        public static Result<T> Fail(string error) => new() { Success = false, Errors = new[] { error } };
    }

}