namespace banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.infra.ada.inp.dto
{
    public class CatalogoUsuarioDto
    {
        #region Constructor
        /// <summary>
        /// Constructor que inicializa las propiedades
        /// </summary>
        public CatalogoUsuarioDto()
        {
            usuario = string.Empty;
            paterno = string.Empty;
            materno = string.Empty;
            nombre = string.Empty;
            sesionActiva = string.Empty;
            usuarioBloqueado = string.Empty;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Identificador del catálogo de usuarios
        /// </summary>
        public required int identificador { get; set; }
        /// <summary>
        /// Identificador de usuario a nivel institucional
        /// </summary>
        public string usuario { get; set; }
        /// <summary>
        /// Apellido paterno del usuario
        /// </summary>
        public string paterno { get; set;}
        /// <summary>
        /// Apellido materno del usaurio
        /// </summary>
        public string materno { get; set; } 
        /// <summary>
        /// Nombre del usuario
        /// </summary>
        public string nombre { get; set;}
        /// <summary>
        /// Indica si el usuario tiene una sesión activa en el aplicativo
        /// </summary>
        public string sesionActiva { get; set; }
        /// <summary>
        /// Indica si el usuario tiene bloqueo en el aplicativo
        /// </summary>
        public string usuarioBloqueado { get; set; }
        /// <summary>
        /// Contabiliza los intentos fallidos
        /// </summary>
        public int intentosFallidos { get; set; }
        /// <summary>
        /// Usuario que realiza el registro
        /// </summary>
        public int usuarioRegistro  { get; set; }
        /// <summary>
        /// Fecha hora en que se realiza el registro
        /// </summary>
        public DateTime fechaRegistro { get; set; }
        /// <summary>
        /// Usuario que realiza una modificación
        /// </summary>
        public int usuarioModifica { get; set; }
        /// <summary>
        /// Fecha hora en que se realiza una modificación
        /// </summary>
        public DateTime fechaModifica { get; set; }
        #endregion
    }
}
