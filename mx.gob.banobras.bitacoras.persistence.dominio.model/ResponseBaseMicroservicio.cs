namespace banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.dominio.model
{
    public class ResponseBaseMicroservicio<T> : ResponseBase where T : class, new()
    {
        #region Properties
        public T Contenido { get; set; }
        #endregion

        #region Constructor
        public ResponseBaseMicroservicio()
        {
            Contenido = new T();
        }
        #endregion
    }
}
