using AutoMapper;
using Blogs.CMS.Commons;
using Blogs.CMS.Controllers.Params;
using Blogs.CMS.Models;
using Blogs.CMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Blogs.CMS.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly CategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(CategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet("chuyen-muc")]
        public async Task<IActionResult> Index()
        {
            List<CategoryGetModel> categories = await _categoryService.GetCategories().Select(x => new CategoryGetModel
            {
                Id = x.Id,
                Title = x.Title,
                Status = x.Status,
                CreatedBy = x.Createdby,
                CreatedDate = x.Createddate,
                IsParentCate = x.Isparentcate,
                ParentId = x.Parentid
            }).ToListAsync();
            return View(categories);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.ListStatus = new SelectList(
            new List<SelectListItem>
                    {
                new SelectListItem {Text = "Kích hoạt", Value = "1"},
                new SelectListItem {Text = "Ẩn", Value = "-1"},
                    }, "Value", "Text");
            var listParent = await _categoryService.GetParentCategories().ToListAsync();

            ViewBag.ParentCates = new SelectList(
                listParent.Select(_ => new SelectListItem
                {
                    Text = _.Title,
                    Value = _.Id.ToString(),
                }), "Value", "Text"
                );
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddCategoryParams input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var cateEntry = await _categoryService.AddCategory(input);
                    if (cateEntry is not null)
                    {
                        this.SetNotification("Tạo chuyên mục thành công", NotificationEnumeration.Success, true);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    //ModelState.AddModelError("", "Có lỗi xảy ra! Không thể thêm mới Chuyên mục");
                    ViewBag.ListStatus = new SelectList(
                    new List<SelectListItem>
                            {
                        new SelectListItem {Text = "Kích hoạt", Value = "1"},
                        new SelectListItem {Text = "Ẩn", Value = "-1"},
                            }, "Value", "Text");

                    this.SetNotification("Tạo chuyên mục thành công", NotificationEnumeration.Error, true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.ListStatus = new SelectList(
            new List<SelectListItem>
                    {
                new SelectListItem {Text = "Kích hoạt", Value = "1"},
                new SelectListItem {Text = "Ẩn", Value = "-1"},
                    }, "Value", "Text");
            var listParent = await _categoryService.GetParentCategories().ToListAsync();

            ViewBag.ParentCates = new SelectList(
                listParent.Select(_ => new SelectListItem
                {
                    Text = _.Title,
                    Value = _.Id.ToString(),
                }), "Value", "Text"
                );

            var model = await _categoryService.GetCategoryDetail(id);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCategoryParams input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var cateEntry = await _categoryService.EditCategory(input);
                    if (cateEntry is not null)
                    {
                        this.SetNotification("Cập nhật chuyên mục thành công", NotificationEnumeration.Success, true);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.ListStatus = new SelectList(
                    new List<SelectListItem>
                            {
                        new SelectListItem {Text = "Kích hoạt", Value = "1"},
                        new SelectListItem {Text = "Ẩn", Value = "-1"},
                            }, "Value", "Text");

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
            var data = await _categoryService.GetCategoryDetail(id);

            var model = _mapper.Map<DeleteCategoryParams>(data);

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteCategoryParams input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var cateEntry = await _categoryService.DeleteCategory(input.Id);
                    if (cateEntry is not null)
                    {
                        this.SetNotification("Xóa chuyên mục thành công", NotificationEnumeration.Success, true);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.ListStatus = new SelectList(
                    new List<SelectListItem>
                            {
                        new SelectListItem {Text = "Kích hoạt", Value = "1"},
                        new SelectListItem {Text = "Ẩn", Value = "-1"},
                            }, "Value", "Text");

                    this.SetNotification("Có gì đó sai sai", NotificationEnumeration.Error, true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("Index");
        }
    }
}
