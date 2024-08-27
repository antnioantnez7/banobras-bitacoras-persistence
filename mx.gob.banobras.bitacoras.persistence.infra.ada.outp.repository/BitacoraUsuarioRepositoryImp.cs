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
    public class BitacoraUsuarioRepositoryImp : IBitacoraUsuarioRepositoryOutPort
    {
        #region Properties
        readonly IConfiguration configuration;
        readonly GetOracleConnection getOracleConnection;
        readonly string scheme = "BITACORA.";
        /// <summary>
        /// Instancia de la interfaz de logueo
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(BitacoraOperacionRepositoryImp));

        public IConfiguration Configuration => configuration;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la implementación repositorio que recibe la inyección de dependencias del archivo de configuración de Program.cs
        /// </summary>
        /// <param name="_configuration"></param>
        public BitacoraUsuarioRepositoryImp(IConfiguration _configuration)
        {
            configuration = _configuration;
            getOracleConnection = new GetOracleConnection(configuration);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Implementación del método 'consultar', se realiza la conexión a la base de datos
        /// </summary>
        /// <param name="bitacoraConsultaDTO"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<List<BitacoraUsuarioDto>>> consultar(BitacoraConsultaDto bitacoraConsultaDTO)
        {
            _log.Info("BitacoraUsuarioRepositoryImp: MÉTODO consultar");
            string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(bitacoraConsultaDTO, Formatting.Indented));
            _log.Info(message1);
            Console.WriteLine(message1);
            BitacoraResponse<List<BitacoraUsuarioDto>> result = ListBitacoraUsuario(bitacoraConsultaDTO);
            string message2 = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
            _log.Info(message2);
            Console.WriteLine(message2);
            return Task.FromResult(result);
        }

        /// <summary>
        /// Implementación del método 'registrar', se realiza la conexión a la base de datos
        /// </summary>
        /// <param name="bitacoraUsuarioDTO"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> registrar(BitacoraUsuarioDto bitacoraUsuarioDTO)
        {
            _log.Info("BitacoraUsuarioRepositoryImp: MÉTODO registrar");
            string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(bitacoraUsuarioDTO, Formatting.Indented));
            _log.Info(message1);
            Console.WriteLine(message1);
            BitacoraResponse<BitacoraDtoResponse> result = AddBitacoraUsuario(bitacoraUsuarioDTO);
            string message2 = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
            _log.Info(message2);
            Console.WriteLine(message2);
            return Task.FromResult(result);
        }
        #endregion

        #region MethodsAuxiliars
        /// <summary>
        /// Método que realiza la consulta a base de datos, mediante el proveedor de datos Oracle Client de ADO.NET 
        /// </summary>
        /// <remarks>Referencia: https://learn.microsoft.com/es-es/dotnet/framework/data/adonet/ado-net-code-examples</remarks>
        /// <param name="bitacoraConsultaDTO"></param>
        /// <returns></returns>
        public BitacoraResponse<List<BitacoraUsuarioDto>> ListBitacoraUsuario(BitacoraConsultaDto bitacoraConsultaDTO)
        {
            BitacoraResponse<List<BitacoraUsuarioDto>> response = new BitacoraResponse<List<BitacoraUsuarioDto>>();
            List<BitacoraUsuarioDto> list = new List<BitacoraUsuarioDto>();
            /*Inicia proceso para traer datos a la base de datos*/
            OracleCommand command = new OracleCommand();
            OracleConnection conn = getOracleConnection.GetConnection("DataSourceBitacora");
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            if (bitacoraConsultaDTO.historico)
                sb.Append("HISTORICO_USUARIO_ID");
            else
                sb.Append("BITACORA_USUARIO_ID");
            sb.Append(",APLICATIVO_ID");
            sb.Append(",CAPA");
            sb.Append(",METODO");
            sb.Append(",PROCESO");
            sb.Append(",SUBPROCESO");
            sb.Append(",DETALLE_OPERACION");
            sb.Append(",TRANSACCION_ID");
            sb.Append(",IP_EQUIPO");
            sb.Append(",FECHA_HORA_TRANSACCION");
            sb.Append(",USUARIO_OPERADOR");
            sb.Append(",NOMBRE_COMPLETO_OPERADOR");
            sb.Append(",EXPEDIENTE_OPERADOR");
            sb.Append(",AREA_OPERADOR");
            sb.Append(",PUESTO_OPERADOR");
            sb.Append(",USUARIO");
            sb.Append(",NOMBRE_COMPLETO_USUARIO");
            sb.Append(",EXPEDIENTE_USUARIO");
            sb.Append(",AREA_USUARIO");
            sb.Append(",PUESTO_USUARIO");
            sb.Append(",ESTATUS_OPERACION");
            sb.Append(",RESPUESTA_OPERACION");
            if (bitacoraConsultaDTO.historico) 
                sb.AppendFormat(" FROM {0}HIS_USUARIOS", scheme);
            else
                sb.AppendFormat(" FROM {0}BIT_USUARIOS", scheme);
            if (!string.IsNullOrEmpty(bitacoraConsultaDTO.aplicativoId) || bitacoraConsultaDTO.aplicativoId != "string" || !string.IsNullOrEmpty(bitacoraConsultaDTO.fechaHoraIni.ToString()) || !string.IsNullOrEmpty(bitacoraConsultaDTO.fechaHoraFin.ToString()))
            {
                if (!string.IsNullOrEmpty(bitacoraConsultaDTO.aplicativoId) && bitacoraConsultaDTO.aplicativoId != "string")
                {
                    sb.AppendFormat(" WHERE APLICATIVO_ID = '{0}'", bitacoraConsultaDTO.aplicativoId);
                    if (!string.IsNullOrEmpty(bitacoraConsultaDTO.fechaHoraIni.ToString()) && !string.IsNullOrEmpty(bitacoraConsultaDTO.fechaHoraFin.ToString()))
                    {
                        sb.AppendFormat(" AND FECHA_HORA_TRANSACCION >= to_date('{0}','DD/MM/YY hh24:mi:ss')", bitacoraConsultaDTO.fechaHoraIni.ToString("dd/MM/yy HH:mm:ss"));
                        sb.AppendFormat(" AND FECHA_HORA_TRANSACCION < to_date('{0}','DD/MM/YY hh24:mi:ss')", bitacoraConsultaDTO.fechaHoraFin.ToString("dd/MM/yy HH:mm:ss"));
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(bitacoraConsultaDTO.fechaHoraIni.ToString()) && !string.IsNullOrEmpty(bitacoraConsultaDTO.fechaHoraFin.ToString()))
                    {
                        sb.AppendFormat(" WHERE FECHA_HORA_TRANSACCION >= to_date('{0}','DD/MM/YY hh24:mi:ss')", bitacoraConsultaDTO.fechaHoraIni.ToString("dd/MM/yy HH:mm:ss"));
                        sb.AppendFormat(" AND FECHA_HORA_TRANSACCION < to_date('{0}','DD/MM/YY hh24:mi:ss')", bitacoraConsultaDTO.fechaHoraFin.ToString("dd/MM/yy HH:mm:ss"));
                    }
                }
            }
            sb.Append(" ORDER BY FECHA_HORA_TRANSACCION DESC");
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
                            BitacoraUsuarioDto usuario = new BitacoraUsuarioDto() { identificador = 0, fechaHoraOperacion = DateTime.Now };
                            if (bitacoraConsultaDTO.historico)
                            {
                                if (reader["HISTORICO_USUARIO_ID"] != DBNull.Value)
                                    usuario.identificador = int.Parse(reader["HISTORICO_USUARIO_ID"].ToString()!);
                            }
                            else
                            {
                                if (reader["BITACORA_USUARIO_ID"] != DBNull.Value)
                                    usuario.identificador = int.Parse(reader["BITACORA_USUARIO_ID"].ToString()!);
                            }
                            if (reader["APLICATIVO_ID"] != DBNull.Value)
                                usuario.aplicativoId = reader["APLICATIVO_ID"].ToString()!;
                            if (reader["CAPA"] != DBNull.Value)
                                usuario.capa = reader["CAPA"].ToString()!;
                            if (reader["METODO"] != DBNull.Value)
                                usuario.metodo = reader["METODO"].ToString()!;
                            if (reader["PROCESO"] != DBNull.Value)
                                usuario.proceso = reader["PROCESO"].ToString()!;
                            if (reader["SUBPROCESO"] != DBNull.Value)
                                usuario.subproceso = reader["SUBPROCESO"].ToString()!;
                            if (reader["DETALLE_OPERACION"] != DBNull.Value)
                                usuario.detalleOperacion = reader["DETALLE_OPERACION"].ToString()!;
                            if (reader["TRANSACCION_ID"] != DBNull.Value)
                                usuario.transaccionId = reader["TRANSACCION_ID"].ToString()!;
                            if (reader["IP_EQUIPO"] != DBNull.Value)
                                usuario.ipEquipo = reader["IP_EQUIPO"].ToString()!;
                            if (reader["FECHA_HORA_TRANSACCION"] != DBNull.Value)
                            {
                                string formatFechaHoraOperacion = reader["FECHA_HORA_TRANSACCION"].ToString()!;
                                CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                                usuario.fechaHoraOperacion = DateTime.Parse(formatFechaHoraOperacion, new CultureInfo(currentCulture.Name));
                            }
                            if (reader["USUARIO_OPERADOR"] != DBNull.Value)
                                usuario.usuarioOperador = reader["USUARIO_OPERADOR"].ToString()!;
                            if (reader["NOMBRE_COMPLETO_OPERADOR"] != DBNull.Value)
                                usuario.nombreOperador = reader["NOMBRE_COMPLETO_OPERADOR"].ToString()!;
                            if (reader["EXPEDIENTE_OPERADOR"] != DBNull.Value)
                                usuario.expedienteOperador = reader["EXPEDIENTE_OPERADOR"].ToString()!;
                            if (reader["AREA_OPERADOR"] != DBNull.Value)
                                usuario.areaOperador = reader["AREA_OPERADOR"].ToString()!;
                            if (reader["PUESTO_OPERADOR"] != DBNull.Value)
                                usuario.puestoOperador = reader["PUESTO_OPERADOR"].ToString()!;
                            if (reader["USUARIO"] != DBNull.Value)
                                usuario.usuario = reader["USUARIO"].ToString()!;
                            if (reader["NOMBRE_COMPLETO_USUARIO"] != DBNull.Value)
                                usuario.nombreUsuario = reader["NOMBRE_COMPLETO_USUARIO"].ToString()!;
                            if (reader["EXPEDIENTE_USUARIO"] != DBNull.Value)
                                usuario.expedienteUsuario = reader["EXPEDIENTE_USUARIO"].ToString()!;
                            if (reader["AREA_USUARIO"] != DBNull.Value)
                                usuario.areaUsuario = reader["AREA_USUARIO"].ToString()!;
                            if (reader["PUESTO_USUARIO"] != DBNull.Value)
                                usuario.puestoUsuario = reader["PUESTO_USUARIO"].ToString()!;
                            if (reader["ESTATUS_OPERACION"] != DBNull.Value)
                                usuario.estatusOperacion = reader["ESTATUS_OPERACION"].ToString()!;
                            if (reader["RESPUESTA_OPERACION"] != DBNull.Value)
                                usuario.respuestaOperacion = reader["RESPUESTA_OPERACION"].ToString()!;
                            list.Add(usuario);
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
                    response.Mensaje = ex.Message;
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
        /// <param name="bitacoraUsuarioDTO"></param>
        /// <returns></returns>
        public BitacoraResponse<BitacoraDtoResponse> AddBitacoraUsuario(BitacoraUsuarioDto bitacoraUsuarioDTO)
        {
            BitacoraResponse<BitacoraDtoResponse> response = new BitacoraResponse<BitacoraDtoResponse>();
            /*Inicia proceso para insertar datos a la base de datos */
            OracleCommand command = new OracleCommand();
            OracleConnection conn = getOracleConnection.GetConnection("DataSourceBitacora");
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("INSERT INTO {0}BIT_USUARIOS (", scheme);
            //Se colocan las columnas
            //----------------------- 1
            sb.Append("BITACORA_USUARIO_ID");
            sb.Append(",APLICATIVO_ID");
            sb.Append(",CAPA");
            sb.Append(",METODO");
            sb.Append(",PROCESO");
            sb.Append(",SUBPROCESO");
            sb.Append(",DETALLE_OPERACION");
            sb.Append(",TRANSACCION_ID");
            sb.Append(",IP_EQUIPO");
            sb.Append(",FECHA_HORA_TRANSACCION");
            sb.Append(",USUARIO_OPERADOR");
            sb.Append(",NOMBRE_COMPLETO_OPERADOR");
            sb.Append(",EXPEDIENTE_OPERADOR");
            sb.Append(",AREA_OPERADOR");
            sb.Append(",PUESTO_OPERADOR");
            sb.Append(",USUARIO");
            sb.Append(",NOMBRE_COMPLETO_USUARIO");
            sb.Append(",EXPEDIENTE_USUARIO");
            sb.Append(",AREA_USUARIO");
            sb.Append(",PUESTO_USUARIO");
            sb.Append(",ESTATUS_OPERACION");
            sb.Append(",RESPUESTA_OPERACION");
            //----------------------- 1
            sb.Append(")");
            sb.Append(" values (");
            //Se colocan los valores
            //----------------------- 2
            sb.Append("SEC_BITACORA_USUARIO_ID.NEXTVAL");
            sb.AppendFormat(",'{0}'", bitacoraUsuarioDTO.aplicativoId);
            sb.AppendFormat(",'{0}'", bitacoraUsuarioDTO.capa);
            sb.AppendFormat(",'{0}'", bitacoraUsuarioDTO.metodo);
            sb.AppendFormat(",'{0}'", bitacoraUsuarioDTO.proceso);
            sb.AppendFormat(",'{0}'", bitacoraUsuarioDTO.subproceso);
            sb.AppendFormat(",'{0}'", bitacoraUsuarioDTO.detalleOperacion);
            sb.AppendFormat(",'{0}'", bitacoraUsuarioDTO.transaccionId);
            sb.AppendFormat(",'{0}'", bitacoraUsuarioDTO.ipEquipo);
            if (bitacoraUsuarioDTO.fechaHoraOperacion != DateTime.MinValue)
                sb.AppendFormat(",to_date('{0}','yyyy-MM-dd hh24:mi:ss')", bitacoraUsuarioDTO.fechaHoraOperacion.ToString("yyyy-MM-dd HH:mm:ss"));
            else
                sb.AppendFormat(",to_date('{0}','yyyy-MM-dd hh24:mi:ss')", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendFormat(",'{0}'", bitacoraUsuarioDTO.usuarioOperador);
            sb.AppendFormat(",'{0}'", bitacoraUsuarioDTO.nombreOperador);
            sb.AppendFormat(",'{0}'", bitacoraUsuarioDTO.expedienteOperador);
            sb.AppendFormat(",'{0}'", bitacoraUsuarioDTO.areaOperador);
            sb.AppendFormat(",'{0}'", bitacoraUsuarioDTO.puestoOperador);
            sb.AppendFormat(",'{0}'", bitacoraUsuarioDTO.usuario);
            sb.AppendFormat(",'{0}'", bitacoraUsuarioDTO.nombreUsuario);
            sb.AppendFormat(",'{0}'", bitacoraUsuarioDTO.expedienteUsuario);
            sb.AppendFormat(",'{0}'", bitacoraUsuarioDTO.areaUsuario);
            sb.AppendFormat(",'{0}'", bitacoraUsuarioDTO.puestoUsuario);
            sb.AppendFormat(",'{0}'", bitacoraUsuarioDTO.estatusOperacion);
            sb.AppendFormat(",'{0}'", bitacoraUsuarioDTO.respuestaOperacion);
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
                            response.Contenido = new BitacoraDtoResponse { identificador = bitacoraUsuarioDTO.identificador };
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
        #endregion
    }
}
