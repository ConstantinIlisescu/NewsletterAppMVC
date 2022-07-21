namespace NewsletterAppMVC.ViewModels
{
    public class SignupVm // best practice to end the View Models classes with Vm
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
    }
}