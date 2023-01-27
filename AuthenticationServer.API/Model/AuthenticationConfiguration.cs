namespace AuthenticationServer.API.Model
{
    public class AuthenticationConfiguration
    {
        public string AccessTokenSecret = "6uIjuTQAxRJPSmCUVb9iChqAGFX_Lf3yjU-OK6Nz3AQ6Uc9YCZhc8wyT_ap9NE5DrwDr8ewE-WcXYcVOGSm6fNYqziNkO6evtfj22UcvBU-0OEB1Gbjk0U686Z2H630geuB_JdWsEsp_FJdA68rDR63gf6iogHHUOe-pvhLNcSs";
        public double AccessTokenExpirationMinutes { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string RefreshTokenSecret = "5uIjuTQAxRJPSmCUVb9iChqAGFX_Lf3yjU-OK6Nz3AQ6Uc9YCZhc8wyT_ap9NE5DrwDr8ewE-WcXYcVOGSm6fNYqziNkO6evtfj22UcvBU-0OEB1Gbjk0U686Z2H630geuB_JdWsEsp_FJdA68rDR63gf6iogHHUOe-pvhLNcSs";
        public double RefreshTokenExpirationMinutes { get; set; }
    }
}
