namespace DcodePe.Catering.Api.Const
{
    /// <summary>
    /// Constantes para la configuración de la API
    /// </summary>
    public class APICONST
    {
        /// <summary>
        /// Constantes para ApiExplorerSettings
        /// </summary>
        public static class ApiExplorer
        {
            /// <summary>
            /// Ignora el endpoint en la documentación de Swagger/OpenAPI
            /// </summary>
            public const bool IgnoreApi = true;

            /// <summary>
            /// Incluye el endpoint en la documentación de Swagger/OpenAPI
            /// </summary>
            public const bool IncludeApi = false;


            public const bool IncludeApiAll = true;
        }

        /// <summary>
        /// Constantes para versiones de API
        /// </summary>
        public static class Versions
        {
            public const string V1 = "v1";
            public const string V2 = "v2";
        }

        /// <summary>
        /// Constantes para rutas de API
        /// </summary>
        public static class Routes
        {
            public const string ApiBase = "api";
            public const string V1Base = $"{ApiBase}/{Versions.V1}";
        }

        /// <summary>
        /// Constantes para nombres de políticas de autorización
        /// </summary>
        public static class Policies
        {
            public const string AdminOnly = "AdminOnly";
            public const string UserOnly = "UserOnly";
            public const string AdminOrUser = "AdminOrUser";
        }
    }
}
