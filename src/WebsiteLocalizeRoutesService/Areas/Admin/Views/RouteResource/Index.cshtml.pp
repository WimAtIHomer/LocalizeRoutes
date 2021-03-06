﻿@using $rootnamespace$.Resources
@model $rootnamespace$.Areas.Admin.Models.RouteResourceModel
<script src="@Url.Content("~/Scripts/ckeditor/ckeditor.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/ResourceEditor.js")" type="text/javascript"></script>
<script type="text/javascript">
    var home = '@Url.Content("~/")';

	$(function() {
		var availableRoutes = [
        @{
            bool first = true;
            foreach (var route in Model.Routes)
            {
                if (!first)
                { <text>,</text> }
                <text>"@route.Text"</text>
                first = false;
            }
        }
		];
		$( "#route" ).autocomplete({
			source: availableRoutes
		});
	});
</script>
<h2>@Resource.Message</h2>
<p>
    @Resource.EditResourceExplanation
</p>
@using (Html.BeginForm(MVC.Admin.RouteResource.Index(), FormMethod.Get, new { id = "routeForm" }))
{
    <div style="float:left;width:23%;">
        <fieldset>
            <legend>@Resource.RouteAndLanguage</legend>
            <div class="editor-label">
                @Resource.Route
            </div>
            <div class="editor-field">
                @Html.TextBoxFor(m => m.Route, new { id = "route" })
            </div>
            <div class="editor-label">
                @Resource.Language
            </div>
            <div class="editor-field">
                @Html.DropDownListFor(m => m.LanguageID, Model.Languages)
            </div>
            <p>
                <button>
                    @Resource.Get</button>
            </p>
        </fieldset>
        <fieldset>
            <legend>@Resource.Resources</legend>
            <div class="ResourceList">
                <ul>
                    @foreach (var resource in Model.Resources)
                    {
                        var missingclass = resource.ID==0 ? "missingResource" : string.Empty;
                        <li><a href="#" onclick="getResource('@resource.ID'); return false;" class="@missingclass">@resource.Key</a></li>
                    }
                </ul>
            </div>
        </fieldset>
    </div>
}

    <div style="float:right;width:75%;">
        <fieldset>
            <legend>@Resource.EditResource</legend>
        @using (Html.BeginForm(MVC.Admin.RouteResource.Save(), FormMethod.Post, new { id = "resourceForm" }))
        {
            @Html.HiddenFor(model => model.ResourceID)
            @Html.HiddenFor(model => model.LanguageID)
            <div class="editor-label">
                @Html.LabelFor(model => model.ResourceRoute)
            </div>
            <div class="editor-field">
                @Html.EditorFor(model => model.ResourceRoute)
                @Html.ValidationMessageFor(model => model.ResourceRoute)
            </div>
            <div class="editor-label">
                @Html.LabelFor(model => model.ResourceKey)
            </div>
            <div class="editor-field">
                @Html.EditorFor(model => model.ResourceKey)
                @Html.ValidationMessageFor(model => model.ResourceKey)
            </div>
            <div class="editor-label">
                @Html.LabelFor(model => model.ResourceType)
            </div>
            <div class="editor-field">
                @Html.DropDownListFor(model => model.ResourceType, Model.Type)
                @Html.ValidationMessageFor(model => model.ResourceType)
            </div>
            <div class="editor-label">
                @Html.LabelFor(model => model.ClearCache)
            </div>
            <div class="editor-field">
                @Html.EditorFor(model => model.ClearCache)
                @Html.ValidationMessageFor(model => model.ClearCache)
            </div>

            {
                if (!string.IsNullOrWhiteSpace(Model.Value))
                {
                    Html.RenderPartial("ResourceEditor", Model.Value);
                }
            }
            
            <p>
                <input type="submit" value="@Resource.Save" />
                <span class="field-validation-error">@Model.SaveError</span>
            </p>
        }
            <p>
            @using (Html.BeginForm(MVC.Admin.RouteResource.Delete(), FormMethod.Post))
            {
                <input type="hidden" id="deleteID" name="deleteID" value="@Model.ResourceID" />
                <input type="hidden" id="pageRoute" name="pageRoute" value="@Model.Route" />
                <input type="hidden" id="pageLanguage" name="pageLanguage" value="@Model.LanguageID" /> 
                <input type="submit" id="deleteResource" value="@Resource.Delete" />
            }
            </p>
        </fieldset>
        <button onclick="setNewResource(); return false;" type="button">@Resource.Add</button>
    </div>
<div class="clear">
</div>
