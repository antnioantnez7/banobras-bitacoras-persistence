namespace banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.dominio.model
{
    public class ResponseBase
    {
        #region Properties
        /// <summary>
        /// Description of response
        /// </summary>
        public int statusCode { get; set; }
        /// <summary>
        /// Description of error
        /// </summary>
        public string? message { get; set; }
        /// <summary>
        /// Is success
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// HTTP Error Code
        /// </summary>
        public ResponseType responseType { get; set; }
        #endregion
    }
    public enum ResponseType
    {
        BadRequest = 400,
        Okay = 200,
        NotAuthorized = 401,
        InvalidToken = 403,
        InternalError = 500,
        TooManyRequest = 429
    }
}
