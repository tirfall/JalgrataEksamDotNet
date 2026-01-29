using Microsoft.AspNetCore.Mvc;

namespace JalgrataEksamDotNet.Controllers;

// MVC lehed (tabel + admin profiil).
public class ExamsController : Controller
{
    public IActionResult Index()
    {
        // Admin režiim kontrollitakse sessioonist.
        ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin") == "true";
        return View();
    }

    public IActionResult Profile()
    {
        // Kui pole admin, siis suuname tagasi tabelisse.
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToAction(nameof(Index));
        }

        return View();
    }

    [HttpPost]
    public IActionResult EnableAdmin([FromBody] AdminLoginRequest request)
    {
        // Lihtne "krüpto": iga numbri +1, seega 1234 -> 2345.
        const string encrypted = "2345";
        var decrypted = DecryptPassword(encrypted);

        if (request.Password != decrypted)
        {
            return Unauthorized(new { error = "Vale parool." });
        }

        HttpContext.Session.SetString("IsAdmin", "true");
        return Ok(new { message = "Admin enabled." });
    }

    [HttpPost]
    public IActionResult DisableAdmin()
    {
        // Admin sessioon eemaldatakse.
        HttpContext.Session.Remove("IsAdmin");
        return Ok(new { message = "Admin disabled." });
    }

    private static string DecryptPassword(string encrypted)
    {
        // Võtame iga numbri ja liigutame ühe võrra tagasi (2345 -> 1234).
        return string.Concat(encrypted.Select(ch => (char)(ch - 1)));
    }
}

public class AdminLoginRequest
{
    public string Password { get; set; } = string.Empty;
}