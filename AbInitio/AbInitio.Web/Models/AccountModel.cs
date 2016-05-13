using AbInitio.Web.DbContexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbInitio.Web.Models
{
    public class RegistreerModel
    {
        [Required]
        public string Gebruikersnaam { get; set; }
        
        [Required]
        public string Wachtwoord { get; set; }

        [Required]
        public string HWachtwoord { get; set; }        

    }

    public class LoginModel
    {
        [Required]
        public string Gebruikersnaam { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Wachtwoord { get; set; }

    }

    public class RelatieModel
    {
        [Required]
        public int persoonid1 { get; set; }

        [Required]
        public int persoonid2 { get; set; }

        [Required]
        public int relatietypeid { get; set; }

        public List<SelectListItem> relatietypes { get; set; }

        public persoon persoon1 { get; set; }

        public List<persoon> relaties { get; set; }

        public persoon persoon2 { get; set; }


    }
    
    
}