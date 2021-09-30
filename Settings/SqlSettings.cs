namespace InmetaTest.Settings
{
    public class SqlSettings
    {
        public string Host { get; set; }
        public string DBName { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }
        
        public string ConnnectionString
        {
            get
            {
                return $"Server={Host};Database={DBName};User Id={Username};Password={Password};";
            }
        }
    }
}