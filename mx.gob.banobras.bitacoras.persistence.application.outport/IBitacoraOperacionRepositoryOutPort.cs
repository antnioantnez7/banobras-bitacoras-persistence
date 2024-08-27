using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.dominio.model;
using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.infra.ada.inp.dto;

namespace banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.application.outport
{
    public interface IBitacoraOperacionRepositoryOutPort
    {
        #region Methods
        /// <summary>
        /// Declaración del método para agregar un registro en la bitácora de operaciones correspondiente al seguimiento de un proceso o actividad y regresa un objeto de respuesta.
        /// </summary>
        /// <param name="bitacoraOperacionDTO"></param>
        /// <returns></returns>
        Task<BitacoraResponse<BitacoraDtoResponse>> registrar(BitacoraOperacionDto bitacoraOperacionDTO);
        /// <summary>
        /// Declaración del método que obtiene los registros de bitácoras de una aplicación que satisfacen las condiciones especificadas en los parámetros.
        /// </summary>
        /// <param name="bitacoraConsultaDTO"></param>
        /// <returns></returns>
        Task<BitacoraResponse<List<BitacoraOperacionDto>>> consultar(BitacoraConsultaDto bitacoraConsultaDTO);
        #endregion
    }
}
