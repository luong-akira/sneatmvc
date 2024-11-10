using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using Sneat.MVC.DAL;

namespace Sneat.MVC.Common
{
    public class Utils
    {
        #region Encode-Decode
        public static string GenPass(string pass)
        {
            return BCrypt.Net.BCrypt.HashPassword(pass, 10);
        }

        public static bool CheckPass(string pass, string userPass)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(pass, userPass);
            }
            catch
            {
                return false;
            }
        }

        public static string CreateMD5(string input)
        {
            //bam du lieu
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static string GenerateJWTAuthetication(int id, string email)
        {
            try
            {
                var claims = new List<Claim>
            {
                new Claim("id", id.ToString()),
                new Claim("email", email.ToString())
            };

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Convert.ToString(ConfigurationManager.AppSettings["JwtKey"])));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires =
                    DateTime.Now.AddDays(
                        Convert.ToDouble(Convert.ToString(ConfigurationManager.AppSettings["JwtExpireDays"])));

                var token = new JwtSecurityToken(
                    Convert.ToString(ConfigurationManager.AppSettings["JwtIssuer"]),
                    Convert.ToString(ConfigurationManager.AppSettings["JwtAudience"]),
                    claims,
                    expires: expires,
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return CreateMD5($"{DateTime.Now.ToString()}_{id}_{email}");
            }
        }
        #endregion

        #region Filtering
        public static string RemoveDiacritics(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            input = input.Replace("đ", "d").Replace("Đ", "D");

            string decomposed = input.Normalize(NormalizationForm.FormD);
            Regex nonDiacritic = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string withoutDiacritics = nonDiacritic.Replace(decomposed, string.Empty).Normalize(NormalizationForm.FormC);

            // Normalize whitespace characters
            string normalizedWhitespace = Regex.Replace(withoutDiacritics, @"\s+", " ");

            return normalizedWhitespace.Trim().ToLower();
        }
        #endregion

        #region Config System
        public static string getFullUrl()
        {
            if (HttpContext.Current == null)
                return "";
            var request = HttpContext.Current.Request;
            var url = request.Url.Scheme + "://" + request.Url.Authority;
            return url;
        }

        #endregion

        #region Handle DateTime
        public static DateTime? DefaultDateTime(DateTime? dt)
        {
            if (dt.HasValue)
            {
                if (dt.Value < new DateTime(1753, 1, 1))
                {
                    return new DateTime(1753, 1, 1);
                }
                return dt.Value;
            }
            return null;
        }
        #endregion

        #region Convert
        public static string ConvertListIntegerToString(List<int> listInt)
        {
            return string.Join(", ", listInt);
        }

        public static List<int> ConvertStringToListInteger(string str)
        {
            var list = str
                .Split(',')
                .Select(s => s.Trim())
                .Select(int.Parse)
                .ToList();

            return list;
        }

        #endregion

        public static string getTokenApp(HttpRequestHeaders headers)
        {
            if (headers.Contains("token"))
            {
                return headers.GetValues("token").FirstOrDefault();
            }
            return "";
        }

        public static int? getUserFromToken(HttpRequestHeaders headers)
        {
            try
            {
                var token = string.Empty;
                if (headers.Contains("token"))
                {
                    token = headers.GetValues("token").FirstOrDefault();
                }

                if (string.IsNullOrEmpty(token))
                {
                    return null;
                }

                SneatContext context = new SneatContext();
                var user = context.Users.Where(u => u.Token == token && u.IsDeleted == 0).FirstOrDefault();
                if (user != null)
                {
                    return user.ID;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}