using System.Windows.Input;
using MAUI_QuestBoard.Models;
using MAUI_QuestBoard.Services;
using MAUI_QuestBoard.Pages;

namespace MAUI_QuestBoard.ViewModels;

public class QuestBoardViewModel : BaseViewModel
{
    public List<Event> Events { get; set; }

    public ICommand SelectEventCommand { get; }

    public QuestBoardViewModel()
    {
        Events = DataService.Events;
        SelectEventCommand = new Command<Event>(OnSelectEvent);
    }

    private async void OnSelectEvent(Event evt)
    {
        await Shell.Current.GoToAsync($"{nameof(EventDetailsPage)}",
            new Dictionary<string, object> { { "Event", evt } });
    }
}
