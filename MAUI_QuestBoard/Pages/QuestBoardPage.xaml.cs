using MAUI_QuestBoard.Models;
using MAUI_QuestBoard.ViewModels;

namespace MAUI_QuestBoard.Pages;

public partial class QuestBoardPage : ContentPage
{
    public QuestBoardPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is QuestBoardViewModel vm)
            await vm.LoadAsync();
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (BindingContext is QuestBoardViewModel vm &&
            e.CurrentSelection.FirstOrDefault() is Event selected)
        {
            vm.SelectEventCommand.Execute(selected);
        }

        // Clear the highlight, otherwise tapping the same event again after
        // coming back does nothing (the selection never "changes").
        if (sender is CollectionView collectionView)
            collectionView.SelectedItem = null;
    }
}
