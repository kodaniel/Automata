// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Automata.ViewModels.Fields;

namespace Automata.Views;
public sealed partial class EditorControl : UserControl
{
    public static readonly DependencyProperty EditorProperty =
        DependencyProperty.Register(nameof(Editor), typeof(Editor), typeof(EditorControl), new PropertyMetadata(null));

    public Editor Editor
    {
        get => (Editor)GetValue(EditorProperty);
        set => SetValue(EditorProperty, value);
    }

    public EditorControl()
    {
        InitializeComponent();


    }

    private void ExpressionTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        // TODO: highlight expression variables
    }
}
