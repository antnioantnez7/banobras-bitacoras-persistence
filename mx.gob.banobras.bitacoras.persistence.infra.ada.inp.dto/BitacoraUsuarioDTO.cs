namespace banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.infra.ada.inp.dto
{
    public class BitacoraUsuarioDto
    {
        public BitacoraUsuarioDto()
        {
            aplicativoId = string.Empty;
            capa = string.Empty;
            metodo = string.Empty;
            proceso = string.Empty;
            subproceso = string.Empty;
            detalleOperacion = string.Empty;
            transaccionId = string.Empty;
            ipEquipo = string.Empty;
            usuarioOperador = string.Empty;
            nombreOperador = string.Empty;
            expedienteOperador = string.Empty;
            areaOperador = string.Empty;
            puestoOperador = string.Empty;
            usuario = string.Empty;
            nombreUsuario = string.Empty;
            expedienteUsuario = string.Empty;
            areaUsuario = string.Empty;
            puestoUsuario = string.Empty;
            estatusOperacion = string.Empty;
            respuestaOperacion = string.Empty;
        }
        #region Properties
        /// <summary>
        /// Identificador de bitácora de Usuarios, llave primaria.
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
        /// Descripción del proceso ejecutado (Usuario, Cuenta, Permiso/Rol).
        /// </summary>
        public string proceso { get; set; }
        /// <summary>
        /// Descripción del subproceso ejecutado (Alta, Baja, Activación, etc.).
        /// </summary>
        public string subproceso { get; set; }
        /// <summary>
        /// Detalle de la operación o información referente a la misma.
        /// </summary>
        public string detalleOperacion { get; set; }
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
        public required DateTime fechaHoraOperacion { get; set; }
        /// <summary>
        /// Usuario que opera.
        /// </summary>
        public string usuarioOperador { get; set; }
        /// <summary>
        /// Nombre completo del usuario operador.
        /// </summary>
        public string nombreOperador { get; set; }
        /// <summary>
        /// Expediente del usuario operador.
        /// </summary>
        public string expedienteOperador { get; set; }
        /// <summary>
        /// Área del usuario operador.
        /// </summary>
        public string areaOperador { get; set; }
        /// <summary>
        /// Puesto del usuario operador.
        /// </summary>
        public string puestoOperador { get; set; }
        /// <summary>
        /// Usuario de la transacción.
        /// </summary>
        public string usuario { get; set; }
        /// <summary>
        /// Expediente del usuario de la transacción.
        /// </summary>
        public string nombreUsuario { get; set; }
        /// <summary>
        /// Expediente del usuario de la transacción.
        /// </summary>
        public string expedienteUsuario { get; set; }
        /// <summary>
        /// Área del usuario de la transacción.
        /// </summary>
        public string areaUsuario { get; set; }
        /// <summary>
        /// Puesto del usuario de la transacción.
        /// </summary>
        public string puestoUsuario { get; set; }
        /// <summary>
        /// Estatus (Correcto, Incorrecto).
        /// </summary>
        public string estatusOperacion { get; set; }
        /// <summary>
        /// Mensaje de la transacción realizada.
        /// </summary>
        public string respuestaOperacion { get; set; }
        #endregion
    }
}
