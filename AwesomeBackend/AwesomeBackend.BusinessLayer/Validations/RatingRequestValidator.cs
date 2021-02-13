using AwesomeBackend.Shared.Models.Requests;
using FluentValidation;
using System;

namespace AwesomeBackend.BusinessLayer.Validations
{
    public class RatingRequestValidator : AbstractValidator<RatingRequest>
    {
        public RatingRequestValidator()
        {
            RuleFor(r => r.Score).InclusiveBetween(1, 5).WithMessage("Score must be between 1 and 5");
            RuleFor(r => r.VisitedAt.Date).LessThanOrEqualTo(DateTime.Now.Date).WithMessage("You can't specify a future date");
        }
    }
}
