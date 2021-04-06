using System;
using System.Collections.Generic;


namespace HowTosApi
{
    public class HowTo
    {
        public string Id { get; private set; }
        public string Link {get; private set; }
        public string Title { get; set; }
        public List<HowToSteps> Steps { get; set; }

        public void SetId(string Id)
        {
            this.Id = Id;
            this.Link = Environment.GetEnvironmentVariable("BASE_URL") + "howtos/" + Id;
        }
    }
}
