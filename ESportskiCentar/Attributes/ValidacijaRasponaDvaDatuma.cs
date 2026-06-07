using ESportskiCentar.Models;
using System.ComponentModel.DataAnnotations;

namespace ESportskiCentar.Attributes
{
    public class ValidacijaRasponaDvaDatuma:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) 
        {
            var model = (Izvjestaj)validationContext.ObjectInstance;
            return model.datumOd <= model.datumDo ? ValidationResult.Success : new ValidationResult("Datum kraja ne može biti prije datuma početka!");
        }
    }
}
