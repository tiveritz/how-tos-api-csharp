using System;


namespace HowTosApi
{
    public class HowTo
    {
        public int Id { get; set; }
        public string Link {get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public void CreateLink(int id)
        {
            this.Link = Environment.GetEnvironmentVariable("BASE_URL") + "howtos/" + id;
        }
    }
}
