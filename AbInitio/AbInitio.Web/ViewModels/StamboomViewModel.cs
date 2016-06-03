using AbInitio.Web.DbContexts;
using AbInitio.Web.Models;
using System.Collections.Generic;

namespace AbInitio.Web.ViewModels
{
    public class StamboomViewModel
    {
        public NieuwPersoonModel nieuwPersoonModel = new NieuwPersoonModel();

        public List<PersoonInStamboom> personen { get; set; }

        public stamboom stamboom { get; set; }

        public StamboomViewModel()
        {

        }
        public int stamboomid { get; set; }
        public int persoonid1 { get; set; }
        public List<stamboom> StambomenGebruiker { get; set; }
        public List<PersoonPartial> PersononenInStamboom { get; set; }
        
    }
}