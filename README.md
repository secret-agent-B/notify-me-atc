# NotifyMe
A simple automation system for Amazon and BestBuy checkout flows for NVIDIA GPUs.


# Setup

### Logging

**Structured Logging and Seq: A Better Solution**

When working with structured logging, relying on Notepad to view logs isn't ideal. Instead, I recommend using **Seq**, a more robust solution. Seq allows you to centralize and manage logs effectively. Here's how to set it up:

1. **Run Seq Container:**
   - Execute the following Docker command to run the Seq container:
     ```
     docker run --name seq -d --restart unless-stopped -e ACCEPT_EULA=Y -p 5341:80 datalust/seq:latest
     ```
   - This command creates a container named "seq" using the latest version of Seq.

2. **Access Seq:**
   - Once the container is running, you can access Seq from your host machine:
     - Open a web browser and go to [http://localhost:5341](http://localhost:5341).
     - Alternatively, if you're using a mobile device, use the host IP address and port 5341.

3. **Benefits:**
   - Seq provides a user-friendly interface for monitoring logs.
   - Developers can easily track system activities and diagnose issues.

Remember, using Seq isn't mandatory, but it significantly improves log management.

#
### Web Drivers
Download the appropriate driver then update the web drivers path in `appsettings.jsonc`.

> Chrome: https://chromedriver.chromium.org/downloads

> MS Edge: https://developer.microsoft.com/en-us/microsoft-edge/tools/webdriver/

#
### Notification via SMS
I assume you already have a Twilio account, if not, you'll need to get one to receive texts from NotifyMe.

Once you have an account, create a copy of `appsettings.SampleNotification.jsonc` and rename it to `appsettings.Notification.jsonc` and update the values in the `jsonc` file. 
If you do not wish to send SMS, just set the `Twilio.IsEnabled` to `false`.
The automation will try and log you in but in case BestBuy or Amazon sends you a captcha, you are on your own.

I am not liable for anything if you F up your BB or AMZ account.
