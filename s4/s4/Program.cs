class Program
{
    static Dictionary<int, int> States = new Dictionary<int, int>();

    static void Main(string[] args)
    {
        var lambda = 2.0;
        var mu = 2.25;
        var p = 0.1;

        var queueLength = 0;
        var workingTime = DateTime.Now.AddSeconds(10);
        long totalCount = 0;

        DateTime processingTime = DateTime.Now;
        DateTime timeBeforeAppearance = ExponentialDistribution(lambda);
        
        while (DateTime.Now < workingTime)
        {
            if (processingTime < DateTime.Now)
            {
                if (queueLength > 0)
                {
                    queueLength--;
                    AddOrUpdateState(queueLength + 1);
                    totalCount++;
                    if (queueLength == 0)
                        timeBeforeAppearance = ExponentialDistribution(lambda);
                    processingTime = ExponentialDistribution(mu);
                }
                else
                {
                    if (timeBeforeAppearance < DateTime.Now)
                    {
                        AddOrUpdateState(0);
                        totalCount++;
                    }
                    else
                    {
                        AddOrUpdateState(1);
                        totalCount++;
                        processingTime = ExponentialDistribution(mu);
                        timeBeforeAppearance = ExponentialDistribution(lambda);
                    }
                }
            }
            else
            {
                if (timeBeforeAppearance < DateTime.Now)
                {
                    AddOrUpdateState(queueLength + 1);
                    totalCount++;
                }
                else
                {
                    queueLength++;
                    AddOrUpdateState(queueLength + 1);
                    totalCount++;
                    timeBeforeAppearance = ExponentialDistribution(lambda);
                }
            }
            //timeBeforeAppearance = DateTime.Now.AddSeconds(ExponentialDistribution(lambda));
            // if (DateTime.Now >= timeBeforeAppearance)
            // {
            //     if (DateTime.Now > processingTime && queueLength == 0)
            //     {
            //         processingTime = ExponentialDistribution(mu);
            //         AddOrUpdateState(1);
            //         totalCount++;
            //         continue;
            //     }
            //     else
            //     {
            //         queueLength++;
            //         AddOrUpdateState(queueLength + 1);
            //     }
            //     timeBeforeAppearance = ExponentialDistribution(lambda);
            // }
            //
            // if (processingTime > DateTime.Now)
            // {
            //     if (queueLength > 0)
            //     {
            //         queueLength--;
            //         processingTime = ExponentialDistribution(mu);
            //         AddOrUpdateState(queueLength + 1);
            //     }
            //     else
            //     {
            //         AddOrUpdateState(0);
            //     }
            // }
            //
            // totalCount++;
        }

        var count = 0;
        foreach (var state in States)
        {
            if (count == 20) break;
            Console.WriteLine($"S{state.Key}: " + (double)state.Value/(double)totalCount);
            count++;
        }

        var ro = lambda / mu;
        var Loch = ro * ro / (1 - ro);
        var Toch = ro * ro / (lambda * (1 - ro));
        var Ts = ro / (lambda * (1 - ro));
        
        Console.WriteLine("Loch " + Loch);
        Console.WriteLine("Toch " + Toch);
        Console.WriteLine("Ts " + Ts);
    }
    
    static DateTime ExponentialDistribution(double val)
    {
        var rand = new Random();
        return DateTime.Now.AddSeconds(-Math.Log(rand.NextDouble()) / val);
    }
    
    static bool ToReService(double p)
    {
        var rand = new Random();
        return rand.NextDouble() <= p;
    }

    public static void AddOrUpdateState(int key)
    {
        if (States.ContainsKey(key))
        {
            States[key] += 1;
        }
        else
        {
            States.Add(key, 1);
        }
    }
}