using MAUI_QuestBoard.Models;
using MAUI_QuestBoard.ViewModels;

namespace MAUI_QuestBoard.Pages;

public partial class MyQuestsPage : ContentPage
{
    public MyQuestsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MyQuestsViewModel vm)
            await vm.LoadAsync();
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (BindingContext is MyQuestsViewModel vm &&
            e.CurrentSelection.FirstOrDefault() is Event selected)
        {
            vm.SelectEventCommand.Execute(selected);
        }

        // Clear the highlight so the same event can be opened again later.
        if (sender is CollectionView collectionView)
            collectionView.SelectedItem = null;
    }
}
