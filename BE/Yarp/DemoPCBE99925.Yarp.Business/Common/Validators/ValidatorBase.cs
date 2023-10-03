using Arc4u.FluentValidation;
using Arc4u.ServiceModel;
using FluentValidation;

namespace EG.DemoPCBE99925.Yarp.Business.Common.Validators;

public abstract class ValidatorBase<TElement> : AbstractValidator<TElement>, IValidator where TElement : class
{
    protected ValidatorBase(Messages messages, TElement? instance)
    {
        ArgumentNullException.ThrowIfNull(messages, nameof(messages));

        _messages = messages;
        _instance = instance;
    }

    protected ValidatorBase(Messages messages, ValidationContext<TElement> validationContext)
    {
        ArgumentNullException.ThrowIfNull(messages, nameof(messages));

        _messages = messages;
        _validationContext = validationContext;
    }

    private readonly Messages _messages;
    private readonly TElement? _instance;
    private readonly ValidationContext<TElement>? _validationContext;

    public void Validate()
    {
        var result = _instance is not null ? base.Validate(_instance) : base.Validate(_validationContext);

        _messages.AddRange(result.ToMessages());
    }

    public async Task ValidateAsync()
    {
        var result = _instance is not null ? await ValidateAsync(_instance).ConfigureAwait(false) : await ValidateAsync(_validationContext).ConfigureAwait(false);

        _messages.AddRange(result.ToMessages());
    }
}