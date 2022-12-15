using AutoMapper;
using Blogs.CMS.Commons;
using Blogs.CMS.Controllers.Params;
using Blogs.CMS.Models;
using Blogs.CMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blogs.CMS.Controllers
{
    public class TagController : BaseController
    {
        private readonly TagService _tagService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IMapper _mapper;

        public TagController(TagService tagService,
            IMapper mapper,
            CategoryService categoryService,
            IWebHostEnvironment hostEnvironment
            )
        {
            _tagService = tagService;
            _mapper = mapper;
            webHostEnvironment = hostEnvironment;
        }

        [HttpGet("tag")]
        public async Task<IActionResult> Index()
        {
            List<TagsGetModel> list = await _tagService.GetTags().Select(_ => new TagsGetModel
            {
                Title = _.Title,
                Slug = _.Slug,
                Id = _.Id
            }).ToListAsync();

            return View(list);
        }

        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddTagParams input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tagEntry = await _tagService.Add(input);
                    if (tagEntry is not null)
                    {
                        this.SetNotification("Tạo tag thành công", NotificationEnumeration.Success, true);
                    }
                    else
                    {
                        this.SetNotification("Tạo tag thành công", NotificationEnumeration.Error, true);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tag = await _tagService.GetTag(id);

            return PartialView(tag);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var data = await _tagService.GetTag(id);

            var model = _mapper.Map<DeleteTagParams>(data);

            return PartialView(model);
        }
    }
}
