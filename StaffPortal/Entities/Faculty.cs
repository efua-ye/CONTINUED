using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffPortal.Entities
{
    public class Faculty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string CreatedBy { get; set; }
        private DateTime? dateCreated = null;
        public DateTime DateCreated
        {
            get
            {
                return dateCreated ?? DateTime.Now;
            }

            set { dateCreated = value; }

        }
    }
}



