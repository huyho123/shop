﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotDetect.Web.Mvc;

namespace Shopproject.Common
{
    public class CaptchaHelper
    {
        public static MvcCaptcha GetRegistrationCaptcha()
        {
            // create the control instance
            MvcCaptcha registrationCaptcha = new MvcCaptcha("RegistrationCaptcha");

            // set up client-side processing of the Captcha code input textbox
            registrationCaptcha.UserInputID = "CaptchaCode";

            // Captcha settings
            registrationCaptcha.ImageSize = new System.Drawing.Size(255, 50);

            return registrationCaptcha;
        }
    }
}
