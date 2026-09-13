namespace MAUI_QuestBoard.Pages;

public partial class QuestBoardPage : ContentPage
{
    public QuestBoardPage()
    {
        InitializeComponent();
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Handle the selection change logic here
        var selectedEvent = e.CurrentSelection.FirstOrDefault();
        // Example: Display the selected event
        Console.WriteLine(selectedEvent);
    }
}