using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartApartmentDataTest.Requests
{
    public class AddGuildMemberRequest : IValidatableObject
    {
        #region Properties

        [Required]
        public string PlayerName { get; set; }
        [Required]
        public string GuildName { get; set; }

        #endregion

        #region Object Validation

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PlayerName.Length < 5)
                yield return new ValidationResult("Please provide a name that's at least 5 characters long", new[] { nameof(PlayerName) });
            if (GuildName.Length < 5)
                yield return new ValidationResult("Please provide a guild name that's at least 5 characters long", new[] { nameof(GuildName) });
        }

        #endregion
    }
}