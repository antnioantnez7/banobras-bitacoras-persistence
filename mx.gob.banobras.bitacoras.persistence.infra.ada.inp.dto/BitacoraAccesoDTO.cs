namespace banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.infra.ada.inp.dto
{
    public class BitacoraAccesoDto
    {
        public BitacoraAccesoDto()
        {
            aplicativoId = string.Empty;
            capa = string.Empty;
            metodo = string.Empty;
            actividad = string.Empty;
            transaccionId = string.Empty;
            ipEquipo = string.Empty;
            usuarioAcceso = string.Empty;
            nombreUsuario = string.Empty;
            expedienteUsuario = string.Empty;
            areaUsuario = string.Empty; 
            puestoUsuario = string.Empty;
            estatusOperacion = string.Empty;
            respuestaOperacion = string.Empty;
        }
        #region Properties
        /// <summary>
        /// Identificador de bitácora de Accesos, llave primaria.
        /// </summary>
        public required int identificador { get; set; }
        /// <summary>
        /// Acrónimo e identificador de aplicativo.
        /// </summary>
        public string aplicativoId { get; set; }
        /// <summary>
        /// Capa desde donde se envía el registro a Bitácora (Front, Negocio, Persistencia).
        /// </summary>
        public string capa { get; set; }
        /// <summary>
        /// Nombre del método desde donde se envía el registro a Bitácora.
        /// </summary>
        public string metodo { get; set; }
        /// <summary>
        /// Descripción de la actividad realizada (Ingreso de usuario).
        /// </summary>
        public string actividad { get; set; }
        /// <summary>
        /// Identificador de la sesión en la que se está realizando la operación.
        /// </summary>
        public string transaccionId { get; set; }
        /// <summary>
        /// IP del equipo donde se requiere el acceso.
        /// </summary>
        public string ipEquipo { get; set; }
        /// <summary>
        /// Fecha hora de la transacción.
        /// </summary>
        public required DateTime fechaHoraAcceso { get; set; }
        /// <summary>
        /// Usuario que opera.
        /// </summary>
        public string usuarioAcceso { get; set; }
        /// <summary>
        /// Nombre completo del usuario.
        /// </summary>
        public string nombreUsuario { get; set; }
        /// <summary>
        /// Expediente del usuario.
        /// </summary>
        public string expedienteUsuario { get; set; }
        /// <summary>
        /// Área del usuario.
        /// </summary>
        public string areaUsuario { get; set; }
        /// <summary>
        /// Puesto del usuario.
        /// </summary>
        public string puestoUsuario { get; set; }
        /// <summary>
        /// Usuario del cual se está realizando la transacción.
        /// </summary>
        public string estatusOperacion { get; set; }
        /// <summary>
        /// Mensaje de la transacción realizada.
        /// </summary>
        public string respuestaOperacion { get; set; }
        #endregion
    }
}
