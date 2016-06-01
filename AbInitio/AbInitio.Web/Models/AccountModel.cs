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

    public class AccountModel
    {
        public int stamboomAccountId { get; set; }
        public string gebruikersnaam { get; set; }
        public int stamboomRechten { get; set; }
        public int accountId { get; set; }
        public int stamboomId { get; set; }
        public List<SelectListItem> RechtOpties { get; set; }
    }
}