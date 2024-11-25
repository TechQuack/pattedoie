using System.ComponentModel.DataAnnotations;

namespace PatteDoie.Models.Platform
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string Nickname { get; set; } = string.Empty;

        public Guid UserUUID { get; set; }

    }
}
