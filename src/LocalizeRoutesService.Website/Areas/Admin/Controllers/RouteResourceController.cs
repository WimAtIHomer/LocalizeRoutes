using System;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IHomer.Services.LocalizeRoutes.Repositories;
using IHomer.Services.LocalizeRoutes;
using IHomer.Services.LocalizeRoutes.Entities;
using System.Web.UI;
using LocalizeRoutesService.Website.Areas.Admin.Models;
using ImageDrawing = System.Drawing.Image;
using System.Collections;

namespace LocalizeRoutesService.Website.Areas.Admin.Controllers
{
    public partial class RouteResourceController : Controller
    {
        public virtual ActionResult Index(int id = 0, string route = Resource.ROUTE_SEPARATOR, int languageID = 1, string saveError = "")
        {
            var repo = new ResourceRepository();
            var pages = repo.GetRoutes();
            var languages = repo.GetLanguages();
            var factory = new RouteResourceProviderFactory();
            var provider = (RouteResourceProvider)factory.CreateGlobalResourceProvider(route);
            Resource resource = null;
            Language selectedLanguage;
            if (id > 0)
            {
                resource = repo.GetResourceByID(id);
                selectedLanguage = resource.Language;
            }
            else
            {
                selectedLanguage =
                    languages.Where(l => l.Value.Default == true || l.Value.ID == languageID).OrderBy(l => l.Value.Default).
                    First().Value;
            }

            var model = new RouteResourceFindModel
            {
                Routes = new SelectList(pages, Resource.ROUTE_SEPARATOR),
                Route = pages.FirstOrDefault(p => p.ToLowerInvariant() == provider.ResourceRoute) ?? provider.ResourceRoute,
                Languages =
                    new SelectList(languages.Values, "ID", "EnglishName",
                                   selectedLanguage.ID),
                LanguageID = selectedLanguage.ID
            };

            var resources = provider.ResourceReader;
            var names = (from DictionaryEntry key in resources
                         select repo.GetResource(key.Key.ToString(), route, selectedLanguage.ID) ??
                                (provider.FindMissingResource(key.Key.ToString(), selectedLanguage.Name) ??
                                new Resource
                                {
                                    Key = key.Key.ToString(),
                                    ResourceType = ResourceType.Text,
                                    Route = Resource.ROUTE_SEPARATOR,
                                    Value = "Missing resource!!!"
                                })).OrderBy(n => n.Key).ToList();

            model.Resources = names;
            if (resource != null)
            {
                model.ResourceID = resource.ID;
                model.ResourceKey = resource.Key;
                model.ResourceRoute = resource.Route;
                model.Value = resource.Value;
                model.ResourceType = (int)resource.ResourceType;
                model.SaveError = saveError;
            }
            else if (names.Count > 0)
            {
                model.ResourceID = names[0].ID;
                model.ResourceKey = names[0].Key;
                model.ResourceRoute = names[0].Route;
                model.Value = names[0].Value;
                model.ResourceType = (int)names[0].ResourceType;
            }

            return View("Index", model);
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public virtual JsonResult Get(int id)
        {
            var repo = new ResourceRepository();
            var resource = repo.GetResourceByID(id) ?? new Resource { Key = "Missing resource!!!", ResourceType = ResourceType.Text, Route = Resource.ROUTE_SEPARATOR, Value = "" };
            var model = new RouteResourceModel()
                {
                    ResourceID = resource.ID,
                    ResourceKey = resource.Key,
                    ResourceRoute = resource.Route,
                    ResourceType = (int) resource.ResourceType,
                    Value = resource.Value
                };         
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(RouteResourceEditModel model)
        {
            var value = string.Empty;
            var type = (ResourceType)model.ResourceType;
            if (type == ResourceType.Text)
            {
                value = model.TextEditor;
            }
            if (type == ResourceType.Html)
            {
                value = model.CKEditor;
            }

            try
            {
                if (model.ResourceID == 0)
                {
                    RouteResourceService.AddResourceString(model.ResourceKey, value, model.LanguageID,
                                                           model.ResourceRoute, type);
                }
                else
                {
                    RouteResourceService.UpdateResourceString(model.ResourceID, model.ResourceKey, value,
                                                              model.LanguageID,
                                                              model.ResourceRoute, type);
                }
            }
            catch (DbUpdateException ex)
            {
                Exception trueException = ex;
                while (trueException.InnerException != null)
                {
                    trueException = trueException.InnerException;
                }

                return RedirectToAction(Actions.Index(model.ResourceID, model.ResourceRoute, model.LanguageID, trueException.Message));
            }
            catch (Exception ex)
            {
                Exception trueException = ex;
                while (trueException.InnerException != null)
                {
                    trueException = trueException.InnerException;
                }

                return RedirectToAction(Actions.Index(model.ResourceID, model.ResourceRoute, model.LanguageID, trueException.Message));
            }
            var repo = new ResourceRepository();
            var resource = repo.GetResource(model.ResourceKey, model.ResourceRoute, model.LanguageID);

            if (model.ClearCache) RouteResourceProviderFactory.ClearCache();

            return RedirectToAction(Actions.Index(resource.ID, model.ResourceRoute, model.LanguageID));
        }

        [HttpPost]
        public virtual ActionResult Delete(int deleteID, string pageRoute, int pageLanguage)
        {
            RouteResourceService.DeleteResourceString(deleteID);
            return RedirectToAction(Actions.Index(0, pageRoute, pageLanguage));
        }

        //
        // GET: /Admin/Upload/
        public virtual ActionResult Upload()
        {
            ViewData["CKEditorFuncNum"] = Request.QueryString["CKEditorFuncNum"];
            ViewData["CKEditor"] = Request.QueryString["CKEditor"];
            ViewData["langCode"] = Request.QueryString["langCode"];

            var dirPath = Path.Combine(Server.MapPath("~/Content/UploadImages/"));
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            var fileNames = Directory.GetFiles(dirPath);
            var images = fileNames.Select(filename => new FileInfo(filename)).ToList();
            return View(images);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Upload(HttpPostedFileBase file, string ckEditorFuncNum, string ckEditor, string langCode)
        {
            if (file.ContentLength > 0)
            {
                var dirPath = Path.Combine(Server.MapPath("~/Content/UploadImages/"));
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                var fileName = Path.GetFileName(file.FileName);
                if (fileName != null)
                {
                    var path = Path.Combine(dirPath, fileName);
                    file.SaveAs(path);
                    ViewData["CKEditorFuncNum"] = ckEditorFuncNum;
                    ViewData["CKEditorUrl"] = Url.Content("~/Content/UploadImages/" + fileName);
                    ViewData["CKEditor"] = ckEditor;
                    ViewData["langCode"] = langCode;

                    return View("UploadNew");
                }
            }
            throw new ApplicationException("Upload failed");
        }

        public virtual ActionResult Images(string id)
        {
            // So that I don't need to specify another route.
            var filename = id;

            // Get image filename from Request object.             
            var photoFolder = Server.MapPath("~/Content/UploadImages/");
            filename = Path.Combine(photoFolder, filename);
            const int width = 150; const int height = 150;
            const string bg = "#ffffff";

            var r = new StreamReader(filename);
            var thumbnail = ImageDrawing.FromStream(r.BaseStream);

            var ms = new MemoryStream();
            thumbnail = Scale(thumbnail, width, height, bg);
            thumbnail.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            byte[] imageByte = ms.ToArray();

            ms.Dispose();
            thumbnail.Dispose();
            r.Dispose();

            return File(imageByte, "image/jpeg");
        }

        private static ImageDrawing Scale(ImageDrawing imgPhoto, int width, int height, string bg)
        {
            var sourceWidth = imgPhoto.Width;
            var sourceHeight = imgPhoto.Height;
            const int sourceX = 0;
            const int sourceY = 0;
            var destX = 0;
            var destY = 0;

            float nPercent;
            var nPercentW = (width / (float)sourceWidth);
            var nPercentH = (height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = Convert.ToInt16((width - (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = Convert.ToInt16((height - (sourceHeight * nPercent)) / 2);
            }
            var destWidth = (int)(sourceWidth * nPercent);
            var destHeight = (int)(sourceHeight * nPercent);

            var bmPhoto = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            var grPhoto = Graphics.FromImage(bmPhoto);
            var c = ColorTranslator.FromHtml(bg);

            grPhoto.Clear(c);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.DrawImage(imgPhoto, new Rectangle(destX, destY, destWidth, destHeight),
            new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
            GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
    }
}
