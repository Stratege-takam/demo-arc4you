using Arc4u.FluentValidation;
using Arc4u.ServiceModel;
using EG.DemoPCBE99925.ManageCourseService.Business.Common.Validators;
using EG.DemoPCBE99925.ManageCourseService.Domain;
using FluentValidation;

namespace EG.DemoPCBE99925.ManageCourseService.Business.Logic.Validators;

internal class StudentSaveValidator : ValidatorBase<Student>
{
	public StudentSaveValidator(Messages messages, Student entity)
		: base(messages, entity)
	{
		RuleFor(e => e.Id).NotEmpty();
		RuleFor(e => e.PersistChange).IsInsert().Unless(ValidatorPredicates.IsUpdate);
		RuleFor(e => e.PersistChange).IsUpdate().Unless(ValidatorPredicates.IsInsert);

		RuleFor(e => e.AuditedBy).NotNull().NotEmpty().MaximumLength(50);
		RuleFor(e => e.AuditedOn).IsUtcDateTime();


        RuleFor(e => e.FirstName).NotNull().NotEmpty().MaximumLength(255);
        RuleFor(e => e.Matricule).NotNull().NotEmpty().MaximumLength(50);
        RuleFor(e => e.LastName);
    }
}
