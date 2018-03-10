var CKEDITOR_BASEPATH = '/areas/admin/content/scripts/ckeditor/';

function Textarea2CK(textarea) {
    //FCKeditor.BasePath = '/admin/fckeditor/' ;

	if (CKEDITOR.instances[textarea.id] == null) {
		var editor = CKEDITOR.replace(textarea.id);
		editor.config.height = textarea.rows * 20;
		editor.config.autoParagraph = false;
		editor.config.allowedContent = true;
		//editor.config.contentsCss = ["/Styles/defaults.css", "/Styles/style.css"];
		return editor;
	}
}

function split(val) {
    return val.split(/,\s*/);
}

function extractLast(term) {
    return split(term).pop();
}

$(function () {
	$("td[target]").css("cursor", "pointer").click(function () {
		var target = $(this).attr("target");
		document.location = target;
	});
	$("#edit-form-tabs").tabs();
	$(".wysiwyg").each(function () {
	    Textarea2CK(this);
	});

	$(".autocomplete").each(function () {
	    makeAutocomplete(this);
	});

	$(".ozi-currency").autoNumeric('init');

	$(document).on('click', 'a.dispose-img-from-collection', function (event) {
		event.preventDefault();
		var $this = $(this);
		var url = $this.data('url');
		var objId = $this.data("modelid");
		var propName = $this.data("path");
		var parentId = $this.data("parentid");
		var imgSrc = $this.data('srcpath');
		$.post(url, { imgSrc: imgSrc, id: objId, propName: propName, parentId: parentId }, function () {
			$this.parent().remove();
		});
	});
});

var makeAutocomplete = function(elem) {
    var $this = $(elem);
    var d;
    try {
        d = $("body").data($this.data("sourcename"));
        var m = $this.attr("multiple");
        if (m) {
            $this.bind("keydown", function (event) {
                if (event.keyCode === $.ui.keyCode.TAB && $this.data("ui-autocomplete").menu.active) {
                    event.preventDefault();
                }
            });
        }
        $this.autocomplete({
            minlength: 0,
            source: function (request, response) {
                response($.ui.autocomplete.filter(
                  d, extractLast(request.term)));
            },
            focus: function (event, ui) {
                if (!m) {
                    $this.val(ui.item.label);
                }
                return false;
            },
            select: autocompleteChanged,
            change: autocompleteChanged
        }).data("ui-autocomplete")._renderItem = function (ul, item) {
            return $("<li>")
              .append("<a>" + item.label + "</a>")
              .appendTo(ul);
        };

        function autocompleteChanged(event, ui) {
			var id = $this.data("hiddenid");
			if (m) {
				var terms = split(this.value);
				terms.pop();
				terms.push(ui.item.label);
				terms.push("");
				this.value = terms.join(", ");
				return false;
			} else {
				if (ui.item) {
					$("#" + id).val(ui.item.value);
					$this.val(ui.item.label);
				} else {
					$("#" + id).val(0);
				}
			}
			return false;
		}

    } catch (e) {
    }
};

var makeFileUpload = function (elem) {
    setTimeout(function () {
        //console.log("вызов в дефолте");
        //alert($("#collection_Floors_2__File").data("url"));
        var $this = $(elem);
        var objId = $this.data("modelid");
        var propName = $this.data("path");
        var parentId = $this.data("parentid");
        $this.fineUploader({
            request: {
                endpoint: $this.data("url")
            },
            multiple: true
        }).on("submit", function (event, id, filename) {
            $(this).fineUploader("setParams", {
                'id': objId,
                'propName': propName,
                'parentId': parentId
            });
        }).on("complete", function (event, id, filename, responseJSON) {
            if (responseJSON.success) {
                //$(this).parent().children("img").attr("src", responseJSON.Url);
                toastr.success("Данные успешно сохранены");
            } else {
                toastr.error("Произошла ошибка: " + responseJSON.error);
            }
        });
    }, 1000);
};



ko.bindingHandlers.wysiwyg = {
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
    	element.id = allBindings().attr.id;
		Textarea2CK(element);
    }
};
