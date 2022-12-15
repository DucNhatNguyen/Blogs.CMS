using AutoMapper;
using Blogs.CMS.Commons;
using Blogs.CMS.Controllers.Params;
using Blogs.CMS.Models;
using Blogs.CMS.Services;
using Google.Apis.Drive.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Upload;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Firebase.Auth;
using Firebase;
using Firebase.Storage;

namespace Blogs.CMS.Controllers
{
    public class BlogController : BaseController
    {
        private readonly BlogService _blogService;
        private readonly CategoryService _categoryService;
        private readonly TagService _tagService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IMapper _mapper;

        private static string ApiKey = "AIzaSyB0474aI9earN2pwQUsjgLOlJyycVYrP9w";
        private static string Bucket = "blog-storage-media.appspot.com";
        private static string AuthEmail = "ducnhat090199@gmail.com";
        private static string AuthPassword = "test@123";

        public BlogController(BlogService blogService,
            IMapper mapper,
            CategoryService categoryService,
            TagService tagService,
            IWebHostEnvironment hostEnvironment
            )
        {
            _blogService = blogService;
            _mapper = mapper;
            _categoryService = categoryService;
            webHostEnvironment = hostEnvironment;
            _tagService = tagService;
        }

        [HttpGet("blog")]
        public async Task<IActionResult> Index()
        {
            List<BlogGetModel> blogs = await _blogService.GetBlogs().Select(_ => new BlogGetModel
            {
                Id = _.Id,
                Title = _.Title,
                Status = _.Status,
                SortDesc = _.Sortdesc,
                Content = _.Content,
                PublicDate = _.Publicdate,
                View = _.View,
                Author = _.Author,
                Thumbnail = _.Thumbnail,
                IsHotBlog = _.Ishotblog,
                FileGoogleDriveId = _.Filegoogledriveid
            }).ToListAsync();
            return View(blogs);
        }

        [HttpGet("blog/chi-tiet-blog")]
        public async Task<IActionResult> Create()
        {
            var listCates = await _categoryService.GetCategories().ToListAsync();

            ViewBag.ListCates = new SelectList(
                listCates.Select(_ => new SelectListItem
                {
                    Text = _.Title,
                    Value = _.Id.ToString(),
                }), "Value", "Text"
                );

            var tags = await _tagService.GetTags().ToListAsync();
            List<SelectListItem> tagList = tags.Select(_ => new SelectListItem
            {
                Text = _.Title,
                Value = _.Id.ToString()
            }).ToList();

            ViewBag.ListTags = tagList;
            return View();
        }

        [HttpPost("blog/chi-tiet-blog")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddBlogParams input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string filename = string.Empty;
                    if (input.ImageFile is not null)
                    {
                        filename = $"{Guid.NewGuid().ToString()}_{DateTime.Now.Ticks.ToString()}{Path.GetExtension(input.ImageFile.FileName)}";
                        input.Thumbnail = filename;

                        // upload file
                        string? fileGdId = await FileUploadInFolder(input.ImageFile, filename);
                        input.FileGoogleDriveId = fileGdId;
                    }
                    var blogEntry = await _blogService.AddBlog(input);
                    if (blogEntry is not null)
                    {
                        this.SetNotification("Tạo blog thành công", NotificationEnumeration.Success, true);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    this.SetNotification("Tạo blog không thành công", NotificationEnumeration.Error, true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("Index");
        }

        [HttpGet("blog/chi-tiet-blog/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var blog = await _blogService.GetBlog(id);
            var blogTag = await _tagService.GetTagOfBlog(id);
            blog.Tag = blogTag.Select(_ => _.Tagid.ToString()).ToList();
            
            var listCates = await _categoryService.GetCategories().ToListAsync();

            ViewBag.ListCates = new SelectList(
                listCates.Select(_ => new SelectListItem
                {
                    Text = _.Title,
                    Value = _.Id.ToString(),
                }), "Value", "Text"
                );

            var tags = await _tagService.GetTags().ToListAsync();
            List<SelectListItem> tagList = tags.Select(_ => new SelectListItem
            {
                Text = _.Title,
                Value = _.Id.ToString()
            }).ToList();

            ViewBag.ListTags = tagList;

            return PartialView(blog);
        }

        [HttpPost("blog/chi-tiet-blog/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBlogParams input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string filename = string.Empty;
                    string oldFileName = input.Thumbnail!;
                    if (input.ImageFile is not null)
                    {
                        filename = $"{Guid.NewGuid().ToString()}_{DateTime.Now.Ticks.ToString()}{Path.GetExtension(input.ImageFile.FileName)}";
                        input.Thumbnail = filename;

                        // upload file
                        string oldpath = Path.Combine(webHostEnvironment.WebRootPath + "/Images/", oldFileName);

                        string? oldFileGdId = input.FileGoogleDriveId;

                        string? fileGdId = await FileUploadInFolder(input.ImageFile, filename, oldFileGdId);
                        input.FileGoogleDriveId = fileGdId;
                    }

                    //excute dbcontext
                    var blogEntry = await _blogService.Edit(input);
                    if (blogEntry is not null)
                    {
                        this.SetNotification("Cập nhật blog thành công", NotificationEnumeration.Success, true);
                        return RedirectToAction("Edit", new { @id = input.Id });
                    }
                }
                else
                {
                    this.SetNotification("Có gì đó sai sai", NotificationEnumeration.Error, true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var data = await _blogService.GetBlog(id);

            var model = _mapper.Map<DeleteBlogParams>(data);

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteBlogParams input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var blogEntry = await _blogService.Delete(input.Id);
                    if (blogEntry is not null)
                    {
                        this.SetNotification("Xóa blog thành công", NotificationEnumeration.Success, true);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    this.SetNotification("Có gì đó sai sai", NotificationEnumeration.Error, true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CopyBlogAsync(int id)
        {
            var data = await _blogService.GetBlog(id);

            return PartialView(data);
        }
        public async Task<IActionResult> CopyBlogAction(EditBlogParams input)
        {
            try
            {
                if (input.Id != 0)
                {
                    //excute dbcontext
                    var blogEntry = await _blogService.CopyBlog(input.Id);
                    if (blogEntry is not null)
                    {
                        this.SetNotification("Copy blog thành công", NotificationEnumeration.Success, true);
                    }
                }
                else
                {
                    this.SetNotification("Có gì đó sai sai", NotificationEnumeration.Error, true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("Index");
        }

        private async Task<string?> UploadedFile(IFormFile thumbnail, string filename)
        {
            if (thumbnail != null)
            {
                string? fileGdId = await FileUploadInFolder(thumbnail, filename);

                string wwwRootPath = webHostEnvironment.WebRootPath;
                string extension = Path.GetExtension(thumbnail.FileName);

                string path = Path.Combine(wwwRootPath + "/Images/", filename);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await thumbnail.CopyToAsync(fileStream);
                }

                return fileGdId;
            }

            return null;
        }

        public async Task<string?> FileUploadInFolder(IFormFile thumbnail, string filename, string? owlFileGdId = null)
        {
            string CSPath = webHostEnvironment.WebRootPath;
            string ApplicationName = "GoogleDriveUploadMedia";

            if (thumbnail != null && thumbnail.Length > 0)
            {
                var credential = GoogleCredential.FromFile("ServiceAccountCred.json").CreateScoped(DriveService.ScopeConstants.Drive);
                // Create the service.
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                //get file path
                string path = Path.Combine(CSPath + "/GoogleDriveFiles/", Path.GetFileName(thumbnail.FileName));
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await thumbnail.CopyToAsync(fileStream);
                }
                //create file metadata
                var FileMetaData = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = filename,
                    Parents = new List<string>
                    {
                        "1A6s3zaOBF3O1mnYcOolsfb-QlQ3_6v3g"
                    }
                };

                FilesResource.CreateMediaUpload request;
                IUploadProgress result;
                string fileId = string.Empty;
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    request = service.Files.Create(FileMetaData, stream, thumbnail.ContentType);
                    request.Fields = "*";

                    result = await request.UploadAsync(CancellationToken.None);

                    if (result.Status != UploadStatus.Completed)
                    {
                        throw new ArgumentException("Upload file to clound fail.");
                    }

                    if (result.Status == UploadStatus.Completed)
                    {
                        var permission = new Permission { AllowFileDiscovery = true, Type = "anyone", Role = "reader" };
                        fileId = request.ResponseBody.Id;
                        await service.Permissions.Create(permission, fileId).ExecuteAsync();

                        // xóa thumnail cũ
                        if (!string.IsNullOrEmpty(owlFileGdId))
                            await service.Files.Delete(owlFileGdId).ExecuteAsync(CancellationToken.None);
                    }
                }
                string linkFirebase = await UploadFileToFirebase(thumbnail);

                return fileId;
            }

            return null;
        }

        private async Task<string> UploadFileToFirebase(IFormFile thumbnail)
        {
            string link;
            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

                var stream = thumbnail.OpenReadStream();

                // you can use CancellationTokenSource to cancel the upload midway
                var cancellation = new CancellationTokenSource();

                var task = new FirebaseStorage(
                    Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                    })
                    .Child("receipts")
                    .Child("test")
                    .Child("someFile.png")
                    .PutAsync(stream, cancellation.Token);

                var meta = new FirebaseStorage(
                    Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                    })
                    .Child("receipts")
                    .Child("test")
                    .Child("someFile.png").GetMetaDataAsync();

                var linkdown = new FirebaseStorage(
                    Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                    })
                    .Child("receipts")
                    .Child("test")
                    .Child("someFile.png").GetDownloadUrlAsync();


                link = await task;

                var abc = await meta;
                var abcd = await linkdown;

                stream.Dispose();
            }
            catch (Exception e)
            {
                throw;
            }

            return link;
        }
    }
}
