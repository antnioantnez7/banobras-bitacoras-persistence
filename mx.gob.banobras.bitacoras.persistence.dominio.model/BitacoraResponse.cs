namespace banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.dominio.model
{
    public class BitacoraResponse<T>
    {
        #region Properties
        /// <summary>
        /// Código de respuesta de exito o falla
        /// </summary>
        public int Codigo { get; set; }

        /// <summary>
        /// Modelo generico de respuesta
        /// </summary>
        public T Contenido { get; set; }

        /// <summary>
        /// Mensaje de la respuesta
        /// </summary>
        public string Mensaje { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructo de la respunesta de bitacora
        /// </summary>
        public BitacoraResponse()
        {
            Codigo = 200;
            Contenido = default(T)!;
            Mensaje = string.Empty;
        }
        #endregion
    }
}
