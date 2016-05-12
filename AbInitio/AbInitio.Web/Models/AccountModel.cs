using AbInitio.Web.DbContexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

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
    
}