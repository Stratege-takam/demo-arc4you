using EG.DemoPCBE99925.ManageCourse.WPF.Common.Controls.MessageBoxControl;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Common.Controls;

public class MessageBoxVM
{
    private MessageBoxRes _resources;
    public MessageBoxRes Resources
    {
        get
        {
            if (_resources == null)
                _resources = new MessageBoxRes();

            return _resources;
        }
    }
}