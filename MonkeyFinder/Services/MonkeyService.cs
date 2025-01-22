
using System.Net.Http.Json;


namespace MonkeyFinder.Services;

public class MonkeyService
{
    HttpClient httpClient;

    public MonkeyService()
    {
        httpClient = new HttpClient();
    }

    List<Monkey> monkeyList = new();
    //implement methods to get monkeys data from internet
    public async Task<List<Monkey>> GetMonkeys()
    {
        if(monkeyList?.Count > 0)
        return monkeyList;

        //get monkey data
        var url = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/MonkeysApp/monkeydata.json";

        var response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            //reads JSON data and deserializes it to a list 
            monkeyList = await response.Content.ReadFromJsonAsync<List<Monkey>>();
        }
        return monkeyList;
    }
}
