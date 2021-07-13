# NotifyMe
A faster notification system for AMZ.


# Setup

### Logging

This app uses structured logging and to gain benefit from it you'll have to use a more robust solution than notepad to view logs. I recommend just running Seq and dump all the logs there.

`docker run --name seq -d --restart unless-stopped -e ACCEPT_EULA=Y -p 5341:80 datalust/seq:latest` 

#
### Web Drivers
Download the appropriate driver update the web drivers path in `appsettings.jsonc`.

#
### Notification via SMS
I assume you already have a Twilio account, if not, you'll need to get one to receive texts from NotifyMe.

Once you have an account, create a copy of `appsettings.SampleNotification.jsonc` and rename it to `appsettings.Notification.jsonc` and update the values in the `jsonc` file. 

If you do not wish to send SMS, just set the `Twilio.IsEnabled` to `false`.
The automation will try and log you in but in case BestBuy or Amazon sends you a captcha, you are on your own.