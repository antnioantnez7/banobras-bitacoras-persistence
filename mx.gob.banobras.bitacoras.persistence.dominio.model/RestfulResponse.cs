using Microsoft.AspNetCore.Mvc;

namespace banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.dominio.model
{
    public class RestfulResponse : ControllerBase
    {
        #region Properties
        public IActionResult? Result { get; set; }
        #endregion

        #region Methods
        public IActionResult GetResponse(object response)
        {
            if (ModelState.IsValid)
            {
                switch (((ResponseBase)response).responseType)
                {
                    case ResponseType.BadRequest:
                        Result = BadRequest(response);
                        break;
                    case ResponseType.Okay:
                        Result = Ok(response);
                        break;
                    case ResponseType.NotAuthorized:
                        Result = Unauthorized(response);
                        break;
                    case ResponseType.InternalError:
                        Result = StatusCode(500, response);
                        break;
                    case ResponseType.TooManyRequest:
                        Result = StatusCode(429, response);
                        break;
                    default:
                        break;
                }
            }
            return Result!;
        }

        public ResponseType GetResponseType(int codigo)
        {
            ResponseType r = ResponseType.Okay;
            if (ModelState.IsValid)
            {
                switch (codigo)
                {
                    case 200:
                        r = ResponseType.Okay;
                        break;
                    case 400:
                        r = ResponseType.BadRequest;
                        break;
                    case 401:
                    case 403:
                        r = ResponseType.NotAuthorized;
                        break;
                    case 500:
                        r = ResponseType.InternalError;
                        break;
                    default:
                        r = ResponseType.InternalError;
                        break;
                }
            }
            return r;
        }
        #endregion
    }
}
