
namespace MonkeyFinder.ViewModel;

public class BaseViewModel : INotifyPropertyChanged
{
    bool isBusy;
    string title;
    public bool IsBusy { 
        get => isBusy;
        set {
            if (isBusy == value) return;
            isBusy = value;
            OnPropertyChanged("IsBusy");
            }
    }
    //subscribe to event
    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(name));
    }
}
