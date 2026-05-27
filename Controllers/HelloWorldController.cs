using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace MVCMovie.Controllers;

public class HelloWorldController : Controller
{
    public String Index()
    {
        return "This is my default action...";
    }

    public string Welcome()
    {
        return "This is the Welcome action method...";
    }
}