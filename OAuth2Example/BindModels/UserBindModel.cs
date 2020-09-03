namespace OAuth2Example.BindModels
{
    public class UserBindModel : BaseBindModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public string new_password { get; set; }
    }
}
