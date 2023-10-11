using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EG.DemoPCBE99925.ManageCourse.Web.ViewModels;

public class NotifyChangeProperty
{
    public event PropertyChangedEventHandler PropertyChanged;

    private List<string> _computedProps = new();

    public NotifyChangeProperty()
    {
        Mont();
    }

    public virtual void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Checks if a property already matches a desired value. Sets the property and
    /// notifies listeners only when necessary. (origin: Prism)
    /// </summary>
    /// <typeparam name="T">Type of the property.</typeparam>
    /// <param name="storage">Reference to a property with both getter and setter.</param>
    /// <param name="value">Desired value for the property.</param>
    /// <param name="propertyName">Name of the property used to notify listeners. This
    /// value is optional and can be provided automatically when invoked from compilers that
    /// support CallerMemberName.</param>
    /// <returns>True if the value was changed, false if the existing value matched the
    /// desired value.</returns>
    protected virtual bool SetProperty<T>(ref T storage, T value,
        [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

        storage = value;
        RaisePropertyChanged(propertyName);

        return true;
    }



    public void SetComputedProps(object o, PropertyChangedEventArgs e)
    {
        if (_computedProps.Contains(e.PropertyName)) return;
        foreach (var prop in _computedProps)
            RaisePropertyChanged(prop);
    }


    public void InvokeMethod(Action action)
    {
        action?.Invoke();
    }

    public virtual void UnMont()
    {
        PropertyChanged -= SetComputedProps;
    }

    public virtual void Mont()
    {
        // pour les propriétés calculées
        PropertyChanged += SetComputedProps;
    }
}
