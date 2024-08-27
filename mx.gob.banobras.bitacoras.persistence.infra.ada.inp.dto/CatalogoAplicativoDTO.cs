namespace banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.infra.ada.inp.dto
{
    public class CatalogoAplicativoDto
    {
        public CatalogoAplicativoDto()
        {
            aplicativoId = string.Empty;
            nombre = string.Empty;
            fechaRegistro = DateTime.Now;
        }
        #region Properties
        /// <summary>
        /// Identificador del catálogo, es un acrónimo como (MAC, SIGEVI)
        /// </summary>
        public required string aplicativoId { get; set; }
        /// <summary>
        /// Nombre completo del acrónimo
        /// </summary>
        public string nombre { get; set; }
        /// <summary>
        /// Identificador del catálogo de usuario (debe estar registrado)
        /// </summary>
        public required int usuarioRegistro { get; set; }
        /// <summary>
        /// Fecha de registro
        /// </summary>
        public DateTime? fechaRegistro { get; set; }
        /// <summary>
        /// Identificador del catálogo de usuario (debe estar registrado)
        /// </summary>
        public required int usuarioModifica { get; set; }
        /// <summary>
        /// Fecha de modificación
        /// </summary>
        public DateTime? fechaModifica { get; set; }
        #endregion
    }
}
