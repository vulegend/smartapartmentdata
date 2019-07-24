using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartApartmentDataTest.Requests
{
    public class DeleteClassRequest : IValidatableObject
    {
        #region Properties

        public int? Id { get; set; }
        public string Name { get; set; }

        #endregion

        #region Object Validation

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Id.HasValue && Id.Value < 0)
                yield return new ValidationResult("The ID value can't be negative", new[] { nameof(Id) });

            if (Id == null && Name == null)
                yield return new ValidationResult("You have to provide either the name or the ID of the class you wish to remove");
        }

        #endregion
    }
}