using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsTrackPro.Domain.Entities
{
    public class ToolTransaction
    {
        public int ToolId { get; set; }
        public string ToolName { get; set; }
        public string StatusName { get; set; }
        public int? UserId { get; set; } 
        public string UserName { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
