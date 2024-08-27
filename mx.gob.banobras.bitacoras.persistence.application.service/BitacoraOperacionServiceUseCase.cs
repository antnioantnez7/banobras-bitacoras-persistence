using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.application.inputport;
using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.application.outport;
using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.dominio.model;
using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.infra.ada.inp.dto;

namespace banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.application.service
{
    public class BitacoraOperacionServiceUseCase : IBitacoraOperacionInputPort
    {
        #region Properties
        /// <summary>
        /// Instancia de la interfaz del repositorio puerto de salida
        /// </summary>
        private readonly IBitacoraOperacionRepositoryOutPort iBitacoraOperacionRepositoryOutPort;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor que recibe la inyección de dependencias de la interfaz del repositorio puerto de salida
        /// </summary>
        /// <param name="iBitacoraOperacionRepositoryOutPort"></param>
        public BitacoraOperacionServiceUseCase(IBitacoraOperacionRepositoryOutPort iBitacoraOperacionRepositoryOutPort)
        {
            this.iBitacoraOperacionRepositoryOutPort = iBitacoraOperacionRepositoryOutPort;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Implementación del método que obtiene los registros de bitácoras de una aplicación que satisfacen las condiciones especificadas en los parámetros.
        /// </summary>
        /// <param name="bitacoraConsultaDTO"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<List<BitacoraOperacionDto>>> consultar(BitacoraConsultaDto bitacoraConsultaDTO)
        {
            return iBitacoraOperacionRepositoryOutPort.consultar(bitacoraConsultaDTO);
        }

        /// <summary>
        /// Implementación del método para agregar el registro en la bitácora de operaciones cuando se intenta ingresar a una aplicación y regresa un objeto de respuesta.
        /// </summary>
        /// <param name="bitacoraOperacionDTO"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> registrar(BitacoraOperacionDto bitacoraOperacionDTO)
        {
            return iBitacoraOperacionRepositoryOutPort.registrar(bitacoraOperacionDTO);
        }
        #endregion
    }
}
