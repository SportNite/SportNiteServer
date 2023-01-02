using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NUnit.Framework;
using SportNiteServer.Services;

namespace SportNiteServer.Tests;

public class UtilsUnitTests
{
    private const string TestUserFirebaseId = "aaaaaaaaaaaaaaaaaaaaaaaaaaaa";
    private const string TestUserPhone = "48123456789";

    [Test]
    public void Average()
    {
        var average = Utils.Average(new List<double> { 1, 2, 3, 4, 5 });
        Assert.AreEqual(3.0, average);
    }

    [Test]
    public void GetFirebaseUserId()
    {
        var claims = new List<Claim>
        {
            new("user_id", TestUserFirebaseId),
            new("phone_number", TestUserPhone)
        };
        var claimsPrincipal = new ClaimsPrincipal();
        claimsPrincipal.AddIdentity(new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme));
        Assert.AreEqual(TestUserFirebaseId, Utils.GetFirebaseUserId(claimsPrincipal));
    }
}