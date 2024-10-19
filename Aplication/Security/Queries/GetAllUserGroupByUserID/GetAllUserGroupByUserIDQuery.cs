using Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Security.Queries.GetAllUserGroupByUserID
{
    public class GetAllUserGroupByUserIDQuery : IRequest<BaseResponse>
    {
        public int UserID { get; set; }
    }
}
