using Avalonia.Controls;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.IO;

namespace JDKSwitcher;

public partial class MainWindow : Window
{
    private List<string> jdks;

    public MainWindow()
    {
        InitializeComponent();
        jdks = JdkFinder.FindJdks();
        foreach (var jdk in jdks)
        {
            JdkComboBox.Items.Add(jdk);
        }
        
        var currentJdk = JdkFinder.GetCurrentJdk();
        string debugInfo = $"Текущий JDK: {currentJdk}\nНайдено JDK:\n" + string.Join("\n", jdks);
        string? match = null;
        if (currentJdk != null)
        {
            foreach (var jdk in jdks)
            {
                if (jdk.EndsWith(Path.GetFileName(currentJdk)))
                {
                    match = jdk;
                    break;
                }
            }
        }
        if (match != null)
        {
            JdkComboBox.SelectedItem = match;
            ConsoleBox.Text = $"Текущий JDK: {match}\n\n{debugInfo}";
        }
        else
        {
            ConsoleBox.Text = $"Текущий JDK не найден в списке!\n\n{debugInfo}";
        }
        SetButton.Click += SetButton_Click;
        JdkComboBox.SelectionChanged += JdkComboBox_SelectionChanged;
    }

    private void JdkComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (JdkComboBox.SelectedItem is string selectedJdk)
        {
            ConsoleBox.Text = $"Выбрано: {selectedJdk}";
        }
    }

    private async void SetButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (JdkComboBox.SelectedItem is string selectedJdk)
        {
            ConsoleBox.Text = $"Смена JDK на: {selectedJdk}...";
            string log = await Task.Run(() => JdkSwitcher.SetJdk(selectedJdk));
            ConsoleBox.Text = $"JDK сменён на: {selectedJdk}\n{log}";
        }
        else
        {
            ConsoleBox.Text = "Сначала выберите JDK!";
        }
    }
}