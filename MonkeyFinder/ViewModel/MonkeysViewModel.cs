
using MonkeyFinder.Services;
using MonkeyFinder.View;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    MonkeyService monkeyService;
    public ObservableCollection<Monkey> Monkeys { get; } = new();

    //dependency injection - builtin connectivity service - an abstraction on top of native APIs
    IConnectivity connectivity;
    IGeolocation geolocation;

    public MonkeysViewModel(MonkeyService monkeyService,IConnectivity connectivity,IGeolocation geolocation)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
        this.connectivity = connectivity;
        this.geolocation = geolocation; 
    }

    [ObservableProperty]
    bool isRefreshing;

    [RelayCommand]
    async Task GoToDetailsAsync(Monkey monkey)
    {
        if (monkey is null) return;

        //do navigation
        await Shell.Current.GoToAsync($"{nameof(DetailsPage)}",true,
            new Dictionary<string, object> {
                {"Monkey",monkey }
            });
    }

    [RelayCommand]//glues the method to UI to load data
    async Task GetMonkeysAsync()
    {
        if (IsBusy) return;

        try
        {
            //check internet connectivity before accessing service
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                //notify user
                await Shell.Current.DisplayAlert("Internet issue!", $"Check your internet and try again!", "OK");
                return;
            }

            IsBusy = true;
            var monkeys = await monkeyService.GetMonkeys();
            if (Monkeys.Count != 0) Monkeys.Clear();

            foreach (var monkey in monkeys)
                Monkeys.Add(monkey);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            //notify user
            await Shell.Current.DisplayAlert("Error!", $"Unable to get monkeys:{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;//bcoz isbusy will be false while refreshing
        }
    }


    [RelayCommand]
    async Task GetClosestMonkey()
    {
        if (IsBusy || Monkeys.Count==0) return;

        try
        {
            //can check and grant permission for the app to use the device location
            //Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>

           //use cached loc, if it is not current, then fetch current location
           var location = await geolocation.GetLastKnownLocationAsync();
            if(location is null)
            {
                location = await geolocation.GetLocationAsync(
                        new GeolocationRequest
                        {
                            DesiredAccuracy = GeolocationAccuracy.Medium,
                            Timeout = TimeSpan.FromSeconds(30)
                        }
                    );                    
            }

            //defensive check
            if (location is null) return;

            //load monkeys 
            //order them by closest to the location
            var first = Monkeys.OrderBy(m => location.CalculateDistance(
                m.Latitude, m.Longitude, DistanceUnits.Miles)).FirstOrDefault();

            if(first is null) return;

            //display the closest monkey
            await Shell.Current.DisplayAlert("Closest Monkey", $"{first.Name} in {first.Location}", "OK");

        }catch(Exception ex)
        {
            Debug.WriteLine(ex);
            //notify user
            await Shell.Current.DisplayAlert("Error!", $"Unable to get closest monkey: {ex.Message}", "OK");
        }
    }
}