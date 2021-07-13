# NotifyMe
A simple automation system for Amazon and BestBuy checkout flows.


# Setup

### Logging

This app uses structured logging and to gain benefit from it you'll have to use a more robust solution than notepad to view logs. I recommend just running Seq and dump all the logs there. 

This is not required; but it makes it easy for devs to track what the system is doing and what happened if it failed to checkout an item.


`docker run --name seq -d --restart unless-stopped -e ACCEPT_EULA=Y -p 5341:80 datalust/seq:latest`

You should be able to monitor the activity from your mobile device through `http://localhost:5341`

#
### Web Drivers
Download the appropriate driver update the web drivers path in `appsettings.jsonc`.

> Chrome: https://chromedriver.chromium.org/downloads

> MS Edge: https://developer.microsoft.com/en-us/microsoft-edge/tools/webdriver/

#
### Notification via SMS
I assume you already have a Twilio account, if not, you'll need to get one to receive texts from NotifyMe.

Once you have an account, create a copy of `appsettings.SampleNotification.jsonc` and rename it to `appsettings.Notification.jsonc` and update the values in the `jsonc` file. 
If you do not wish to send SMS, just set the `Twilio.IsEnabled` to `false`.
The automation will try and log you in but in case BestBuy or Amazon sends you a captcha, you are on your own.