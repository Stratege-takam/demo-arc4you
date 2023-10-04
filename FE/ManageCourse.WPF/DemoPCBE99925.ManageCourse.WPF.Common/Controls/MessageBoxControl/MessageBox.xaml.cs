using System;
using System.Windows;
using System.Windows.Media;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Common.Controls;

/// <summary>
/// Interaction logic for MessageBox.xaml
/// </summary>
public partial class MessageBox 
{
    public MessageBox()
    {
        InitializeComponent();

        DataContext = new MessageBoxVM();
    }

 

    #region Properties

    #region MessageBoxText

    private string MessageBoxText
    {
        get { return (string)GetValue(MessageBoxTextProperty); }
        set { SetValue(MessageBoxTextProperty, value); }
    }

    private static readonly DependencyProperty MessageBoxTextProperty =
        DependencyProperty.Register("MessageBoxText",
                                    typeof(string),
                                    typeof(MessageBox),
                                    new UIPropertyMetadata(string.Empty));

    #endregion	

    #region MessageBoxButton

    private MessageBoxButton MessageBoxButton
    {
        get { return (MessageBoxButton)GetValue(MessageBoxButtonProperty); }
        set { SetValue(MessageBoxButtonProperty, value); }
    }

    private static readonly DependencyProperty MessageBoxButtonProperty =
        DependencyProperty.Register("MessageBoxButton",
                                    typeof(MessageBoxButton),
                                    typeof(MessageBox),
                                    new UIPropertyMetadata(MessageBoxButton.OK));

    #endregion	

    #region MessageBoxImage

    private MessageBoxImage MessageBoxImage
    {
        get { return (MessageBoxImage)GetValue(MessageBoxImageProperty); }
        set { SetValue(MessageBoxImageProperty, value); }
    }

    private static readonly DependencyProperty MessageBoxImageProperty =
        DependencyProperty.Register("MessageBoxImage",
                                    typeof(MessageBoxImage),
                                    typeof(MessageBox),
                                    new UIPropertyMetadata(MessageBoxImage.None));

    #endregion	

    #region DefaultMessageBoxResult

    private MessageBoxResult DefaultMessageBoxResult
    {
        get { return (MessageBoxResult)GetValue(DefaultMessageBoxResultProperty); }
        set { SetValue(DefaultMessageBoxResultProperty, value); }
    }

    private static readonly DependencyProperty DefaultMessageBoxResultProperty =
        DependencyProperty.Register("DefaultMessageBoxResult",
                                    typeof(MessageBoxResult),
                                    typeof(MessageBox),
                                    new UIPropertyMetadata(MessageBoxResult.Cancel));

    #endregion	

    #region Result

    private MessageBoxResult Result
    {
        get { return (MessageBoxResult)GetValue(ResultProperty); }
        set { SetValue(ResultProperty, value); }
    }

    private static readonly DependencyProperty ResultProperty =
        DependencyProperty.Register("Result",
                                    typeof(MessageBoxResult),
                                    typeof(MessageBox),
                                    new UIPropertyMetadata(MessageBoxResult.Cancel));

    #endregion	

    #region ButtonYesVisibility

    private Visibility ButtonYesVisibility
    {
        get { return (Visibility)GetValue(ButtonYesVisibilityProperty); }
        set { SetValue(ButtonYesVisibilityProperty, value); }
    }

    private static readonly DependencyProperty ButtonYesVisibilityProperty =
        DependencyProperty.Register("ButtonYesVisibility",
                                    typeof(Visibility),
                                    typeof(MessageBox),
                                    new UIPropertyMetadata(Visibility.Collapsed));

    #endregion	

    #region ButtonNoVisibility

    private Visibility ButtonNoVisibility
    {
        get { return (Visibility)GetValue(ButtonNoVisibilityProperty); }
        set { SetValue(ButtonNoVisibilityProperty, value); }
    }

    private static readonly DependencyProperty ButtonNoVisibilityProperty =
        DependencyProperty.Register("ButtonNoVisibility",
                                    typeof(Visibility),
                                    typeof(MessageBox),
                                    new UIPropertyMetadata(Visibility.Collapsed));

    #endregion	

    #region ButtonCancelVisibility

    private Visibility ButtonCancelVisibility
    {
        get { return (Visibility)GetValue(ButtonCancelVisibilityProperty); }
        set { SetValue(ButtonCancelVisibilityProperty, value); }
    }

    private static readonly DependencyProperty ButtonCancelVisibilityProperty =
        DependencyProperty.Register("ButtonCancelVisibility",
                                    typeof(Visibility),
                                    typeof(MessageBox),
                                    new UIPropertyMetadata(Visibility.Collapsed));

    #endregion	

    #region ButtonOkVisibility

    private Visibility ButtonOkVisibility
    {
        get { return (Visibility)GetValue(ButtonOkVisibilityProperty); }
        set { SetValue(ButtonOkVisibilityProperty, value); }
    }

    private static readonly DependencyProperty ButtonOkVisibilityProperty =
        DependencyProperty.Register("ButtonOkVisibility",
                                    typeof(Visibility),
                                    typeof(MessageBox),
                                    new UIPropertyMetadata(Visibility.Collapsed));

    #endregion	

    #region WarningIconVisibility

    private Visibility WarningIconVisibility
    {
        get { return (Visibility)GetValue(WarningIconVisibilityProperty); }
        set { SetValue(WarningIconVisibilityProperty, value); }
    }

    private static readonly DependencyProperty WarningIconVisibilityProperty =
        DependencyProperty.Register("WarningIconVisibility",
                                    typeof(Visibility),
                                    typeof(MessageBox),
                                    new UIPropertyMetadata(Visibility.Collapsed));

    #endregion	

    #region InformationIconVisibility

    private Visibility InformationIconVisibility
    {
        get { return (Visibility)GetValue(InformationIconVisibilityProperty); }
        set { SetValue(InformationIconVisibilityProperty, value); }
    }

    private static readonly DependencyProperty InformationIconVisibilityProperty =
        DependencyProperty.Register("InformationIconVisibility",
                                    typeof(Visibility),
                                    typeof(MessageBox),
                                    new UIPropertyMetadata(Visibility.Collapsed));

    #endregion	

    #region ErrorIconVisibility

    private Visibility ErrorIconVisibility
    {
        get { return (Visibility)GetValue(ErrorIconVisibilityProperty); }
        set { SetValue(ErrorIconVisibilityProperty, value); }
    }

    private static readonly DependencyProperty ErrorIconVisibilityProperty =
        DependencyProperty.Register("ErrorIconVisibility",
                                    typeof(Visibility),
                                    typeof(MessageBox),
                                    new UIPropertyMetadata(Visibility.Collapsed));

    #endregion	

    #region QuestionIconVisibility

    private Visibility QuestionIconVisibility
    {
        get { return (Visibility)GetValue(QuestionIconVisibilityProperty); }
        set { SetValue(QuestionIconVisibilityProperty, value); }
    }

    private static readonly DependencyProperty QuestionIconVisibilityProperty =
        DependencyProperty.Register("QuestionIconVisibility",
                                    typeof(Visibility),
                                    typeof(MessageBox),
                                    new UIPropertyMetadata(Visibility.Collapsed));

    #endregion	

    #region IsYesButtonDefault

    private bool IsYesButtonDefault
    {
        get { return (bool)GetValue(IsYesButtonDefaultProperty); }
        set { SetValue(IsYesButtonDefaultProperty, value); }
    }

    private static readonly DependencyProperty IsYesButtonDefaultProperty =
        DependencyProperty.Register("IsYesButtonDefault",
                                    typeof(bool),
                                    typeof(MessageBox),
                                    new UIPropertyMetadata(false));

    #endregion	

    #region IsNoButtonDefault

    private bool IsNoButtonDefault
    {
        get { return (bool)GetValue(IsNoButtonDefaultProperty); }
        set { SetValue(IsNoButtonDefaultProperty, value); }
    }

    private static readonly DependencyProperty IsNoButtonDefaultProperty =
        DependencyProperty.Register("IsNoButtonDefault",
                                    typeof(bool),
                                    typeof(MessageBox),
                                    new UIPropertyMetadata(false));

    #endregion	

    #region IsOkButtonDefault

    private bool IsOkButtonDefault
    {
        get { return (bool)GetValue(IsOkButtonDefaultProperty); }
        set { SetValue(IsOkButtonDefaultProperty, value); }
    }

    private static readonly DependencyProperty IsOkButtonDefaultProperty =
        DependencyProperty.Register("IsOkButtonDefault",
                                    typeof(bool),
                                    typeof(MessageBox),
                                    new UIPropertyMetadata(false));

    #endregion	

    #region IsCancelButtonDefault

    private bool IsCancelButtonDefault
    {
        get { return (bool)GetValue(IsCancelButtonDefaultProperty); }
        set { SetValue(IsCancelButtonDefaultProperty, value); }
    }

    private static readonly DependencyProperty IsCancelButtonDefaultProperty =
        DependencyProperty.Register("IsCancelButtonDefault",
                                    typeof(bool),
                                    typeof(MessageBox),
                                    new UIPropertyMetadata(false));

    #endregion	

    #endregion

    #region Private Methods

    private MessageBoxResult ShowInternal()
    {
        ToogleButtonsVisibility(MessageBoxButton);
        ToogleIconsVisibility(MessageBoxImage);
        ToogleDefaultButton(DefaultMessageBoxResult);
        this.ShowDialog();

        return Result;
    }

    private void ToogleDefaultButton(MessageBoxResult defaultMessageBoxResult)
    {
        switch (defaultMessageBoxResult)
        {
            case MessageBoxResult.None:
                IsYesButtonDefault = false;
                IsNoButtonDefault = false;
                IsOkButtonDefault = false;
                IsCancelButtonDefault = false;
                break;
            case MessageBoxResult.OK:
                IsYesButtonDefault = false;
                IsNoButtonDefault = false;
                IsOkButtonDefault = true;
                IsCancelButtonDefault = false;
                break;
            case MessageBoxResult.Cancel:
                IsYesButtonDefault = false;
                IsNoButtonDefault = false;
                IsOkButtonDefault = false;
                IsCancelButtonDefault = true;
                break;
            case MessageBoxResult.No:
                IsYesButtonDefault = false;
                IsNoButtonDefault = true;
                IsOkButtonDefault = false;
                IsCancelButtonDefault = false;
                break;
            case MessageBoxResult.Yes:
                IsYesButtonDefault = true;
                IsNoButtonDefault = false;
                IsOkButtonDefault = false;
                IsCancelButtonDefault = false;
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void ToogleIconsVisibility(MessageBoxImage messageBoxImage)
    {
        switch (messageBoxImage)
        {
            case MessageBoxImage.Information:
                InformationIconVisibility = Visibility.Visible;
                ErrorIconVisibility = Visibility.Collapsed;
                QuestionIconVisibility = Visibility.Collapsed;
                WarningIconVisibility = Visibility.Collapsed;
                this.Icon = FindResource("SmallInformationSchield") as ImageSource;
                break;
            case MessageBoxImage.Error:
                InformationIconVisibility = Visibility.Collapsed;
                ErrorIconVisibility = Visibility.Visible;
                QuestionIconVisibility = Visibility.Collapsed;
                WarningIconVisibility = Visibility.Collapsed;
                this.Icon = FindResource("SmallErrorSchield") as ImageSource;
                break;
            case MessageBoxImage.Warning:
                InformationIconVisibility = Visibility.Collapsed;
                ErrorIconVisibility = Visibility.Collapsed;
                QuestionIconVisibility = Visibility.Collapsed;
                WarningIconVisibility = Visibility.Visible;
                this.Icon = FindResource("SmallWarningSchield") as ImageSource;
                break;
            case MessageBoxImage.Question:
                InformationIconVisibility = Visibility.Collapsed;
                ErrorIconVisibility = Visibility.Collapsed;
                QuestionIconVisibility = Visibility.Visible;
                WarningIconVisibility = Visibility.Collapsed;
                this.Icon = FindResource("SmallQuestionSchield") as ImageSource;
                break;
            case MessageBoxImage.None:
                InformationIconVisibility = Visibility.Collapsed;
                ErrorIconVisibility = Visibility.Collapsed;
                QuestionIconVisibility = Visibility.Collapsed;
                WarningIconVisibility = Visibility.Collapsed;
                this.Icon = null;
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void ToogleButtonsVisibility(MessageBoxButton messageBoxButton)
    {
        switch(messageBoxButton)
        {
            case MessageBoxButton.OK:
                ButtonOkVisibility = Visibility.Visible;
                ButtonNoVisibility = Visibility.Collapsed;
                ButtonCancelVisibility = Visibility.Collapsed;
                ButtonYesVisibility = Visibility.Collapsed;
                break;
            case MessageBoxButton.OKCancel:
                ButtonOkVisibility = Visibility.Visible;
                ButtonNoVisibility = Visibility.Collapsed;
                ButtonCancelVisibility = Visibility.Visible;
                ButtonYesVisibility = Visibility.Collapsed;
                break;
            case MessageBoxButton.YesNo:
                ButtonOkVisibility = Visibility.Collapsed;
                ButtonNoVisibility = Visibility.Visible;
                ButtonCancelVisibility = Visibility.Collapsed;
                ButtonYesVisibility = Visibility.Visible;
                break;
            case MessageBoxButton.YesNoCancel:
                ButtonOkVisibility = Visibility.Collapsed;
                ButtonNoVisibility = Visibility.Visible;
                ButtonCancelVisibility = Visibility.Visible;
                ButtonYesVisibility = Visibility.Visible;
                break;
        }
    }

    #endregion

    #region Static Methods

    public static MessageBoxResult Show(string messageBoxText)
    {
        return Show(null, messageBoxText, string.Empty, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
    }

    public static MessageBoxResult Show(string messageBoxText, string caption)
    {
        return Show(null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
    }

    public static MessageBoxResult Show(Window owner, string messageBoxText)
    {
        return Show(owner, messageBoxText, string.Empty, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
    }

    public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button)
    {
        return Show(null, messageBoxText, string.Empty, button, MessageBoxImage.None, GetDefaultMessageBoxResult(button));
    }

    public static MessageBoxResult Show(Window owner, string messageBoxText, string caption)
    {
        return Show(owner, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
    }

    public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
    {
        return Show(null, messageBoxText, caption, button, icon, GetDefaultMessageBoxResult(button));
    }

    public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button)
    {
        return Show(owner, messageBoxText, string.Empty, button, MessageBoxImage.None, GetDefaultMessageBoxResult(button));
    }

    public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult)
    {
        return Show(null, messageBoxText, caption, button, icon, defaultResult);
    }

    public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
    {
        return Show(owner, messageBoxText, caption, button, icon, GetDefaultMessageBoxResult(button));
    }

    public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult)
    {
            var result = Application.Current.Dispatcher.Invoke(() => {
                var messageBox = new MessageBox
                {
                    Owner = owner,
                    MessageBoxText = messageBoxText,
                    Title = caption,
                    MessageBoxButton = button,
                    MessageBoxImage = icon,
                    DefaultMessageBoxResult = defaultResult
                };
                return messageBox.ShowInternal();
        });
        
        return result;
    }

    private static MessageBoxResult GetDefaultMessageBoxResult(MessageBoxButton button)
    {
        switch (button)
        {
            case MessageBoxButton.OK:
            case MessageBoxButton.OKCancel:
                return MessageBoxResult.OK;
            case MessageBoxButton.YesNo:
            case MessageBoxButton.YesNoCancel:
                return MessageBoxResult.Yes;
            default:
                throw new NotImplementedException();
        }
    }

    #endregion

    private void MessageBox_Click(object sender, RoutedEventArgs e)
    {
        if (e.Source == yes)
            Result = MessageBoxResult.Yes;
        else if (e.Source == no)
            Result = MessageBoxResult.No;
        else if (e.Source == ok)
            Result = MessageBoxResult.OK;
        else if (e.Source == cancel)
            Result = MessageBoxResult.Cancel;
        else
            throw new NotImplementedException();

        this.Close();
    }
}