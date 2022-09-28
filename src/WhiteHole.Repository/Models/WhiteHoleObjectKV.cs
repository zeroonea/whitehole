using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteHole.Repository.Models
{
    [Table("WhiteHoleKV")]
    public class WhiteHoleObjectKV : IModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id", TypeName = "int")]
        public int Id { get; set; }

        [ForeignKey("Object")]
        public int ObjectId { get; set; }
        public virtual WhiteHoleObject Object { get; set; }

        [Column("ObjectName", TypeName = "varchar(255)")]
        public string ObjectName { get; set; }

        [Column("ObjectKey", TypeName = "varchar(255)")]
        public string ObjectKey { get; set; }

        [Column("ObjectValue", TypeName = "nvarchar(max)")]
        public string ObjectValue { get; set; }

        [Required]
        [Column("CreatedBy", TypeName = "varchar(255)")]
        public string CreatedBy { get; set; }

        [Column("CreatedAt", TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Column("UpdatedBy", TypeName = "varchar(255)")]
        public string UpdatedBy { get; set; }

        [Column("UpdatedAt", TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }
    }
}
