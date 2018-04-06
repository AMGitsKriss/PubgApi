using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    class MatchRelationshipModel
    {
        public string pubg_user_id { get; set; }
        public string pubg_username { get; set; }
        public string pubg_match_id { get; set; }
        public int kills { get; set; }
        public string match_type { get; set; }
    }
}
