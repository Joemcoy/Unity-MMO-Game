$.extend({
	apiAjax: function(settings)
	{
		try
		{
			var url = window.location;
			var bas = url.protocol + "//" + url.host + "/api/";

			settings.url = bas + settings.url;
			settings.async = false;
			settings.cache = false;
			settings.error = function(xhr, msg)
			{
				console.error("Error " + xhr.status + ": " + msg);
			};

			if(!settings.hasOwnProperty("type"))
				settings.type = "POST";

			var ajax = $.ajax(settings);
			return ajax;
		}
		catch(err)
		{
			console.log(err);
			return false;
		}
	}
});

$.fn.success = function(message)
{
	return this.text(message).css("color", "darkgreen");
};

$.fn.warning = function(message)
{
	return this.text(message).css("color", "darkgoldenrod");
};

$.fn.error = function(message)
{
	return this.text(message).css("color", "darkred");
};


function fixChildrens(elements, atName)
{
	var url = window.location;
	var bas = url.protocol + "//" + url.host + "/";

	for(var i = 0; i < elements.length; i++)
	{
		var node = elements[i];
		if(node.hasAttribute(atName))
			node.setAttribute(atName, bas + node.getAttribute(atName));
	}
}

if (!String.prototype.format) 
{
	String.prototype.format = function() 
	{
		var args = arguments;
		return this.replace(/{(\d+)}/g, function(match, number) 
		{ 
			return typeof args[number] != 'undefined' ? args[number] : match;
		});
	};
}