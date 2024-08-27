using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.application.outport;
using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.dominio.model;
using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.infra.ada.inp.dto;
using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.infra.config;
using log4net;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;
using System.Text;

namespace banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.infra.ada.outp.repository
{
    public class CatalogoRepositoryImp : ICatalogoRepositoryOutPort
    {
        #region Properties
        readonly IConfiguration configuration;
        readonly GetOracleConnection getOracleConnection;
        readonly string scheme = "BITACORA.";
        /// <summary>
        /// Instancia de la interfaz de logueo
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(BitacoraAccesoRepositoryImp));

        public IConfiguration Configuration => configuration;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la implementación repositorio que recibe la inyección de dependencias del archivo de configuración de Program.cs
        /// </summary>
        /// <param name="_configuration"></param>
        public CatalogoRepositoryImp(IConfiguration _configuration)
        {
            configuration = _configuration;
            getOracleConnection = new GetOracleConnection(configuration);
        }
        #endregion

        #region Methods - Catálogos Aplicativos
        /// <summary>
        /// Implementación del método que obtiene los registros del catálogo de Aplicativos.
        /// </summary>
        /// <returns></returns>
        public Task<BitacoraResponse<List<CatalogoAplicativoDto>>> consultarAplicativos()
        {
            _log.Info("CatalogoRepositoryImp: MÉTODO consultarAplicativos");
            string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject("GET - Sin REQUEST", Formatting.Indented));
            _log.Info(message1);
            Console.WriteLine(message1);
            BitacoraResponse<List<CatalogoAplicativoDto>> result = ListCatalogosAplicativos();
            string message2 = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
            _log.Info(message2);
            Console.WriteLine(message2);
            return Task.FromResult(result);
        }

        /// <summary>
        /// Implementación del método que elimina el registro del catálogo de Aplicativos.
        /// </summary>
        /// <param name="aplicativoId"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> eliminarAplicativo(string aplicativoId)
        {
            _log.Info("CatalogoRepositoryImp: MÉTODO eliminarAplicativo");
            string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(aplicativoId, Formatting.Indented));
            _log.Info(message1);
            Console.WriteLine(message1);
            BitacoraResponse<BitacoraDtoResponse> result = RemoveAplicativo(aplicativoId);
            string message2 = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
            _log.Info(message2);
            Console.WriteLine(message2);
            return Task.FromResult(result);
        }

        /// <summary>
        /// Implementación del método para agregar un registro en el catálogo de Aplicativos.
        /// </summary>
        /// <param name="catalogoAplicativoDTO"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> registrarAplicativo(CatalogoAplicativoDto catalogoAplicativoDTO)
        {
            _log.Info("CatalogoRepositoryImp: MÉTODO registrarAplicativo");
            string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(catalogoAplicativoDTO, Formatting.Indented));
            _log.Info(message1);
            Console.WriteLine(message1);
            BitacoraResponse<BitacoraDtoResponse> result = AddCatalogoAplicativo(catalogoAplicativoDTO);
            string message2 = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
            _log.Info(message2);
            Console.WriteLine(message2);
            return Task.FromResult(result);
        }

        /// <summary>
        /// Implementación del método para actualizar un registro en el catálogo de Aplicativos.
        /// </summary>
        /// <param name="catalogoAplicativoDTO"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> actualizarAplicativo(CatalogoAplicativoDto catalogoAplicativoDTO)
        {
            _log.Info("CatalogoRepositoryImp: MÉTODO actualizarAplicativo");
            string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(catalogoAplicativoDTO, Formatting.Indented));
            _log.Info(message1);
            Console.WriteLine(message1);
            BitacoraResponse<BitacoraDtoResponse> result = UpdateCatalogoAplicativo(catalogoAplicativoDTO);
            string message2 = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
            _log.Info(message2);
            Console.WriteLine(message2);
            return Task.FromResult(result);
        }
        #endregion

        #region Methods Auxiliares - Catálogos Aplicativos
        /// <summary>
        /// Método que realiza la consulta a base de datos, mediante el proveedor de datos Oracle Client de ADO.NET 
        /// </summary>
        /// <remarks>Referencia: https://learn.microsoft.com/es-es/dotnet/framework/data/adonet/ado-net-code-examples</remarks>
        /// <returns></returns>
        public BitacoraResponse<List<CatalogoAplicativoDto>> ListCatalogosAplicativos()
        {
            BitacoraResponse<List<CatalogoAplicativoDto>> response = new BitacoraResponse<List<CatalogoAplicativoDto>>();
            List<CatalogoAplicativoDto> list = new List<CatalogoAplicativoDto>();
            /*Inicia proceso para traer datos a la base de datos*/
            OracleCommand command = new OracleCommand();
            OracleConnection conn = getOracleConnection.GetConnection("DataSourceBitacora");
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append("APLICATIVO_ID");
            sb.Append(",NOMBRE");
            sb.Append(",USUARIO_REGISTRO");
            sb.Append(",FECHA_REGISTRO");
            sb.Append(",USUARIO_MODIFICA");
            sb.Append(",FECHA_MODIFICA");
            sb.AppendFormat(" FROM {0}CAT_APLICATIVOS", scheme);
            sb.Append(" ORDER BY APLICATIVO_ID ASC");
            if (sb.ToString() != string.Empty)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        command.Parameters.Clear();
                        command.Connection = conn;
                        command.CommandText = sb.ToString();
                        command.CommandType = CommandType.Text;
                        command.Transaction = conn.BeginTransaction();
                        string message1 = string.Format("OracleCommand -> QUERY:\n {0}", command.CommandText);
                        _log.Info(message1);
                        Console.WriteLine(message1);
                        OracleDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            CatalogoAplicativoDto catalogoAplicativo = new CatalogoAplicativoDto() { aplicativoId = string.Empty, fechaRegistro = DateTime.Now, fechaModifica = DateTime.Now, usuarioModifica = 0, usuarioRegistro = 0};
                            if (reader["APLICATIVO_ID"] != DBNull.Value)
                                catalogoAplicativo.aplicativoId = reader["APLICATIVO_ID"].ToString()!;
                            if (reader["NOMBRE"] != DBNull.Value)
                                catalogoAplicativo.nombre = reader["NOMBRE"].ToString()!;
                            if (reader["USUARIO_REGISTRO"] != DBNull.Value)
                                catalogoAplicativo.usuarioRegistro = int.Parse(reader["USUARIO_REGISTRO"].ToString()!);
                            if (reader["FECHA_REGISTRO"] != DBNull.Value)
                            {
                                string formatFechaRegistro = reader["FECHA_REGISTRO"].ToString()!;
                                CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                                catalogoAplicativo.fechaRegistro = DateTime.Parse(formatFechaRegistro, new CultureInfo(currentCulture.Name));
                            }
                            if (reader["USUARIO_MODIFICA"] != DBNull.Value)
                                catalogoAplicativo.usuarioModifica = int.Parse(reader["USUARIO_MODIFICA"].ToString()!);
                            if (reader["FECHA_MODIFICA"] != DBNull.Value)
                            {
                                string formatFechaModifica = reader["FECHA_MODIFICA"].ToString()!;
                                CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                                catalogoAplicativo.fechaModifica = DateTime.Parse(formatFechaModifica, new CultureInfo(currentCulture.Name));
                            }
                            list.Add(catalogoAplicativo);
                        }
                        reader.Close();
                        reader.Dispose();
                        response.Codigo = 200;
                    }
                }
                catch (Exception ex)
                {
                    _log.Error("EXCEPCIÓN EJECUCIÓN DE QUERY:\n", ex);
                    Console.WriteLine("EXCEPCIÓN EJECUCIÓN DE QUERY:\n" + ex);
                    response.Codigo = 500;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            /*Termina proceso de traer datos de la base de datos*/
            response.Contenido = list;
            return response;
        }

        /// <summary>
        /// Método que realiza la inserción a base de datos, mediante el proveedor de datos Oracle Client de ADO.NET 
        /// </summary>
        /// <remarks>Referencia: https://learn.microsoft.com/es-es/dotnet/framework/data/adonet/ado-net-code-examples</remarks>
        /// <param name="catalogoAplicativoDTO"></param>
        /// <returns></returns>
        public BitacoraResponse<BitacoraDtoResponse> AddCatalogoAplicativo(CatalogoAplicativoDto catalogoAplicativoDTO)
        {
            BitacoraResponse<BitacoraDtoResponse> response = new BitacoraResponse<BitacoraDtoResponse>();
            /*Inicia proceso para insertar datos a la base de datos */
            OracleCommand command = new OracleCommand();
            OracleConnection conn = getOracleConnection.GetConnection("DataSourceBitacora");
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("INSERT INTO {0}CAT_APLICATIVOS (", scheme);
            //Se colocan las columnas
            //----------------------- 1
            sb.Append("APLICATIVO_ID");
            sb.Append(",NOMBRE");
            sb.Append(",USUARIO_REGISTRO");
            sb.Append(",FECHA_REGISTRO");
            sb.Append(",USUARIO_MODIFICA");
            sb.Append(",FECHA_MODIFICA");
            //----------------------- 1
            sb.Append(")");
            sb.Append(" values (");
            //Se colocan los valores
            //----------------------- 2
            sb.AppendFormat("'{0}'", catalogoAplicativoDTO.aplicativoId);
            sb.AppendFormat(",'{0}'", catalogoAplicativoDTO.nombre);
            sb.AppendFormat(",{0}", catalogoAplicativoDTO.usuarioRegistro);
            sb.Append(",CURRENT_TIMESTAMP");
            sb.AppendFormat(",{0}", catalogoAplicativoDTO.usuarioModifica);
            sb.Append(",CURRENT_TIMESTAMP");
            //----------------------- 2
            sb.Append(")");
            if (sb.ToString() != string.Empty)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        try
                        {
                            command.Parameters.Clear();
                            command.Connection = conn;
                            command.CommandText = sb.ToString();
                            command.CommandType = CommandType.Text;
                            command.Transaction = conn.BeginTransaction();
                            Console.WriteLine("Query ejecutado: " + command.ExecuteNonQuery());
                            string message1 = string.Format("OracleCommand -> QUERY:\n {0}", command.CommandText);
                            _log.Info(message1);
                            Console.WriteLine(message1);
                            command.Transaction.Commit();
                            response.Codigo = 200;
                            response.Contenido = new BitacoraDtoResponse { identificador = 0 };
                        }
                        catch (OracleException oe)
                        {
                            Console.WriteLine(oe.Message);
                            command.Transaction.Rollback();
                            response.Codigo = 500;
                            response.Mensaje = oe.Message;
                            _log.Error("EXCEPCIÓN EJECUCIÓN DE QUERY:\n", oe);
                            Console.WriteLine("EXCEPCIÓN EJECUCIÓN DE QUERY:\n" + oe);
                            if (oe.Message.StartsWith("ORA-00001"))
                            {
                                response.Codigo = 400;
                                response.Mensaje = string.Format("El aplicativo '{0}' ya existe.", catalogoAplicativoDTO.aplicativoId);
                            }
                        }
                        catch (Exception ex)
                        {
                            command.Transaction.Rollback();
                            response.Codigo = 500;
                            response.Mensaje = ex.Message;
                            _log.Error("EXCEPCIÓN EJECUCIÓN DE QUERY:\n", ex);
                            Console.WriteLine("EXCEPCIÓN EJECUCIÓN DE QUERY:\n" + ex);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.Codigo = 500;
                    response.Mensaje = ex.Message;
                    _log.Error("EXCEPCIÓN EJECUCIÓN DE QUERY:\n", ex);
                    Console.WriteLine("EXCEPCIÓN EJECUCIÓN DE QUERY:\n" + ex);
                }
            }
            /*Termina proceso para insertar datos a la base de datos*/
            return response;
        }

        /// <summary>
        /// Método que realiza la eliminación en base de datos, mediante el proveedor de datos Oracle Client de ADO.NET 
        /// </summary>
        /// <remarks>Referencia: https://learn.microsoft.com/es-es/dotnet/framework/data/adonet/ado-net-code-examples</remarks>
        /// <param name="aplicativoId"></param>
        /// <returns></returns>
        public BitacoraResponse<BitacoraDtoResponse> RemoveAplicativo(string aplicativoId)
        {
            BitacoraResponse<BitacoraDtoResponse> response = new BitacoraResponse<BitacoraDtoResponse>();
            /*Inicia proceso para insertar datos a la base de datos */
            OracleCommand command = new OracleCommand();
            OracleConnection conn = getOracleConnection.GetConnection("DataSourceBitacora");
            if (!string.IsNullOrEmpty(aplicativoId))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("DELETE FROM {0}CAT_APLICATIVOS", scheme);
                sb.AppendFormat(" WHERE APLICATIVO_ID='{0}'", aplicativoId);
                if (sb.ToString() != string.Empty)
                {
                    try
                    {
                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }
                        if (conn.State == ConnectionState.Open)
                        {
                            try
                            {
                                command.Parameters.Clear();
                                command.Connection = conn;
                                command.CommandText = sb.ToString();
                                command.CommandType = CommandType.Text;
                                command.Transaction = conn.BeginTransaction();
                                Console.WriteLine("Query ejecutado: " + command.ExecuteNonQuery());
                                string message1 = string.Format("OracleCommand -> QUERY:\n {0}", command.CommandText);
                                _log.Info(message1);
                                Console.WriteLine(message1);
                                command.Transaction.Commit();
                                response.Codigo = 200;
                                response.Contenido = new BitacoraDtoResponse { identificador = 0 };
                            }
                            catch (Exception ex)
                            {
                                command.Transaction.Rollback();
                                response.Codigo = 500;
                                response.Mensaje = ex.Message;
                                _log.Error("EXCEPCIÓN EJECUCIÓN DE QUERY:\n", ex);
                                Console.WriteLine("EXCEPCIÓN EJECUCIÓN DE QUERY:\n" + ex);
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        response.Codigo = 500;
                        response.Mensaje = ex.Message;
                        _log.Error("EXCEPCIÓN EJECUCIÓN DE QUERY:\n", ex);
                        Console.WriteLine("EXCEPCIÓN EJECUCIÓN DE QUERY:\n" + ex);
                    }
                }
            }
            /*Termina proceso para insertar datos a la base de datos*/
            return response;
        }

        /// <summary>
        /// Método que realiza la actualización a base de datos, mediante el proveedor de datos Oracle Client de ADO.NET 
        /// </summary>
        /// <remarks>Referencia: https://learn.microsoft.com/es-es/dotnet/framework/data/adonet/ado-net-code-examples</remarks>
        /// <param name="catalogoAplicativoDTO"></param>
        /// <returns></returns>
        public BitacoraResponse<BitacoraDtoResponse> UpdateCatalogoAplicativo(CatalogoAplicativoDto catalogoAplicativoDTO)
        {
            BitacoraResponse<BitacoraDtoResponse> response = new BitacoraResponse<BitacoraDtoResponse>();
            /*Inicia proceso para insertar datos a la base de datos */
            OracleCommand command = new OracleCommand();
            OracleConnection conn = getOracleConnection.GetConnection("DataSourceBitacora");
            if (!string.IsNullOrEmpty(catalogoAplicativoDTO.aplicativoId))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("UPDATE {0}CAT_APLICATIVOS SET ", scheme);
                //Se colocan los valores
                //----------------------- 1
                sb.AppendFormat("NOMBRE='{0}'", catalogoAplicativoDTO.nombre);
                sb.AppendFormat(",USUARIO_MODIFICA={0}", catalogoAplicativoDTO.usuarioModifica);
                sb.Append(",FECHA_MODIFICA=");
                sb.Append("CURRENT_TIMESTAMP");
                //----------------------- 1
                //Se colocan los valores
                //----------------------- 2
                sb.AppendFormat(" WHERE APLICATIVO_ID='{0}'", catalogoAplicativoDTO.aplicativoId);
                //----------------------- 2
                if (sb.ToString() != string.Empty)
                {
                    try
                    {
                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }
                        if (conn.State == ConnectionState.Open)
                        {
                            try
                            {
                                command.Parameters.Clear();
                                command.Connection = conn;
                                command.CommandText = sb.ToString();
                                command.CommandType = CommandType.Text;
                                command.Transaction = conn.BeginTransaction();
                                Console.WriteLine("Query ejecutado: " + command.ExecuteNonQuery());
                                string message1 = string.Format("OracleCommand -> QUERY:\n {0}", command.CommandText);
                                _log.Info(message1);
                                Console.WriteLine(message1);
                                command.Transaction.Commit();
                                response.Codigo = 200;
                                response.Contenido = new BitacoraDtoResponse { identificador = 0 };
                            }
                            catch (Exception ex)
                            {
                                command.Transaction.Rollback();
                                response.Codigo = 500;
                                response.Mensaje = ex.Message;
                                _log.Error("EXCEPCIÓN EJECUCIÓN DE QUERY:\n", ex);
                                Console.WriteLine("EXCEPCIÓN EJECUCIÓN DE QUERY:\n" + ex);
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        response.Codigo = 500;
                        response.Mensaje = ex.Message;
                        _log.Error("EXCEPCIÓN EJECUCIÓN DE QUERY:\n", ex);
                        Console.WriteLine("EXCEPCIÓN EJECUCIÓN DE QUERY:\n" + ex);
                    }
                }
            }
            /*Termina proceso para insertar datos a la base de datos*/
            return response;
        }
        #endregion

        #region Methods - Catálogo Usuarios
        /// <summary>
        /// Implementación del método para agregar un registro en el catálogo de Usuarios.
        /// </summary>
        /// <param name="catalogoUsuarioDto"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> guardarUsuario(CatalogoUsuarioDto catalogoUsuarioDto)
        {
            _log.Info("CatalogoRepositoryImp: MÉTODO guardarUsuario");
            string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(catalogoUsuarioDto, Formatting.Indented));
            _log.Info(message1);
            Console.WriteLine(message1);
            BitacoraResponse<BitacoraDtoResponse> result = SaveCatalogoUsuario(catalogoUsuarioDto);
            string message2 = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
            _log.Info(message2);
            Console.WriteLine(message2);
            return Task.FromResult(result);
        }

        /// <summary>
        /// Implementación del método que obtiene los registros del catálogo de Usuarios.
        /// </summary>
        /// <param name="catalogoUsuarioDto"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<List<CatalogoUsuarioDto>>> consultarUsuarios(CatalogoUsuarioDto catalogoUsuarioDto)
        {
            _log.Info("CatalogoRepositoryImp: MÉTODO consultarUsuarios");
            string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(catalogoUsuarioDto, Formatting.Indented));
            _log.Info(message1);
            Console.WriteLine(message1);
            BitacoraResponse<List<CatalogoUsuarioDto>> result = ListCatalogosUsuarios(catalogoUsuarioDto);
            string message2 = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
            _log.Info(message2);
            Console.WriteLine(message2);
            return Task.FromResult(result);
        }

        /// <summary>
        /// Implementación del método que elimina el registro del catálogo de Usuarios.
        /// </summary>
        /// <param name="identificador"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> eliminarUsuario(int identificador)
        {
            _log.Info("CatalogoRepositoryImp: MÉTODO eliminarUsuario");
            string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(identificador, Formatting.Indented));
            _log.Info(message1);
            Console.WriteLine(message1);
            BitacoraResponse<BitacoraDtoResponse> result = RemoveUsuario(identificador);
            string message2 = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
            _log.Info(message2);
            Console.WriteLine(message2);
            return Task.FromResult(result);
        }
        #endregion

        #region Methods Auxiliars - Catálogo Usuarios
        /// <summary>
        /// Método que realiza la consulta a base de datos, mediante el proveedor de datos Oracle Client de ADO.NET 
        /// </summary>
        /// <remarks>Referencia: https://learn.microsoft.com/es-es/dotnet/framework/data/adonet/ado-net-code-examples</remarks>
        /// <param name="catalogoUsuarioDto"></param>
        /// <returns></returns>
        public BitacoraResponse<List<CatalogoUsuarioDto>> ListCatalogosUsuarios(CatalogoUsuarioDto catalogoUsuarioDto)
        {
            BitacoraResponse<List<CatalogoUsuarioDto>> response = new BitacoraResponse<List<CatalogoUsuarioDto>>();
            List<CatalogoUsuarioDto> list = new List<CatalogoUsuarioDto>();
            /*Inicia proceso para traer datos a la base de datos*/
            OracleCommand command = new OracleCommand();
            OracleConnection conn = getOracleConnection.GetConnection("DataSourceBitacora");
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append("USUARIO_ID");
            sb.Append(",USUARIO");
            sb.Append(",PATERNO");
            sb.Append(",MATERNO");
            sb.Append(",NOMBRE");
            sb.Append(",SESION_ACTIVA");
            sb.Append(",USUARIO_BLOQUEADO");
            sb.Append(",INTENTOS_FALLIDOS");
            sb.Append(",USUARIO_REGISTRO");
            sb.Append(",FECHA_REGISTRO");
            sb.Append(",USUARIO_MODIFICA");
            sb.Append(",FECHA_MODIFICA");
            sb.AppendFormat(" FROM {0}CAT_USUARIOS", scheme);
            if (catalogoUsuarioDto.identificador > 0 
                || (!string.IsNullOrEmpty(catalogoUsuarioDto.usuario) && catalogoUsuarioDto.usuario != "string")
                || (!string.IsNullOrEmpty(catalogoUsuarioDto.paterno) && catalogoUsuarioDto.paterno != "string")
                || (!string.IsNullOrEmpty(catalogoUsuarioDto.materno) && catalogoUsuarioDto.materno != "string")
                || (!string.IsNullOrEmpty(catalogoUsuarioDto.nombre) && catalogoUsuarioDto.nombre != "string"))
            {
                sb.Append(" WHERE ");
                if (catalogoUsuarioDto.identificador > 0)
                {
                    sb.AppendFormat(" USUARIO_ID={0} {1}", catalogoUsuarioDto.identificador, "AND");
                }
                if (!string.IsNullOrEmpty(catalogoUsuarioDto.usuario) && catalogoUsuarioDto.usuario != "string")
                {
                    sb.AppendFormat(" USUARIO='{0}' {1}", catalogoUsuarioDto.usuario, "AND");
                }
                if (!string.IsNullOrEmpty(catalogoUsuarioDto.paterno) && catalogoUsuarioDto.paterno != "string")
                {
                    sb.AppendFormat(" PATERNO='{0}' {1}", catalogoUsuarioDto.paterno, "AND");
                }
                if (!string.IsNullOrEmpty(catalogoUsuarioDto.materno) && catalogoUsuarioDto.materno != "string")
                {
                    sb.AppendFormat(" MATERNO='{0}' {1}", catalogoUsuarioDto.materno, "AND");
                }
                if (!string.IsNullOrEmpty(catalogoUsuarioDto.nombre) && catalogoUsuarioDto.nombre != "string")
                {
                    sb.AppendFormat(" NOMBRE='{0}' {1}", catalogoUsuarioDto.nombre, "AND");
                }
                var index = sb.ToString().LastIndexOf("AND");
                if (index >= 0)
                    sb.Remove(index, 3);
            }
            sb.Append(" ORDER BY USUARIO_ID ASC");
            if (sb.ToString() != string.Empty)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        command.Parameters.Clear();
                        command.Connection = conn;
                        command.CommandText = sb.ToString();
                        command.CommandType = CommandType.Text;
                        command.Transaction = conn.BeginTransaction();
                        string message1 = string.Format("OracleCommand -> QUERY:\n {0}", command.CommandText);
                        _log.Info(message1);
                        Console.WriteLine(message1);
                        OracleDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            CatalogoUsuarioDto catalogoUsuario = new CatalogoUsuarioDto() { identificador = 0};
                            if (reader["USUARIO_ID"] != DBNull.Value)
                                catalogoUsuario.identificador = int.Parse(reader["USUARIO_ID"].ToString()!);
                            if (reader["USUARIO"] != DBNull.Value)
                                catalogoUsuario.usuario = reader["USUARIO"].ToString()!;
                            if (reader["PATERNO"] != DBNull.Value)
                                catalogoUsuario.paterno = reader["PATERNO"].ToString()!;
                            if (reader["MATERNO"] != DBNull.Value)
                                catalogoUsuario.materno = reader["MATERNO"].ToString()!;
                            if (reader["NOMBRE"] != DBNull.Value)
                                catalogoUsuario.nombre = reader["NOMBRE"].ToString()!;
                            if (reader["SESION_ACTIVA"] != DBNull.Value)
                                catalogoUsuario.sesionActiva = reader["SESION_ACTIVA"].ToString()!;
                            if (reader["USUARIO_BLOQUEADO"] != DBNull.Value)
                                catalogoUsuario.usuarioBloqueado = reader["USUARIO_BLOQUEADO"].ToString()!;
                            if (reader["INTENTOS_FALLIDOS"] != DBNull.Value)
                                catalogoUsuario.intentosFallidos = int.Parse(reader["INTENTOS_FALLIDOS"].ToString()!);
                            if (reader["USUARIO_REGISTRO"] != DBNull.Value)
                                catalogoUsuario.usuarioRegistro = int.Parse(reader["USUARIO_REGISTRO"].ToString()!);
                            if (reader["FECHA_REGISTRO"] != DBNull.Value)
                            {
                                string formatFechaRegistro = reader["FECHA_REGISTRO"].ToString()!;
                                CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                                catalogoUsuario.fechaRegistro = DateTime.Parse(formatFechaRegistro, new CultureInfo(currentCulture.Name));
                            }
                            if (reader["USUARIO_MODIFICA"] != DBNull.Value)
                                catalogoUsuario.usuarioModifica = int.Parse(reader["USUARIO_MODIFICA"].ToString()!);
                            if (reader["FECHA_MODIFICA"] != DBNull.Value)
                            {
                                string formatFechaModifica = reader["FECHA_MODIFICA"].ToString()!;
                                CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                                catalogoUsuario.fechaModifica = DateTime.Parse(formatFechaModifica, new CultureInfo(currentCulture.Name));
                            }
                            list.Add(catalogoUsuario);
                        }
                        reader.Close();
                        reader.Dispose();
                        response.Codigo = 200;
                    }
                }
                catch (Exception ex)
                {
                    response.Codigo = 500;
                    response.Mensaje = ex.Message;
                    _log.Error("EXCEPCIÓN EJECUCIÓN DE QUERY:\n", ex);
                    Console.WriteLine("EXCEPCIÓN EJECUCIÓN DE QUERY:\n" + ex);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            /*Termina proceso de traer datos de la base de datos*/
            response.Contenido = list;
            return response;
        }

        /// <summary>
        /// Método que realiza la inserción a base de datos, mediante el proveedor de datos Oracle Client de ADO.NET 
        /// </summary>
        /// <remarks>Referencia: https://learn.microsoft.com/es-es/dotnet/framework/data/adonet/ado-net-code-examples</remarks>
        /// <param name="catalogoUsuarioDto"></param>
        /// <returns></returns>
        public BitacoraResponse<BitacoraDtoResponse> SaveCatalogoUsuario(CatalogoUsuarioDto catalogoUsuarioDto)
        {
            BitacoraResponse<BitacoraDtoResponse> response = new BitacoraResponse<BitacoraDtoResponse>();
            /*Inicia proceso para insertar datos a la base de datos */
            OracleCommand command = new OracleCommand();
            OracleConnection conn = getOracleConnection.GetConnection("DataSourceBitacora");
            StringBuilder sb = GetQuerySaveUsuario(catalogoUsuarioDto);
            if (sb.ToString() != string.Empty)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        try
                        {
                            command.Parameters.Clear();
                            command.Connection = conn;
                            command.CommandText = sb.ToString();
                            command.CommandType = CommandType.Text;
                            command.Transaction = conn.BeginTransaction();
                            Console.WriteLine("Query ejecutado: " + command.ExecuteNonQuery());
                            string message1 = string.Format("OracleCommand -> QUERY:\n {0}", command.CommandText);
                            _log.Info(message1);
                            Console.WriteLine(message1);
                            command.Transaction.Commit();
                            response.Codigo = 200;
                            response.Contenido = new BitacoraDtoResponse { identificador = catalogoUsuarioDto.identificador };
                        }
                        catch (OracleException oe)
                        {
                            Console.WriteLine(oe.Message);
                            command.Transaction.Rollback();
                            response.Codigo = 500;
                            response.Mensaje = oe.Message;
                            _log.Error("EXCEPCIÓN EJECUCIÓN DE QUERY:\n", oe);
                            Console.WriteLine("EXCEPCIÓN EJECUCIÓN DE QUERY:\n" + oe);
                            if (oe.Message.StartsWith("ORA-00001"))
                            {
                                response.Codigo = 400;
                                response.Mensaje = string.Format("El usuario '{0}' ya existe.", catalogoUsuarioDto.identificador);
                            }
                        }
                        catch (Exception ex)
                        {
                            command.Transaction.Rollback();
                            response.Codigo = 500;
                            response.Mensaje = ex.Message;
                            _log.Error("EXCEPCIÓN EJECUCIÓN DE QUERY:\n", ex);
                            Console.WriteLine("EXCEPCIÓN EJECUCIÓN DE QUERY:\n" + ex);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.Codigo = 500;
                    response.Mensaje = ex.Message;
                    _log.Error("EXCEPCIÓN EJECUCIÓN DE QUERY:\n", ex);
                    Console.WriteLine("EXCEPCIÓN EJECUCIÓN DE QUERY:\n" + ex);
                }
            }
            /*Termina proceso para insertar datos a la base de datos*/
            return response;
        }

        /// <summary>
        /// Método que realiza la eliminación en base de datos, mediante el proveedor de datos Oracle Client de ADO.NET 
        /// </summary>
        /// <remarks>Referencia: https://learn.microsoft.com/es-es/dotnet/framework/data/adonet/ado-net-code-examples</remarks>
        /// <param name="identificador"></param>
        /// <returns></returns>
        public BitacoraResponse<BitacoraDtoResponse> RemoveUsuario(int identificador)
        {
            BitacoraResponse<BitacoraDtoResponse> response = new BitacoraResponse<BitacoraDtoResponse>();
            /*Inicia proceso para insertar datos a la base de datos */
            OracleCommand command = new OracleCommand();
            OracleConnection conn = getOracleConnection.GetConnection("DataSourceBitacora");
            if (identificador > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("DELETE FROM {0}CAT_USUARIOS", scheme);
                sb.AppendFormat(" WHERE USUARIO_ID='{0}'", identificador);
                if (sb.ToString() != string.Empty)
                {
                    try
                    {
                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }
                        if (conn.State == ConnectionState.Open)
                        {
                            try
                            {
                                command.Parameters.Clear();
                                command.Connection = conn;
                                command.CommandText = sb.ToString();
                                command.CommandType = CommandType.Text;
                                command.Transaction = conn.BeginTransaction();
                                Console.WriteLine("Query ejecutado: " + command.ExecuteNonQuery());
                                string message1 = string.Format("OracleCommand -> QUERY:\n {0}", command.CommandText);
                                _log.Info(message1);
                                Console.WriteLine(message1);
                                command.Transaction.Commit();
                                response.Codigo = 200;
                                response.Contenido = new BitacoraDtoResponse { identificador = 0 };
                            }
                            catch (Exception ex)
                            {
                                command.Transaction.Rollback();
                                response.Codigo = 500;
                                response.Mensaje = ex.Message;
                                _log.Error("EXCEPCIÓN EJECUCIÓN DE QUERY:\n", ex);
                                Console.WriteLine("EXCEPCIÓN EJECUCIÓN DE QUERY:\n" + ex);
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        response.Codigo = 500;
                        response.Mensaje = ex.Message;
                        _log.Error("EXCEPCIÓN EJECUCIÓN DE QUERY:\n", ex);
                        Console.WriteLine("EXCEPCIÓN EJECUCIÓN DE QUERY:\n" + ex);
                    }
                }
            }
            /*Termina proceso para insertar datos a la base de datos*/
            return response;
        }

        /// <summary>
        /// Crea el query para guardar el usuario (insert o update)
        /// </summary>
        /// <param name="catalogoUsuarioDto"></param>
        /// <returns></returns>
        public StringBuilder GetQuerySaveUsuario(CatalogoUsuarioDto catalogoUsuarioDto)
        {
            StringBuilder sb = new StringBuilder();
            if (catalogoUsuarioDto != null)
            {
                if (catalogoUsuarioDto.identificador > 0)
                {
                    sb.AppendFormat("UPDATE {0}CAT_USUARIOS SET ", scheme);
                    sb.AppendFormat("INTENTOS_FALLIDOS={0}{1}", catalogoUsuarioDto.intentosFallidos, ",");
                    if (!string.IsNullOrEmpty(catalogoUsuarioDto.usuario) && catalogoUsuarioDto.usuario != "string")
                        sb.AppendFormat("USUARIO='{0}'{1}", catalogoUsuarioDto.usuario, ",");
                    if (!string.IsNullOrEmpty(catalogoUsuarioDto.paterno) && catalogoUsuarioDto.paterno != "string")
                        sb.AppendFormat("PATERNO='{0}'{1}", catalogoUsuarioDto.paterno, ",");
                    if (!string.IsNullOrEmpty(catalogoUsuarioDto.materno) && catalogoUsuarioDto.materno != "string")
                        sb.AppendFormat("MATERNO='{0}'{1}", catalogoUsuarioDto.materno, ",");
                    if (!string.IsNullOrEmpty(catalogoUsuarioDto.nombre) && catalogoUsuarioDto.nombre != "string")
                        sb.AppendFormat("NOMBRE='{0}'{1}", catalogoUsuarioDto.nombre, ",");
                    if (!string.IsNullOrEmpty(catalogoUsuarioDto.sesionActiva) && catalogoUsuarioDto.sesionActiva != "string")
                        sb.AppendFormat("SESION_ACTIVA='{0}'{1}", catalogoUsuarioDto.sesionActiva, ",");
                    if (!string.IsNullOrEmpty(catalogoUsuarioDto.usuarioBloqueado) && catalogoUsuarioDto.usuarioBloqueado != "string")
                        sb.AppendFormat("USUARIO_BLOQUEADO='{0}'{1}", catalogoUsuarioDto.usuarioBloqueado, ",");
                    if (catalogoUsuarioDto.usuarioModifica > 0)
                        sb.AppendFormat("USUARIO_MODIFICA='{0}'{1}", catalogoUsuarioDto.usuarioModifica, ",");
                    if (catalogoUsuarioDto.fechaModifica != DateTime.MinValue)
                        sb.AppendFormat("FECHA_MODIFICA=to_date('{0}','yyyy-MM-dd hh24:mi:ss')", catalogoUsuarioDto.fechaModifica.ToString("yyyy-MM-dd HH:mm:ss"));
                    else
                        sb.AppendFormat("FECHA_MODIFICA=to_date('{0}','yyyy-MM-dd hh24:mi:ss')", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    sb.AppendFormat(" WHERE USUARIO_ID={0}", catalogoUsuarioDto.identificador);
                }
                else
                {
                    sb.AppendFormat("INSERT INTO {0}CAT_USUARIOS (", scheme);
                    //Se colocan las columnas
                    //----------------------- 1
                    sb.Append("USUARIO_ID");
                    sb.Append(",USUARIO");
                    sb.Append(",PATERNO");
                    sb.Append(",MATERNO");
                    sb.Append(",NOMBRE");
                    sb.Append(",SESION_ACTIVA");
                    sb.Append(",USUARIO_BLOQUEADO");
                    sb.Append(",INTENTOS_FALLIDOS");
                    sb.Append(",USUARIO_REGISTRO");
                    sb.Append(",FECHA_REGISTRO");
                    sb.Append(",USUARIO_MODIFICA");
                    sb.Append(",FECHA_MODIFICA");
                    //----------------------- 1
                    sb.Append(")");
                    sb.Append(" values (");
                    //Se colocan los valores
                    //----------------------- 2
                    sb.Append("SEC_USUARIO_ID.NEXTVAL");
                    sb.AppendFormat(",'{0}'", catalogoUsuarioDto.usuario);
                    sb.AppendFormat(",'{0}'", catalogoUsuarioDto.paterno);
                    sb.AppendFormat(",'{0}'", catalogoUsuarioDto.materno);
                    sb.AppendFormat(",'{0}'", catalogoUsuarioDto.nombre);
                    sb.Append(",'N'");
                    sb.Append(",'N'");
                    sb.Append(",0");
                    sb.AppendFormat(",{0}", catalogoUsuarioDto.usuarioRegistro);
                    if (catalogoUsuarioDto.fechaRegistro != DateTime.MinValue)
                        sb.AppendFormat(",to_date('{0}','yyyy-MM-dd hh24:mi:ss')", catalogoUsuarioDto.fechaRegistro.ToString("yyyy-MM-dd HH:mm:ss"));
                    else
                        sb.AppendFormat(",to_date('{0}','yyyy-MM-dd hh24:mi:ss')", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    sb.AppendFormat(",{0}", catalogoUsuarioDto.usuarioModifica);
                    sb.Append(",CURRENT_TIMESTAMP");
                    //----------------------- 2
                    sb.Append(")");
                }
            }
            return sb;
        }
        #endregion
    }
}
