using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.DTOs
{
    public class PlayerModelDto : IValidatableObject
    {
        #region Properties

        public string Name { get; set; }
        public string Realm { get; set; }
        public string Region { get; set; }
        public int ClassId { get; set; }
        public int FactionId { get; set; }
        public int RaceId { get; set; }

        #endregion

        #region Object Validation

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Realm.Length < 5)
                yield return new ValidationResult("Please provide a realm with at least 5 characters", new[] { nameof(Realm) });
            if (Region.Length != 2)
                yield return new ValidationResult("Region can only be represented with 2 characters (e.g EU)", new[] { nameof(Region) });
            if (ClassId < 0)
                yield return new ValidationResult("Class ID can't be a negative value", new[] { nameof(ClassId) });
            if (FactionId < 0)
                yield return new ValidationResult("Faction ID can't be a negative value", new[] { nameof(FactionId) });
            if (RaceId < 0)
                yield return new ValidationResult("Race ID can't be a negative value", new[] { nameof(RaceId) });
        }

        #endregion

        #region Public Methods

        public Entity.player ToDbObject(int userId) => new Entity.player()
        {
            name = Name,
            realm = Realm,
            region = Region,
            fk_class = ClassId,
            fk_faction = FactionId,
            fk_race = RaceId,
            fk_user = userId
        };

        #endregion
    }
}
