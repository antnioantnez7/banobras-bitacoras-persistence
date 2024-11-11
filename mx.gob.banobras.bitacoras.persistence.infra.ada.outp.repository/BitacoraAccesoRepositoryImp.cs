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
    public class BitacoraAccesoRepositoryImp : IBitacoraAccesoRepositoryOutPort
    {
        #region Properties
        private readonly IConfiguration configuration;
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
        public BitacoraAccesoRepositoryImp(IConfiguration _configuration)
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
        public Task<BitacoraResponse<List<BitacoraAccesoDto>>> consultar(BitacoraConsultaDto bitacoraConsultaDTO)
        {
            _log.Info("BitacoraAccesoRepositoryImp: MÉTODO consultar");
            string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(bitacoraConsultaDTO, Formatting.Indented));
            _log.Info(message1);
            Console.WriteLine(message1);
            BitacoraResponse<List<BitacoraAccesoDto>> result = ListBitacoraAcceso(bitacoraConsultaDTO);
            string message2 = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
            _log.Info(message2);
            Console.WriteLine(message2);
            return Task.FromResult(result);
        }

        /// <summary>
        /// Implementación del método 'registrar', se realiza la conexión a la base de datos
        /// </summary>
        /// <param name="bitacoraAccesoDTO"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> registrar(BitacoraAccesoDto bitacoraAccesoDTO)
        {
            _log.Info("BitacoraAccesoRepositoryImp: MÉTODO registrar");
            string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(bitacoraAccesoDTO, Formatting.Indented));
            _log.Info(message1);
            Console.WriteLine(message1);
            BitacoraResponse<BitacoraDtoResponse> result = AddBitacoraAcceso(bitacoraAccesoDTO);
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
        public BitacoraResponse<List<BitacoraAccesoDto>> ListBitacoraAcceso(BitacoraConsultaDto bitacoraConsultaDTO)
        {
            BitacoraResponse<List<BitacoraAccesoDto>> response = new BitacoraResponse<List<BitacoraAccesoDto>>();
            List<BitacoraAccesoDto> list = new List<BitacoraAccesoDto>();
            /*Inicia proceso para traer datos a la base de datos*/
            OracleCommand command = new OracleCommand();
            OracleConnection conn = getOracleConnection.GetConnection("DataSourceBitacora");
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            if (bitacoraConsultaDTO.historico)
                sb.Append("HISTORICO_ACCESO_ID");
            else
                sb.Append("BITACORA_ACCESO_ID");
            sb.Append(",APLICATIVO_ID");
            sb.Append(",CAPA");
            sb.Append(",METODO");
            sb.Append(",ACTIVIDAD");
            sb.Append(",TRANSACCION_ID");
            sb.Append(",IP_EQUIPO");
            sb.Append(",FECHA_HORA_ACCESO");
            sb.Append(",USUARIO_ACCESO");
            sb.Append(",NOMBRE_USUARIO");
            sb.Append(",EXPEDIENTE_USUARIO");
            sb.Append(",AREA_USUARIO");
            sb.Append(",PUESTO_USUARIO");
            sb.Append(",ESTATUS_OPERACION");
            sb.Append(",RESPUESTA_OPERACION");
            if (bitacoraConsultaDTO.historico)
                sb.AppendFormat(" FROM {0}HIS_ACCESOS", scheme);
            else
                sb.AppendFormat(" FROM {0}BIT_ACCESOS", scheme);
            if (!string.IsNullOrEmpty(bitacoraConsultaDTO.aplicativoId) || bitacoraConsultaDTO.aplicativoId != "string" || bitacoraConsultaDTO.aplicativoId != "string" || !string.IsNullOrEmpty(bitacoraConsultaDTO.fechaHoraIni.ToString()) || !string.IsNullOrEmpty(bitacoraConsultaDTO.fechaHoraFin.ToString()))
            {
                if (!string.IsNullOrEmpty(bitacoraConsultaDTO.aplicativoId) && bitacoraConsultaDTO.aplicativoId != "string")
                {
                    sb.AppendFormat(" WHERE APLICATIVO_ID = '{0}'", bitacoraConsultaDTO.aplicativoId);
                    if (!string.IsNullOrEmpty(bitacoraConsultaDTO.fechaHoraIni.ToString()) && !string.IsNullOrEmpty(bitacoraConsultaDTO.fechaHoraFin.ToString()))
                    {
                        sb.AppendFormat(" AND FECHA_HORA_ACCESO >= to_date('{0}','DD/MM/YY hh24:mi:ss')", bitacoraConsultaDTO.fechaHoraIni.ToString("dd/MM/yy HH:mm:ss"));
                        sb.AppendFormat(" AND FECHA_HORA_ACCESO < to_date('{0}','DD/MM/YY hh24:mi:ss')", bitacoraConsultaDTO.fechaHoraFin.ToString("dd/MM/yy HH:mm:ss"));
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(bitacoraConsultaDTO.fechaHoraIni.ToString()) && !string.IsNullOrEmpty(bitacoraConsultaDTO.fechaHoraFin.ToString()))
                    {
                        sb.AppendFormat(" WHERE FECHA_HORA_ACCESO >= to_date('{0}','DD/MM/YY hh24:mi:ss')", bitacoraConsultaDTO.fechaHoraIni.ToString("dd/MM/yy HH:mm:ss"));
                        sb.AppendFormat(" AND FECHA_HORA_ACCESO < to_date('{0}','DD/MM/YY hh24:mi:ss')", bitacoraConsultaDTO.fechaHoraFin.ToString("dd/MM/yy HH:mm:ss"));
                    }
                }
            }
            sb.Append(" ORDER BY FECHA_HORA_ACCESO DESC");
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
                            BitacoraAccesoDto acceso = new BitacoraAccesoDto() { identificador = 0, fechaHoraAcceso = DateTime.Now };
                            if (bitacoraConsultaDTO.historico)
                            {
                                if (reader["HISTORICO_ACCESO_ID"] != DBNull.Value)
                                    acceso.identificador = int.Parse(reader["HISTORICO_ACCESO_ID"].ToString()!);
                            }
                            else
                            {
                                if (reader["BITACORA_ACCESO_ID"] != DBNull.Value)
                                    acceso.identificador = int.Parse(reader["BITACORA_ACCESO_ID"].ToString()!);
                            }
                            if (reader["APLICATIVO_ID"] != DBNull.Value)
                                acceso.aplicativoId = reader["APLICATIVO_ID"].ToString()!;
                            if (reader["CAPA"] != DBNull.Value)
                                acceso.capa = reader["CAPA"].ToString()!;
                            if (reader["METODO"] != DBNull.Value)
                                acceso.metodo = reader["METODO"].ToString()!;
                            if (reader["ACTIVIDAD"] != DBNull.Value)
                                acceso.actividad = reader["ACTIVIDAD"].ToString()!;
                            if (reader["TRANSACCION_ID"] != DBNull.Value)
                                acceso.transaccionId = reader["TRANSACCION_ID"].ToString()!;
                            if (reader["IP_EQUIPO"] != DBNull.Value)
                                acceso.ipEquipo = reader["IP_EQUIPO"].ToString()!;
                            if (reader["FECHA_HORA_ACCESO"] != DBNull.Value)
                            {
                                string formatFechaHoraAcceso = reader["FECHA_HORA_ACCESO"].ToString()!;
                                CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                                acceso.fechaHoraAcceso = DateTime.Parse(formatFechaHoraAcceso, new CultureInfo(currentCulture.Name));
                            }
                            if (reader["USUARIO_ACCESO"] != DBNull.Value)
                                acceso.usuarioAcceso = reader["USUARIO_ACCESO"].ToString()!;
                            if (reader["NOMBRE_USUARIO"] != DBNull.Value)
                                acceso.nombreUsuario = reader["NOMBRE_USUARIO"].ToString()!;
                            if (reader["EXPEDIENTE_USUARIO"] != DBNull.Value)
                                acceso.expedienteUsuario = reader["EXPEDIENTE_USUARIO"].ToString()!;
                            if (reader["AREA_USUARIO"] != DBNull.Value)
                                acceso.areaUsuario = reader["AREA_USUARIO"].ToString()!;
                            if (reader["PUESTO_USUARIO"] != DBNull.Value)
                                acceso.puestoUsuario = reader["PUESTO_USUARIO"].ToString()!;
                            if (reader["ESTATUS_OPERACION"] != DBNull.Value)
                                acceso.estatusOperacion = reader["ESTATUS_OPERACION"].ToString()!;
                            if (reader["RESPUESTA_OPERACION"] != DBNull.Value)
                                acceso.respuestaOperacion = reader["RESPUESTA_OPERACION"].ToString()!;

                            string myEnvVar1 = Environment.GetEnvironmentVariable("DataSourceBitacora");

                            // Verificar si las variables existen y mostrarlas
                            if (!string.IsNullOrEmpty(myEnvVar1))
                            {
                                Console.WriteLine("DataSourceBitacora: " + myEnvVar1);
                                acceso.respuestaOperacion = "DataSourceBitacora: " + myEnvVar1;
                            }
                            list.Add(acceso);
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
        /// <param name="bitacoraAccesoDTO"></param>
        /// <returns></returns>
        public BitacoraResponse<BitacoraDtoResponse> AddBitacoraAcceso(BitacoraAccesoDto bitacoraAccesoDTO)
        {
            BitacoraResponse<BitacoraDtoResponse> response = new BitacoraResponse<BitacoraDtoResponse>();
            /*Inicia proceso para insertar datos a la base de datos */
            OracleCommand command = new OracleCommand();
            OracleConnection conn = getOracleConnection.GetConnection("DataSourceBitacora");
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("INSERT INTO {0}BIT_ACCESOS (", scheme);
            //Se colocan las columnas
            //----------------------- 1
            sb.Append("BITACORA_ACCESO_ID");
            sb.Append(",APLICATIVO_ID");
            sb.Append(",CAPA");
            sb.Append(",METODO");
            sb.Append(",ACTIVIDAD");
            sb.Append(",TRANSACCION_ID");
            sb.Append(",IP_EQUIPO");
            sb.Append(",FECHA_HORA_ACCESO");
            sb.Append(",USUARIO_ACCESO");
            sb.Append(",NOMBRE_USUARIO");
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
            sb.Append("SEC_BITACORA_ACCESO_ID.NEXTVAL");
            sb.AppendFormat(",'{0}'", bitacoraAccesoDTO.aplicativoId);
            sb.AppendFormat(",'{0}'", bitacoraAccesoDTO.capa);
            sb.AppendFormat(",'{0}'", bitacoraAccesoDTO.metodo);
            sb.AppendFormat(",'{0}'", bitacoraAccesoDTO.actividad);
            sb.AppendFormat(",'{0}'", bitacoraAccesoDTO.transaccionId);
            sb.AppendFormat(",'{0}'", bitacoraAccesoDTO.ipEquipo);
            if (bitacoraAccesoDTO.fechaHoraAcceso != DateTime.MinValue)
                sb.AppendFormat(",to_date('{0}','yyyy-MM-dd hh24:mi:ss')", bitacoraAccesoDTO.fechaHoraAcceso.ToString("yyyy-MM-dd HH:mm:ss"));
            else
                sb.AppendFormat(",to_date('{0}','yyyy-MM-dd hh24:mi:ss')", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendFormat(",'{0}'", bitacoraAccesoDTO.usuarioAcceso);
            sb.AppendFormat(",'{0}'", bitacoraAccesoDTO.nombreUsuario);
            sb.AppendFormat(",'{0}'", bitacoraAccesoDTO.expedienteUsuario);
            sb.AppendFormat(",'{0}'", bitacoraAccesoDTO.areaUsuario);
            sb.AppendFormat(",'{0}'", bitacoraAccesoDTO.puestoUsuario);
            sb.AppendFormat(",'{0}'", bitacoraAccesoDTO.estatusOperacion);
            sb.AppendFormat(",'{0}'", bitacoraAccesoDTO.respuestaOperacion);
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
                            response.Contenido = new BitacoraDtoResponse { identificador = bitacoraAccesoDTO.identificador };
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
                    Console.WriteLine(ex);
                }
            }
            /*Termina proceso para insertar datos a la base de datos*/
            return response;
        }
        #endregion
    }
}
