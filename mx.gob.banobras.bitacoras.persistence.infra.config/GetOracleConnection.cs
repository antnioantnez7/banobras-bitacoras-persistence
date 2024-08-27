using Oracle.ManagedDataAccess.Client;

namespace banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.infra.config
{
    public class GetOracleConnection
    {
        #region Properties
        readonly IConfiguration configuration;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor que recibe la inyección de dependencias del archivo de configuración en Program.cs
        /// </summary>
        /// <param name="_configuration"></param>
        public GetOracleConnection(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Crear la conexión a la base de datos, según la cadena conexión proporcionada
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public OracleConnection GetConnection(string source)
        {
            var connectionString = new Utilerias().DesEncriptarPass(configuration.GetSection("ConnectionStrings").GetSection(source).Value!);
            var conn = new OracleConnection(connectionString);
            return conn;
        }
        #endregion
    }
}
