using SAIMMOD3;

var tactsCount = 1000000;

var lambda = 0.5;

var isQueueBusy = false;

var states = new Dictionary<string, int>()
{
    { "2000", 1 },
    { "1000", 0 },
    { "1001", 0 },
    { "2100", 0 },
    { "1100", 0 },
    { "2101", 0 },
    { "1101", 0 },
    { "2111", 0 },
    { "1011", 0 },
    { "1111", 0 }
};

var source = new Source();
var channel1 = new Channel(0.5);
var channel2 = new Channel(0.5);
var request = false;

for (var i = 0; i < tactsCount; i++)
{
    try
    {
        request = source.Tact();

        request = channel1.Tact(request);

        isQueueBusy = isQueueBusy ? true : request ? true : false;

        if (!channel2.IsBusy)
            isQueueBusy = false;

        channel2.Tact(request);

        states[$"{source.TactsToBid}{Convert.ToInt32(channel1.IsBusy)}{Convert.ToInt32(isQueueBusy)}{Convert.ToInt32(channel2.IsBusy)}"] += 1;
    }
    catch
    {
        continue;
    }
}

foreach (var state in states)
    Console.WriteLine($"{state.Key}  {(double)state.Value/(double)tactsCount}");