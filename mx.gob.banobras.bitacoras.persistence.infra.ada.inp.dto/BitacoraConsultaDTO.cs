namespace banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.infra.ada.inp.dto
{
    public class BitacoraConsultaDto
    {
        public BitacoraConsultaDto()
        {
            aplicativoId = string.Empty;
        }
        #region Properties
        /// <summary>
        /// Acrónimo e identificador de aplicativo.
        /// </summary>
        public string aplicativoId { get; set; }
        /// <summary>
        /// Fecha hora inicio de la transacción.
        /// </summary>
        public required DateTime fechaHoraIni { get; set; }
        /// <summary>
        /// Fecha hora fin de la transacción.
        /// </summary>
        public required DateTime fechaHoraFin { get; set; }
        /// <summary>
        /// Valor booleano para bitacora historica o no.
        /// </summary>
        public required bool historico { get; set; }
        #endregion
    }
}
