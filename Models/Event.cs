﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectCMS.Models
{
    public class Event
    {
        [Key]
        public int EvId { get; set; }
        public string Name { get; set; }
        public DateTime First_Closure { get; set; }
        public DateTime Last_Closure { get; set;}

        [ForeignKey("CateId")]
        public int CateId { get; set; }
        public ICollection<Idea> Ideas { get; set; }
    }
}
