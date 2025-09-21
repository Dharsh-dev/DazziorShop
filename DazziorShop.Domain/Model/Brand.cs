using DazziorShop.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DazziorShop.Domain.Model
{
    public class Brand : BaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int EstablishedYear { get; set; }
    }
}
