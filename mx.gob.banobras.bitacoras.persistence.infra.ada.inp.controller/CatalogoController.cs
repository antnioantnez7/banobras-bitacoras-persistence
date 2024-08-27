using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.application.inputport;
using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.dominio.model;
using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.infra.ada.inp.dto;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.infra.ada.inp.controller
{
    [Route("persistence/catalogos/")]
    [ApiController]

    public class CatalogoController : ControllerBase
    {
        #region Properties
        /// <summary>
        /// Instancia de la interfaz de puerto de entrada
        /// </summary>
        readonly ICatalogoInputPort iCatalogoInputPort;
        /// <summary>
        /// Instancia de la interfaz de logueo
        /// </summary>
        private readonly ILog _log;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor del controlador CatalogoController
        /// </summary>
        /// <remarks>
        /// Utiliza la inyección de dependencia de la interfaz a implementar definida en el Program.cs
        /// </remarks>
        /// <param name="iCatalogoInputPort"></param>
        /// <param name="log"></param>
        public CatalogoController(ILog log, ICatalogoInputPort iCatalogoInputPort)
        {
            _log = log;
            this.iCatalogoInputPort = iCatalogoInputPort;
            _log.Info("ENTRA -> CatalogoController");
        }
        #endregion

        #region Methods - Catálogos Aplicativos
        /// <summary>
        /// Servicio para agregar un registro en el catálogo de aplicativos.
        /// </summary>
        /// <param name="request">Request: </param>
        /// <remarks>
        /// <br>Ejemplo request: </br>
        /// 
        ///     POST persistence/catalogos/aplicativos/registrar
        ///     {
        ///         "aplicativoId": "SIGEVI",
        ///         "nombre": "Sistema de Gestión de Viáticos",
        ///         "usuarioRegistro": 1,
        ///         "usuarioModifica": 1
        ///     }
        /// </remarks>
        /// <br>Ejemplo response: </br>
        /// {
        ///     "statusCode": 200,
        ///     "message": "",
        ///     "success": true,
        ///     "responseType": 200,
        ///     "contenido": 
        ///     {
        ///         "identificador": 99
        ///     }
        /// }
        /// <response code="200">Operación exitosa</response>
        /// <response code="400">Error en la petición</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Produces("application/json", Type = typeof(string))]
        [Consumes("application/json")]
        [Route("aplicativos/registrar")]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<BitacoraDtoResponse>), 200)]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<BitacoraDtoResponse>), 400)]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<BitacoraDtoResponse>), 500)]
        public async Task<IActionResult> registrarAplicativo([FromBody] CatalogoAplicativoDto request)
        {
            _log.Info("MÉTODO registrarAplicativo");
            string message1 = string.Format("REQUEST: \n{0}", JsonConvert.SerializeObject(request, Formatting.Indented));
            _log.Info(message1);
            ResponseBaseMicroservicio<BitacoraDtoResponse> response = new ResponseBaseMicroservicio<BitacoraDtoResponse>();
            if (ModelState.IsValid)
            {
                var result = await iCatalogoInputPort.registrarAplicativo(request);
                string message2 = string.Format("RESPONSE: \n{0}", JsonConvert.SerializeObject(result, Formatting.Indented));
                _log.Info(message2);
                if (result != null)
                {
                    if (result.Codigo == 200)
                    {
                        response.success = true;
                        response.Contenido = result.Contenido;
                        response.statusCode = 200;
                    }
                    else
                    {
                        response.statusCode = result.Codigo;
                        response.message = result.Mensaje;
                    }
                }
                else
                {
                    response.statusCode = 400;
                    response.message = "Error en la petición.";
                }
            }
            else
            {
                response.statusCode = 400;
                response.message = "Modelo inválido.";
            }
            response.responseType = new RestfulResponse().GetResponseType(response.statusCode);
            Console.WriteLine(JsonConvert.SerializeObject(new RestfulResponse().GetResponse(response), Formatting.Indented));
            string message3 = string.Format("RESPONSE MICROSERVICIO: \n{0}", JsonConvert.SerializeObject(new RestfulResponse().GetResponse(response), Formatting.Indented));
            _log.Info(message3);
            return new RestfulResponse().GetResponse(response);
        }

        /// <summary>
        /// Servicio para consultar los registros del catálogo de aplicativos.
        /// </summary>
        /// <remarks>
        /// 
        ///     GET persistence/catalogos/aplicativos/consultar
        /// </remarks>
        /// <br>Ejemplo response: </br>
        /// {
        ///     "statusCode": 200,
        ///     "message": "",
        ///     "success": true,
        ///     "responseType": 200,
        ///     "contenido": 
        ///     [
        ///         { "identificador": 99 }
        ///     ]
        /// }
        /// <response code="200">Operación exitosa</response>
        /// <response code="400">Error en la petición</response>
        /// <response code="403">Token incorrecto.</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [Produces("application/json", Type = typeof(string))]
        [Consumes("application/json")]
        [Route("aplicativos/consultar")]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<List<CatalogoAplicativoDto>>), 200)]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<List<CatalogoAplicativoDto>>), 400)]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<List<CatalogoAplicativoDto>>), 500)]
        public async Task<IActionResult> consultarAplicativos()
        {
            _log.Info("MÉTODO consultarAplicativos");
            string message1 = string.Format("REQUEST: \n{0}", JsonConvert.SerializeObject("GET - Sin REQUEST", Formatting.Indented));
            _log.Info(message1);
            ResponseBaseMicroservicio<List<CatalogoAplicativoDto>> response = new ResponseBaseMicroservicio<List<CatalogoAplicativoDto>>();
            var result = await iCatalogoInputPort.consultarAplicativos();
            string message2 = string.Format("RESPONSE: \n{0}", JsonConvert.SerializeObject(result, Formatting.Indented));
            _log.Info(message2);
            if (result != null)
            {
                if (result.Codigo == 200)
                {
                    response.success = true;
                    response.Contenido = result.Contenido;
                    response.statusCode = 200;
                }
                else
                {
                    response.statusCode = result.Codigo;
                    response.message = result.Mensaje;
                }
            }
            else
            {
                response.statusCode = 400;
                response.message = "Error en la petición."; 
            }
            response.responseType = new RestfulResponse().GetResponseType(response.statusCode);
            Console.WriteLine(JsonConvert.SerializeObject(new RestfulResponse().GetResponse(response), Formatting.Indented));
            string message3 = string.Format("RESPONSE MICROSERVICIO: \n{0}", JsonConvert.SerializeObject(new RestfulResponse().GetResponse(response), Formatting.Indented));
            _log.Info(message3);
            return new RestfulResponse().GetResponse(response);
        }

        /// <summary>
        /// Servicio para eliminar un registro del catálogo de aplicativos.
        /// </summary>
        /// <remarks>
        /// 
        ///     DELETE persistence/catalogos/aplicativos/eliminar/MAC
        /// </remarks>
        /// <br>Ejemplo response: </br>
        /// {
        ///     "statusCode": 200,
        ///     "message": "",
        ///     "success": true,
        ///     "responseType": 200,
        ///     "contenido": 
        ///     [
        ///         { "identificador": 99 }
        ///     ]
        /// }
        /// <response code="200">Operación exitosa</response>
        /// <response code="400">Error en la petición</response>
        /// <response code="403">Token incorrecto.</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpDelete]
        [Produces("application/json", Type = typeof(string))]
        [Consumes("application/json")]
        [Route("aplicativos/eliminar/{aplicativoId}")]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<BitacoraDtoResponse>), 200)]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<BitacoraDtoResponse>), 400)]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<BitacoraDtoResponse>), 500)]
        public async Task<IActionResult> eliminarAplicativo(string aplicativoId)
        {
            _log.Info("MÉTODO eliminarAplicativo");
            string message1 = string.Format("REQUEST: \n{0}", JsonConvert.SerializeObject(string.Format("DELETE - {0}", aplicativoId), Formatting.Indented));
            _log.Info(message1);
            ResponseBaseMicroservicio<BitacoraDtoResponse> response = new ResponseBaseMicroservicio<BitacoraDtoResponse>();
            var result = await iCatalogoInputPort.eliminarAplicativo(aplicativoId);
            string message2 = string.Format("RESPONSE: \n{0}", JsonConvert.SerializeObject(result, Formatting.Indented));
            _log.Info(message2);
            if (result != null)
            {
                if (result.Codigo == 200)
                {
                    response.success = true;
                    response.Contenido = result.Contenido;
                    response.statusCode = 200;
                }
                else
                {
                    response.statusCode = result.Codigo;
                    response.message = result.Mensaje;
                }
            }
            else
            {
                response.statusCode = 400;
                response.message = "Error en la petición.";
            }
            response.responseType = new RestfulResponse().GetResponseType(response.statusCode);
            Console.WriteLine(JsonConvert.SerializeObject(new RestfulResponse().GetResponse(response), Formatting.Indented));
            string message3 = string.Format("RESPONSE MICROSERVICIO: \n{0}", JsonConvert.SerializeObject(new RestfulResponse().GetResponse(response), Formatting.Indented));
            _log.Info(message3);
            return new RestfulResponse().GetResponse(response);
        }

        /// <summary>
        /// Servicio para actualizar un registro del catálogo de aplicativos.
        /// </summary>
        /// <param name="request">Request: </param>
        /// <remarks>
        /// <br>Ejemplo request: </br>
        /// 
        ///     POST bitacora/catalogos/aplicativos/actualizar
        ///     {
        ///         "aplicativoId": "SIGEVI",
        ///         "nombre": "Sistema de Gestión de Viáticos",
        ///         "usuarioModifica": 1
        ///     }
        /// </remarks>
        /// <br>Ejemplo response: </br>
        /// {
        ///     "statusCode": 200,
        ///     "message": "",
        ///     "success": true,
        ///     "responseType": 200,
        ///     "contenido": 
        ///     {
        ///         "identificador": 99
        ///     }
        /// }
        /// <response code="200">Operación exitosa</response>
        /// <response code="400">Error en la petición</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Produces("application/json", Type = typeof(string))]
        [Consumes("application/json")]
        [Route("aplicativos/actualizar")]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<BitacoraDtoResponse>), 200)]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<BitacoraDtoResponse>), 400)]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<BitacoraDtoResponse>), 500)]
        public async Task<IActionResult> actualizarAplicativo([FromBody] CatalogoAplicativoDto request)
        {
            _log.Info("MÉTODO actualizarAplicativo");
            string message1 = string.Format("REQUEST: \n{0}", JsonConvert.SerializeObject(request, Formatting.Indented));
            _log.Info(message1);
            ResponseBaseMicroservicio<BitacoraDtoResponse> response = new ResponseBaseMicroservicio<BitacoraDtoResponse>();
            if (ModelState.IsValid)
            {
                var result = await iCatalogoInputPort.actualizarAplicativo(request);
                string message2 = string.Format("RESPONSE: \n{0}", JsonConvert.SerializeObject(result, Formatting.Indented));
                _log.Info(message2);
                if (result != null)
                {
                    if (result.Codigo == 200)
                    {
                        response.success = true;
                        response.Contenido = result.Contenido;
                        response.statusCode = 200;
                    }
                    else
                    {
                        response.statusCode = result.Codigo;
                        response.message = result.Mensaje;
                    }
                }
                else
                {
                    response.statusCode = 400;
                    response.message = "Error en la petición.";
                }
            }
            else
            {
                response.statusCode = 400;
                response.message = "Modelo inválido.";
            }
            response.responseType = new RestfulResponse().GetResponseType(response.statusCode);
            Console.WriteLine(JsonConvert.SerializeObject(new RestfulResponse().GetResponse(response), Formatting.Indented));
            string message3 = string.Format("RESPONSE MICROSERVICIO: \n{0}", JsonConvert.SerializeObject(new RestfulResponse().GetResponse(response), Formatting.Indented));
            _log.Info(message3);
            return new RestfulResponse().GetResponse(response);
        }
        #endregion

        #region Methods - Catálogos Usuarios
        /// <summary>
        /// Servicio para agregar un registro en el catálogo de usuarios.
        /// </summary>
        /// <param name="request">Request: </param>
        /// <remarks>
        /// <br>Ejemplo request: </br>
        /// 
        ///     POST persistence/catalogos/usuarios/guardar
        ///     {
        ///         "identificador": 0,
        ///         "usuario": "antnioantnz7",
        ///         "paterno": "Antunez",
        ///         "materno": "Barbosa",
        ///         "nombre": "Antonio",
        ///         "sesionActiva": "N",
        ///         "usuarioBloqueado": "N",
        ///         "intentosFallidos": 0,
        ///         "usuarioRegistro": 1,
        ///         "fechaRegistro": "2024-08-12T02:14:21.765Z",
        ///         "usuarioModifica": 1,
        ///         "fechaModifica": "2024-08-12T02:14:21.765Z"
        ///     }
        /// </remarks>
        /// <br>Ejemplo response: </br>
        /// {
        ///     "statusCode": 200,
        ///     "message": "",
        ///     "success": true,
        ///     "responseType": 200,
        ///     "contenido": 
        ///     {
        ///         "identificador": 99
        ///     }
        /// }
        /// <response code="200">Operación exitosa</response>
        /// <response code="400">Error en la petición</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Produces("application/json", Type = typeof(string))]
        [Consumes("application/json")]
        [Route("usuarios/guardar")]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<BitacoraDtoResponse>), 200)]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<BitacoraDtoResponse>), 400)]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<BitacoraDtoResponse>), 500)]
        public async Task<IActionResult> guardarUsuario([FromBody] CatalogoUsuarioDto request)
        {
            _log.Info("MÉTODO guardarUsuario");
            string message1 = string.Format("REQUEST: \n{0}", JsonConvert.SerializeObject(request, Formatting.Indented));
            _log.Info(message1);
            ResponseBaseMicroservicio<BitacoraDtoResponse> response = new ResponseBaseMicroservicio<BitacoraDtoResponse>();
            if (ModelState.IsValid)
            {
                var result = await iCatalogoInputPort.guardarUsuario(request);
                string message2 = string.Format("RESPONSE: \n{0}", JsonConvert.SerializeObject(result, Formatting.Indented));
                _log.Info(message2);
                if (result != null)
                {
                    if (result.Codigo == 200)
                    {
                        response.success = true;
                        response.Contenido = result.Contenido;
                        response.statusCode = 200;
                    }
                    else
                    {
                        response.statusCode = result.Codigo;
                        response.message = result.Mensaje;
                    }
                }
                else
                {
                    response.statusCode = 400;
                    response.message = "Error en la petición.";
                }
            }
            else
            {
                response.statusCode = 400;
                response.message = "Modelo inválido.";
            }
            response.responseType = new RestfulResponse().GetResponseType(response.statusCode);
            Console.WriteLine(JsonConvert.SerializeObject(new RestfulResponse().GetResponse(response), Formatting.Indented));
            string message3 = string.Format("RESPONSE MICROSERVICIO: \n{0}", JsonConvert.SerializeObject(new RestfulResponse().GetResponse(response), Formatting.Indented));
            _log.Info(message3);
            return new RestfulResponse().GetResponse(response);
        }

        /// <summary>
        /// Servicio para consultar los registros del catálogo de usuarios.
        /// </summary>
        /// <remarks>
        /// 
        ///     POST persistence/catalogos/usuarios/consultar
        ///     {
        ///         "identificador": 1,
        ///         "usuario": "",
        ///         "paterno": "",
        ///         "materno": "",
        ///         "nombre": ""
        ///     }
        /// </remarks>
        /// <br>Ejemplo response: </br>
        /// {
        ///     "statusCode": 200,
        ///     "message": "",
        ///     "success": true,
        ///     "responseType": 200,
        ///     "contenido": 
        ///     [
        ///         { "identificador": 99 }
        ///     ]
        /// }
        /// <response code="200">Operación exitosa</response>
        /// <response code="400">Error en la petición</response>
        /// <response code="403">Token incorrecto.</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Produces("application/json", Type = typeof(string))]
        [Consumes("application/json")]
        [Route("usuarios/consultar")]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<List<CatalogoUsuarioDto>>), 200)]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<List<CatalogoUsuarioDto>>), 400)]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<List<CatalogoUsuarioDto>>), 500)]
        public async Task<IActionResult> consultarUsuarios([FromBody] CatalogoUsuarioDto request)
        {
            _log.Info("MÉTODO consultarUsuarios");
            string message1 = string.Format("REQUEST: \n{0}", JsonConvert.SerializeObject(request, Formatting.Indented));
            _log.Info(message1);
            ResponseBaseMicroservicio<List<CatalogoUsuarioDto>> response = new ResponseBaseMicroservicio<List<CatalogoUsuarioDto>>();
            var result = await iCatalogoInputPort.consultarUsuarios(request);
            string message2 = string.Format("RESPONSE: \n{0}", JsonConvert.SerializeObject(result, Formatting.Indented));
            _log.Info(message2);
            if (result != null)
            {
                if (result.Codigo == 200)
                {
                    response.success = true;
                    response.Contenido = result.Contenido;
                    response.statusCode = 200;
                }
                else
                {
                    response.statusCode = result.Codigo;
                    response.message = result.Mensaje;
                }
            }
            else
            {
                response.statusCode = 400;
                response.message = "Error en la petición.";
            }
            response.responseType = new RestfulResponse().GetResponseType(response.statusCode);
            Console.WriteLine(JsonConvert.SerializeObject(new RestfulResponse().GetResponse(response), Formatting.Indented));
            string message3 = string.Format("RESPONSE MICROSERVICIO: \n{0}", JsonConvert.SerializeObject(new RestfulResponse().GetResponse(response), Formatting.Indented));
            _log.Info(message3);
            return new RestfulResponse().GetResponse(response);
        }

        /// <summary>
        /// Servicio para eliminar un registro del catálogo de usuarios.
        /// </summary>
        /// <remarks>
        /// 
        ///     DELETE persistence/catalogos/usuarios/eliminar/1
        /// </remarks>
        /// <br>Ejemplo response: </br>
        /// {
        ///     "statusCode": 200,
        ///     "message": "",
        ///     "success": true,
        ///     "responseType": 200,
        ///     "contenido": 
        ///     [
        ///         { "identificador": 99 }
        ///     ]
        /// }
        /// <response code="200">Operación exitosa</response>
        /// <response code="400">Error en la petición</response>
        /// <response code="403">Token incorrecto.</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpDelete]
        [Produces("application/json", Type = typeof(int))]
        [Consumes("application/json")]
        [Route("usuarios/eliminar/{identificador}")]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<BitacoraDtoResponse>), 200)]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<BitacoraDtoResponse>), 400)]
        [ProducesResponseType(typeof(ResponseBaseMicroservicio<BitacoraDtoResponse>), 500)]
        public async Task<IActionResult> eliminarUsuario(int identificador)
        {
            _log.Info("MÉTODO eliminarUsuario");
            string message1 = string.Format("REQUEST: \n{0}", JsonConvert.SerializeObject(string.Format("DELETE - {0}", identificador), Formatting.Indented));
            _log.Info(message1);
            ResponseBaseMicroservicio<BitacoraDtoResponse> response = new ResponseBaseMicroservicio<BitacoraDtoResponse>();
            var result = await iCatalogoInputPort.eliminarUsuario(identificador);
            string message2 = string.Format("RESPONSE: \n{0}", JsonConvert.SerializeObject(result, Formatting.Indented));
            _log.Info(message2);
            if (result != null)
            {
                if (result.Codigo == 200)
                {
                    response.success = true;
                    response.Contenido = result.Contenido;
                    response.statusCode = 200;
                }
                else
                {
                    response.statusCode = result.Codigo;
                    response.message = result.Mensaje;
                }
            }
            else
            {
                response.statusCode = 400;
                response.message = "Error en la petición.";
            }
            response.responseType = new RestfulResponse().GetResponseType(response.statusCode);
            Console.WriteLine(JsonConvert.SerializeObject(new RestfulResponse().GetResponse(response), Formatting.Indented));
            string message3 = string.Format("RESPONSE MICROSERVICIO: \n{0}", JsonConvert.SerializeObject(new RestfulResponse().GetResponse(response), Formatting.Indented));
            _log.Info(message3);
            return new RestfulResponse().GetResponse(response);
        }
        #endregion
    }
}