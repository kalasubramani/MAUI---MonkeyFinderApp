
using MonkeyFinder.Services;
using MonkeyFinder.View;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    MonkeyService monkeyService;
    public ObservableCollection<Monkey> Monkeys { get; } = new();

    public MonkeysViewModel(MonkeyService monkeyService)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
    }

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
        }
    }
}