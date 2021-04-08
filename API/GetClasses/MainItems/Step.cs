using System;
using System.Collections.Generic;


namespace HowTosApi
{
    public class Step
    {
        public string Id { get; private set; }
        public string Link {get; private set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsSuper { get; set; }
        public List<StepsOrderItem> Steps { get; set; }

        public void SetId(string Id)
        {
            this.Id = Id;
            this.Link = UriIdGenerator.GetLink("step/", Id);
        }
    }
}
