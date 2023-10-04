using System;
using EG.DemoPCBE99925.ManageCourse.WPF.Common.Menu;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Contracts;

public interface IMenuMgr
{
    MenuItemsCollection RootMenu { get; set; }

    MenuItemsCollection RightMenu { get; set; }

	MenuItemsCollection Load(Uri menu);

    void LoadMenu();
}