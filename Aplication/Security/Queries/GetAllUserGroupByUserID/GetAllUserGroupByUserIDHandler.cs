using Aplication.Security.Queries.GetUserPersonByCredential;
using Common.Base;
using Common.Encryptation;
using Common.MapperDto;
using Common.Models;
using Domain.Models;
using Infraestructure.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplication.Security.Queries.GetAllUserGroupByUserID
{
    public class GetAllUserGroupByUserIDHandler : IRequestHandler<GetAllUserGroupByUserIDQuery, BaseResponse>
    {
        readonly IUnitOfWork _unitOfWork;
        public GetAllUserGroupByUserIDHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Handle(GetAllUserGroupByUserIDQuery request, CancellationToken cancellationToken)
        {
            BaseResponse response;

            try
            {
                var user = await _unitOfWork.UserPersonRepository.GetByIdAsync(request.UserID);
                if (user != null)
                {
                    response = new BaseResponse(HttpStatusCode.OK, new BaseResponse<UserPersonDto>
                    {
                        Code = (int)HttpStatusCode.OK,
                        Payload = user.MapTo<UserPersonDto>(),
                    }); ;
                }
                else
                {
                    response = new BaseResponse(HttpStatusCode.OK, new BaseResponse<object>
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "Ocurrio un error al obtener el usuario",
                        Payload = null
                    });
                }
            }
            catch (Exception e)
            {
                response = new BaseResponse(HttpStatusCode.InternalServerError, new BaseResponse<object>
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Payload = null,
                    Message = e.Message
                });
            }
            return response;
        }
    }
}
