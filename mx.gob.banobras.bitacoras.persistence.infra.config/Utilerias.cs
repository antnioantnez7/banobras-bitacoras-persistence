using System.Text;

namespace banobras_bitacoras_persistence.mx.gob.banobras.bitacoras.persistence.infra.config
{
    public class Utilerias
    {
        #region Methods
        /// <summary>
        /// Encripta una cadena en formato Unicode
        /// </summary>
        /// <param name="CadenaAencriptar"></param>
        /// <returns></returns>
        public string EncriptarPass(string CadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = Encoding.Unicode.GetBytes(CadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        /// <summary>
        /// Descencripta una cadena en formato Unicode
        /// </summary>
        /// <param name="CadenaAdesencriptar"></param>
        /// <returns></returns>
        public string DesEncriptarPass(string CadenaAdesencriptar)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(CadenaAdesencriptar);
            result = Encoding.Unicode.GetString(decryted);
            return result;
        }
        #endregion
    }
}
