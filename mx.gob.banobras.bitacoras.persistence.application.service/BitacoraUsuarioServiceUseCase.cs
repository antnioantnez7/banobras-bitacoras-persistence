using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.application.inputport;
using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.application.outport;
using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.dominio.model;
using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.infra.ada.inp.dto;

namespace banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.application.service
{
    public class BitacoraUsuarioServiceUseCase: IBitacoraUsuarioInputPort
    {
        #region Properties
        /// <summary>
        /// Instancia de la interfaz del repositorio puerto de salida
        /// </summary>
        private readonly IBitacoraUsuarioRepositoryOutPort iBitacoraUsuarioRepositoryOutPort;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor que recibe la inyección de dependencias de la interfaz del repositorio puerto de salida
        /// </summary>
        /// <param name="iBitacoraUsuarioRepositoryOutPort"></param>
        public BitacoraUsuarioServiceUseCase(IBitacoraUsuarioRepositoryOutPort iBitacoraUsuarioRepositoryOutPort)
        {
            this.iBitacoraUsuarioRepositoryOutPort = iBitacoraUsuarioRepositoryOutPort;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Implementación del método que obtiene los registros de bitácoras de una aplicación que satisfacen las condiciones especificadas en los parámetros.
        /// </summary>
        /// <param name="bitacoraConsultaDTO"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<List<BitacoraUsuarioDto>>> consultar(BitacoraConsultaDto bitacoraConsultaDTO)
        {
            return iBitacoraUsuarioRepositoryOutPort.consultar(bitacoraConsultaDTO);
        }

        /// <summary>
        /// Implementación del método para crear un registro en la bitácora de cambios a usuarios y regresa un objeto de respuesta.
        /// </summary>
        /// <param name="bitacoraUsuarioDTO"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> registrar(BitacoraUsuarioDto bitacoraUsuarioDTO)
        {
            return iBitacoraUsuarioRepositoryOutPort.registrar(bitacoraUsuarioDTO);
        }
        #endregion
    }
}
