using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    public class MyEntityBase
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //[DataType(DataType.Date, ErrorMessage = "olmuyorusa zorlama")]burası bi tık gugullanıcak
        [Required(ErrorMessage = "{0} eskiden buralar hep dutluktu")]
        public DateTime CratedOn { get; set; }
        [Required]
        public DateTime ModifiedOn { get; set; }
        [StringLength(30)]
        public string ModifiedUsername { get; set; }
    }
}
