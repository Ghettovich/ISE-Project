using AbInitio.Web.DbContexts;
using System.Collections.Generic;

namespace AbInitio.Web.ViewModels
{
    public class StamboomViewModel
    {

        public int stamboomid { get; set; }
        public List<stamboom> StambomenGebruiker { get; set; }
        public List<PersoonPartial> PersononenInStamboom { get; set; }
        
    }
}