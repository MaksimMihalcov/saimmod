using SAIMMOD3;

var tactsCount = 1000000;
var states = new Dictionary<string, double>()
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
var p1 = 0.5;
var p2 = 0.5;
var channel1 = new Channel(0.5);
var channel2 = new Channel(0.5);

var request = false;
var isQueueBusy = false;
var c1r = false;

for (var i = 0; i < tactsCount; i++)
{
    request = source.Tact();
    
    if (channel2.IsBusy)
    {
        if (channel2.Tact2(isQueueBusy))
        {
            if (isQueueBusy)
                channel2.IsBusy = true;
            isQueueBusy = false;
        }
    }
    else
    {
        if (isQueueBusy)
        {
            channel2.IsBusy = true;
            isQueueBusy = false;
        }
    }

    if (request && !channel1.IsBusy)
    {
        channel1.IsBusy = true;
    }
    else
    {
        c1r = channel1.Tact1(request);
    
        if (!isQueueBusy)
        {
            if (!channel2.IsBusy)
                channel2.IsBusy = c1r;
            else
                isQueueBusy = c1r;
        }
    }
    
    states[$"{source.TactsToBid}{Convert.ToInt32(channel1.IsBusy)}{Convert.ToInt32(isQueueBusy)}{Convert.ToInt32(channel2.IsBusy)}"] += 1;
}

foreach (var state in states)
    states[state.Key] = state.Value / tactsCount;

var K1 = states["2100"] + states["1100"] + states["2101"] + states["1101"] + states["2111"] + states["1111"];
var K2 = states["1001"] + states["2101"] + states["1101"] + states["2111"] + states["1011"] + states["1111"];
var A = K2 * (1-p2);
var lambda = 0.5;
var Q = A/lambda;
var Potk = 1 -  Q;
var Loch = states["2111"] + states["1011"] + states["1111"];
var Lc = states["1001"] + states["2100"] + states["1100"] + 
         2*(states["2101"] + states["1101"] + states["1011"]) 
         + 3*(states["2111"] + states["1111"]);
var Woch = Loch / A;
var Wc = Lc / lambda;

foreach(var state in states)
    Console.WriteLine($"{state.Key}  {state.Value}");
Console.WriteLine(); 

Console.WriteLine($"K1 {K1}");
Console.WriteLine($"K2 {K2}");
Console.WriteLine($"A {A}");
Console.WriteLine($"Q {Q}");
Console.WriteLine($"Potk {Potk}");
Console.WriteLine($"Loch {Loch}");
Console.WriteLine($"Lc {Lc}");
Console.WriteLine($"Woch {Woch}");
Console.WriteLine($"Wc {Wc}");