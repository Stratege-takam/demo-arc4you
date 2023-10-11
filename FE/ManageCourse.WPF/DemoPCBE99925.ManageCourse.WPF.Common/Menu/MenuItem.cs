using Arc4u.Security.Principal;
using Prism.Commands;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Common.Menu
{
	[DataContract(Namespace = "urn:menuItem")]
    public class MenuItem : INotifyPropertyChanged
    {
        private bool _isChecked;
        private bool _isEnabled = true;
        private readonly MenuItemsCollection _items;

        public MenuItem()
        {
            _items = new MenuItemsCollection(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">The fully qualified name of the menu. A dot naming structure from the root menu.</param>
        /// <returns></returns>
        public MenuItem FindItem(string key)
        {
            var name = GetFullyQualifiedName();

            if (String.Compare(name, key, StringComparison.InvariantCultureIgnoreCase) == 0)
                return this;

            return Items.FindItem(key);
        }

        private String GetFullyQualifiedName()
        {
            var menu = this;
            var name = menu.Key;
            var parent = menu.Parent;
            while (null != parent && null != parent.Key)
            {
                name = parent.Key + "." + menu.Key;
                parent = parent.Parent;
            }

            return name;
        }

        /// <summary>
        /// The default action command is used to attach the action to the MenuCommand. This is useful during deserialization where a common behaviour
        /// can be attached!
        /// </summary>
        public static Action<MenuItem> DefaultActionCommand { get; set; }
        

        private Action<MenuItem> _actionCommand;

        /// <summary>
        /// The action to execute when a user click on the menu.
        /// </summary>
        public Action<MenuItem> ActionCommand
        {
            get
            {
                return _actionCommand;
            }
            set
            {
               _actionCommand = value;
                if (null != _actionCommand)
                    MenuCommand = new DelegateCommand<MenuItem>(_actionCommand);
                else
                {
                    if (Items.Count == 0)
                        MenuCommand = new DelegateCommand<MenuItem>(DefaultActionCommand);
                }
            }
        }

        /// <summary>
        /// The parent item in the hierarchical structure.
        /// </summary>
        public MenuItem Parent { get; set; }

        /// <summary>
        /// The command used to bind the command from the MenuItem control.
        /// </summary>
        public ICommand MenuCommand { get; set; }

        /// <summary>
        /// Gets or sets the keys used to control if a user has access to the menu. If no access exist, the item is not displayed.
        /// </summary>
        [DataMember(Order = 1, EmitDefaultValue = false)]
        public String Access { get; set; }

        /// <summary>
        /// A group name used to check only on item in a collection of menu having the same group name.
        /// </summary>
        [DataMember(Order = 2, EmitDefaultValue = false)]
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the optional absolute or relative path to the action's icon image. 
        /// </summary>
        [DataMember(Order = 3, EmitDefaultValue = false)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Load the image specified from the string url property.
        /// </summary>
        public Image Image
        {
            get
            {
                if (null != ImageUrl)
                {

                    var copyimage = new BitmapImage();
                    copyimage.BeginInit();
                    var myUri = new Uri(ImageUrl, UriKind.RelativeOrAbsolute);
                    copyimage.UriSource = myUri;
                    copyimage.EndInit();

                    // I assume we seek a 16 by 16 pixels image (or icon).
                    var iconImage = new Image { Source = copyimage, Width = 16, Height = 16 };

                    return iconImage;
                }

                return null;
            }
        }

        /// <summary>
        /// Get or sets the text describing an input gesture that will call the command tied to the specified item.
        /// </summary>
        [DataMember(Order = 4, EmitDefaultValue = false)]
        public String InputGestureText { get; set; }

        /// <summary>
        /// Gets a value that indicates whether a <see cref="System.Windows.Controls.MenuItem"/> can be checked.
        /// </summary>
        [DataMember(Order = 5, EmitDefaultValue = false)]
        public bool IsCheckable { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the <see cref="System.Windows.Controls.MenuItem"/> is checked.
        /// </summary>
        [DataMember(Order = 6, EmitDefaultValue = false)]
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                if (value != _isChecked)
                {
                    _isChecked = value;
                    OnPropertyChanged("IsChecked");

                    if (!string.IsNullOrEmpty(GroupName))
                    {
                        if (IsChecked)
                        {
                            UncheckOtherItemsInGroup();
                        }
                        else
                        {
                            IsChecked = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this element is enabled in the user interface (UI). 
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    OnPropertyChanged("IsEnabled");
                }
            }
        }
        private bool _isSeparator;
        /// <summary>
        /// if true, the element will be displayed as a <see cref="Separator"/> Separator UI element.
        /// </summary>
        [DataMember(Order = 7, EmitDefaultValue = false)]
        public bool IsSeparator
        {
            get
            {
                return _isSeparator;
            }
            set
            {
                _isSeparator = value;
                if (value)
                    Tag = "MenuItemSeparator";
            }
        }

        /// <summary>
        /// The header text displayed in the menu.
        /// </summary>
        [DataMember(Order = 8, EmitDefaultValue = false)]
        public string Text { get; set; }

        private String _key;

        /// <summary>
        /// Each item should have a key allowing to search the menu in the collections of items.
        /// </summary>
        [DataMember(Order = 9, EmitDefaultValue = false)]
        public String Key
        {
            get
            {
                return IsSeparator ? null : _key;
            }
            set { _key = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates that the submenu in which this <see cref="System.Windows.Controls.MenuItem"/> is located should not close when this item is clicked
        /// </summary>
        [DataMember(Order = 10, EmitDefaultValue = false)]
        public bool StaysOpenOnClick { get; set; }


        private String _toolTip;
        /// <summary>
        /// Gets or sets the tool-tip object that is displayed for this element in the user interface (UI). 
        /// </summary>
        [DataMember(Order = 11, EmitDefaultValue = false)]
        public String ToolTip
        {
            get
            {
                return _toolTip;
            }
                
            set
            {
                if (value != _toolTip)
                {
                    _toolTip = value;
                    OnPropertyChanged("ToolTip");
                }
            }
        }

        [DataMember(Order = 12, EmitDefaultValue = false, IsRequired = true)]
        public String ResourceType { get; set; }

        [DataMember(Order = 100, Name = "MenuItems", EmitDefaultValue = false)]
        public MenuItemsCollection Items
        {
            get
            {
                return _items;
            }
        }

		public String Tag { get; set; }

        private void UncheckOtherItemsInGroup()
        {
            var groupItems = Parent.Items.Where(item => item != null).Cast<MenuItem>().Where(item => item.GroupName == GroupName);
            foreach (var item in groupItems)
            {
                if (item != this)
                {
                    item._isChecked = false;
                    item.OnPropertyChanged("IsChecked");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
