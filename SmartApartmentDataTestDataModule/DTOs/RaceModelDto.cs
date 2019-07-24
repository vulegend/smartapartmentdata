using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.DTOs
{
    public class RaceModelDto
    {
        #region Properties

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsPlayable { get; set; }

        #endregion

        #region Object Validation

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name.Equals(string.Empty))
                yield return new ValidationResult("You need to provide the name for the race", new[] { nameof(Name) });
        }

        #endregion

        #region Public Methods

        public Entity.race ToDbObject() =>
            new Entity.race
            {
                name = Name,
                description = Description,
                playable = IsPlayable
            };

        #endregion
    }
}
