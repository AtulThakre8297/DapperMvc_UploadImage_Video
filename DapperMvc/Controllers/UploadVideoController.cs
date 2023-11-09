using DapperMvc.Context;
using DapperMvc_Upload_Image.Models.ViewModel;
using DapperMvc_Upload_Image.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Dapper;

namespace DapperMvc_Upload_Image.Controllers
{
    public class UploadVideoController : Controller
    {
         readonly DapperContext _dapperContext;
         readonly IWebHostEnvironment _environment;

      
        public UploadVideoController(DapperContext dapperContext, IWebHostEnvironment environment)
        {
            _dapperContext = dapperContext;
            _environment = environment;
        }


        public IActionResult Index()
        {
            using (IDbConnection dbConnection = _dapperContext.CreateConnection())
            {
                var data = dbConnection.Query<Video>("SELECT * FROM Videos").ToList();
                return View(data);
            }
        }

        [HttpGet]
        public IActionResult AddVideo()
        {
            return View();
        }



        //[HttpPost]
        //public IActionResult AddVideo(VideoVM video)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var path = _environment.WebRootPath;
        //        var videoFilePath = "Content/Video/" + video.VideoPath.FileName;
        //        var fullPath = Path.Combine(path, videoFilePath);

        //        using (var stream = new FileStream(fullPath, FileMode.Create))
        //        {
        //            video.VideoPath.CopyTo(stream);
        //        }

        //        using (IDbConnection dbConnection = _dapperContext.CreateConnection())
        //        {
        //            var data = new Video
        //            {
        //                Name = video.Name,
        //                VideoPath = video.VideoPath
        //            };

        //            dbConnection.Execute("INSERT INTO Videos (Name, VideoPath) VALUES (@Name, @VideoPath)", data);

        //            return RedirectToAction("Index");
        //        }
        //    }
        //    else
        //    {
        //        return View(video);
        //    }
        //}

        [HttpPost]
        public IActionResult AddVideo(VideoVM video)
        {
            if (ModelState.IsValid)
            {
                var path = _environment.WebRootPath;
                var videoFileName = Path.GetFileName(video.VideoPath.FileName);
                var videoFilePath = "Content/Video/" + videoFileName;
                var fullPath = Path.Combine(path, videoFilePath);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    video.VideoPath.CopyTo(stream);
                }

                using (IDbConnection dbConnection = _dapperContext.CreateConnection())
                {
                    var data = new Video
                    {
                        Name = video.Name,
                        VideoPath = videoFilePath // Store the file path, not the IFormFile object
                    };

                    dbConnection.Execute("INSERT INTO Videos (Name, VideoPath) VALUES (@Name, @VideoPath)", data);

                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(video);
            }
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            using (IDbConnection dbConnection = _dapperContext.CreateConnection())
            {
                var vid = dbConnection.QueryFirstOrDefault<Video>("SELECT * FROM Videos WHERE Id = @Id", new { Id = id });

                if (vid != null)
                {
                    var video = new Video
                    {
                        Id = vid.Id,
                        Name = vid.Name
                    };

                    return View(vid);
                }
                else
                {
                    return NotFound(); // Handle the case when the video with the given id doesn't exist.
                }
            }
        }

        [HttpPost]
        public IActionResult Update(Video video)
        {
            if (ModelState.IsValid)
            {
                using (IDbConnection dbConnection = _dapperContext.CreateConnection())
                {
                    var data = new Video
                    {
                        Id = video.Id,
                        Name = video.Name
                    };

                    // Update the video in the database
                    var affectedRows = dbConnection.Execute("UPDATE Videos SET Name = @Name WHERE Id = @Id", data);

                    if (affectedRows > 0)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return NotFound(); // Handle the case when the video with the given id doesn't exist or the update operation fails.
                    }
                }
            }
            else
            {
                return View(video);
            }
        }


        public IActionResult Delete(int id)
        {
            using (IDbConnection dbConnection = _dapperContext.CreateConnection())
            {
                // Check if the video exists
                var video = dbConnection.QueryFirstOrDefault<Video>("SELECT * FROM Videos WHERE Id = @Id", new { Id = id });

                if (video != null)
                {
                    // Delete the video from the database
                    dbConnection.Execute("DELETE FROM Videos WHERE Id = @Id", new { Id = id });
                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound(); // Handle the case when the video with the given id doesn't exist.
                }
            }
        }



    }
}
