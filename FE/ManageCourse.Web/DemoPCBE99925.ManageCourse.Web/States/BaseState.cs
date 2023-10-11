using EG.DemoPCBE99925.ManageCourse.Web.ViewModels;

namespace EG.DemoPCBE99925.ManageCourse.Web.States;

public abstract class BaseState: NotifyChangeProperty
{
    public event Action OnChange;

    public override void RaisePropertyChanged(string propertyName)
    {
        base.RaisePropertyChanged(propertyName);
        OnChange?.Invoke();
    }

    public void RaisePropertyChanged()
    {
        OnChange?.Invoke();
    }

    public void UnMont(Action action)
    {
        base.UnMont();
        OnChange -= action;
    }

    public void Mont(Action action)
    {
        OnChange += action;
    }
}
