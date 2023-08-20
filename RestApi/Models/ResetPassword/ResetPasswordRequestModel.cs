namespace RestApi.Models.ResetPassword;

public class ResetPasswordRequestModel
{
    
    public string ResetPasswordToken { get; set; }
   
    public string NewPassword { get; set; }
    
    public string NewPasswordConfirmation { get; set; }
}