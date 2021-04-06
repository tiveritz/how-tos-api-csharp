using System;


namespace HowTosApi
{
    public class HowToListItem
    {
        public string Id { get; private set; }
        public string Link {get; private set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public void SetId(string Id)
        {
            this.Id = Id;
            this.Link = Environment.GetEnvironmentVariable("BASE_URL") + "howtos/" + Id;
        }
    }
}
