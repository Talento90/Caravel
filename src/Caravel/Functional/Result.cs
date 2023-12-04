using System.Collections.Generic;
using System.Linq;
using Caravel.Errors;

namespace Caravel.Functional
{
    public class Result
    {
        private readonly List<Error> _errors;
        public IEnumerable<Error> Errors => _errors.AsReadOnly();
        public Error Error => _errors.First();
        public bool HasErrors => _errors.Any();

        public Result()
        {
            _errors = new List<Error>();
        }
        private Result(Error error)
        {
            _errors = new List<Error> {error};
        }

        private Result(IEnumerable<Error> errors)
        {
            _errors = errors.ToList();
        }

        public Result AddError(Error error)
        {
            _errors.Add(error);
            return this;
        }

        public static Result Success() => new();
        public static Result Failure(Error error) => new(error);
        public static Result Failure(IEnumerable<Error> errors) => new(errors);
    }

    public class Result<T>
    {
        public T Data { get; } = default!;
        private readonly List<Error> _errors;
        public IEnumerable<Error> Errors => _errors.AsReadOnly();
        public Error Error => _errors.First();
        public bool HasErrors => _errors.Any();

        private Result(T data)
        {
            Data = data;
            _errors = new List<Error>();
        }

        private Result(Error error)
        {
            _errors = new List<Error> {error};
        }

        private Result(IEnumerable<Error> errors)
        {
            _errors = errors.ToList();
        }

        public Result<T> AddError(Error error)
        {
            _errors.Add(error);
            return this;
        }

        public static Result<T> Success(T value) => new(value);
        public static Result<T> Failure(Error error) => new(error);
        public static Result<T> Failure(IEnumerable<Error> errors) => new(errors);
    }
}