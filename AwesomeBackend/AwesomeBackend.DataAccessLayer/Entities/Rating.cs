using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwesomeBackend.DataAccessLayer.Entities
{
    [Table("Ratings")]
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid RestaurantId { get; set; }

        public DateTime Date { get; set; }

        [Column("Rating")]
        public double Score { get; set; }

        public string Comment { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public virtual Restaurant Restaurant { get; set; }
    }
}
