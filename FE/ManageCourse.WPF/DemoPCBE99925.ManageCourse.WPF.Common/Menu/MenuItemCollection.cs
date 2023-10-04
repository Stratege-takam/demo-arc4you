using Arc4u.Security.Principal;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Common.Menu
{
    [CollectionDataContract(Name = "MenuItems" ,Namespace = "urn:menuItem")]
    public class MenuItemsCollection : ObservableCollection<MenuItem>
    {
        public MenuItemsCollection() : this(null)
        {
        }

        public MenuItemsCollection(MenuItem parent)
        {
            Parent = parent;
        }

        [DataMember(EmitDefaultValue = false)]
        public MenuItem Parent { get; set; }


		/// <summary>
        /// Search in the collection of MenuItems (Items) the MenuItem based on the fully qualified name.
        /// The fully qualified name of the menu item. Based on the concatenation of the MenuItem.Key property of each item.
        /// </summary>
        /// <param name="key">The fully qualified name of the item to search.</param>
        /// <returns>The menuItem found or null if nothing is found.</returns>
        public MenuItem FindItem(string key)
        {
            foreach(MenuItem item in Items)
            {
                var name = GetFullyQualifiedName(item);

                if (string.Compare(name, key, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return item;
                }

                var result = item.Items.FindItem(key);
                
                if (null != result)
                {
                    return result;
                }
            }

            return null;
        }


        private static String GetFullyQualifiedName(MenuItem menu)
        {
            var name = menu.Key;
            var parent = menu.Parent;
            while (null != parent && null != parent.Key)
            {
                name = parent.Key + "." + menu.Key;
                parent = parent.Parent;
            }

            return name;
        }

        protected override void InsertItem(int index, MenuItem item)
        {
            item.Parent = Parent;
            base.InsertItem(index, item);
        }
                
        /// <summary>
        /// Add the MenuItem after the MenuItem.Key found in the collection of <see cref="MenuItem"/>.
        /// If no <see cref="MenuItem"/> is found, the insertion is done at the end of the collection. Otherwhise just after the item found.
        /// </summary>
        /// <param name="key">The key to find in the collection of <see cref="MenuItem"/>. The child <see cref="MenuItem"/> are not considered.</param>
        /// <param name="menu">The <see cref="MenuItem"/> to add.</param>
        public void AddAfter(String key, MenuItem menu)
        {
            MenuItem result = null;

            foreach (MenuItem item in Items)
            {
                var name = item.Key;

                if (string.Compare(name, key, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    result = item;
                    break;
                }
            }

            if (null != result)
            {
                var idx = IndexOf(result);
                Insert(idx + 1, menu);
                return;
            }

            Insert(Count, menu);
        }

        /// <summary>
        /// Add the MenuItem after the MenuItem.Key found in the collection of <see cref="MenuItem"/>.
        /// If no <see cref="MenuItem"/> is found, the insertion is done at the start of the collection. Otherwhise just before the item found.
        /// </summary>
        /// <param name="key">The key to find in the collection of <see cref="MenuItem"/>. The child <see cref="MenuItem"/> are not considered.</param>
        /// <param name="menu">The <see cref="MenuItem"/> to add.</param>
        public void AddBefore(String key, MenuItem menu)
        {
            MenuItem result = null;

            foreach (MenuItem item in Items)
            {
                var name = item.Key;

                if (string.Compare(name, key, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    result = item;
                    break;
                }
            }

            if (null != result)
            {
                var idx = IndexOf(result);
                Insert(idx, menu);
                return;
            }

            Insert(0, menu);
            

        }

		/// <summary>
        /// Add a <see cref="MenuItem"/> at the end of the existing collections of MenuItems.
        /// </summary>
        /// <param name="menu">The <see cref="MenuItem"/> to add.</param>
        public new void Add(MenuItem menu)
        {
            base.Add(menu);
        }
        public void SaveMenu(String file)
        {
            if (File.Exists(file))
                File.Delete(file);
            
            var save = new DataContractSerializer(typeof (MenuItemsCollection));
            save.WriteObject(file, this);

        }
    }

}
