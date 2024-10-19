using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Security.Queries.GetAllUserGroupByUserID
{
    public class UserGroupDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int UserID { get; set; }
    }
}
