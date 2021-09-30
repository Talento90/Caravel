using System;
using Caravel.MediatR.Security;
using FluentValidation;
using MediatR;

namespace Caravel.MediatR.Tests.Behaviours
{
    public class TestDataResponse
    {
        public Guid? Id { get; set; }
        public string Data { get; set; }
    }
    
    [Authorize(Roles = "admin", Policy = "CanRead")]
    public class GetTestDataQuery : IRequest<TestDataResponse>
    {
        public Guid Id { get; set; }
        public string Query { get; set; }
        
        public class GetTestDataQueryValidator : AbstractValidator<GetTestDataQuery>
        {
            public GetTestDataQueryValidator()
            {
                RuleFor(t => t.Id).NotEmpty();
                RuleFor(t => t.Query)
                    .NotEmpty()
                    .Must(q => !string.IsNullOrEmpty(q) && q.StartsWith("q_"))
                    .MinimumLength(3);
            }
        }
    }
}