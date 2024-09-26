//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.IO;
//using System.Threading.Tasks;

//namespace PT_Management_System_V2.Controllers
//{
//    public class ImageController : Controller
//    {
//        // Move these two to seperate file in future. 

//        public async Task<IActionResult> GetImageByIdAsync(int id)
//        {
//            var image = await GetImageByIdAsync(id);

//            if (image == null)
//            {
//                return NotFound();
//            }

//            var imagePath = image.FilePath;

//            if (!System.IO.File.Exists(imagePath)) {
//                return NotFound();
//            }

//            var memory = new MemoryStream();
//            using (var stream = new FileStream(imagePath, FileMode.Open))
//            {
//                await stream.CopyToAsync(memory);
//            }

//            return await _context.Images.FindAsync(id);
//        }

//    }
//}





//namespace PT_Management_System_V2.Controllers
//{
//    public class ImageController : Controller
//    {
//        // Move these two to seperate file in future. 
//        public ImageService(YourDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<Image> GetImageByIdAsync(int id)
//        {
//            return await _context.Images.FindAsync(id);
//        }

//        public class ImageController(IImageService imageService)
//        {
//            _imageService = imageService;
//        }
//    }
//}






//using Microsoft.AspNetCore.Mvc;
//using PT_Management_System_V2.Controllers;

//private readonly IWebHostEnvironment _env;

//public HomeController(IWebHostEnvironment env)
//{
//    _env = env;

//}

//public IActionResult About()
//{
//    var path = _env.WebRootPath;
//}