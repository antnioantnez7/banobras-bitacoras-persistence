using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.dominio.model;
using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.infra.ada.inp.dto;

namespace banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.application.outport
{
    public interface IBitacoraUsuarioRepositoryOutPort
    {
        #region Methods
        /// <summary>
        /// Declaración del método para crear un registro en la bitácora de cambios a usuarios y regresa un objeto de respuesta.
        /// </summary>
        /// <param name="bitacoraUsuarioDTO"></param>
        /// <returns></returns>
        Task<BitacoraResponse<BitacoraDtoResponse>> registrar(BitacoraUsuarioDto bitacoraUsuarioDTO);
        /// <summary>
        /// Declaración del método que obtiene los registros de bitácoras de una aplicación que satisfacen las condiciones especificadas en los parámetros.
        /// </summary>
        /// <param name="bitacoraConsultaDTO"></param>
        /// <returns></returns>
        Task<BitacoraResponse<List<BitacoraUsuarioDto>>> consultar(BitacoraConsultaDto bitacoraConsultaDTO);
        #endregion
    }
}
