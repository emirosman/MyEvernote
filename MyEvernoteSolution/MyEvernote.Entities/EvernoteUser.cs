using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    [Table("EvernoteUsers")]
    public class EvernoteUser:MyEntityBase
    {
        [StringLength(25)]
        public string Name { get; set; }

        [StringLength(25)]
        public string Surname { get; set; }

        [StringLength(25),Required]
        public string Username { get; set; }

        [StringLength(70), Required]
        public string Email { get; set; }

        [StringLength(25),Required]
        public string Password { get; set; }

        [StringLength(50)] 
        public string ProfileImageFilename { get; set; }

        public bool IsActive { get; set; }
        [Required]
        public Guid ActivateGuid { get; set; }

        public bool IsAdmin  { get; set; }

        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }

    }
}
