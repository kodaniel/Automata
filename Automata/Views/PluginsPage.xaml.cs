﻿using Automata.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Automata.Views;

public sealed partial class PluginsPage : Page
{
    public PluginsViewModel ViewModel
    {
        get;
    }

    public PluginsPage()
    {
        ViewModel = App.GetService<PluginsViewModel>();
        InitializeComponent();
    }
}
