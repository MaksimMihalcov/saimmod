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
var proc1 = true;
var proc2 = true;
var p = "";

for (var i = 0; i < tactsCount; i++)
{
    proc1 = true;
    proc2 = true;

        request = source.Tact();

        if(!channel1.IsBusy && request)
        {
            channel1.IsBusy = true;
        proc1 = false;
        }

        if (proc1)
            request = channel1.Tact1(request);

        if(proc1)
            isQueueBusy = isQueueBusy || request;

        if(!channel2.IsBusy && isQueueBusy)
        {
            isQueueBusy = false;
            channel2.IsBusy = true;
            proc2 = false;
        }

        if(proc2)
            channel2.Tact2(isQueueBusy);

        if(!channel2.IsBusy)
            isQueueBusy = false;

        states[$"{source.TactsToBid}{Convert.ToInt32(channel1.IsBusy)}{Convert.ToInt32(isQueueBusy)}{Convert.ToInt32(channel2.IsBusy)}"] += 1;
    p = $"{source.TactsToBid}{Convert.ToInt32(channel1.IsBusy)}{Convert.ToInt32(isQueueBusy)}{Convert.ToInt32(channel2.IsBusy)}";
}

foreach (var state in states)
    Console.WriteLine($"{state.Key}  {(double)state.Value/(double)tactsCount}");