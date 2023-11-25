using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAIMMOD3
{
    internal class Channel
    {
        private double failure;
        private Random rand;
        public bool IsBusy { get; set; }
        public Channel(double failure)
        {
            this.failure = failure;
            IsBusy = false;
            rand = new Random();
        }

        // возвращает заявку или нет
        public bool Tact1(bool request)
        {
            if (rand.NextDouble() <= failure)
            {
                IsBusy = IsBusy || request;
                return false;
            }
            if (!IsBusy || IsBusy && request)
                return request;
            if (IsBusy && !request)
            {
                IsBusy = false;
                return true;
            }
            throw new Exception("Tact case not found!");
        }

        public bool Tact2(bool request)
        {
            if (rand.NextDouble() <= failure)
            {
                IsBusy = IsBusy || request;
                return false;
            }
            if (!IsBusy || IsBusy && request)
            {
                IsBusy = false;
                return request;
            }
                
            if (IsBusy && !request)
            {
                IsBusy = false;
                return true;
            }
            throw new Exception("Tact case not found!");
        }
    }
}
