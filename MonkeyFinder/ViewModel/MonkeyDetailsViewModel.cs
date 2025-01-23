

namespace MonkeyFinder.ViewModel;

[QueryProperty("Monkey", "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
    IMap map;
    public MonkeyDetailsViewModel(IMap map)
    {
        this.map = map;
    }

    [ObservableProperty]
    Monkey monkey;

    [RelayCommand]
    async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task OpenMapAsync()
    {
        try
        {
            //open map and display monkey name and loc at the bottom
            await map.OpenAsync(Monkey.Latitude, Monkey.Longitude,
                new MapLaunchOptions
                {
                    Name = Monkey.Name,
                    NavigationMode = NavigationMode.None
                });
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            //notify user
            await Shell.Current.DisplayAlert("Error!", $"Unable to open map: {ex.Message}", "OK");
        }
    }
}
