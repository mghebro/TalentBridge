using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;

namespace TalentBridge.Common.SMTP;

public class SMTPService
{
    private const string SenderEmail = "khulelidzeliza@gmail.com";
    private const string AppPassword = "rbbk idqw ense ijee";

    public static async Task SendPasswordResetLinkAsync(string toEmail, string userName, string resetLink)
    {
        var subject = "Reset Your Password - ORA";
        var htmlBody = GeneratePasswordResetEmailTemplate(userName, resetLink);

        using var mail = new MailMessage();
        mail.From = new MailAddress(SenderEmail, "ORA");
        mail.To.Add(toEmail);
        mail.Subject = subject;
        mail.Body = htmlBody;
        mail.IsBodyHtml = true;

        using var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            EnableSsl = true,
            Credentials = new NetworkCredential(SenderEmail, AppPassword),
        };

        await smtpClient.SendMailAsync(mail);
    }

    private static string GeneratePasswordResetEmailTemplate(string userName, string resetLink)
    {
        return $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Reset Your Password</title>
</head>
<body style='margin:0; padding:0; background-color:#f0f0f0;'>
    <table role='presentation' width='100%' cellpadding='0' cellspacing='0' border='0' style='padding: 40px 20px; font-family: -apple-system, BlinkMacSystemFont, Segoe UI, Roboto, Helvetica, sans-serif;'>
        <tr>
            <td align='center'>
                <table role='presentation' cellpadding='0' cellspacing='0' border='0' style='max-width: 600px; background-color: #e8e8e8; border-radius: 30px; padding: 40px 20px;'>
                    <tr>
                        <td style='padding-bottom: 20px;'>
                            <h1 style='font-size: 26px; font-weight: 600; color: rgb(98, 98, 245); margin: 0 0 20px;'>Reset Your Password</h1>
                            <p style='font-size: 15px; color: #666; line-height: 1.5; margin: 0 0 30px; max-width: 500px;'>
                                Hi {userName},<br /><br />
                                We received a request to reset your password for your ORA account.<br />
                                Click the button below to create a new password. This link will expire in 1 hour.
                            </p>
                        </td>
                    </tr>

                    <!-- Reset Button -->
                    <tr>
                        <td align='center' style='padding-bottom: 30px;'>
                            <table role='presentation' cellpadding='0' cellspacing='0' border='0'>
                                <tr>
                                    <td style='background: linear-gradient(135deg, rgb(98, 98, 245) 0%, rgb(60, 51, 184) 100%); border-radius: 25px; padding: 0;'>
                                        <a href='{resetLink}' style='display: inline-block; padding: 18px 40px; color: white; text-decoration: none; font-weight: 600; font-size: 16px; border-radius: 25px;'>
                                            Reset My Password
                                        </a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <!-- Security Notice -->
                    <tr>
                        <td style='background-color: #fed7d7; border: 1px solid #feb2b2; border-radius: 8px; padding: 20px; margin: 20px 0;'>
                            <p style='font-size: 14px; color: #c53030; font-weight: 600; margin: 0 0 8px;'>🛡️ Security Notice</p>
                            <p style='font-size: 13px; color: #742a2a; line-height: 1.5; margin: 0;'>
                                For your security, this link can only be used once and will expire in 1 hour. Never share this link with anyone.
                            </p>
                        </td>
                    </tr>

                    <!-- Alternative Link -->
                    <tr>
                        <td style='padding-top: 20px; border-top: 1px solid #ddd; margin-top: 20px;'>
                            <p style='font-size: 14px; color: #666; margin: 0 0 10px;'><strong>Having trouble with the button?</strong> Copy and paste this link into your browser:</p>
                            <div style='background-color: #f7fafc; padding: 15px; border-radius: 8px; border: 1px solid #e2e8f0; font-family: Courier New, monospace; font-size: 13px; word-break: break-all; color: #4a5568;'>
                                {resetLink}
                            </div>
                        </td>
                    </tr>

                    <!-- Footer Message -->
                    <tr>
                        <td style='padding-top: 30px;'>
                            <p style='font-size: 14px; color: rgb(98, 98, 245); margin: 0 0 20px;'>If you didn't request this password reset, feel free to ignore this email.</p>
                            <p style='font-size: 14px; color: rgb(60, 51, 184); font-weight: 500; margin: 0;'>With purpose and protection,<br/>The ORA Team (Magnora & Zenora)</p>
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
    }

    public static string GenerateVerificationCode()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[4];
        rng.GetBytes(bytes);
        var randomNumber = BitConverter.ToUInt32(bytes, 0);
        return (randomNumber % 900000 + 100000).ToString();
    }

    public static async Task<string> SendVerificationCodeAsync(string toEmail, string userName)
    {
        string verificationCode = GenerateVerificationCode();

        string htmlContent = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
  <meta charset='UTF-8' />
  <meta name='viewport' content='width=device-width, initial-scale=1.0' />
  <title>Your Verification Code</title>
</head>
<body style='margin:0; padding:0; background-color:#f0f0f0;'>
  <table role='presentation' width='100%' cellpadding='0' cellspacing='0' border='0' style='padding: 40px 20px; font-family: -apple-system, BlinkMacSystemFont, Segoe UI, Roboto, Helvetica, sans-serif;'>
    <tr>
      <td align='center'>
        <table role='presentation' cellpadding='0' cellspacing='0' border='0' style='max-width: 600px; background-color: #e8e8e8; border-radius: 30px; padding: 40px 20px;'>
          <tr>
            <td style='padding-bottom: 20px;'>
              <h1 style='font-size: 26px; font-weight: 600; color: rgb(98, 98, 245); margin: 0 0 20px;'>Here's Your Verification Code</h1>
              <p style='font-size: 15px; color: #666; line-height: 1.5; margin: 0 0 30px; max-width: 500px;'>To continue your journey with Ora, please enter the code below<br />
              to verify your identity. This helps us keep your experience<br />
              secure and personalized.</p>
            </td>
          </tr>

          <!-- Code Digits -->
          <tr>
            <td align='center' style='padding-bottom: 30px;'>
              <table role='presentation' cellpadding='0' cellspacing='8' border='0'>
                <tr>
                  <td style='background-color: rgb(219, 220, 224); width: 40px; height: 40px; border-radius: 8px; text-align: center; vertical-align: middle; font-size: 18px; font-weight: bold; color: rgb(60, 51, 184); line-height: 40px;'>{verificationCode[0]}</td>
                  <td style='background-color: rgb(219, 220, 224); width: 40px; height: 40px; border-radius: 8px; text-align: center; vertical-align: middle; font-size: 18px; font-weight: bold; color: rgb(60, 51, 184); line-height: 40px;'>{verificationCode[1]}</td>
                  <td style='background-color: rgb(219, 220, 224); width: 40px; height: 40px; border-radius: 8px; text-align: center; vertical-align: middle; font-size: 18px; font-weight: bold; color: rgb(60, 51, 184); line-height: 40px;'>{verificationCode[2]}</td>
                  <td style='background-color: rgb(219, 220, 224); width: 40px; height: 40px; border-radius: 8px; text-align: center; vertical-align: middle; font-size: 18px; font-weight: bold; color: rgb(60, 51, 184); line-height: 40px;'>{verificationCode[3]}</td>
                  <td style='background-color: rgb(219, 220, 224); width: 40px; height: 40px; border-radius: 8px; text-align: center; vertical-align: middle; font-size: 18px; font-weight: bold; color: rgb(60, 51, 184); line-height: 40px;'>{verificationCode[4]}</td>
                  <td style='background-color: rgb(219, 220, 224); width: 40px; height: 40px; border-radius: 8px; text-align: center; vertical-align: middle; font-size: 18px; font-weight: bold; color: rgb(60, 51, 184); line-height: 40px;'>{verificationCode[5]}</td>
                </tr>
              </table>
            </td>
          </tr>

          <!-- Footer Message -->
          <tr>
            <td style='padding-top: 10px;'>
              <p style='font-size: 14px; color: rgb(98, 98, 245); margin: 0 0 30px;'>If you didn't request this code, feel free to ignore this email.</p>
              <p style='font-size: 14px; color: rgb(60, 51, 184); font-weight: 500; margin: 0;'>With purpose and protection,<br/>The Ora Team (Magnora & Zenora)</p>
            </td>
          </tr>

        </table>
      </td>
    </tr>
  </table>
</body>
</html>
";


        using var mail = new MailMessage();
        mail.From = new MailAddress(SenderEmail, "ORA");
        mail.To.Add(toEmail);
        mail.Subject = "Your Verification Code - ORA";
        mail.Body = htmlContent;
        mail.IsBodyHtml = true;

        using var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            EnableSsl = true,
            Credentials = new NetworkCredential(SenderEmail, AppPassword),
        };

        await smtpClient.SendMailAsync(mail);
        return verificationCode;
    }

    public static string SendVerificationCode(string toEmail, string userName)
    {
        string verificationCode = GenerateVerificationCode();

        string htmlContent = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
  <meta charset='UTF-8' />
  <meta name='viewport' content='width=device-width, initial-scale=1.0' />
  <title>Your Verification Code</title>
</head>
<body style='margin:0; padding:0; background-color:#f0f0f0;'>
  <table role='presentation' width='100%' cellpadding='0' cellspacing='0' border='0' style='padding: 40px 20px; font-family: -apple-system, BlinkMacSystemFont, Segoe UI, Roboto, Helvetica, sans-serif;'>
    <tr>
      <td align='center'>
        <table role='presentation' cellpadding='0' cellspacing='0' border='0' style='max-width: 600px; background-color: #e8e8e8; border-radius: 30px; padding: 40px 20px;'>
          <tr>
            <td style='padding-bottom: 20px;'>
              <h1 style='font-size: 26px; font-weight: 600; color: rgb(98, 98, 245); margin: 0 0 20px;'>Here's Your Verification Code</h1>
              <p style='font-size: 15px; color: #666; line-height: 1.5; margin: 0 0 30px; max-width: 500px;'>To continue your journey with Ora, please enter the code below<br />
              to verify your identity. This helps us keep your experience<br />
              secure and personalized.</p>
            </td>
          </tr>

          <!-- Code Digits -->
          <tr>
            <td align='center' style='padding-bottom: 30px;'>
              <table role='presentation' cellpadding='0' cellspacing='8' border='0'>
                <tr>
                  <td style='background-color: rgb(219, 220, 224); width: 40px; height: 40px; border-radius: 8px; text-align: center; vertical-align: middle; font-size: 18px; font-weight: bold; color: rgb(60, 51, 184); line-height: 40px;'>{verificationCode[0]}</td>
                  <td style='background-color: rgb(219, 220, 224); width: 40px; height: 40px; border-radius: 8px; text-align: center; vertical-align: middle; font-size: 18px; font-weight: bold; color: rgb(60, 51, 184); line-height: 40px;'>{verificationCode[1]}</td>
                  <td style='background-color: rgb(219, 220, 224); width: 40px; height: 40px; border-radius: 8px; text-align: center; vertical-align: middle; font-size: 18px; font-weight: bold; color: rgb(60, 51, 184); line-height: 40px;'>{verificationCode[2]}</td>
                  <td style='background-color: rgb(219, 220, 224); width: 40px; height: 40px; border-radius: 8px; text-align: center; vertical-align: middle; font-size: 18px; font-weight: bold; color: rgb(60, 51, 184); line-height: 40 40px;'>{verificationCode[3]}</td>
                  <td style='background-color: rgb(219, 220, 224); width: 40px; height: 40px; border-radius: 8px; text-align: center; vertical-align: middle; font-size: 18px; font-weight: bold; color: rgb(60, 51, 184); line-height: 40px;'>{verificationCode[4]}</td>
                  <td style='background-color: rgb(219, 220, 224); width: 40px; height: 40px; border-radius: 8px; text-align: center; vertical-align: middle; font-size: 18px; font-weight: bold; color: rgb(60, 51, 184); line-height: 40px;'>{verificationCode[5]}</td>
                </tr>
              </table>
            </td>
          </tr>

          <!-- Footer Message -->
          <tr>
            <td style='padding-top: 10px;'>
              <p style='font-size: 14px; color: rgb(98, 98, 245); margin: 0 0 30px;'>If you didn't request this code, feel free to ignore this email.</p>
              <p style='font-size: 14px; color: rgb(60, 51, 184); font-weight: 500; margin: 0;'>With purpose and protection,<br/>The Ora Team (Magnora & Zenora)</p>
            </td>
          </tr>

        </table>
      </td>
    </tr>
  </table>
</body>
</html>

";

        using var mail = new MailMessage();
        mail.From = new MailAddress(SenderEmail, "ORA");
        mail.To.Add(toEmail);
        mail.Subject = "Your Verification Code - ORA";
        mail.Body = htmlContent;
        mail.IsBodyHtml = true;

        using var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            EnableSsl = true,
            Credentials = new NetworkCredential(SenderEmail, AppPassword),
        };

        smtpClient.Send(mail);
        return verificationCode;
    }
}
