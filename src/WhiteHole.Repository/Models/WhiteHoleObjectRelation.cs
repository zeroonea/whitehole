using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteHole.Repository.Models
{
    [Table("WhiteHoleObjectRelation")]
    public class WhiteHoleObjectRelation : IModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id", TypeName = "int")]
        public int Id { get; set; }

        [ForeignKey("Object1")]
        public int Object1Id { get; set; }
        public virtual WhiteHoleObject Object1 { get; set; }

        [Column("Object1Name", TypeName = "varchar(255)")]
        public string Object1Name { get; set; }

        [ForeignKey("ObjectN")]
        public int ObjectNId { get; set; }
        public virtual WhiteHoleObject ObjectN { get; set; }

        [Column("ObjectNName", TypeName = "varchar(255)")]
        public string ObjectNName { get; set; }

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
