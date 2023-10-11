using Arc4u.Dependency;
using EG.DemoPCBE99925.ManageCourse.WPF.Common.Events;using Microsoft.Extensions.Logging;
using Prism.Events;
using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Common;

public class VmBase<TResource, TViewModel> : BindableBase
{
    protected VmBase(TResource ressource, IContainerResolve container, ILogger<TViewModel> logger)
    {
        _resource = ressource;
        _eventAggregator = container.Resolve<IEventAggregator>();
        _container = container;
        _logger = logger;

        // When a culture change, we need to update the view.
        _eventAggregator.GetEvent<CultureInfoChangedEvent>().Subscribe(UpdateCulture, ThreadOption.UIThread, false);
    }

    private readonly IContainerResolve _container;
    private readonly ILogger<TViewModel> _logger;

    protected IContainerResolve Container => _container;
    protected ILogger<TResource> Logger;

    protected VmBase(TResource ressource, IEventAggregator eventAggregator, IContainerResolve container, ILogger<TViewModel> logger)
    {
        _resource = ressource;
        _eventAggregator = eventAggregator;
        _container = container;
        _logger = logger;

        // When a culture change, we need to update the view.
        _eventAggregator.GetEvent<CultureInfoChangedEvent>().Subscribe(UpdateCulture, ThreadOption.UIThread, false);
    }

    protected void ValidateProperty(object value, string propertyName)
    {
        ValidationContext validationContext = new ValidationContext(this, null, null) { MemberName = propertyName };
        Validator.ValidateProperty(value, validationContext);
    }



    protected IEventAggregator _eventAggregator;

    public void UpdateCulture(CultureInfo cultureInfo)
    {
        RaisePropertyChanged("Resource");
    }


    private TResource _resource;
    public TResource Resource
    {
        get
        {
            return _resource;
        }
        set
        {
            SetProperty(ref _resource, value);
        }
    }
}