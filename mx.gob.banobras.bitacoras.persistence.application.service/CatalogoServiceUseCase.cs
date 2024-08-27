using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.application.inputport;
using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.application.outport;
using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.dominio.model;
using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.infra.ada.inp.dto;

namespace banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.application.service
{
    public class CatalogoServiceUseCase : ICatalogoInputPort
    {
        #region Properties
        /// <summary>
        /// Instancia de la interfaz del repositorio puerto de salida
        /// </summary>
        private readonly ICatalogoRepositoryOutPort iCatalogoRepositoryOutPort;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor que recibe la inyección de dependencias de la interfaz del repositorio puerto de salida
        /// </summary>
        /// <param name="iCatalogoRepositoryOutPort"></param>
        public CatalogoServiceUseCase(ICatalogoRepositoryOutPort iCatalogoRepositoryOutPort)
        {
            this.iCatalogoRepositoryOutPort = iCatalogoRepositoryOutPort;
        }
        #endregion

        #region Methods - Catálogo Aplicativos
        /// <summary>
        /// Implementación del método que obtiene los registros del catálogo de Aplicativos que satisfacen las condiciones especificadas en los parámetros.
        /// </summary>
        /// <returns></returns>
        public Task<BitacoraResponse<List<CatalogoAplicativoDto>>> consultarAplicativos()
        {
            return iCatalogoRepositoryOutPort.consultarAplicativos();
        }

        /// <summary>
        /// Implementación del método que elimina el registro del catálogo de Aplicativos.
        /// </summary>
        /// <param name="aplicativoId"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> eliminarAplicativo(string aplicativoId)
        {
            return iCatalogoRepositoryOutPort.eliminarAplicativo(aplicativoId);
        }

        /// <summary>
        /// Implementación del método para agregar un registro en el catálogo de Aplicativos.
        /// </summary>
        /// <param name="catalogoAplicativoDTO"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> registrarAplicativo(CatalogoAplicativoDto catalogoAplicativoDTO)
        {
            return iCatalogoRepositoryOutPort.registrarAplicativo(catalogoAplicativoDTO);
        }

        /// <summary>
        /// Implementación del método para actualizar un registro en el catálogo de Aplicativos.
        /// </summary>
        /// <param name="catalogoAplicativoDTO"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> actualizarAplicativo(CatalogoAplicativoDto catalogoAplicativoDTO)
        {
            return iCatalogoRepositoryOutPort.actualizarAplicativo(catalogoAplicativoDTO);
        }
        #endregion

        #region Methods - Catálogo Usuarios
        /// <summary>
        /// Implementación del método para guardar (registrar o actualizar) un usuario en el catálogo de Usuarios.
        /// </summary>
        /// <param name="catalogoUsuarioDto"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> guardarUsuario(CatalogoUsuarioDto catalogoUsuarioDto)
        {
            return iCatalogoRepositoryOutPort.guardarUsuario(catalogoUsuarioDto);
        }

        /// <summary>
        /// Implementación del método que obtiene los registros en el catálogo de Usuarios que satisfacen las condiciones especificadas en los parámetros.
        /// </summary>
        /// <param name="catalogoUsuarioDto"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<List<CatalogoUsuarioDto>>> consultarUsuarios(CatalogoUsuarioDto catalogoUsuarioDto)
        {
            return iCatalogoRepositoryOutPort.consultarUsuarios(catalogoUsuarioDto);
        }

        /// <summary>
        /// Implementación del método que elimina el registro del catálogo de Usuarios.
        /// </summary>
        /// <param name="identificador"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> eliminarUsuario(int identificador)
        {
            return iCatalogoRepositoryOutPort.eliminarUsuario(identificador);
        }
        #endregion
    }
}
