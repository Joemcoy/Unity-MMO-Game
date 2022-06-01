function Dashboard() { }
Dashboard.currentPage = "";
Dashboard.timers = [];

Dashboard.registerTimer = function(callback, interval)
{
	var index = Dashboard.timers.length - 1;
	Dashboard.timers.push(setInterval(function() { callback(index); }, interval));
	return index;
};

Dashboard.removeTimer = function(index)
{
	if(index > -1 && index < Dashboard.timers.length)
	{
		clearInterval(Dashboard.timers[index]);
		array.splice(index, 1);
	}
};

$(document).ready(function()
{
	var baseTitle = document.title;
	if("onhashchange" in window)
	{
		$(window).on("hashchange", function()
		{

			for(var i = 0; i < Dashboard.timers.length; i++)
				clearInterval(Dashboard.timers[i]);
			Dashboard.timers.length = 0;

			var hash = location.hash;
			if(hash != "")
				Dashboard.currentPage = hash.substring(1);
			else
				location.hash = Dashboard.currentPage = "home";

			var container = $(".container-fluid");
			container.empty().html("Loading...");
			container.load("View/" + Dashboard.currentPage, function()
			{

				var ptitle = $("meta[name='web-title']");
				if(ptitle.length !== 0)
					document.title = baseTitle + " - " + ptitle.attr("content");
			});
		});

		$(document).ready(function()
		{
			$(window).trigger("hashchange");
		});
	}
	else
	{
		alert("The browser does not support hashing on the URL. Logging out ...");
		API.logout();
	}

	$(document).on("keypress", ".onlyNumbers", function (e) 
	{
		return !(e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57));
	});
});