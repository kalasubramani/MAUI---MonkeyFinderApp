namespace MonkeyFinder.View;

public partial class DetailsPage : ContentPage
{
	public Monkey Monkey { get; set; }	
	public DetailsPage(MonkeyDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext=viewModel;
	}

	//set monkey details
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}