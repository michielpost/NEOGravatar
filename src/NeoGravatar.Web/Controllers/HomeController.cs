using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeoGravatar.Web.Models;
using NeoLux;
using SixLabors.ImageSharp;

namespace NeoGravatar.Web.Controllers
{
    public class HomeController : Controller
    {
    private readonly string _scriptHash = "b79ec10bb435f13d5c46e337f71d3c37a5a8c337";

    public IActionResult Index()
        {
            return View();
        }

    /// <summary>
    /// Upload an image
    /// </summary>
    /// <param name="formFile"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("Upload")]
    public IActionResult Upload(IFormFile formFile, UploadResultViewModel model)
    {
      if(model == null)
        model = new UploadResultViewModel();

      //Check submitted file
      if (formFile != null && formFile.Length > 0)
      {
        using (var fileStream = formFile.OpenReadStream())

        {
          using (Image<Rgba32> image = Image.Load(fileStream))
          {
            //Resize and create Base64 string
            image.Mutate(x => x
                 .Resize(80,80));
            model.Image = image.ToBase64String(ImageFormats.Png);
          }
        }

      }
      else if(!string.IsNullOrEmpty(model.Image) && !string.IsNullOrEmpty(model.Address))
      {

        try
        {
          //Submit base64 image to smartcontract
          var script = NeoAPI.GenerateScript(_scriptHash, "add", new object[] { model.Address, model.Image });
          var invoke = NeoAPI.TestInvokeScript(NeoAPI.Net.Test, script);

          //Check result
          if (invoke.result is byte[])
          {
            var textresult = System.Text.Encoding.ASCII.GetString((byte[])invoke.result);
          }
        }
        catch { }

        //Get hash
        //string sha1hash = Sha1Hash(model.Address);
        //return RedirectToAction("Preview", new { id = sha1hash });

        return View("Preview", new PreviewViewModel() { Image = model.Image });

      }

      return View(model);
    }

    /// <summary>
    /// Preview an image
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IActionResult Preview(string id)
    {
      //Get base64string for this id from the NEO blockchain
      var image = NeoAPI.getStorage(NeoAPI.Net.Test, _scriptHash, id);

      //Check if we find an image
      if (image == null || image.Value == null)
        return new NotFoundResult();

      PreviewViewModel model = new PreviewViewModel();
      model.Image = image.Value;

      return View(model);
    }

    /// <summary>
    /// Returns only the base64 string of an image
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IActionResult Img(string id)
    {
      //Get base64string for this id from the NEO blockchain
      var image = NeoAPI.getStorage(NeoAPI.Net.Test, _scriptHash, id);

      //Check if we find an image
      if (image == null || image.Value == null)
        return new NotFoundResult();

      return Content(image.Value);
    }


    private static string Sha1Hash(string stringToHash)
    {
      using (var sha1 = new SHA1Managed())
      {
        return BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(stringToHash)));
      }
    }

    public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
