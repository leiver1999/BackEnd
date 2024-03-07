namespace ProjectAPI.Helper
{
    public static class RecoverPassEmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"<html>
                        <head>
                        </head>
                        <body style=""margin: 0;padding: 0;font-family: Arial, Helvetica, sans-serif;"">
                            <div style=""height: auto;background: Linear-gradient(to top, #c9c9ff 50%, #6e6ef6 90%) no-repeat;width:400px;padding:30px"">
                                <div>
                                    <div>
                                        <h1>Cambia tu contraseña</h1>
                                        <hr>
                                        <p>Recibiste este correo porque solicitaste un cambio de contraseña de tu cuenta en el sistema SCPF.</p>

                                        <p>Por favor presiona el botón de abajo para escoger una nueva contraseña.</p>

                                        <a href=""http://localhost:4200/reset?email={email}&code={emailToken}"" target=""_blank"" style=""background: #0d6efd;padding: 10px;border: none;
                                        color:white;border-radius:4px;display:block;margin:0 auto;width:50%;text-align:center;text-decoration:none"">Cambiar contraseña</a><br>

                                        <p>Bienvenido.</p>
                                    </div>
                                </div>
                            </div>
                        </body>
                    </html>";
        }
    }
}
