﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sneat.MVC.Templates
{
    public class SendMailTemplate
    {
        public static string ForgotPasswordTemplate(string userName, string newPass, string loginUrl)
        {
            string htmlContent = $@"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns:v=""urn:schemas-microsoft-com:vml"">

<head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
    <meta name=""viewport"" content=""width=device-width; initial-scale=1.0; maximum-scale=1.0;"" />
    <!--[if !mso]--><!-- -->
    <link href='https://fonts.googleapis.com/css?family=Work+Sans:300,400,500,600,700' rel=""stylesheet"">
    <link href='https://fonts.googleapis.com/css?family=Quicksand:300,400,700' rel=""stylesheet"">
    <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css"" rel=""stylesheet"" integrity=""sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC"" crossorigin=""anonymous"">
    <!-- <![endif]-->

    <title>WesternTech VN</title>

    <style type=""text/css"">
        body {{
            width: 100%;
            background-color: #ffffff;
            margin: 0;
            padding: 0;
            -webkit-font-smoothing: antialiased;
            mso-margin-top-alt: 0px;
            mso-margin-bottom-alt: 0px;
            mso-padding-alt: 0px 0px 0px 0px;
        }}
        
        p,
        h1,
        h2,
        h3,
        h4 {{
            margin-top: 0;
            margin-bottom: 0;
            padding-top: 0;
            padding-bottom: 0;
        }}
        
        span.preheader {{
            display: none;
            font-size: 1px;
        }}
        
        html {{
            width: 100%;
        }}
        
        table {{
            font-size: 14px;
            border: 0;
        }}
        /* ----------- responsivity ----------- */
        
        @media only screen and (max-width: 640px) {{
            /*------ top header ------ */
            .main-header {{
                font-size: 20px !important;
            }}
            .main-section-header {{
                font-size: 28px !important;
            }}
            .show {{
                display: block !important;
            }}
            .hide {{
                display: none !important;
            }}
            .align-center {{
                text-align: center !important;
            }}
            .no-bg {{
                background: none !important;
            }}
            /*----- main image -------*/
            .main-image img {{
                width: 440px !important;
                height: auto !important;
            }}
            /* ====== divider ====== */
            .divider img {{
                width: 440px !important;
            }}
            /*-------- container --------*/
            .container590 {{
                width: 440px !important;
            }}
            .container580 {{
                width: 400px !important;
            }}
            .main-button {{
                width: 220px !important;
            }}
            /*-------- secions ----------*/
            .section-img img {{
                width: 320px !important;
                height: auto !important;
            }}
            .team-img img {{
                width: 100% !important;
                height: auto !important;
            }}
        }}
        
        @media only screen and (max-width: 479px) {{
            /*------ top header ------ */
            .main-header {{
                font-size: 18px !important;
            }}
            .main-section-header {{
                font-size: 26px !important;
            }}
            /* ====== divider ====== */
            .divider img {{
                width: 280px !important;
            }}
            /*-------- container --------*/
            .container590 {{
                width: 280px !important;
            }}
            .container590 {{
                width: 280px !important;
            }}
            .container580 {{
                width: 260px !important;
            }}
            /*-------- secions ----------*/
            .section-img img {{
                width: 280px !important;
                height: auto !important;
            }}
        }}
    </style>
    <!-- [if gte mso 9]><style type=”text/css”>
        body {{
        font-family: arial, sans-serif!important;
        }}
        </style>
    <![endif]-->
</head>


<body class=""respond"" leftmargin=""0"" topmargin=""0"" marginwidth=""0"" marginheight=""0"">
    <!-- pre-header -->
    <table style=""display:none!important;"">
        <tr>
            <td>
                <div style=""overflow:hidden;display:none;font-size:1px;color:#ffffff;line-height:1px;font-family:Arial;maxheight:0px;max-width:0px;opacity:0;"">
                    Pre-header for the newsletter template
                </div>
            </td>
        </tr>
    </table>
    <!-- pre-header end -->
    <!-- header -->
    <table border=""0"" width=""100%"" cellpadding=""0"" cellspacing=""0"" bgcolor=""ffffff"">

        <tr>
            <td align=""center"">
                <table border=""0"" align=""center"" width=""590"" cellpadding=""0"" cellspacing=""0"" class=""container590"">

                    <tr>
                        <td height=""25"" style=""font-size: 25px; line-height: 25px;"">&nbsp;</td>
                    </tr>

                    <tr>
                        <td align=""center"">

                            <table border=""0"" align=""center"" width=""590"" cellpadding=""0"" cellspacing=""0"" class=""container590"">

                                <tr>
                                    <td align=""center"" height=""70"" style=""height:70px;"">
                                        <a href="""" style=""display: block; border-style: none !important; border: 0 !important;"">
                                            <img width=""100"" border=""0"" style=""display: block; width: 100px;"" src=""https://res.cloudinary.com/dduv8pom4/image/upload/v1721178529/favicon_epkkts.png"" alt="""" />
                                        </a>
                                        <div style=""line-height: 35px"">
                                            <span style=""color: #5caad2; font-size: 35px;"">Sneat System</span>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td height=""25"" style=""font-size: 25px; line-height: 25px;"">&nbsp;</td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>
    <!-- end header -->

    <!-- big image section -->
    <table border=""0"" width=""100%"" cellpadding=""0"" cellspacing=""0"" bgcolor=""ffffff"" class=""bg_color"">

        <tr>
            <td align=""center"">
                <table border=""0"" align=""center"" width=""590"" cellpadding=""0"" cellspacing=""0"" class=""container590"">
                    <tr>

                        <td align=""center"" class=""section-img"">
                            <a href="""" style="" border-style: none !important; display: block; border: 0 !important;"">
                                <img src=""https://res.cloudinary.com/dduv8pom4/image/upload/v1719937506/hero-services-img_c6vgq2.webp"" style=""display: block; width: 590px;"" width=""590"" border=""0"" alt="""" />
                            </a>
                        </td>
                    </tr>
                    <tr>
                        <td height=""20"" style=""font-size: 20px; line-height: 20px;"">&nbsp;</td>
                    </tr>
                    <tr>
                        <td align=""center"" style=""color: #343434; font-size: 24px; font-family: Quicksand, Calibri, sans-serif; font-weight:700;letter-spacing: 3px; line-height: 35px;"" class=""main-header"">


                            <div style=""line-height: 35px"">

                                Xin chào <span style=""color: #5caad2;"">{userName}</span>! Mật khẩu mới của bạn là <span style=""color: #5caad2;"">{newPass}</span>
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td height=""10"" style=""font-size: 10px; line-height: 10px;"">&nbsp;</td>
                    </tr>

                    <tr>
                        <td align=""center"">
                            <table border=""0"" width=""40"" align=""center"" cellpadding=""0"" cellspacing=""0"" bgcolor=""eeeeee"">
                                <tr>
                                    <td height=""2"" style=""font-size: 2px; line-height: 2px;"">&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td height=""20"" style=""font-size: 20px; line-height: 20px;"">&nbsp;</td>
                    </tr>

                    <tr>
                        <td align=""center"">
                            <table border=""0"" width=""400"" align=""center"" cellpadding=""0"" cellspacing=""0"" class=""container590"">
                                <tr>
                                    <td align=""center"" style=""color: #888888; font-size: 16px; font-family: 'Work Sans', Calibri, sans-serif; line-height: 24px;"">


                                        <div style=""line-height: 24px"">

                                            Vui lòng xác nhận quay trở lại đăng nhập bằng mật khẩu mới!
                                        </div>
                                        <a type=""button"" href=""{loginUrl}"" class=""btn btn-outline-primary"">Chuyển hướng đăng nhập</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td height=""25"" style=""font-size: 25px; line-height: 25px;"">&nbsp;</td>
                    </tr>

                </table>

            </td>
        </tr>

    </table>
    <!-- end section -->

</body>

</html>
          ";

            return htmlContent;
        }
    }
}