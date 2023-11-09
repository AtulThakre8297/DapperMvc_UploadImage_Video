  

using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Data;
using DapperMvc_Upload_Image.Models.ViewModel;
using DapperMvc_Upload_Image.Models;
using DapperMvc.Context;  // Make sure to include your DapperContext namespace

namespace DapperMvc_Upload_Image.Controllers
{
    public class UploadController : Controller
    {
        private readonly DapperContext _dapperContext;
        private readonly IWebHostEnvironment _environment;

        public UploadController(DapperContext dapperContext, IWebHostEnvironment environment)
        {
            _dapperContext = dapperContext;
            _environment = environment;
        }

        public IActionResult Index()
        {
            using (IDbConnection dbConnection = _dapperContext.CreateConnection())
            {
                var data = dbConnection.Query<Image>("SELECT * FROM Images").ToList();
                return View(data);
            }
        }

        [HttpGet]
        public IActionResult AddImage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddImage(ImageVM img)
        {
            if (ModelState.IsValid)
            {
                var path = _environment.WebRootPath;
                var filePath = "Content/Image/" + img.ImagePath.FileName;
                var fullPath = Path.Combine(path, filePath);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    img.ImagePath.CopyTo(stream);
                }

                using (IDbConnection dbConnection = _dapperContext.CreateConnection())
                {
                    var data = new Image
                    {
                        Name = img.Name,
                        ImagePath = filePath
                    };

                    dbConnection.Execute("INSERT INTO Images (Name, ImagePath) VALUES (@Name, @ImagePath)", data);

                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(img);
            }


        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            using (IDbConnection dbConnection = _dapperContext.CreateConnection())
            {
                var image = dbConnection.QueryFirstOrDefault<Image>("SELECT * FROM Images WHERE Id = @Id", new { Id = id });

                if (image != null)
                {
                    var imageVM = new Image
                    {
                        Id = image.Id,
                        Name = image.Name,
                        ImagePath = image.ImagePath
                    };

                    return View(imageVM);
                }
                else
                {
                    return NotFound(); // Handle the case when the image with the given id doesn't exist.
                }
            }
        }

        [HttpPost]
        public IActionResult Update(Image img)
        {
            if (ModelState.IsValid)
            {
                using (IDbConnection dbConnection = _dapperContext.CreateConnection())
                {
                    var data = new Image
                    {
                        Id = img.Id,
                        Name = img.Name,
                        ImagePath = img.ImagePath
                    };

                    // Update the image in the database
                    var affectedRows = dbConnection.Execute("UPDATE Images SET Name = @Name, ImagePath = @ImagePath WHERE Id = @Id", data);

                    if (affectedRows > 0)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return NotFound(); // Handle the case when the image with the given id doesn't exist or the update operation fails.
                    }
                }
            }
            else
            {
                return View(img);
            }
        }

        public IActionResult Delete(int id)
        {
            using (IDbConnection dbConnection = _dapperContext.CreateConnection())
            {
                // Check if the image exists
                var image = dbConnection.QueryFirstOrDefault<Image>("SELECT * FROM Images WHERE Id = @Id", new { Id = id });

                if (image != null)
                {
                    // Delete the image from the database
                    dbConnection.Execute("DELETE FROM Images WHERE Id = @Id", new { Id = id });
                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound(); // Handle the case when the image with the given id doesn't exist.
                }
            }
        }


    }
}
