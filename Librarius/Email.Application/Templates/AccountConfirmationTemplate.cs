namespace Email.Application.Templates;

public static class AccountConfirmationTemplate
{
    public static string GetConfirmationEmailBody(string userName)
    {
        var template = @"<!DOCTYPE html>
                            <html>
                            <head>
                                <meta charset='UTF-8'>
                                <title>Account Confirmation</title>
                                <style>
                                    body {
                                        font-family: Arial, sans-serif;
                                        background-color: #f2f2f2;
                                        padding: 20px;
                                    }

                                    .container {
                                        max-width: 600px;
                                        margin: 0 auto;
                                        background-color: #ffffff;
                                        padding: 30px;
                                        border-radius: 5px;
                                        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                                    }

                                    h1 {
                                        color: #333333;
                                        font-size: 24px;
                                        margin-bottom: 20px;
                                    }

                                    p {
                                        color: #555555;
                                        font-size: 16px;
                                        line-height: 1.5;
                                        margin-bottom: 10px;
                                    }

                                    .button {
                                        display: inline-block;
                                        padding: 10px 20px;
                                        border: 1px solid #FFA500;
                                        color: #FFA500;
                                        text-decoration: none;
                                        border-radius: 5px;
                                        font-size: 16px;
                                    }

                                    .footer {
                                        margin-top: 30px;
                                        border-top: 1px solid #dddddd;
                                        padding-top: 20px;
                                        font-size: 14px;
                                        color: #777777;
                                    }

                                </style>
                            </head>
                            <body>
                                <div class='container'>
                                    <h1>Welcome to Our Website, " + userName + @"!</h1>
                                    <p>Dear " + userName + @",</p>
                                    <p>Thank you for registering an account on our website. We're excited to have you join our community!</p>
                                    <p>To get started, please click the button below to confirm your account:</p>
                                    <p>
                                        <a class='button' href='http://localhost:4200/#/landing' target='_blank'>Confirm Account</a>
                                    </p>
                                    <p>If the button above doesn't work, you can also copy and paste the following URL into your browser:</p>
                                    <p>http://localhost:4200/#/landing</p>
                                    <p>We're here to help if you have any questions or need assistance. Feel free to reach out to our support team.</p>
                                    <p>Once again, welcome aboard!</p>
                                    <p>Sincerely,</p>
                                    <p>The Librarius Team</p>

                                    <div class='footer'>
                                        <p>If you wish to unsubscribe from our mailing list, please <a href='#'>click here</a>.</p>
                                    </div>
                                </div>
                            </body>
                            </html>";

        return template;
    }
}