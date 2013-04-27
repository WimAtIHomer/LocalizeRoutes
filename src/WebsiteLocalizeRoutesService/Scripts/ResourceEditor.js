function bindDropDownList(sourceDropDownList, targetDropDownList, list) {
    var key = $(sourceDropDownList).val();
    $('#' + targetDropDownList).empty();
    var option = "";
    list = $(list)
              .filter(function (index, element) {
                  return element.Key == key;
              })
              .map(function (index, element) {
                  var node = "<option value='" + element.Value + "'>" + element.Text + "</option>";
                  option += node;
                  return node;
              });
              $('#' + targetDropDownList).html(option);
              getResource();
}

function getResource(name) {
    jQuery("#ResourceID").val(name);
    $.getJSON(home + "Admin/RouteResource/Get", { id: name }, function (data, textResult) {
        jQuery("#ResourceType").val(data.ResourceType.Value);
        jQuery("#ResourceRoute").val(data.Route);
        jQuery("#ResourceKey").val(data.Key);
        //CKEDITOR.instances.CKEditor.setData(data.Value);
        setResourceValue(data.Value);
    });
}

function setNewResource() {
    jQuery("#ResourceID").val(0);
    jQuery("#ResourceType").val(1);
    jQuery("#ResourceRoute").val('/');
    jQuery("#ResourceKey").val(''); 
    setResourceValue('')
}

function setResourceValue(value) {
    jQuery("#TextEditor").val(value);
    CKEDITOR.instances.CKEditor.setData(value);
    setEditor();
}

function getResourceValue() {
    var val = jQuery("#TextEditor").val();
    if ($('#ResourceType').val() == 2) {
        val = CKEDITOR.instances.CKEditor.getData();
    }
    return val;
}

function setEditor() {
    if ($('#ResourceType').val() == 2) {
        jQuery("#TextEdit").hide();
        jQuery("#HtmlEdit").show();
    }
    else {
        jQuery("#TextEdit").show();
        jQuery("#HtmlEdit").hide();
    }
}

$(document).ready(function () {
    $('#Name').change(function () {
        var name = $('#Name').val();
        getResource(name);
    });
    $('#ResourceType').change(function () {
        setEditor();
    }).change();
});