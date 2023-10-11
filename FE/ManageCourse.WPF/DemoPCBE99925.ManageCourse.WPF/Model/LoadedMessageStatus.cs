using System;
using System.ComponentModel;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Model;

public class LoadedMessageStatus : INotifyPropertyChanged
{
    private string _key;
    public String Key
    {
        get
        {
            return _key;
        }
        set
        {
            _key = value;
            NotifyPropertyChanged("Key");
        }
    }

    private bool _isDone;
    public bool IsDone
    {
        get
        {
            return _isDone;
        }
        set
        {
            _isDone = value;
            NotifyPropertyChanged("IsDone");
        }
    }

    private String _message;
    public String Message
    {
        get
        {
            return _message;
        }
        set
        {
            _message = value;
            NotifyPropertyChanged("Message");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(String info)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}