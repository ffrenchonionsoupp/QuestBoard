using MAUI_QuestBoard.Models;
using MAUI_QuestBoard.ViewModels;

namespace MAUI_QuestBoard.Pages;

public partial class MyAdventuresPage : ContentPage
{
    public MyAdventuresPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MyAdventuresViewModel vm)
            await vm.LoadAsync();
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (BindingContext is MyAdventuresViewModel vm &&
            e.CurrentSelection.FirstOrDefault() is Event selected)
        {
            vm.SelectEventCommand.Execute(selected);
        }
    }
}
