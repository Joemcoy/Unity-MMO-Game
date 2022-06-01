$(document).ready(function()
{
	var tick = function(index)
	{
		$("#accounts-count").text("{0} accounts!".format(API.countAccounts()));
		$("#channels-count").text("{0} channels!".format(API.countChannels()));
		$("#characters-count").text("{0} characters!".format(API.countCharacters()));
		$("#api-access-count").text("{0} API keys!".format(API.countAPIAccess()));

		var logs = API.getLogs();
		if(logs == null)
			Dashboard.removeTimer(index);
		var logs_container = $("#logs-table");
		logs_container.empty();

		$("#logs-update").text("Updated yesterday at " + new Date(logs.LastUpdate).toLocaleString("en-US"));
		$.each(logs.Logs.reverse(), function(i, item)
		{
			var date_td = $("<td/>"), logger_td = $("<td/>"), type_td = $("<td/>"), message_td = $("<td/>");
			var date = new Date(item.Time);
			date_td.text(date.toLocaleString("en-US"));
			logger_td.text(item.Logger.Name);

			var type_format = "<span style='color: {0}; font-weight:bold'>{1}</span>";
			switch(item.Type)
			{
				case 0:
					type_td.html($(type_format.format("darkcyan", "Information")));
				break;
				case 1:
					type_td.html($(type_format.format("darkgreen", "Success")));
				break;
				case 2:
					type_td.html($(type_format.format("darkgoldenrod", "Warning")));
				break;
				case 3:
					type_td.html($(type_format.format("darkred", "Error")));
				break;
				case 4:
					type_td.html($(type_format.format("red", "Fatal")));
				break;
			}
			message_td.text(item.Message);

			logs_container.append($("<tr/>")
				.append(date_td)
				.append(logger_td)
				.append(type_td)
				.append(message_td)
				);
		});
	};

	tick(Dashboard.registerTimer(tick, 5000));
});