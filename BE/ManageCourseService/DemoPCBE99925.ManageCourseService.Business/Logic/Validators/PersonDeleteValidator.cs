using Arc4u.FluentValidation;
using Arc4u.ServiceModel;
using EG.DemoPCBE99925.ManageCourseService.Business.Common.Validators;
using EG.DemoPCBE99925.ManageCourseService.Domain;
using FluentValidation;

namespace EG.DemoPCBE99925.ManageCourseService.Business.Logic.Validators;

internal class PersonDeleteValidator : ValidatorBase<Person>
{
    public PersonDeleteValidator(Messages messages, Person entity)
        : base(messages, entity)
    {
        RuleFor(e => e.Id).NotEmpty();
        RuleFor(e => e.PersistChange).IsDelete();
    }
}