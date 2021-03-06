﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Supra.Classes;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using Supra.Models;
using System.Data;

namespace Supra.Controllers
{
    public class SignInController : Controller
    {
        RespResult Result;
        Datamanager dm;
        DataTable Dt;
        string SqlQuery = "";


        //[Route("api/SignUp/registerbyphone")]
        //[HttpPost]
        //public RespResult RegisterByPhone([FromBody] PhoneNumber obj)
        //{
        //    bool OkSms;
        //    int smscode = SMS.SmsCode();
        //    if (!CheckPhoneExist(obj.Phone)) // add ! character 
        //    {

        //        dm = new Datamanager();
        //        SqlQuery = SqlQuery = "EXEC spwRegisterPhone @phone = " + SQL.String(obj.Phone) + ", @code = " + SQL.String(smscode);
        //        Dt = dm.RunQuery(SqlQuery);
        //        // send sms code
        //        string paradoxLogin = Datamanager.getValue("SMS_LOGIN_PARADOX");
        //        string paradoxUrl = Datamanager.getValue("SMS_URL_PARADOX");
        //        string paradoxPassword = Datamanager.getValue("SMS_PASSWORD_PARADOX");

        //        SMS sms = new SMS();
        //        OkSms = sms.SendSMSParadox(smscode.ToString(), paradoxUrl, paradoxLogin, paradoxPassword, obj.Phone, "Idram-LLC");

        //        if (OkSms)
        //        {
        //            Result = Methods.GetErrorDesc(0, null);
        //        }
        //        else
        //        {

        //        }

        //    }
        //    else
        //    {
        //        // this number already registrated
        //        string paradoxLogin = Datamanager.getValue("SMS_LOGIN_PARADOX");
        //        string paradoxUrl = Datamanager.getValue("SMS_URL_PARADOX");
        //        string paradoxPassword = Datamanager.getValue("SMS_PASSWORD_PARADOX");

        //        SMS sms = new SMS();
        //        sms.SendSMSParadox(smscode.ToString(), paradoxUrl, paradoxLogin, paradoxPassword, obj.Phone, "Idram-LLC");
        //    }
        //    //phone = ParsePhone(phone);
        //    return Result;
        //}









        private List<Person> people = new List<Person>
        {
            new Person {Login="admin@gmail.com", Password="12345", Role = "admin" },
            new Person { Login="qwerty", Password="55555", Role = "user" }
        };

        //[HttpPost("/token")]
        public async Task Token()
        {
            Datamanager dm = new Datamanager();


            var username = Request.Form["username"];
            var password = Request.Form["password"];

            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password.");
                return;
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            // сериализация ответа
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            Person person = people.FirstOrDefault(x => x.Login == username && x.Password == password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}