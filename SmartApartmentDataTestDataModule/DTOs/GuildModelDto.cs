using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartApartmentDataTestDataModule.Entity;

namespace SmartApartmentDataTestDataModule.DTOs
{
    public class GuildModelDto : IValidatableObject
    {
        #region Properties

        [Required]
        public string Name { get; set; }
        [Required]
        public string Region { get; set; }
        [Required]
        public string Realm { get; set; }
        [Required]
        public int FactionId { get; set; }

        #endregion

        #region Object Validation

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name.Length < 5)
                yield return new ValidationResult("Please provide a name with at least 5 characters", new[] { nameof(Name) });
            if (Realm.Length < 5)
                yield return new ValidationResult("Please provide a realm with at least 5 characters", new[] { nameof(Realm) });
            if (Region.Length != 2)
                yield return new ValidationResult("Region can only be represented with 2 characters (e.g EU)", new[] { nameof(Region) });
            if (FactionId < 0)
                yield return new ValidationResult("Faction ID can't be a negative value", new[] { nameof(FactionId) });
        }

        #endregion

        #region Public Methods

        public Entity.guild ToDbObject() => new guild
        {
            name = Name,
            region = Region,
            realm = Realm,
            fk_faction = FactionId
        };

        #endregion
    }
}
