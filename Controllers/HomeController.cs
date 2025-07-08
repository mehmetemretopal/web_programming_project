using authenticatedapp.data;
using authenticatedapp.Models;
using authenticatedapp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace authenticatedapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogDbContext _blogContext;
      

        public HomeController(ILogger<HomeController> logger, BlogDbContext blogContext)
        {
            _logger = logger;
            _blogContext = blogContext;
          
        }
       
        public IActionResult Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var blogs = from s in _blogContext.Blogs
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                blogs = blogs.Where(s => s.title.Contains(searchString)
                                       || s.name.Contains(searchString));
            }
            int totalBlogs = _blogContext.Blogs.Count();
            ViewBag.TotalBlogs = totalBlogs;
			return View(blogs);
        }

        public IActionResult BlogsList()
        {
            var blogs = _blogContext.Blogs.ToList();
            return View(blogs);
        }

        [Authorize]
        [HttpGet]
        public async Task <IActionResult> CreateBlog()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> CreateBlog(Blog blog)
        {
            if (ModelState.IsValid)
            {
                _blogContext.Blogs.Add(blog);
                await _blogContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(blog);

        }


        //delete iþlemlerini yaptýðým kýsým
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _blogContext.Blogs.AsNoTracking()
		.FirstOrDefaultAsync(m => m.Id == id);
			if (blog == null)
            {
                return NotFound();
            }
			
			return View(blog); // Delete.cshtml sayfasýna yönlendirilir.
        }

        [HttpPost, ActionName("Delete")] // Bu yöntem DeleteConfirmed olarak da adlandýrýlabilir.
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _blogContext.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            _blogContext.Blogs.Remove(blog);
            await _blogContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Update iþlemleri
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _blogContext.Blogs.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Blog blog)
        {
            if (id != blog.Id)
            {
                return BadRequest();
            }

            var blogToUpdate = await _blogContext.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blogToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync(blogToUpdate, "",
                b => b.name, b => b.title, b => b.blogtext))
            {
                try
                {
                    await _blogContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
                }
            }

            return View(blogToUpdate);
        }

        private bool BlogExists(int id)
        {
            return _blogContext.Blogs.Any(e => e.Id == id);
        }



        public IActionResult CardDetail(int id)
		{
			// BlogContext'ten ilgili blogu getiriyoruz
			var blog = _blogContext.Blogs.FirstOrDefault(b => b.Id == id);

			// Eðer blog bulunamazsa bir hata sayfasý dönebilirsiniz
			if (blog == null)
			{
				return NotFound();
			}

			// Blog detaylarýný View'e gönderiyoruz
			return View(blog);
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
		
    }
}
/*
 *  public IActionResult Create()
        {
            return View();
        }

        // Kullanýcýyý veritabanýna kaydet
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index"); // Kullanýcý listeleme sayfasýna yönlendirme
            }
            return View(user);
        }

        // Kullanýcýlarý listele
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
 */