using EG.DemoPCBE99925.ManageCourse.WPF.Controls.Helpers;
using Microsoft.Win32;
using System.Windows;
using Telerik.Windows.Controls;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Controls;
public class ImportButton : RadButton
{
    static ImportButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ImportButton), new FrameworkPropertyMetadata(typeof(ImportButton)));
    }

    protected override void OnClick()
    {
        base.OnClick();

        // Create SaveFileDialog 
        var dlg = new OpenFileDialog();

        // Set filter for file extension and default file extension 
        dlg.DefaultExt = ".csv";
        dlg.Filter = "Csv document (*.csv)|*.csv";
        dlg.Title = "Open data to from csv...";
        var hasSuffix = !string.IsNullOrWhiteSpace(FileNameSuffix);

        // Display SaveFileDialog by calling ShowDialog method 
        bool? result = dlg.ShowDialog(this.GetParentWindow());

        // Get the selected file name and save Excel content
        if (result == true)
        {
            RaiseClicked(dlg.FileName);
        }
    }

    #region Properties

    #region FileNameSuffix
    public static readonly DependencyProperty FileNameSuffixProperty = DependencyProperty.Register(
        "FileNameSuffix", typeof(string), typeof(ImportButton), new PropertyMetadata(default(string)));

    public string FileNameSuffix
    {
        get { return (string)GetValue(FileNameSuffixProperty); }
        set { SetValue(FileNameSuffixProperty, value); }
    }
    #endregion
    #endregion

    #region Events
    #region ExcelClicked
    public delegate void ClickedRoutedEventHandler(object sender, ImportClickedEventArgs e);

    public static readonly RoutedEvent ClickedEvent =
        EventManager.RegisterRoutedEvent(
        "Clicked",
        RoutingStrategy.Bubble,
        typeof(ClickedRoutedEventHandler),
        typeof(ImportButton));

    public event ClickedRoutedEventHandler Clicked
    {
        add { AddHandler(ClickedEvent, value); }
        remove { RemoveHandler(ClickedEvent, value); }
    }
    public void RaiseClicked(string filename)
    {
        ImportClickedEventArgs args = new ImportClickedEventArgs(ClickedEvent, filename);
        RaiseEvent(args);
    }
    #endregion
    #endregion

}

public class ImportClickedEventArgs : RoutedEventArgs
{
    private readonly string _filename;
    public string Filename
    {
        get { return _filename; }
    }

    public ImportClickedEventArgs(RoutedEvent routedEvent, string filename)
        : base(routedEvent)
    {
        _filename = filename;
    }
}
