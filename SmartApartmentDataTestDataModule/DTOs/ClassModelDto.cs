using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.DTOs
{
    public class ClassModelDto : IValidatableObject
    {
        #region Properties

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        #endregion

        #region Object Validation

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name.Equals(string.Empty))
                yield return new ValidationResult("You need to provide the name for the class", new[] { nameof(Name) });
        }

        #endregion

        #region Public Methods

        public Entity.@class ToDbObject() =>
            new Entity.@class
            {
                name = Name,
                description = Description
            };

        #endregion
    }
}
