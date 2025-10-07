namespace microInventario.API.Utils
{
    public class Variables
    {
        /// <summary>
        /// Se asigna en el startup
        /// </summary>
        public static string env = "appsettings.json";

        public static class Conexion
        {
            //Local
            public static string cnx = new ConfigurationBuilder().AddJsonFile(env).Build().GetSection("AppSettings")["conexion"];
        }

        public static class Token
        {
            private static IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(env)
                .Build();

            public static string PasswordHash = Environment.GetEnvironmentVariable("TOKEN_PASSWORD_HASH")
                ?? config.GetSection("AppSettings").GetSection("Token")["PasswordHash"];

            public static string SaltKey = Environment.GetEnvironmentVariable("TOKEN_SALT_KEY")
                ?? config.GetSection("AppSettings").GetSection("Token")["SaltKey"];

            public static string VIKey = Environment.GetEnvironmentVariable("TOKEN_VI_KEY")
                ?? config.GetSection("AppSettings").GetSection("Token")["VIKey"];

            public static string Bearer = Environment.GetEnvironmentVariable("TOKEN_BEARER")
                ?? config.GetSection("AppSettings").GetSection("Token")["Bearer"];

            public static string Llave = Environment.GetEnvironmentVariable("TOKEN_KEY")
                ?? config.GetSection("AppSettings").GetSection("Token")["Llave"];

            public static int Expiration = int.TryParse(Environment.GetEnvironmentVariable("TOKEN_EXPIRATION"), out int exp)
                ? exp
                : int.Parse(config.GetSection("AppSettings").GetSection("Token")["Expiration"]);
        }

        public static class TipoPersona
        {
            public static string Natural = new ConfigurationBuilder().AddJsonFile(env).Build().GetSection("AppSettings").GetSection("TipoPersona")["Natural"];
            public static string Juridica = new ConfigurationBuilder().AddJsonFile(env).Build().GetSection("AppSettings").GetSection("TipoPersona")["Juridica"];

        }
        public static class Response
        {
            public static int OK = StatusCodes.Status200OK;
            public static int ERROR = StatusCodes.Status500InternalServerError;
            public static int NotFound = StatusCodes.Status404NotFound;
            public static int BadRequest = StatusCodes.Status400BadRequest;
            public static int Inautorizado = StatusCodes.Status401Unauthorized;

        }
        //public static class Smtp
        //{
        //    public static string BCC = new ConfigurationBuilder().AddJsonFile(env).Build().GetSection("AppSettings").GetSection("Smtp")["BCC"];
        //    public static string SMTP = new ConfigurationBuilder().AddJsonFile(env).Build().GetSection("AppSettings").GetSection("Smtp")["SMTP"];
        //    public static string PUERTO = new ConfigurationBuilder().AddJsonFile(env).Build().GetSection("AppSettings").GetSection("Smtp")["PUERTO"];
        //    public static string USUARIO = new ConfigurationBuilder().AddJsonFile(env).Build().GetSection("AppSettings").GetSection("Smtp")["USUARIO"];
        //    public static string PASSWORD = new ConfigurationBuilder().AddJsonFile(env).Build().GetSection("AppSettings").GetSection("Smtp")["PASSWORD"];
        //    public static string ENVIAR = new ConfigurationBuilder().AddJsonFile(env).Build().GetSection("AppSettings").GetSection("Smtp")["ENVIAR"];
        //    public static string TLS = new ConfigurationBuilder().AddJsonFile(env).Build().GetSection("AppSettings").GetSection("Smtp")["TLS"];
        //    // pruebas
        //    public static string PRUEBA = new ConfigurationBuilder().AddJsonFile(env).Build().GetSection("AppSettings").GetSection("Smtp")["PRUEBA"];
        //    public static string CORREO_PRUEBA = new ConfigurationBuilder().AddJsonFile(env).Build().GetSection("AppSettings").GetSection("Smtp")["CORREO_PRUEBA"];
        //}

    }
}
