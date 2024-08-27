using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.dominio.model;
using banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.infra.ada.inp.dto;

namespace banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.application.outport
{
    public interface ICatalogoRepositoryOutPort
    {
        #region Methods - Catálogo Aplicativos
        /// <summary>
        /// Declaración del método para agregar un registro en el catálogo de Aplicativos.
        /// </summary>
        /// <param name="catalogoAplicativoDTO"></param>
        /// <returns></returns>
        Task<BitacoraResponse<BitacoraDtoResponse>> registrarAplicativo(CatalogoAplicativoDto catalogoAplicativoDTO);
        /// <summary>
        /// Declaración del método que obtiene los registros del catálogo de Aplicativos que satisfacen las condiciones especificadas en los parámetros.
        /// </summary>
        /// <returns></returns>
        Task<BitacoraResponse<List<CatalogoAplicativoDto>>> consultarAplicativos();
        /// <summary>
        /// Declaración del método que elimina el registro del catálogo de Aplicativos.
        /// </summary>
        /// <param name="aplicativoId"></param>
        /// <returns></returns>
        Task<BitacoraResponse<BitacoraDtoResponse>> eliminarAplicativo(string aplicativoId);
        /// <summary>
        /// Declaración del método que actualiza el registro del catálogo de Aplicativos.
        /// </summary>
        /// <param name="catalogoAplicativoDTO"></param>
        /// <returns></returns>
        Task<BitacoraResponse<BitacoraDtoResponse>> actualizarAplicativo(CatalogoAplicativoDto catalogoAplicativoDTO);
        #endregion

        #region Methods - Catálogo Usuarios
        /// <summary>
        /// Declaración del método para registrar o actualizar un registro en el catálogo de Usuarios.
        /// </summary>
        /// <param name="catalogoUsuarioDto"></param>
        /// <returns></returns>
        Task<BitacoraResponse<BitacoraDtoResponse>> guardarUsuario(CatalogoUsuarioDto catalogoUsuarioDto);
        /// <summary>
        /// Declaración del método que obtiene los registros del catálogo de Usuarios que satisfacen las condiciones especificadas en los parámetros.
        /// </summary>
        /// <returns></returns>
        Task<BitacoraResponse<List<CatalogoUsuarioDto>>> consultarUsuarios(CatalogoUsuarioDto catalogoUsuarioDto);
        /// <summary>
        /// Declaración del método que elimina el registro del catálogo de Usuarios.
        /// </summary>
        /// <param name="identificador"></param>
        /// <returns></returns>
        Task<BitacoraResponse<BitacoraDtoResponse>> eliminarUsuario(int identificador);
        #endregion
    }
}
