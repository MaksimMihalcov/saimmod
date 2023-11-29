class Program
{
    static Dictionary<int, int> States = new Dictionary<int, int>();

    static void Main(string[] args)
    {
        var lambda = 2.0;
        var mu = 2.5;
        var p = 0.1;

        var queueLength = 0;
        var workingTime = DateTime.Now.AddSeconds(30);
        var totalCount = 0;
        
        DateTime timeBeforeAppearance = DateTime.Now.AddSeconds(ExponentialDistribution(lambda));
        DateTime processingTime = DateTime.Now;

        while (DateTime.Now < workingTime)
        {
            if (DateTime.Now > timeBeforeAppearance)
            {
                if (DateTime.Now > processingTime && queueLength == 0)
                {
                    processingTime = DateTime.Now.AddSeconds(ExponentialDistribution(mu));
                    AddOrUpdateState(1);
                }
                else
                {
                    queueLength++;
                    AddOrUpdateState(queueLength + 1);
                }
                timeBeforeAppearance = DateTime.Now.AddSeconds(ExponentialDistribution(lambda));
            }

            if (processingTime > DateTime.Now)
            {
                if (queueLength > 0)
                {
                    queueLength--;
                    processingTime = DateTime.Now.AddSeconds(ExponentialDistribution(mu));
                    AddOrUpdateState(queueLength + 1);
                }
                else
                {
                    AddOrUpdateState(0);
                }
            }
            
            totalCount++;
        }

        foreach (var state in States)
        {
            Console.WriteLine($"S{state.Key}: " + (double)state.Value/(double)totalCount);
        }
    }
    
    static double ExponentialDistribution(double val)
    {
        var rand = new Random();
        return -Math.Log(rand.NextDouble()) / val;
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