namespace TodoList.Data
{
    public class Config
    {
        public string appName { get; set; }
        public Config(string newAppName)
        {
            appName = newAppName;
        }
    }
}
