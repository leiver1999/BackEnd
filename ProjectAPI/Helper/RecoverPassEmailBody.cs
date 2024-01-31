namespace ProjectAPI.Helper
{
    public static class RecoverPassEmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"Prueba en donde iria el email a enviar 
            <a href=""http://localhost:4200/reset?email={email}&code={emailToken}";
        }
    }
}
