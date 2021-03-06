using System;
using System.Collections.Generic;


namespace HowTosApi
{
    public class StepsOrderItem
    {
        public string Id { get; private set; }
        public string Link {get; private set; }
        public string Title { get; set; }
        public bool IsSuper { get; set; }
        public List<StepsOrderItem> Substeps { get; set; }

        public void SetId(string Id)
        {
            this.Id = Id;
            this.Link = Environment.GetEnvironmentVariable("BASE_URL") + "steps/" + Id;
        }
    }
}
