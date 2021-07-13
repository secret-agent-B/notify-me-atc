namespace NotifyMe.Core
{
    using Microsoft.Extensions.Options;
    using NotifyMe.Core.Contracts;
    using NotifyMe.Core.Settings;
    using Serilog;
    using Twilio;
    using Twilio.Rest.Api.V2010.Account;
    using Twilio.Types;

    public class TwilioSMSManager : ISMSManager
    {
        private readonly ILogger _logger;
        private readonly TwilioSettings _twilioSettings;

        public TwilioSMSManager(ILogger logger, IOptions<TwilioSettings> twilioOpts)
        {
            this._logger = logger;
            this._twilioSettings = twilioOpts.Value;

            TwilioClient.Init(this._twilioSettings.AccountSID, this._twilioSettings.AuthToken);
        }

        public string SendMessage(string message, string phoneNumber)
        {
            if (!this._twilioSettings.IsEnabled)
            {
                const string warnMessage = "Twilio support is disabled.";

                this._logger.Warning(warnMessage);
                return warnMessage;
            }

            var twilioMsg = MessageResource.Create(
                body: message,
                from: new PhoneNumber(this._twilioSettings.SenderPhoneNumber),
                to: new PhoneNumber(phoneNumber)
            );

            this._logger.Information("Sent SMS notification {@message} to {@phoneNumber}.", twilioMsg, phoneNumber);

            return twilioMsg.Sid;
        }
    }
}