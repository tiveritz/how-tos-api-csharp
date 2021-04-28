using System.Collections.Generic;


namespace HowTosApi
{
    public class TreeListItem
    {
        public StepsOrderItem Superstep { get; set; }
        public int Title { get; set; }
        public List<StepsOrderItem> Substeps { get; set; }

    }
}
